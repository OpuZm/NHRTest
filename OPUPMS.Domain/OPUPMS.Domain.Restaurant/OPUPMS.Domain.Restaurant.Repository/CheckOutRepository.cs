using System.Collections.Generic;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using System;
using System.Data.SqlClient;
using OPUPMS.Domain.Base.Repositories;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class CheckOutRepository : SqlSugarService, ICheckOutRepository
    {
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        public CheckOutRepository(IExtendItemRepository extendItemRepository)
        {
            _extendItemRepository = extendItemRepository;
        }
        /// <summary>
        /// 根据订单台号id集合获得订单明细集合
        /// </summary>
        /// <param name="tableOrderIds">订单台号id集合</param>
        /// <returns>订单明细集合</returns>
        public List<CheckOutOrderDetailDTO> GetOrderDetailListBy(List<int> tableOrderIds)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<CheckOutOrderDetailDTO> orderDetailDTOList = db.Queryable<R_OrderDetail>()
                    .Where<R_OrderDetail>((s1) => tableOrderIds.Contains(s1.R_OrderTable_Id))
                    .Select<CheckOutOrderDetailDTO>("*")
                    .ToList();
                return orderDetailDTOList;
            }
        }

        /// <summary>
        /// 根据订单明细id集合获取拓展集合
        /// </summary>
        /// <param name="orderDetailIdList">订单明细id集合</param>
        /// <returns>拓展集合</returns>
        public List<OrderDetailExtendDTO> GetExtendListBy(List<int> orderDetailIdList)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<OrderDetailExtendDTO> extendList = db.Queryable<R_OrderDetailExtend>()
                    .JoinTable<R_ProjectExtend>((s1, s2) => s1.R_ProjectExtend_Id == s2.Id)
                    .Where<R_OrderDetailExtend>((s1) => orderDetailIdList.Contains(s1.R_OrderDetail_Id))
                    .Select<R_ProjectExtend, OrderDetailExtendDTO>(
                        (s1, s2) =>
                        new OrderDetailExtendDTO
                        {
                            ExtendType = s2.CyxmKzType,
                            Id = s1.Id,
                            Name = s1.Name,
                            Price = s1.Price,
                            R_OrderDetail_Id = s1.R_OrderDetail_Id,
                            R_ProjectExtend_Id = s1.R_ProjectExtend_Id,
                            Unit = s1.Unit
                        }).ToList();
                return extendList;
            }
        }

        /// <summary>
        /// 根据订单明细id集合获取操作记录集合
        /// </summary>
        /// <param name="orderDetailIdList">订单明细id集合</param>
        /// <returns>操作记录集合</returns>
        public List<R_OrderDetailRecord> GetRecordListBy(List<int> orderDetailIdList)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var recordList = db.Queryable<R_OrderDetailRecord>()
                    .Where(s1 => orderDetailIdList.Contains(s1.R_OrderDetail_Id))
                    .Select("*")
                    .ToList();
                return recordList;
            }
        }

        /// <summary>
        /// 根据订单明细id集合获取订单套餐明细集合
        /// </summary>
        /// <param name="orderDetailIdList">订单明细id集合</param>
        /// <returns>订单套餐明细集合</returns>
        public List<R_OrderDetailPackageDetail> GetPackageDetailListBy(List<int> orderDetailIdList)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var packageDetailList = db.Queryable<R_OrderDetailPackageDetail>()
                    .Where(s1 => orderDetailIdList.Contains(s1.Id))
                    .Select("*")
                    .ToList();
                return packageDetailList;
            }
        }

        /// <summary>
        /// 根据餐饮项目id集合获取餐饮项目集合
        /// </summary>
        /// <param name="projectIdList">餐饮项目id集合</param>
        /// <returns>餐饮项目集合</returns>
        public List<ProjectJoinDetailDTO> GetProjectListBy(List<int> projectIdList)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ProjectJoinDetailDTO> list = db.Queryable<R_Project>()
                    .JoinTable<R_ProjectDetail>((s1, s2) => s1.Id == s2.R_Project_Id)
                    .Where<R_ProjectDetail>((s1, s2) => projectIdList.Contains(s2.Id))
                    .Select<R_ProjectDetail, ProjectJoinDetailDTO>((s1, s2) =>
                        new ProjectJoinDetailDTO
                        {
                            Id = s2.Id,
                            ProjectName = s1.Name,
                            CategoryId = s1.R_Category_Id,
                            CostPrice = s2.CostPrice,
                            Description = s2.Description,
                            Price = s2.Price,
                            Property = s1.Property,
                            R_Project_Id = s2.R_Project_Id,
                            Unit = s2.Unit,
                            UnitRate = s2.UnitRate
                        }).ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据餐饮套餐id集合获取餐饮套餐集合
        /// </summary>
        /// <param name="packageIdList">餐饮套餐id集合</param>
        /// <returns>餐饮套餐集合</returns>
        public List<R_Package> GetPackageListBy(List<int> packageIdList)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var packageList = db.Queryable<R_Package>()
                    .Where(s1 => packageIdList.Contains(s1.Id))
                    .Select("*").ToList();
                return packageList;
            }
        }

        /// <summary>
        /// 根据订单明细id修改单价
        /// </summary>
        /// <param name="orderDetailId">订单明细id</param>
        /// <param name="newPrice">新单价</param>
        /// <returns>是否修改成功</returns>
        public bool UpdatePriceById(int orderDetailId, decimal newPrice)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                R_OrderDetail orderDetail = GetOrderDetailById(orderDetailId);
                bool isModify = false;//是否允许改价

                if (orderDetail.CyddMxType == CyddMxType.餐饮项目)
                {
                    int property = GetProjectById(orderDetail.CyddMxId).Property;
                    isModify = (property & (int)CyxmProperty.是否可改价) > 0;
                }
                else if (orderDetail.CyddMxType == CyddMxType.餐饮套餐)
                {
                    int property = GetPackageById(orderDetail.CyddMxId).Property;
                    isModify = (property & (int)CytcProperty.是否可改价) > 0;
                }

                if (isModify)//菜品或套餐设置允许改价
                {
                    bool bo = db.Update<R_OrderDetail>(new
                    {
                        Price = newPrice
                    }, s1 => s1.Id == orderDetailId);

                    return bo;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 根据订单id获得订单台号列表
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>订单台号实体类集合</returns>
        public List<CheckOutOrderTableDTO> GetOrderTableListBy(int orderId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.Sqlable()
                    .From<R_OrderTable>("s1")
                    .Join<R_Table>("s2", "s1.R_Table_Id", "s2.Id", JoinType.Left)
                    .Where("s1.R_Order_Id=" + orderId + " AND s1.IsCheckout!=1 ")
                    .SelectToList<CheckOutOrderTableDTO>(@"s1.*, s2.Name, s2.SeatNum, 
                        s2.Describe, s2.CythStatus, s2.ServerRate, s2.R_Area_Id");
                return list;
            }
        }

        /// <summary>
        /// 根据订单id和桌号id集合查询订单台号集合
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="tableIdList">桌号id集合</param>
        /// <returns>订单台号集合</returns>
        public List<CheckOutOrderTableDTO> GetOrderTableListBy(int orderId, List<int> tableIdList,OrderTableStatus oStatus)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                //var list = db.Queryable<R_OrderTable>()
                //    .JoinTable<R_Table>((s1, s2) => s1.R_Table_Id == s2.Id)
                //    .Where<R_Table>((s1,s2) => tableIdList.Contains(s2.Id) && s1.R_Order_Id == orderId && s1.IsCheckOut==false)
                //    .Select<CheckOutOrderTableDTO>(@"s1.*,s2.Name, s2.SeatNum, 
                //        s2.Describe, s2.CythStatus, ISNULL(s2.ServerRate, 0) AS ServerRate, s2.R_Area_Id")
                //    .ToList();
                var list = db.Queryable<R_OrderTable>()
                    .JoinTable<R_Table>((s1, s2) => s1.R_Table_Id == s2.Id);
                //list.Where<R_Table>((s1, s2) => tableIdList.Contains(s2.Id) && s1.R_Order_Id == orderId);
                list.Where<R_Table>((s1, s2) => s1.R_Order_Id == orderId);
                if (tableIdList.Any(p=>p!=0))
                {
                    list.Where<R_Table>((s1, s2) => tableIdList.Contains(s2.Id));
                }
                switch (oStatus)
                {
                    case OrderTableStatus.所有:
                        break;
                    case OrderTableStatus.未结:
                        list.Where<R_Table>((s1, s2) => s1.IsCheckOut == false && s1.IsOpen==true);
                        break;
                    case OrderTableStatus.已结:
                        list.Where<R_Table>((s1, s2) => s1.IsCheckOut == true && s1.R_OrderMainPay_Id!=0);
                        break;
                    default:
                        break;
                }
                var res=list.Select<CheckOutOrderTableDTO>(@"s1.*,isnull(s2.Name,'无台号') as Name, s2.SeatNum, 
                    s2.Describe, s2.CythStatus, ISNULL(s2.ServerRate, 0) AS ServerRate, s2.R_Area_Id")
                .ToList();
                return res;
            }
        }

        /// <summary>
        ///  根据订单明细id查询单条订单详细
        /// </summary>
        /// <param name="orderDetailId">订单明细id</param>
        /// <returns>订单明细实体类</returns>
        public R_OrderDetail GetOrderDetailById(int orderDetailId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                return db.Queryable<R_OrderDetail>()
                    .FirstOrDefault(s => s.Id == orderDetailId);
            }
        }

        /// <summary>
        /// 根据餐饮项目id查询单条餐饮项目
        /// </summary>
        /// <param name="projectId">餐饮项目id</param>
        /// <returns>餐饮项目实体类</returns>
        public R_Project GetProjectById(int projectId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                return db.Queryable<R_Project>()
                    .FirstOrDefault(s => s.Id == projectId);
            }
        }

        /// <summary>
        /// 根据餐饮套餐id查询单条餐饮套餐
        /// </summary>
        /// <param name="packageId">餐饮套餐id</param>
        /// <returns>餐饮套餐实体类</returns>
        public R_Package GetPackageById(int packageId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                return db.Queryable<R_Package>()
                    .FirstOrDefault(s => s.Id == packageId);
            }
        }

        /// <summary>
        /// 根据订单明细查询
        /// </summary>
        /// <param name="orderDetailId">订单明细id</param>
        /// <returns>订单明细</returns>
        public List<R_OrderDetailPackageDetail> GetOrderDetailPackageDetailBy(int orderDetailId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var packageDetailList = db.Queryable<R_OrderDetailPackageDetail>()
                    .Where(s1 => s1.R_OrderDetail_Id == orderDetailId)
                    .Select("*")
                    .ToList();
                return packageDetailList;
            }
        }

        /// <summary>
        /// 根据订单id获得订单实体
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>订单实体类</returns>
        public CheckOutOrderDTO GetOrderById(int orderId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var data = db.Sqlable()
                    .From<R_Order>("s1")
                    .Where("s1.Id=" + orderId)
                    .SelectToList<CheckOutOrderDTO>("s1.*, OrderTypeName = ISNULL((SELECT Name FROM dbo.ExtendItems WHERE Id = s1.OrderType), ''),RestaurantName=(select name from r_restaurant where id=s1.R_Restaurant_Id),MarketName=(select name from r_market where id=s1.R_Market_Id)")
                    .FirstOrDefault();

                return data;
            }
        }

        public OrderMainPayDTO GetPreOrderMainPay(int orderId)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                OrderMainPayDTO res = null;
                var data = db.Queryable<R_ReOrderMainPay>()
                    .SingleOrDefault(p => p.R_Order_Id == orderId);
                if (data!=null)
                {
                    res = new OrderMainPayDTO()
                    {
                        Id=data.Id,
                        BillDate=data.BillDate,
                        DiscountType=(DiscountMethods)Enum.Parse(typeof(DiscountMethods), data.DiscountType.ToString()),
                        DiscountId=data.R_Discount_Id,
                        DiscountRate=data.DiscountRate,
                        OrderId=data.R_Order_Id,
                        MarketId=data.R_Market_Id
                    };
                }
                return res;
            }
        }

        public CheckOutResultDTO ReverseOrder(ReverseOrderDTO req)
        {
            using (var db= new SqlSugarClient(Connection))
            {
                CheckOutResultDTO res = null;
                try
                {
                    var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();

                    if (dateItem == null)
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");
                    DateTime accDate = DateTime.Today;
                    if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                        throw new Exception("餐饮账务日期设置错误，请联系管理员");
                    var tables = db.Queryable<R_Table>().JoinTable<R_OrderTable>
                        ((s1, s2) => s1.Id == s2.R_Table_Id).Where<R_OrderTable>((s1, s2) =>
                             s2.R_OrderMainPay_Id == req.MainPayId).Select("s1.*").ToList();
                    var tableIds = tables.Select(p => p.Id).ToArray();
                    var tableOrderIngs = tables.Where(p => p.CythStatus == CythStatus.在用).ToList();
                    if (tableOrderIngs.Any())
                    {
                        var tableNames = string.Join(",", tableOrderIngs.Select(p => p.Name));
                        throw new Exception(string.Format("({0}) 正在开台状态,请结账后再执行该账单的反结操作",tableNames));
                    }

                    var reverTableNames = string.Join(",", tables.Select(p => p.Name));
                    var mainPayModel = db.Queryable<R_OrderMainPay>().First(p => p.Id == req.MainPayId);
                    if (mainPayModel.BillDate.Date!=accDate.Date)
                    {
                        throw new Exception("该账单账务日期和系统当前账务日期不一致,不允许反结");
                    }
                    var orderModel = db.Queryable<R_Order>().First(p => p.Id == mainPayModel.R_Order_Id);
                    var orderTables = db.Queryable<R_OrderTable>()
                        .Where(p => p.R_OrderMainPay_Id == mainPayModel.Id).ToList();
                    var orderTableIds = orderTables.Select(p => p.Id).ToArray();
                    var orderDetails = db.Queryable<R_OrderDetail>()
                        .Where(p => orderTableIds.Contains(p.R_OrderTable_Id)).ToList();
                    var orderPayRecords = db.Queryable<R_OrderPayRecord>()
                        .Where(p => p.R_OrderMainPay_Id == mainPayModel.Id).ToList();
                    if (orderPayRecords.Any(p=>p.CyddJzStatus==CyddJzStatus.已结 && p.CyddJzType!=CyddJzType.定金))
                    {
                        throw new Exception(string.Format("该主结单已经做过反结操作，不能重复反结"));
                    }
                    db.BeginTran();
                    //db.Delete<R_OrderPayRecord>(p => p.R_OrderMainPay_Id == mainPayModel.Id);

                    //db.Delete<R_OrderMainPay>(p => p.Id == mainPayModel.Id);
                    //db.Update<R_OrderPayRecord>(new { CyddJzStatus = (int)CyddJzStatus.已付 }, p => p.R_Order_Id == orderModel.Id);
                    //db.Delete<R_OrderPayRecord>(p=>p.R_Order_Id == orderModel.Id && p.PayAmount<=0 && p.CyddJzType==CyddJzType.定金);

                    var depositReverAll = db.Queryable<R_OrderPayRecord>().Where(p => p.R_Order_Id == orderModel.Id && p.R_OrderMainPay_Id == 0
                      && p.CyddJzType == CyddJzType.定金).ToList();
                    var depositRePids = depositReverAll.Where(p => p.CyddJzStatus == CyddJzStatus.已退).Select(p => p.PId).ToList();
                    var depositReverIds = depositReverAll.Where(p => !depositRePids.Contains(p.Id) && p.CyddJzStatus == CyddJzStatus.已结)
                        .Select(p=>p.Id).ToList();
                    db.Update<R_OrderPayRecord>(new { CyddJzStatus = (int)CyddJzStatus.已付 }, p => depositReverIds.Contains(p.Id));
                    db.Update<R_OrderPayRecord>(new { CyddJzStatus = (int)CyddJzStatus.已结 }, p => p.R_OrderMainPay_Id == mainPayModel.Id);
                    db.Delete<R_OrderPayRecord>(p => p.R_Order_Id == orderModel.Id && p.CyddJzType == CyddJzType.定金
                    && p.CyddJzStatus == CyddJzStatus.已结 && p.R_OrderMainPay_Id > 0);
                    db.Update<R_OrderDetail>(new { PayableTotalPrice = 0, DiscountRate=1 }, p => orderTableIds.Contains(p.R_OrderTable_Id));
                    db.Update<R_OrderTable>(new { IsCheckOut = 0, IsOpen = 1, R_OrderMainPay_Id=0 }, p => orderTableIds.Contains(p.Id));
                    db.Update<R_Order>(new { RealAmount = 0, CyddStatus = (int)CyddStatus.反结, OriginalAmount=0, ConAmount=0, DiscountRate=0, DiscountAmount=0, GiveAmount=0, ClearAmount=0 }, p => p.Id == orderModel.Id);
                    db.Update<R_Table>(new { CythStatus=(int)CythStatus.在用 }, p => tableIds.Contains(p.Id));
                    db.Insert<R_OrderRecord>(new R_OrderRecord()
                    {
                        CreateDate=DateTime.Now,
                        CreateUser=req.UserId,
                        CyddCzjlStatus=CyddStatus.反结,
                        CyddCzjlUserType=CyddCzjlUserType.员工,
                        Remark = string.Format("执行了反结操作,台号:({0})", reverTableNames),
                        R_OrderTable_Id =0,
                        R_Order_Id=orderModel.Id
                    });

                    List<R_OrderPayRecord> reverseRecords = new List<R_OrderPayRecord>();
                    #region 反写应收账
                    foreach (var item in orderPayRecords)
                    {
                        if (item.CyddPayType == (int)CyddPayType.挂账 || item.CyddPayType == (int)CyddPayType.转客房
                        || item.CyddPayType == (int)CyddPayType.会员卡)
                        {
                            if (item.CyddPayType == (int)CyddPayType.会员卡)
                            {
                                if (EnabelGroupFlag)
                                {
                                    try
                                    {
                                        var dto= AutoMapper.Mapper.Map<OrderPayRecordDTO>(item);
                                        dto.Remark = string.Format("{0} {1}", "反结会员卡", item.Remark);
                                        List<OrderPayRecordDTO> dtoList = new List<OrderPayRecordDTO>
                                        {
                                            dto
                                        };
                                        SaveMemberConsumeInfo(dtoList, req.UserCode, false,orderModel.R_Restaurant_Id);
                                        //ApplyChangeMemberToDb(item.SourceId, item.SourceName, req.UserCode,
                                        //    -item.PayAmount, string.Format("{0} {1}", "反结会员卡", item.Remark), false, new SqlSugarClient(ConnentionGroup));
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("反结集团库记录会员卡消费信息操作失败：" + ex.Message);
                                    }
                                    
                                }
                                else
                                {
                                    try
                                    {
                                        ApplyChangeMemberToDb(item.SourceId, item.SourceName, req.UserCode,
                                            -item.PayAmount, string.Format("{0} {1}", "反结会员卡", item.Remark), false, db, orderModel.R_Restaurant_Id);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("反结本地库记录会员卡消费信息操作失败：" + ex.Message);
                                    }
                                }
                                //if (!EnabelGroupFlag)
                                //{
                                //    //若已启用集团会员库，里面则不再执行，本地库会员消费记录已在插入集团库时一并插入到本地库
                                //}
                            }
                            else if (item.CyddPayType == (int)CyddPayType.挂账)
                            {
                                var verifyInfo = new VerifySourceInfoDTO();
                                verifyInfo.SourceId = item.SourceId;
                                verifyInfo.SourceName = item.SourceName;
                                verifyInfo.RestaruantId = orderModel.R_Restaurant_Id;
                                verifyInfo.PayMethod = (int)CyddPayType.挂账;
                                verifyInfo.OperateValue = item.PayAmount;
                                string remark = string.Format("反结挂账客户【{0}】- 代码：({1})", item.SourceName, item.SourceId);
                                List<string> resultList = SearchVerifyOutsideInfo(verifyInfo, db);
                                try
                                {
                                    var paras = SqlSugarTool.GetParameters(new
                                    {
                                        xh = orderModel.Id, //餐饮单序号
                                        dh = orderModel.R_Restaurant_Id + "." + orderModel.Id, //餐厅代码+'.'+餐饮单单号
                                        lx = resultList[0].Trim(), //协议单位代码(lxdmdm00)
                                        je = -item.PayAmount,//金额
                                        cz = req.UserCode, //操作员代码
                                        ctmc = orderModel.R_Restaurant_Id, //餐厅名称
                                        fsmc = "", //分市名称
                                        th = orderModel.Id,
                                        rs = orderModel.PersonNum,
                                        bz = remark,
                                        mz = "",
                                        atr = 0
                                    });
                                    db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                                    db.ExecuteCommand("p_po_toys_newCY", paras);
                                    db.CommandType = System.Data.CommandType.Text;//还原回默认
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("反结挂账操作失败：" + ex.Message);
                                }
                            }
                            else if (item.CyddPayType == (int)CyddPayType.转客房)
                            {
                                #region 转客房处理

                                var verifyInfo = new VerifySourceInfoDTO();
                                verifyInfo.SourceId = item.SourceId;
                                verifyInfo.SourceName = item.SourceName;
                                verifyInfo.RestaruantId = orderModel.R_Restaurant_Id;
                                verifyInfo.PayMethod = (int)CyddPayType.转客房;
                                verifyInfo.OperateValue = item.PayAmount;

                                List<string> resultList = SearchVerifyOutsideInfo(verifyInfo, db);
                                try
                                {
                                    var paras = SqlSugarTool.GetParameters(new
                                    {
                                        zh00 = Convert.ToInt32(resultList[1]), //客人账号(krzlzh00)
                                        zwdm = resultList[0], //账项代码
                                        hsje = -item.PayAmount,//金额
                                        ckhm = item.SourceName, //房号(krzlfh00)
                                        czdm = req.UserCode, //操作员代码
                                        xfje = 1,
                                        bz00 = "反结餐厅转客房",
                                        bc00 = "",
                                    });
                                    db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                                    db.ExecuteCommand("p_zw_addx", paras);
                                    db.CommandType = System.Data.CommandType.Text;//还原回默认
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("反结转客房操作失败：" + ex.Message);
                                }
                                #endregion
                            }
                        }

                        if (item.PayAmount != 0 && (item.CyddJzStatus != CyddJzStatus.已退 || item.CyddJzStatus != CyddJzStatus.已结) && item.CyddJzType!=CyddJzType.定金)
                        {
                            item.PayAmount = -item.PayAmount;
                            item.CreateDate = DateTime.Now;
                            item.BillDate = accDate;
                            item.R_Market_Id = req.CurrentMarketId;
                            item.CreateUser = req.UserId;
                            item.CyddJzStatus = CyddJzStatus.已结;
                            if (item.CyddJzType==CyddJzType.找零)
                            {
                                item.Remark = string.Format("反结找零纪录 {0}", item.SourceName);
                            }
                            else
                            {
                                item.Remark = string.Format("反结付款纪录 {0}", item.SourceName);
                            }
                            reverseRecords.Add(item);
                        }

                        //if (item.CyddJzType==CyddJzType.转结 && item.CyddJzStatus==CyddJzStatus.已结)
                        //{
                        //    reverseRecords.Add(new R_OrderPayRecord()
                        //    {
                        //        PayAmount = -item.PayAmount,
                        //        CreateDate = DateTime.Now,
                        //        BillDate = accDate,
                        //        R_Market_Id = req.CurrentMarketId,
                        //        CreateUser = req.UserId,
                        //        CyddJzStatus = CyddJzStatus.已付,
                        //        CyddJzType = CyddJzType.定金,
                        //        CyddPayType=item.CyddPayType,
                        //        SourceId=0,
                        //        R_OrderMainPay_Id= mainPayModel.Id,
                        //        Remark = string.Format("反结重置定金纪录")
                        //    });
                        //}
                    }
                    db.InsertRange<R_OrderPayRecord>(reverseRecords);
                    #endregion
                    db.CommitTran();
                    res = new CheckOutResultDTO()
                    {
                        OrderId=orderModel.Id,
                        OrderMainPayId=mainPayModel.Id,
                        OrderTables= orderTableIds.ToList(),
                        ReverTableNames=reverTableNames
                    };
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    throw ex;
                }
                return res;
            }
        }

        /// <summary>
        /// 保存会员卡消费记录
        /// </summary>
        /// <param name="listOrderPayRecordDTO"></param>
        /// <param name="userCode">用户代码</param>
        /// <param name="isNormal">true：正常保存，false ：回滚金额</param>
        private List<MemberInfoDTO> SaveMemberConsumeInfo(List<OrderPayRecordDTO> listOrderPayRecordDTO, string userCode, bool isNormal,int restaurantId)
        {
            List<MemberInfoDTO> res = new List<MemberInfoDTO>();
            if (EnabelGroupFlag)
            {
                using (var groupDB = new SqlSugarClient(ConnentionGroup))
                {
                    try
                    {
                        //var payRecords = listOrderPayRecordDTO.Where(x => x.Id <= 0 && x.CyddPayType == (int)CyddPayType.会员卡).ToList();
                        var payRecords = listOrderPayRecordDTO.Where(x => x.CyddPayType == (int)CyddPayType.会员卡).ToList();
                        if (payRecords != null && payRecords.Count > 0)
                        {
                            groupDB.BeginTran();
                            foreach (var item in payRecords)
                            {
                                //if (item.Id > 0)
                                //    continue;

                                if (item.CyddPayType == (int)CyddPayType.会员卡)
                                {
                                    res.Add(ApplyChangeMemberToDb(item.SourceId, item.SourceName, userCode, (isNormal ? item.PayAmount : -item.PayAmount), item.Remark, true, groupDB,restaurantId));
                                }
                            }
                            groupDB.CommitTran();
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg1 = isNormal ? "会员库记录会员卡消费信息操作失败" : "会员库回滚会员卡消费信息操作失败";
                        groupDB.RollbackTran();
                        throw new Exception(msg1 + "：" + ex.Message);
                    }
                }
            }
            return res;
        }

        public List<string> SearchVerifyOutsideInfo(VerifySourceInfoDTO verifySource, SqlSugarClient db)
        {
            bool isOutside = false;
            if (db == null)
            {
                db = new SqlSugarClient(Connection);
                isOutside = true;
            }

            List<string> infoList = new List<string>();
            if (verifySource.PayMethod == (int)CyddPayType.挂账)
            {
                if (verifySource.SourceId <= 0)
                    throw new Exception("请选择有效的挂账客户！");
                string lxdmSql = string.Format("SELECT lxdmdm00 AS Code, lxdmgzxe AS LimitAmount, lxdmye00 AS RemainAmount FROM dbo.lxdm WHERE Id = {0}", verifySource.SourceId);

                var info = db.SqlQuery<SearchLxdmInfo>(lxdmSql);

                if (info == null || info.Count == 0)
                    throw new Exception(string.Format("无法找到此客户（{0}）,请重新确认挂账客户！", verifySource.SourceName));

                infoList.Add(info[0].Code.Trim());
            }
            else if (verifySource.PayMethod == (int)CyddPayType.转客房)
            {
                #region 转客房验证
                if (string.IsNullOrEmpty(verifySource.SourceName))
                    throw new Exception("请输入房号！");

                string sqlRoom = string.Format("SELECT TOP 1 krzlfh00 " +
                                            "FROM krzl WHERE krzlzt00 = 'I' AND (krzlzh00 = '{0}' or (krzlkrlx='G' and krzlth00='{0}'))", verifySource.SourceId);

                var rooms = db.SqlQuery<string>(sqlRoom);
                if (rooms == null || rooms.Count == 0)
                    throw new Exception(string.Format("无此在住房号（{0}）,请重新确认转客房房号！", verifySource.SourceName));

                //取在住主结人
                string customerSql = string.Format("SELECT TOP 1 krzlzh00 AS CustomerId, ISNULL(krzlgzxe, 0) AS LastAmount, isnull(krzlye00, 0) as LimitAmount, " +
                    "(select sum(ysq0je00) from ysq0 where ysq0zh00=k.krzlzh00) as PreAmount FROM krzl k " +
                    "WHERE krzlzt00 = 'I' AND (krzlzh00 = '{0}' or (krzlkrlx='G' and krzlth00='{0}')) AND krzlzh00 = krzltzxh", verifySource.SourceId);
                var customers = db.SqlQuery<SearchKrzlInfo>(customerSql);

                if (customers == null || customers.Count == 0)
                    throw new Exception(string.Format("此房号（{0}）无在住客人！", verifySource.SourceName));

                if (customers[0].LastAmount != 0)
                {
                    if (customers[0].LastAmount < verifySource.OperateValue)
                        throw new Exception(string.Format("此客户（{0}）可挂账限额（{1}）小于当前输入金额无法挂账, 请重新确认！", verifySource.SourceName, customers[0].LastAmount));
                }

                //if (customers[0].LimitAmount != 0)
                //{
                //    if (customers[0].LimitAmount + verifySource.OperateValue>0)
                //        throw new Exception(string.Format("此客户（{0}）可挂账余额（{1}）小于当前输入金额无法挂账, 请重新确认！", verifySource.SourceName, customers[0].LimitAmount));
                //}

                var codes = db.SqlQuery<string>("select top 1 cyctzwdm from cyct where id=" + verifySource.RestaruantId);
                if (codes == null || codes.Count == 0)
                    throw new Exception("当前餐厅未设置账务代码！");

                var hasCode = db.SqlQuery<string>("select top 1 zwdmdm00 from zwdm where zwdmdm00='" + codes[0] + "'");
                if (hasCode == null || hasCode.Count == 0)
                    throw new Exception("当前餐厅账务代码设置不正确，请重新确认！");

                infoList.Add(codes[0]);
                infoList.Add(customers[0].CustomerId.ToString());
                #endregion
            }
            else if (verifySource.PayMethod == (int)CyddPayType.会员卡)
            {
                var member = VerifyMemberInfo(verifySource.SourceId, verifySource.OperateValue, verifySource.SourceName);
                if (member != null && member.Id > 0)
                    infoList.Add(member.Id.ToString());
            }

            if (isOutside)
                db.Dispose();

            return infoList;
        }

        private MemberInfoDTO VerifyMemberInfo(int memberId, decimal amount, string memberPwd, SqlSugarClient db = null)
        {
            string connString = Connection;
            if (EnabelGroupFlag)
                connString = ConnentionGroup;

            bool isOutside = false;
            if (db == null)
            {
                db = new SqlSugarClient(connString);
                isOutside = true;
            }

            MemberInfoDTO member = null;
            try
            {
                //搜索会员信息
                string memberSql = string.Format(@"
                    SELECT krlsxh00 AS Id, krlsvpkh AS MemberCardNo, krlsmm00 AS MemberPwbByte, krlsye00 AS CardBalance,
                            krlszjhm AS MemberIdentityNo, krlsmm00 AS MemberPwbByte, krlsdh00 AS MemberPhoneNo,
                            LTRIM(RTRIM(krlszwxm)) AS MemberName, krlsGPID AS MemberGPID, krlsGUID AS MemberGUID, krlsxb00 AS MemberGender, krlsdh00 as MemberPhoneNo
                    FROM krls WHERE krlsxh00 = {0}", memberId);

                var members = db.SqlQuery<MemberInfoDTO>(memberSql);
                if (members == null || members.Count == 0)
                    throw new Exception("客户信息无效！");

                member = members[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (isOutside)
                db.Dispose();

            return member;
        }

        private MemberInfoDTO ApplyChangeMemberToDb(int memberId, string pwd, string userCode, decimal amount, string remark,
    bool isGroup, SqlSugarClient db,int restaurantId)
        {
            SqlSugarClient localDb = null;
            if (isGroup)//是否集团库连接
                localDb = new SqlSugarClient(Connection);

            string xtdmSql = "SELECT xtdmbz00 FROM xtdm WHERE xtdmlx00='YKBZ' AND xtdmdm00='YKBZ' AND xtdmbzs0='Y'";

            List<string> list = null; //始终查本地库的会员卡配置的系统代码标识
            if (isGroup)
                list = localDb.SqlQuery<string>(xtdmSql);
            else
                list = db.SqlQuery<string>(xtdmSql);

            if (list == null || list.Count == 0)
            {
                if (isGroup && localDb != null)
                    localDb.Dispose();
                throw new Exception("会员卡消费挂账关联的系统代码配置不正确，请联系管理员！");
            }

            SqlParameter[] paras;

            var member = VerifyMemberInfo(memberId, amount, pwd, db);

            Guid guid = Guid.NewGuid();
            paras = SqlSugarTool.GetParameters(new
            {
                Zs = member.Id, //客人历史序号Id
                KH = member.Id, //客人历史序号Id
                Lx = "B",
                dd = restaurantId.ToString(),
                bz = remark,//备注
                je = amount, //金额
                cz = userCode, //操作员代码
                fs = "01", //
                GPID = list[0], //协议单位代码(lxdmdm00)
                GUID = guid
            });
            db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
            db.ExecuteCommand("p_zw_gbxf", paras);
            db.CommandType = System.Data.CommandType.Text;//还原回默认

            if (isGroup)
            {
                string krlsSql = string.Format("SELECT krlsxh00 FROM krls WHERE krlsGUID = '{0}'", member.MemberGUID);
                var memberList = localDb.SqlQuery<int>(krlsSql);//查本地库的会员
                if (memberList == null || memberList.Count == 0)
                {
                    string insertSql = string.Format(
                        "INSERT krls(krlszwxm, krlszt00, krlsvpkh, krlsxb00,  krlsGPID, krlsGUID, krlsdh00, krlszjhm) " +
                        "VALUES('{0}', 'Y', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}') ",
                        member.MemberName, member.MemberCardNo, member.MemberGender,
                        member.MemberGPID, member.MemberGUID, member.MemberPhoneNo, member.MemberIdentityNo);

                    localDb.CommandType = System.Data.CommandType.Text;
                    localDb.ExecuteCommand(insertSql, paras);
                    memberList = localDb.SqlQuery<int>(krlsSql);//查本地库的会员
                }

                paras = SqlSugarTool.GetParameters(new
                {
                    Zs = memberList[0], //本地库客人历史序号Id
                    KH = memberList[0], //本地库客人历史序号Id
                    Lx = "B",
                    dd = restaurantId.ToString(),
                    bz = remark,//备注
                    je = amount, //金额
                    cz = userCode, //操作员代码
                    fs = "01", //
                    GPID = list[0], //协议单位代码(lxdmdm00)
                    GUID = guid
                });
                localDb.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                localDb.ExecuteCommand("p_zw_gbxf", paras);
                localDb.CommandType = System.Data.CommandType.Text;//还原回默认
                localDb.Dispose();//销毁本地连接
            }
            return member;
        }
    }
}
