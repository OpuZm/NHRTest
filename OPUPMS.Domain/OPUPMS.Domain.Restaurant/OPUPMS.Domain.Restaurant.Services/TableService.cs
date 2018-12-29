using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Dapper;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Services
{
    public class TableService : SqlSugarService, ITableService
    {
        readonly ITableRepository _tableRep;
        readonly IOrderRepository _orderRep;
        readonly IRestaurantRepository _resRep;
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表

        public TableService(
            ITableRepository tableRepository,
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            IExtendItemRepository extendItemRepository)
        {
            _tableRep = tableRepository;
            _orderRep = orderRepository;
            _resRep = restaurantRepository;
            _extendItemRepository = extendItemRepository;
        }

        public List<TableListDTO> GetTableList(TableSearchDTO conditionDto)
        {
            List<TableListDTO> list = new List<TableListDTO>();
            var resList = _resRep.GetList(conditionDto.CompanyId);//获取此公司（租户）下关联的餐厅信息

            var ids = resList.Select(x => x.Id).ToArray();
            if (conditionDto.RestaurantId > 0)//只取当前餐厅
                ids = ids.Where(x => x == conditionDto.RestaurantId).ToArray();

            var tableList = _tableRep.GetTables(ids, conditionDto.AreaId, conditionDto.CythStatus,conditionDto.InCludVirtual);

            List<TableLinkOrderDTO> orderTableList = new List<TableLinkOrderDTO>();
            if (conditionDto.CythStatus == CythStatus.在用)
            {
                var statusArray = new int[] { (int)CyddStatus.开台, (int)CyddStatus.点餐,
                    (int)CyddStatus.用餐中, (int)CyddStatus.送厨, (int)CyddStatus.订单菜品修改,(int)CyddStatus.反结 };
                orderTableList = _orderRep.GetOrderTableListBy(
                    tableList.Select(x => x.Id).ToArray(), statusArray);
            }

            foreach (var item in tableList)
            {
                TableListDTO obj = new TableListDTO
                {
                    AreaId = item.R_Area_Id,
                    Description = item.Describe,
                    Id = item.Id,
                    Name = item.Name,
                    SeatNum = item.SeatNum,
                    ServerRate = item.ServerRate,
                    Restaurant = resList
                        .Where(x => x.Id == item.R_Restaurant_Id)
                        .Select(x => x.Name).FirstOrDefault(),
                    IsVirtual=item.IsVirtual
                };

                if (conditionDto.CythStatus == CythStatus.在用)
                {
                    List<OrderTableDTO> orderDtoList = new List<OrderTableDTO>();
                    var filerByTableList = orderTableList.Where(x => x.R_Table_Id == item.Id).ToList();
                    if (conditionDto.OrderTableId > 0)//菜品转台时，去除原来的订单台号
                    {
                        filerByTableList = filerByTableList.Where(x => x.Id != conditionDto.OrderTableId).ToList();
                        if (filerByTableList.Count == 0)
                            continue;
                    }
                    foreach (var orderTab in filerByTableList)
                    {
                        OrderTableDTO dto = new OrderTableDTO();
                        dto.Id = orderTab.Id;
                        dto.IsCheckOut = orderTab.IsCheckOut;
                        dto.IsOpen = orderTab.IsOpen;
                        dto.CreateDate = orderTab.CreateDate;
                        dto.TableId = orderTab.R_Table_Id;
                        dto.OrderId = orderTab.R_Order_Id;
                        dto.OrderNo = orderTab.OrderNo;
                        dto.IsLock = orderTab.IsLock;
                        dto.IsControl = orderTab.IsControl;
                        dto.PersonNum = orderTab.PersonNum;
                        orderDtoList.Add(dto);
                    }
                    obj.CurrentOrderList = orderDtoList;
                }
                list.Add(obj);
            }

            return list;
        }

        public bool JoinTableHandle(JoinTableSubmitDTO handleDto)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    var fromOrderModel = db.Queryable<R_Order>()
                        .Where(x => x.Id == handleDto.FromTableObj.OrderId)
                        .FirstOrDefault();
                    var toOrderModel = db.Queryable<R_Order>()
                        .Where(x => x.Id == handleDto.ToTableObj.OrderId)
                        .FirstOrDefault();
                    var fromTableModel = db.Queryable<R_Table>()
                        .FirstOrDefault(p => p.Id == handleDto.FromTableObj.TableId);
                    var toTableModel = db.Queryable<R_Table>()
                        .FirstOrDefault(p => p.Id == handleDto.ToTableObj.TableId);
                    if (fromOrderModel.CyddStatus != CyddStatus.点餐 && fromOrderModel.CyddStatus != CyddStatus.开台)
                    {
                        throw new Exception("当前订单状态异常,请退出后重新操作");
                    }
                    if (toOrderModel.CyddStatus != CyddStatus.开台 && toOrderModel.CyddStatus != CyddStatus.点餐)
                    {
                        throw new Exception("并台的订单状态异常,请退出后重新操作");
                    }
                    if (fromTableModel.CythStatus!=CythStatus.在用)
                    {
                        throw new Exception("当前台号状态异常,请退出后重新操作");
                    }
                    if (toTableModel.CythStatus != CythStatus.在用)
                    {
                        throw new Exception("并台的台号状态异常,请退出后重新操作");
                    }

                    var originalOrderList = db.Queryable<R_OrderTable>()
                        .Where(x => x.R_Order_Id == handleDto.FromTableObj.OrderId)
                        .ToList();
                    var originalTableList = db.Queryable<R_OrderTable>()
                        .Where(x =>
                            x.R_Table_Id == handleDto.FromTableObj.TableId &&
                            !x.IsCheckOut && x.IsOpen)
                        .ToList();
                    db.BeginTran();
                    //此订单只有一张台时，更改原订单状态
                    if (originalOrderList.Count == 1)
                    {
                        db.Update<R_Order>(new { CyddStatus = CyddStatus.取消 }, x => x.Id == handleDto.FromTableObj.OrderId);
                    }

                    db.Delete<R_OrderTable>(x => x.Id == handleDto.FromTableObj.OrderTableId);//删除原订单台号记录

                    if (originalTableList.Count == 1)
                    {
                        db.Update<R_Table>(new
                        {
                            CythStatus = CythStatus.空置
                        }, x => x.Id == handleDto.FromTableObj.TableId);
                    }

                    //更新订单明细关联订单台号信息
                    db.Update<R_OrderDetail>(
                        new
                        {
                            R_OrderTable_Id = handleDto.ToTableObj.OrderTableId
                        },
                        x => x.R_OrderTable_Id == handleDto.FromTableObj.OrderTableId);

                    //插入两条操作订单记录
                    R_OrderRecord entity = new R_OrderRecord();
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUser = handleDto.UserId;
                    entity.CyddCzjlStatus = CyddStatus.并台;
                    entity.CyddCzjlUserType = CyddCzjlUserType.员工;
                    entity.R_OrderTable_Id = handleDto.FromTableObj.OrderTableId;
                    entity.R_Order_Id = handleDto.FromTableObj.OrderId;
                    entity.Remark = string.Format(
                        "并台操作 - 把订单({0})的台号[{1}]的菜品信息,并台到订单({2})下的台号[{3}]中",
                        fromOrderModel.OrderNo, fromTableModel.Name,
                        toOrderModel.OrderNo, toTableModel.Name);
                    db.Insert<R_OrderRecord>(entity);

                    entity = new R_OrderRecord
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = handleDto.UserId,
                        CyddCzjlStatus = CyddStatus.并台,
                        CyddCzjlUserType = CyddCzjlUserType.员工,
                        R_OrderTable_Id = handleDto.ToTableObj.OrderTableId,
                        R_Order_Id = handleDto.ToTableObj.OrderId,
                        Remark = string.Format(
                            "并台操作 - 收到订单({0})的台号[{1}]的菜品信息,并台到订单({2})下的台号[{3}]中",
                            fromOrderModel.OrderNo, fromTableModel.Name,
                            toOrderModel.OrderNo, toTableModel.Name)
                    };

                    db.Insert<R_OrderRecord>(entity);

                    //OrderPayRecord
                    db.Update<R_OrderPayRecord>(new { R_Order_Id = handleDto.ToTableObj.OrderId }, x => x.R_Order_Id == handleDto.FromTableObj.OrderId && x.CyddJzStatus==CyddJzStatus.已付 && x.PId==0);
                    db.CommitTran();
                }
                catch (Exception e)
                {
                    result = false;
                    db.RollbackTran();
                    throw e;
                }
                return result;
            }
        }

        /// <summary>
        /// 开台 拼台操作处理
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tableIds"></param>
        /// <param name="msg"></param>
        /// <param name="reuse">false 开台，true 拼台</param>
        /// <returns></returns>
        public OpenTableCreateResultDTO OpenTableHandle(
            ReserveCreateDTO req, List<int> tableIds, out string msg, bool reuse = false)
        {
            //取餐饮账务日期 TypeId=10003
            var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();

            using (var db = new SqlSugarClient(Connection))
            {
                if (dateItem == null)
                    throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

                DateTime accDate = DateTime.Today;

                if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                    throw new Exception("餐饮账务日期设置错误，请联系管理员");

                string messge = string.Empty;
                OpenTableCreateResultDTO res = new OpenTableCreateResultDTO();
                string ids = string.Join(",", tableIds);

                try
                {
                    db.BeginTran();

                    var isCanOpen = db.Queryable<R_Table>()
                        .Any(p => p.CythStatus != CythStatus.空置 && tableIds.Contains(p.Id));

                    if ((!isCanOpen && !reuse) || reuse)
                    {
                        int insertId = req.Id;
                        R_Order model = Mapper.Map<ReserveCreateDTO, R_Order>(req);
                        if (req.Id <= 0)
                        {
                            model.OpenDate = model.CreateDate;
                            var insert = db.Insert<R_Order>(model);  //订单主表
                            insertId = Convert.ToInt32(insert);
                        }
                        else
                        {
                            if (req.Id > 0 &&
                                req.CyddStatus == CyddStatus.预定 &&
                                req.ReserveDate.HasValue &&
                                req.ReserveDate.Value < DateTime.Now.AddHours(-1))//判断是否超过时间，保留一小时
                                throw new Exception("此预订订单已过期，当前时间已超过预订时间("
                                    + req.ReserveDate.Value.ToString("yyyy-MM-dd HH:mm") + ")！");

                            if (req.Id > 0 && req.CyddStatus == CyddStatus.预定 &&
                                req.ReserveDate.HasValue &&
                                req.ReserveDate.Value > DateTime.Now.AddHours(8))//判断是否已到预订时间
                                throw new Exception("此预订订单未到预订开台时间(" +
                                    req.ReserveDate.Value.ToString("yyyy-MM-dd HH:mm") +
                                    ")，但可在此预订时间提前8小时开台！");

                            db.Update<R_Order>(new
                            {
                                CyddStatus = CyddStatus.开台,
                                OpenDate = DateTime.Now
                            }, x => x.Id == req.Id);
                        }

                        if (!reuse)//开台时更新台状态
                            db.Update<R_Table>(new
                            {
                                CythStatus = (int)CythStatus.在用,
                            }, p => tableIds.Contains(p.Id));
                        res.OrderId = insertId;

                        int personNumAvg = tableIds.Count() > 1 ?
    req.PersonNum / tableIds.Count() : req.PersonNum;    //台号人均
                        int personNumRemainder = tableIds.Count() > 1 ?
                            req.PersonNum % tableIds.Count() : 0;  //台号余人
                        int eachRemainder = 0;
                        if (tableIds != null && tableIds.Count > 0)
                        {
                            res.OrderTableIds = new List<int>();
                            int orderTableId = 0;
                            if (req.Id <= 0)
                            {
                                List<R_OrderTable> otList = new List<R_OrderTable>();
                                foreach (var item in tableIds)
                                {
                                    eachRemainder++;
                                    R_OrderTable obj = new R_OrderTable();
                                    obj.R_Order_Id = insertId;
                                    obj.R_Table_Id = item;
                                    obj.CreateDate = DateTime.Now;
                                    obj.IsOpen = true; //开台标识
                                    obj.PersonNum = personNumAvg + (personNumRemainder - eachRemainder >= 0 ? 1 : 0);
                                    obj.R_Market_Id = req.CurrentMarketId;
                                    obj.BillDate = accDate;
                                    otList.Add(obj);
                                }
                                db.InsertRange(otList);
                            }
                            else
                            {
                                db.Update<R_OrderTable>(new { IsOpen = true, R_Market_Id = req.CurrentMarketId, BillDate = accDate }
                                    , x => x.R_Order_Id == req.Id);
                            }
                            res.OrderTableIds = db.Queryable<R_OrderTable>()
                                .Where(x => x.R_Order_Id == insertId).Select(x => x.Id).ToList();
                            var tableNames = db.Queryable<R_Table>()
                                .Where(x => tableIds.Contains(x.Id))
                                .Select(x => x.Name).ToList();

                            res.TablesName = tableNames;
                            R_OrderRecord record = new R_OrderRecord
                            {
                                CreateDate = DateTime.Now,
                                R_Order_Id = insertId,
                                CreateUser = req.CreateUser,
                                CyddCzjlStatus = CyddStatus.开台,
                                CyddCzjlUserType = req.UserType,
                                Remark = string.Format(
                                    "开台操作-订单（{0}）开台（{1}）",
                                    model.OrderNo, tableNames.Join(",")),
                                R_OrderTable_Id = res.OrderTableIds.Count == 1 ? orderTableId : 0 //订单操作纪录
                            };

                            db.Insert<R_OrderRecord>(record);
                        }
                        else
                        {
                            res = null;
                            msg = "开台请选择餐台！";
                            //db.RollbackTran();
                            //return res;
                        }
                    }
                    else
                    {
                        res = null;
                        messge = "所选台号已被占用，请重新选择";
                    }

                    db.CommitTran();
                }
                catch (Exception e)
                {
                    res = null;
                    messge = e.Message;
                    db.RollbackTran();
                }

                msg = messge;
                return res;
            }
        }

        /// <summary>
        /// 换桌操作处理
        /// </summary>
        /// <param name="handleDto"></param>
        /// <returns></returns>
        public bool ChangeTableHandle(ChangeTableSubmitDTO handleDto)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    var userInfo = OperatorProvider.Provider.GetCurrent();
                    db.BeginTran();
                    var orderTable = db.Queryable<R_OrderTable>()
                        .Where(p => p.Id == handleDto.OrderTableId)
                        .FirstOrDefault();
                    var newTable = db.Queryable<R_Table>()
                        .FirstOrDefault(p => p.Id == handleDto.NewTableId);
                    if (newTable.CythStatus == CythStatus.在用)
                        throw new Exception(string.Format(
                            "所选台号（{0}）已被使用，请重新选择！", newTable.Name));

                    //var orderTableList = OrderRep.GetOrderTableListBy(handleDto.OldTableId, SearchTypeBy.台号Id);
                    var orderTableList = db.Queryable<R_OrderTable>()
                        .Where(x => x.R_Table_Id == handleDto.OldTableId
                                && !x.IsCheckOut && x.IsOpen)
                        .ToList();

                    db.Update<R_OrderTable>(new
                    {
                        R_Table_Id = handleDto.NewTableId
                    }, p => p.Id == handleDto.OrderTableId);

                    if (orderTableList.Count == 1)
                        db.Update<R_Table>(new
                        {
                            CythStatus = CythStatus.清理
                        }, p => p.Id == handleDto.OldTableId);

                    db.Update<R_Table>(new
                    {
                        CythStatus = CythStatus.在用
                    }, p => p.Id == handleDto.NewTableId);

                    var order = db.Queryable<R_Order>()
                        .Where(x => x.Id == orderTable.R_Order_Id)
                        .FirstOrDefault();
                    var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == order.R_Market_Id);
                    var resturant = db.Queryable<R_Restaurant>().FirstOrDefault(p => p.Id == order.R_Restaurant_Id);

                    var oldTableName = db.Queryable<R_Table>()
                        .Where(x => x.Id == handleDto.OldTableId)
                        .Select(x => x.Name)
                        .FirstOrDefault();

                    R_OrderRecord record = new R_OrderRecord
                    {
                        CreateDate = DateTime.Now,
                        R_Order_Id = orderTable.R_Order_Id,
                        R_OrderTable_Id = handleDto.OrderTableId,
                        CyddCzjlStatus = CyddStatus.换桌,
                        Remark = string.Format("订单（{0}）换桌从台[{1}]换至:[{2}]",
                            order.OrderNo, oldTableName, newTable.Name),
                        CyddCzjlUserType = CyddCzjlUserType.员工,
                        CreateUser = handleDto.CreateUser
                    };
                    db.Insert<R_OrderRecord>(record);

                    var areaIds = newTable.R_Area_Id;
                    var prints = db.Queryable<Printer>()
                        .JoinTable<R_WeixinPrint>((s1, s2) => s1.Id == s2.Print_Id)
                        .JoinTable<R_WeixinPrint, R_WeixinPrintArea>((s1, s2, s3) => s2.Id == s3.R_WeixinPrint_Id)
                        .Where<R_WeixinPrint, R_WeixinPrintArea>((s1, s2, s3) => s1.IsDelete == false && 
                        s2.PrintType == PrintType.换台区域出单 && areaIds == s3.R_Area_Id).Select("s1.*").ToList();
                    var printerList = prints.Distinct().ToList();
                    if (printerList.Any())
                    {
                        string cpdyThGuid = string.Empty;   //出品打印按桌号出生成标识
                        cpdyThGuid = orderTable.Id + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        List<Cpdy> cydyInsert = new List<Cpdy>();
                        foreach (var print in printerList)
                        {
                            cydyInsert.Add(new Cpdy
                            {
                                cymxxh00 = orderTable.Id,
                                cyzdxh00 = orderTable.R_Order_Id,
                                cymxdm00 = market.Name,
                                cymxmc00 = string.Format("订单号:({0}) 台号:({1}) 转至 台号:({2})",order.OrderNo,oldTableName,newTable.Name),
                                cymxdw00 = userInfo.UserName,
                                cymxsl00 = string.Empty,
                                cymxdybz = false,
                                cymxyj00 = print.IpAddress,
                                cymxclbz = "0",
                                cymxczrq = DateTime.Now,
                                cymxzdbz = "2",
                                //cymxyq00 = detail.CyddMxName,
                                //cymxzf00 = detail.CyddMxName,
                                //cymxpc00 = detail.CyddMxName,
                                cymxczy0 = userInfo.UserName,
                                cymxfwq0 = print.PcName,
                                cymxczdm = userInfo.UserCode,
                                cymxje00 = "",
                                cymxth00 = oldTableName,
                                cymxrs00 = order.PersonNum.ToString(),
                                cymxct00 = resturant.Name,
                                cymxzdid = cpdyThGuid,
                                cymxbt00 = "换台单",
                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                cpdysdxh = cpdyThGuid,
                                cymxdk00 = print.Name,
                                cymxgdbz = false,
                                cpdyfsl0 = market.Name
                            });
                        }
                        db.InsertRange<Cpdy>(cydyInsert);
                    }

                    db.CommitTran();
                }
                catch (Exception e)
                {
                    res = false;
                    db.RollbackTran();
                    throw e;
                }

                return res;
            }
        }

        public bool SeparateTableHandle(SeparateTableSubmitDTO handleDto)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    db.BeginTran();
                    var originalOrderTable = db.Queryable<R_OrderTable>()
                        .Where(p => p.Id == handleDto.OrderTableId)
                        .FirstOrDefault();
                    var originalOrder = db.Queryable<R_Order>()
                        .Where(x => x.Id == originalOrderTable.R_Order_Id)
                        .FirstOrDefault();

                    var newTable = db.Queryable<R_Table>()
                        .FirstOrDefault(p => p.Id == handleDto.NewTableId);
                    var oldTable = db.Queryable<R_Table>()
                        .FirstOrDefault(p => p.Id == handleDto.OldTableId);
                    if (newTable.CythStatus == CythStatus.在用)
                        throw new Exception(string.Format(
                            "所选台号（{0}）已被使用，请重新选择！", newTable.Name));

                    R_Order order = new R_Order
                    {
                        CyddStatus = CyddStatus.用餐中,
                        OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        CreateDate = DateTime.Now,
                        CreateUser = handleDto.CreateUser,
                        CyddOrderSource = originalOrder.CyddOrderSource,
                        ContactPerson = originalOrder.ContactPerson,
                        ContactTel = originalOrder.ContactTel,
                        R_Market_Id = originalOrder.R_Market_Id,
                        TableNum = 1,
                        PersonNum = 1,
                        R_Restaurant_Id = newTable.R_Restaurant_Id,
                        Remark = string.Format("从原订单({0})中拆台生成的新订单", originalOrder.OrderNo)
                    };
                    var newOrderId = db.Insert(order);

                    R_OrderTable orderTable = new R_OrderTable();
                    orderTable.CreateDate = DateTime.Now;
                    orderTable.IsOpen = true;
                    orderTable.PersonNum = order.PersonNum;
                    orderTable.R_Order_Id = newOrderId.ObjToInt();
                    orderTable.R_Table_Id = handleDto.NewTableId;
                    var newOrderTableId = db.Insert(orderTable);

                    res = db.Update<R_Table>(new
                    {
                        CythStatus = CythStatus.在用
                    }, p => p.Id == handleDto.NewTableId);

                    var oldDetailList = db.Queryable<R_OrderDetail>()
                        .Where(x => x.R_OrderTable_Id == handleDto.OrderTableId)
                        .ToList();

                    foreach (var item in handleDto.SelectedList)
                    {
                        var detail = oldDetailList.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (detail != null)
                        {
                            //判断拆台勾选菜品项数量是否全部转至其它台
                            if (detail.Num >= item.Num)
                                db.Delete<R_OrderDetail>(x => x.Id == detail.Id);
                            else
                                db.Update<R_OrderDetail>(new { Num = (detail.Num - item.Num) }, x => x.Id == detail.Id);//更新明细菜品数量

                            R_OrderDetail newDetail = detail;
                            newDetail.Id = 0;
                            newDetail.Num = item.Num;
                            newDetail.R_OrderTable_Id = newOrderTableId.ObjToInt();
                            db.Insert<R_OrderDetail>(newDetail);
                        }
                    }

                    //插入两条操作订单记录
                    R_OrderRecord entity = new R_OrderRecord();
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUser = handleDto.CreateUser;
                    entity.CyddCzjlStatus = CyddStatus.拆台;
                    entity.CyddCzjlUserType = CyddCzjlUserType.员工;
                    entity.R_OrderTable_Id = handleDto.OrderTableId;
                    entity.R_Order_Id = originalOrder.Id;
                    entity.Remark = string.Format(
                        "拆台操作 - 把订单({0})的台号[{1}]勾选的菜品信息拆台到新订单({2})下的台号[{3}]中",
                        originalOrder.OrderNo, oldTable.Name, order.OrderNo, newTable.Name);
                    db.Insert<R_OrderRecord>(entity);

                    entity = new R_OrderRecord
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = handleDto.CreateUser,
                        CyddCzjlStatus = CyddStatus.拆台,
                        CyddCzjlUserType = CyddCzjlUserType.员工,
                        R_OrderTable_Id = newOrderTableId.ObjToInt(),
                        R_Order_Id = newOrderId.ObjToInt(),
                        Remark = string.Format(
                            "拆台操作 - 把订单({0})的台号[{1}]勾选的菜品信息拆台到新订单({2})下的台号[{3}]中",
                            originalOrder.OrderNo, oldTable.Name, order.OrderNo, newTable.Name)
                    };
                    db.Insert<R_OrderRecord>(entity);

                    db.CommitTran();
                    res = true;
                }
                catch (Exception)
                {
                    res = false;
                    db.RollbackTran();
                    throw;
                }

                return res;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateTablesStatus(List<int> tableIds, CythStatus status)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                try
                {
                    db.BeginTran();
                    var tableList = db.Queryable<R_Table>().Where(x => tableIds.Contains(x.Id)).ToList();
                    
                    if(tableList == null || tableList.Count == 0)
                        throw new Exception("指定餐台不存在，不能执行空置清理操作!");

                    StringBuilder errMsg = new StringBuilder();
                    foreach (var item in tableList)
                    {
                        if (item.CythStatus != CythStatus.清理)
                            errMsg.AppendLine(string.Format(
                                "当前餐台({0})状态为：(1)，不能执行空置清理操作!",
                                item.Name, Enum.GetName(typeof(CythStatus), item.CythStatus)));
                        item.CythStatus = status;
                    }

                    if (errMsg != null && errMsg.Length > 0)
                        throw new Exception(errMsg.ToString());

                    var result = db.UpdateRange(tableList);
                    db.CommitTran();
                    return result.Any(x => !x) ? false : true;
                }
                catch (Exception e)
                {
                    db.RollbackTran();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 加台操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<int> AddTableHandle(AddTableSubmitDTO req)
        {
            List<int> res = new List<int>();
            using (var db=new SqlSugarClient(Connection))
            {
                try
                {
                    db.BeginTran();
                    List<R_OrderTable> inserOTList = new List<R_OrderTable>();
                    var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();
                    if (dateItem == null)
                    {
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");
                    }
                    if (db.Queryable<R_OrderTable>().Any(p => (p.IsOpen == true && p.IsCheckOut==false)
                    && req.NewTableIds.Contains(p.R_Table_Id)))
                    {
                        throw new Exception("选择的台号已经被别的订单使用，请重新选择");
                    }
                    var orderModel = db.Queryable<R_Order>().JoinTable<R_OrderTable>((s1, s2) =>
                      s1.Id == s2.R_Order_Id).Where<R_OrderTable>((s1,s2)=>s2.Id==req.OrderTableId)
                      .Select<R_Order>("s1.*").First();
                    var tables = db.Queryable<R_Table>().Where(p => req.NewTableIds.Contains(p.Id))
                        .Select(p => p.Name).ToList();

                    db.Update<R_Table>(new { CythStatus = CythStatus.在用 }, p =>req.NewTableIds.Contains(p.Id));

                    foreach (var item in req.NewTableIds)
                    {
                        inserOTList.Add(new R_OrderTable
                        {
                            R_Order_Id= orderModel.Id,
                            R_Table_Id=item,
                            CreateDate=DateTime.Now,
                            PersonNum=0,
                            IsCheckOut=false,
                            IsOpen=true,
                            IsLock=false,
                            BillDate=Convert.ToDateTime(dateItem.ItemValue),
                            R_Market_Id=req.CurrentMarketId
                        });
                    }

                    var orderTableIds= db.InsertRange<R_OrderTable>(inserOTList).ToList();

                    var orderTables = db.Queryable<R_OrderTable>()
                        .Where(p => p.R_Order_Id == orderModel.Id).ToList();

                    int personNumAvg = orderTables.Count() > 1 ?
orderModel.PersonNum / orderTables.Count() : orderModel.PersonNum;    //台号人均
                    int personNumRemainder = orderTables.Count() > 1 ?
                        orderModel.PersonNum % orderTables.Count() : 0;  //台号余人
                    int eachRemainder = 0;

                    if (orderTables != null && orderTables.Count() > 0)
                    {
                        foreach (var item in orderTableIds)
                        {
                            res.Add(Convert.ToInt32(item));
                        }
                        foreach (var orderTable in orderTables)
                        {
                            eachRemainder++;
                            orderTable.PersonNum = personNumAvg + (personNumRemainder - eachRemainder >= 0 ? 1 : 0);
                        }
                        db.UpdateRange<R_OrderTable>(orderTables);//更新订单台号人数
                    }

                    R_OrderRecord record = new R_OrderRecord
                    {
                        CreateDate = DateTime.Now,
                        R_Order_Id = orderModel.Id,
                        CreateUser = req.CreateUser,
                        CyddCzjlStatus = CyddStatus.开台,
                        CyddCzjlUserType = CyddCzjlUserType.员工,
                        Remark = string.Format(
        "开台操作-订单（{0}）开台（{1}）",orderModel.OrderNo, tables.Join(",")),
                        R_OrderTable_Id = orderTableIds.Count == 1 ? Convert.ToInt32(orderTableIds[0]) : 0
                    };

                    db.Insert<R_OrderRecord>(record);
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    throw ex;
                }
            }
            return res;
        }

        /// <summary>
        /// 辙台操作
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public bool CancelOrderTable(CancelOrderTableSubmitDTO req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    db.BeginTran();
                    var detailList = db.Queryable<R_OrderDetail>()
                        .Where(p => p.R_OrderTable_Id == req.OrderTableId && p.CyddMxStatus != CyddMxStatus.保存);
                    var orderTableModel = db.Queryable<R_OrderTable>()
                        .Where(p => p.Id == req.OrderTableId).First();
                    var orderModel = db.Queryable<R_Order>()
                        .Where(p => p.Id == orderTableModel.R_Order_Id).First();
                    if (detailList.Any())
                    {
                        throw new Exception("订单台号下已有下单菜品,不能执行撤台操作");
                    }
                    else
                    {
                        List<R_OrderRecord> records = new List<R_OrderRecord>();
                        var otherOrderTables = db.Queryable<R_OrderTable>()
                            .Where(p => p.R_Table_Id == orderTableModel.R_Table_Id 
                            && p.IsCheckOut == false && p.IsOpen==true && p.Id!=orderTableModel.Id);   //该台号下未结账的记录,用于判断是否设该台号为清理状态
                        var orderTables = db.Queryable<R_OrderTable>()
                            .Where(p => p.R_Order_Id == orderModel.Id && p.Id!=orderTableModel.Id);    //该订单下其它订单台号纪录
                        db.Update<R_OrderTable>(new { IsCheckOut=true, PersonNum=0, IsOpen=false }, p => p.Id == orderTableModel.Id);
                        if (!otherOrderTables.Any())
                        {
                            db.Update<R_Table>(new { CythStatus=CythStatus.清理 }, p => p.Id == orderTableModel.R_Table_Id);
                        }

                        if (!orderTables.Any())
                        {
                            orderModel.CyddStatus = CyddStatus.取消;
                        }
                        else
                        {
                            orderModel.PersonNum = orderModel.PersonNum - orderTableModel.PersonNum;
                            var lastNoCheckOut = orderTables.Any(p => p.IsCheckOut==false);
                            var AllNoCheckOutCount = orderTables.Where(p => p.IsCheckOut == false).Count();
                            if (!lastNoCheckOut)
                            {
                                orderModel.CyddStatus = CyddStatus.结账;
                            }
                        }
                        records.Add(new R_OrderRecord
                        {
                            CreateUser = req.CreateUser,
                            CreateDate = DateTime.Now,
                            CyddCzjlStatus = CyddStatus.取消,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            R_OrderTable_Id = orderTableModel.Id,
                            R_Order_Id = orderModel.Id,
                            Remark = string.Format("台号已撤消")
                        });

                        db.Update<R_Order>(orderModel);
                        db.InsertRange<R_OrderRecord>(records);
                        db.CommitTran();
                        res = true;
                    }
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    throw ex;
                }
                return res;
            }
        }

        public List<TableListDTO> GetTableListForApi(TableSearchDTO conditionDto)
        {
            List<TableListDTO> list = new List<TableListDTO>();
            int[] resIds = new int[] { conditionDto.RestaurantId};
            var tableList = _tableRep.GetTables(resIds, conditionDto.AreaId);

            List<TableLinkOrderDTO> orderTableList = new List<TableLinkOrderDTO>();
            if (conditionDto.CythStatus == CythStatus.在用)
            {
                var statusArray = new int[] { (int)CyddStatus.开台, (int)CyddStatus.点餐,
                    (int)CyddStatus.用餐中, (int)CyddStatus.送厨, (int)CyddStatus.订单菜品修改 };
                orderTableList = _orderRep.GetOrderTableListBy(
                    tableList.Select(x => x.Id).ToArray(), statusArray);
            }

            foreach (var item in tableList)
            {
                TableListDTO obj = new TableListDTO
                {
                    AreaId = item.R_Area_Id,
                    Description = item.Describe,
                    Id = item.Id,
                    Name = item.Name,
                    SeatNum = item.SeatNum,
                    ServerRate = item.ServerRate,
                    RestaurantId=conditionDto.RestaurantId,
                    CythStatus=(int)item.CythStatus
                };

                if (conditionDto.CythStatus == CythStatus.在用)
                {
                    List<OrderTableDTO> orderDtoList = new List<OrderTableDTO>();
                    var filerByTableList = orderTableList.Where(x => x.R_Table_Id == item.Id).ToList();
                    //if (conditionDto.OrderTableId > 0)//菜品转台时，去除原来的订单台号
                    //{
                    //    filerByTableList = filerByTableList.Where(x => x.Id != conditionDto.OrderTableId).ToList();
                    //    if (filerByTableList.Count == 0)
                    //        continue;
                    //}
                    foreach (var orderTab in filerByTableList)
                    {
                        OrderTableDTO dto = new OrderTableDTO();
                        dto.Id = orderTab.Id;
                        dto.IsCheckOut = orderTab.IsCheckOut;
                        dto.IsOpen = orderTab.IsOpen;
                        dto.CreateDate = orderTab.CreateDate;
                        dto.TableId = orderTab.R_Table_Id;
                        dto.OrderId = orderTab.R_Order_Id;
                        dto.OrderNo = orderTab.OrderNo;
                        dto.IsLock = orderTab.IsLock;
                        dto.ContactPerson = orderTab.ContactPerson;
                        dto.ContactTel = orderTab.ContactTel;
                        dto.IsControl = orderTab.IsControl;
                        dto.PersonNum = orderTab.PersonNum;
                        orderDtoList.Add(dto);
                    }
                    obj.CurrentOrderList = orderDtoList;
                }
                list.Add(obj);
            }

            return list;
        }
    }
}