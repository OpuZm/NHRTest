using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Dapper;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Services
{
    public class OrderService : SqlSugarService, IOrderService
    {
        readonly IMultiDbDbFactory _currentFactory;
        readonly ITableRepository _tableRep;
        readonly IOrderRepository _orderRep;
        readonly IRestaurantRepository _resRep;
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        readonly IMarketRepository _marketRep;
        readonly IOrderPayRecordRepository _payRep;

        public OrderService(IMultiDbDbFactory factory, 
            ITableRepository tableRepository, 
            IOrderRepository orderRepository, 
            IRestaurantRepository restaurantRepository,
            IExtendItemRepository extendItemRepository,
            IMarketRepository marketRepository,
            IOrderPayRecordRepository orderPayRecordRepository)
        {
            _currentFactory = factory;
            _tableRep = tableRepository;
            _orderRep = orderRepository;
            _resRep = restaurantRepository;
            _extendItemRepository = extendItemRepository;
            _marketRep = marketRepository;
            _payRep = orderPayRecordRepository;
        }

        public bool CancelOrderHandle(CancelOrderOperateDTO operateDTO)
        {
            //取餐饮账务日期 TypeId=10003
            var dateItem = _extendItemRepository.GetModelList(operateDTO.CompanyId, 10003).FirstOrDefault();
            
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = false;
                try
                {
                    db.BeginTran();
                    var orderModel = db.Queryable<R_Order>().Where(x => x.Id == operateDTO.OrderId).FirstOrDefault();
                    if(orderModel == null || orderModel.Id <= 0)
                        throw new Exception("此订单不存在！");

                    if (orderModel.CyddStatus != CyddStatus.预定 && orderModel.CyddStatus != CyddStatus.开台)
                        throw new Exception(string.Format("不能取消{0}状态订单！", Enum.GetName(typeof(CyddStatus), orderModel.CyddStatus)));

                    var orderTableLsit = db.Queryable<R_OrderTable>()
                        .Where(x => x.R_Order_Id == operateDTO.OrderId).ToList();//取当前订单关联的订单台列表

                    List<int> ordTabIds = new List<int>();
                    List<int> tabIds = new List<int>();
                    if (orderTableLsit != null && orderTableLsit.Any())
                    {
                        ordTabIds = orderTableLsit.Select(x => x.Id).ToList();// 取关联的订单台Id
                        tabIds = orderTableLsit.Select(x => x.R_Table_Id).ToList();//取关联的台Id
                    }

                    //查当前关联台号下是否同时存在的其它订单（已开台未结账的）
                    var otherOrderTableList = db.Queryable<R_OrderTable>()
                            .Where(x => tabIds.Contains(x.R_Table_Id) 
                                    && x.R_Order_Id != operateDTO.OrderId 
                                    && !x.IsCheckOut && x.IsOpen).ToList();

                    var orderDetailList = db.Queryable<R_OrderDetail>()
                        .Where(x => ordTabIds.Contains(x.R_OrderTable_Id) && x.CyddMxStatus!=CyddMxStatus.保存).ToList();
                    
                    if (orderDetailList != null && orderDetailList.Any())
                        throw new Exception("不能取消已点餐订单！");

                    if (orderModel.CyddStatus == CyddStatus.开台)//开台状态下的订单才更新餐台状态，预订订单则跳过
                    {
                        foreach (var id in tabIds)
                        {
                            // 判断当前餐台是否当前同时存在多个订单，若包含则跳过
                            if (otherOrderTableList.Where(x => x.R_Table_Id == id).Any()) 
                                continue;

                            db.Update<R_Table>(new { CythStatus = CythStatus.清理 }, x => x.Id == id);
                        }
                    }

                    var payList = db.Queryable<R_OrderPayRecord>()
                        .Where(x => x.R_Order_Id == operateDTO.OrderId && x.CyddJzType == CyddJzType.定金).ToList();

                    if(payList != null && payList.Count > 0)
                    {
                        List<R_OrderPayRecord> newPayList = new List<R_OrderPayRecord>();
                        
                        foreach (var item in payList)
                        {
                            var model = Mapper.Map<R_OrderPayRecord, R_OrderPayRecord>(item);
                            model.Id = 0;
                            model.PayAmount = -model.PayAmount;
                            model.CreateDate = DateTime.Now;
                            model.CreateUser = operateDTO.OperateUserId;
                            model.BillDate = dateItem.ItemValue.ToDateOrNull();
                            newPayList.Add(model);
                        }

                        db.InsertRange(newPayList);
                    }

                    result = db.Update<R_Order>(new { CyddStatus = CyddStatus.取消 }, x => x.Id == operateDTO.OrderId);
                    
                    //更新订单台号 状态关闭
                    result = db.Update<R_OrderTable>(new { IsOpen = false }, x => x.R_Order_Id == operateDTO.OrderId);

                    R_OrderRecord entity = new R_OrderRecord();
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUser = operateDTO.OperateUserId;
                    entity.CyddCzjlStatus = CyddStatus.取消;
                    entity.CyddCzjlUserType = CyddCzjlUserType.员工;
                    entity.R_OrderTable_Id = 0;
                    entity.R_Order_Id = operateDTO.OrderId;
                    entity.Remark = string.Format("取消订单操作 - 订单({0})取消", orderModel.OrderNo);
                    db.Insert<R_OrderRecord>(entity);
                    db.CommitTran();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    db.RollbackTran();
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }

        
        /// <summary>
        /// 预定保存
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tableIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ReserveCreateDTO SaveReserveOrderHandle(ReserveCreateDTO req, List<int> tableIds, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string messge = string.Empty;
                ReserveCreateDTO res = new ReserveCreateDTO();
                if (!req.ReserveDate.HasValue || (req.ReserveDate.HasValue && req.ReserveDate.Value < DateTime.Now))
                {
                    msg = "所选预订日期时间无效，请重新选择";
                    res = null;
                    return res;
                }

                string date = req.ReserveDate.Value.ToString("yyyyMMdd");
                string ids = string.Join(",", tableIds);
                try
                {
                    db.BeginTran();
                    if(req.Id > 0)
                    {
                        var ordTabList = db.Queryable<R_OrderTable>().Where(x => x.R_Order_Id == req.Id).ToList();
                        if (ordTabList != null && ordTabList.Count > 0)
                        {
                            var ordTabIds = ordTabList.Select(x => x.Id).ToList();
                            db.Delete<R_OrderDetail>(x => ordTabIds.Contains(x.R_OrderTable_Id));//删除旧预订菜品明细
                        }

                        db.Delete<R_OrderTable>(ordTabList); //删除预订的旧订单台号记录
                    }

                    var data = db.Sqlable().From<R_Order>("s1")
						.Join<R_OrderTable>("s2", "s1.Id", "s2.R_Order_Id", JoinType.Inner);
                    data = data.Where("s1.CyddStatus=" + (int)CyddStatus.预定);
                    data = data.Where("s1.R_Market_Id=" + req.R_Market_Id);
                    data = data.Where("CONVERT(varchar(20), s1.ReserveDate, 112)=" + date);
                    data = data.Where("s2.R_Table_Id in (" + ids + ")");

                    var list = data.SelectToList<R_Order>("s1.Id");
                    //var sql = data.Sql;
                    if (list == null || list.Count == 0)
                    {
                        string recordRemark = "";
                        int orderId = req.Id;
                        R_Order model = Mapper.Map<ReserveCreateDTO, R_Order>(req);
                        if (req.Id > 0)
                        {
                            db.Update<R_Order>(
                                new
                                {
                                    ContactPerson = req.ContactPerson,
                                    ContactTel = req.ContactTel,
                                    CyddOrderSource = req.CyddOrderSource,
                                    PersonNum = req.PersonNum,
                                    ReserveDate = req.ReserveDate,
                                    R_Market_Id = req.R_Market_Id,
                                    R_Restaurant_Id = req.R_Restaurant_Id,
                                    Remark = req.Remark,
                                    TableNum = req.TableNum,
                                    CustomerId = req.CustomerId,
                                    BillingUser = req.BillingUser,
                                    OrderType = req.OrderType,
                                    BillDepartMent=req.BillDepartMent
                                }, x => x.Id == req.Id);//更新预订单信息

                            recordRemark = string.Format("修改预订 - 预订订单号({0})更新预订信息", req.OrderNo);
                        }
                        else
                        {
                            var insert = db.Insert<R_Order>(model);  //订单主表
                            orderId = Convert.ToInt32(insert);  //订单操作纪录
                            recordRemark = string.Format("新加预订 - 新增预订信息订单号({0})", model.OrderNo);

                            if (req.DepositAmount > 0)
                            {
                                db.Insert(new R_OrderPayRecord()
                                {
                                    BillDate = req.AccountingDate,
                                    CreateDate = DateTime.Now,
                                    CreateUser = req.CreateUser,
                                    CyddJzStatus = CyddJzStatus.已付,
                                    CyddJzType = CyddJzType.定金,
                                    CyddPayType = (int)CyddPayType.现金,
                                    PayAmount = req.DepositAmount,
                                    R_Order_Id = orderId,
                                    R_Market_Id = req.CurrentMarketId,//当前用户登录的分市Id
                                    R_OrderMainPay_Id = 0,//预订定金不生成当次主结Id
                                    Remark = "预订定金",
                                    R_Restaurant_Id = req.CurrentRestaurantId
                                });
                            }
                        }

                        string tableNames = string.Empty;

                        int personNumAvg = tableIds.Count() > 1 ?
                            req.PersonNum / tableIds.Count() : req.PersonNum;    //台号人均
                        int personNumRemainder = tableIds.Count() > 1 ?
                            req.PersonNum % tableIds.Count() : 0;  //台号余人
                        int eachRemainder = 0;
                        if (tableIds != null && tableIds.Count > 0)
                        {
                            List<R_OrderTable> otList = new List<R_OrderTable>();
                            foreach (var tId in tableIds)
                            {
                                eachRemainder++;
                                R_OrderTable otModel = new R_OrderTable();

                                otModel.R_Order_Id = orderId;
                                otModel.R_Table_Id = tId;
                                otModel.CreateDate = DateTime.Now;
                                otModel.PersonNum = personNumAvg + (personNumRemainder - eachRemainder >= 0 ? 1 : 0);
                                otModel.BillDate = req.AccountingDate;
                                otModel.R_Market_Id = req.CurrentMarketId;
                                otModel.R_OrderMainPay_Id = 0;
                                otList.Add(otModel);
                            }
                            db.InsertRange<R_OrderTable>(otList);//订单台号

                            req.OrderTableIds = db.Queryable<R_OrderTable>()
                                .Where(x => x.R_Order_Id == orderId).Select(x => x.Id).ToList();

                            tableNames = db.Queryable<R_Table>()
                                .Where(x => tableIds.Contains(x.Id)).Select(x => x.Name).ToList().Join(",");
                        }

                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateDate = DateTime.Now,
                            R_Order_Id = orderId,
                            CreateUser = req.CreateUser,
                            CyddCzjlStatus = CyddStatus.预定,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            Remark = recordRemark + (tableNames.IsEmpty() ? "" : string.Format("；预订台：（{0}）", tableNames)),
                        });

                        res = req;
                        res.Id = orderId;
                        db.CommitTran();
                    }
                    else
                    {
                        res = null;
                        messge = "订单台号在预定日期已被占用，请重新选择";
                        db.RollbackTran();
                    }
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



        public ForecastInfoDTO ForecastSearch(ForecastSearchDTO req)
        {
            int days = (req.EndDate - req.BeginDate).Days + 1;

            ForecastInfoDTO infoObj = new ForecastInfoDTO();
            //分市
            var marketList = _marketRep.GetList(req.Restaurant);
            int marketCount = marketList.Count;

            infoObj.MarketList = marketList;
            
            List<ForecastDateDTO> dateList = new List<ForecastDateDTO>();
            for (int i = 0; i < days; i++)
            {
                ForecastDateDTO dateDTO = new ForecastDateDTO();
                dateDTO.DayOfDate = req.BeginDate.AddDays(i);
                dateDTO.DayOfWeekName = System.Globalization.CultureInfo.CurrentCulture
                    .DateTimeFormat.GetDayName(req.BeginDate.AddDays(i).DayOfWeek);
                dateDTO.TitleDate = req.BeginDate.AddDays(i).ToString("MM-dd");
                
                dateList.Add(dateDTO);
            }
            infoObj.DateList = dateList;
            
            List<ForecastTableDTO> tableList = new List<ForecastTableDTO>();

            //台号
            var tables = _tableRep.GetTables(new int[] { req.Restaurant }, null, 0);
            //台号预定明细
            var reserveInfoList = _orderRep.GetForecastList(req);

            foreach (var item in tables)
            {
                ForecastTableDTO tableDTO = new ForecastTableDTO();
                tableDTO.TableId = item.Id;
                tableDTO.TableName = item.Name;
                tableDTO.RestaurantId = item.R_Restaurant_Id;
                var contentList = reserveInfoList.Where(x => x.TableId == item.Id).ToList();
                List<ForecastReserveInfoDTO> records = new List<ForecastReserveInfoDTO>();
                foreach (var date in dateList)
                {
                    var filteByDateList = contentList.Where(x => x.BookingDate == date.DayOfDate).ToList();
                    foreach (var market in marketList)
                    {
                        ForecastReserveInfoDTO info = new ForecastReserveInfoDTO();
                        if (filteByDateList != null && filteByDateList.Count > 0)
                        {
                            var obj = filteByDateList.Where(x => x.BookingDate == date.DayOfDate 
                                && x.MarketId == market.Id 
                                && x.TableId == item.Id).FirstOrDefault();
                            if (obj != null)
                                info = obj;
                        }
                        records.Add(info);
                    }
                }
                tableDTO.BookingList = records;
                tableList.Add(tableDTO);
            }
            infoObj.TableList = tableList;

            return infoObj;
        }

        public bool RefundDepositHandler(RefundDepositDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                db.BeginTran();
                try
                {
                    string msg = null;
                    bool result = false;
                    var depositRecord = _payRep.GetModelByPayId(req.OrigianlDepositId);

                    if (depositRecord == null || depositRecord.Id <= 0)
                    {
                        msg = "无法找到此定金支付记录，重新确认！";
                    }
                    else if (depositRecord.CyddJzType != CyddJzType.定金)
                    {
                        msg = "非定金支付记录不能操作退回，重新确认！";
                    }
                    else if (depositRecord.CyddJzStatus != CyddJzStatus.已付)
                    {
                        msg = "当前定金记录已操作过转结或退款操作，请重新确认！";
                    }
                    else if (depositRecord.PayAmount <= 0)
                    {
                        msg = "未发现可退定金金额！";
                    }
                    else
                    {
                        OrderPayHistoryDTO refundObj = new OrderPayHistoryDTO();
                        var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();
                        if (dateItem != null)
                        {
                            var billDateNow = Convert.ToDateTime(dateItem.ItemValue);
                            if (billDateNow > DateTime.Today)
                            {
                                msg = "账务日期不能超过当前日期！";
                            }
                            else
                            {
                                R_OrderPayRecord model = new R_OrderPayRecord();
                                model.CreateDate = DateTime.Now;
                                model.CreateUser = req.CurrentUserId;
                                model.R_Order_Id = depositRecord.R_Order_Id;
                                model.CyddJzStatus = CyddJzStatus.已退;
                                model.CyddJzType = CyddJzType.定金;
                                model.CyddPayType = depositRecord.CyddPayType;
                                model.PayAmount = -depositRecord.PayAmount;
                                model.R_Market_Id = req.CurrentMarketId;
                                model.BillDate = billDateNow;
                                model.Remark = "退回定金";
                                model.PId = req.OrigianlDepositId;
                                model.R_Restaurant_Id = req.RestaurantId;
                                var orderRecord = new R_OrderRecord()
                                {
                                    CreateUser = req.CurrentUserId,
                                    CreateDate = DateTime.Now,
                                    CyddCzjlStatus = CyddStatus.结账,
                                    CyddCzjlUserType = CyddCzjlUserType.员工,
                                    R_Order_Id = depositRecord.R_Order_Id,
                                    Remark = "退定金操作，原定金支付记录Id：" + depositRecord.Id 
                                            + "，退回金额（" + depositRecord.PayAmount.ToString("f2") + "）",
                                };
                                db.Insert(orderRecord);//记录退定金操作

                                //更新原定金记录状态为已结
                                db.Update<R_OrderPayRecord>(new { CyddJzStatus = CyddJzStatus.已结 }, x => x.Id == depositRecord.Id);

                                result = db.Insert<R_OrderPayRecord>(model) == null ? false : true;

                                db.CommitTran();
                            }
                        }
                        else
                        {
                            msg = "餐饮账务日期尚未初始化，请联系管理员！";
                        }
                    }

                    if (msg != null)
                        throw new Exception(msg);

                    return result;
                }
                catch (Exception e)
                {
                    db.RollbackTran();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 创建订单发票
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateOrderInvoice(InvoiceCreateDTO req)
        {
            bool res = false;
            try
            {
                var restaurant = _resRep.GetModel(req.RestaurantId);
                var dateItem = _extendItemRepository.GetModelList(restaurant.R_Company_Id, 10003).FirstOrDefault();
                req.BillDate = Convert.ToDateTime(dateItem.ItemValue);
                res = _orderRep.CreateOrderInvoice(req);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }

        public InvoiceCreateDTO GetInvoice(int id)
        {
            return _orderRep.GetInvoice(id);
        }
    }
}
