using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Security;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Services
{
    class CheckOutService : SqlSugarService, ICheckOutService
    {
        readonly ICheckOutRepository _checkOutRepository;
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        readonly IUserRepository_Old _oldUserRepository; //(Czdm) 用户表
        readonly IOrderRepository _orderRep;
        readonly IOrderPayRecordRepository _payRep;
        readonly IRestaurantRepository _resRep;
        readonly IDiscountRepository _discountRep;
        readonly ICategoryRepository _categoryRepository;

        public CheckOutService(ICheckOutRepository checkOutRepository, 
            IExtendItemRepository extendItemRepository,
            IUserRepository_Old oldUserRepository,
            IOrderRepository orderRepository,
            IOrderPayRecordRepository payRecordRepository,
            IRestaurantRepository restaurantRepository,
            IDiscountRepository discountRepository,
            ICategoryRepository categoryRepository)
        {
            _checkOutRepository = checkOutRepository;
            _extendItemRepository = extendItemRepository;
            _oldUserRepository = oldUserRepository;
            _orderRep = orderRepository;
            _payRep = payRecordRepository;
            _resRep = restaurantRepository;
            _discountRep = discountRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// 根据订单台号id集合查询订单明细
        /// </summary>
        /// <param name="orderTableIdList">订单台号id集合</param>
        /// <returns>订单明细集合</returns>
        public List<CheckOutOrderDetailDTO> GetOrderDetailByOrderTableId(List<int> orderTableIdList)
        {
            List<CheckOutOrderDetailDTO> orderDetailDTOList =
                _checkOutRepository.GetOrderDetailListBy(orderTableIdList);

            List<int> orderDetailIdList = new List<int>();
            List<int> packageIdList = new List<int>();
            List<int> projectDetailIdList = new List<int>();

            orderDetailIdList = orderDetailDTOList.Select(x => x.Id).ToList();
            packageIdList = orderDetailDTOList.Where(x => x.CyddMxType == CyddMxType.餐饮套餐).Select(x => x.CyddMxId).ToList();
            projectDetailIdList = orderDetailDTOList.Where(x => x.CyddMxType == CyddMxType.餐饮项目).Select(x => x.CyddMxId).ToList();
            
            //套餐的订单明细Id 列表
            var packageOrdDetailIds = orderDetailDTOList.Where(x => x.CyddMxType == CyddMxType.餐饮套餐).Select(x => x.Id).ToList();

            List<OrderDetailExtendDTO> extendList = _checkOutRepository.GetExtendListBy(orderDetailIdList);
            
            List<R_OrderDetailRecord> recordList = _checkOutRepository.GetRecordListBy(orderDetailIdList);
            List<ProjectJoinDetailDTO> projectDetailList = _checkOutRepository.GetProjectListBy(projectDetailIdList);
            List<R_Package> packageList = _checkOutRepository.GetPackageListBy(packageIdList);

            recordList = recordList.Where(x => x.IsCalculation).ToList();//过滤不纳入计算的信息
            foreach (var orderDetail in orderDetailDTOList)
            {
                foreach (var item in extendList)
                {
                    if (item.R_OrderDetail_Id == orderDetail.Id)
                    {
                        orderDetail.OrderDetailAllExtends.Add(item);

                        if (item.ExtendType == CyxmKzType.做法)
                            orderDetail.ProExtend.Add(item);
                        else if (item.ExtendType == CyxmKzType.要求)
                            orderDetail.ProExtendRequire.Add(item);
                        else if (item.ExtendType == CyxmKzType.配菜)
                            orderDetail.ProExtendExtra.Add(item);
                    }
                }

                foreach (var item in recordList)
                {
                    if (item.R_OrderDetail_Id == orderDetail.Id)
                    {
                        orderDetail.R_OrderDetailRecord_List.Add(item);
                    }
                }

                orderDetail.ProjectName = orderDetail.CyddMxName;
                orderDetail.Unit = orderDetail.Unit ?? ""; 
                if (orderDetail.CyddMxType == CyddMxType.餐饮项目)
                {
                    var detailItem = projectDetailList.Where(x => x.Id == orderDetail.CyddMxId).SingleOrDefault();
                    if (detailItem == null)
                        continue;
                    
                    orderDetail.IsDiscount = (detailItem.Property & (int)CyxmProperty.是否可打折) > 0 ? 1 : 0;
                    orderDetail.ProjectName = detailItem.ProjectName;
                    orderDetail.CategoryId = detailItem.CategoryId;
                    orderDetail.IsForceDiscount = (detailItem.Property & (int)CyxmProperty.是否强制打折) > 0 ? 1 : 0;
                    orderDetail.IsServiceCharge = (detailItem.Property & (int)CyxmProperty.是否收取服务费) > 0 ? 1 : 0;

                }
                else if (orderDetail.CyddMxType == CyddMxType.餐饮套餐)
                {
                    var packageItem = packageList.Where(x => x.Id == orderDetail.CyddMxId).SingleOrDefault();
                    if (packageItem == null)
                        continue;
                    
                    orderDetail.IsDiscount = (packageItem.Property & (int)CytcProperty.是否可打折) > 0 ? 1 : 0;
                    orderDetail.ProjectName = packageItem.Name;
                    orderDetail.CategoryId = packageItem.R_Category_Id;
                }
            }

            return orderDetailDTOList;
        }


        /// <summary>
        /// 根据订单ID和该订单下的台号ID集合获取CheckOutOrderDTO
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tableIdList"></param>
        /// <returns></returns>
        public CheckOutOrderDTO GetCheckOutOrderDTO(int orderId, List<int> tableIdList, OrderTableStatus oStatus = OrderTableStatus.所有)
        {
            //查询订单实体
            CheckOutOrderDTO checkoutOrder = _checkOutRepository.GetOrderById(orderId);

            //查询订单台号集合
            List<CheckOutOrderTableDTO> orderTableList =
                _checkOutRepository.GetOrderTableListBy(orderId, tableIdList, oStatus);
            
            //提取订单台号id
            List<int> orderTableIdList = orderTableList.Select(x => x.Id).ToList();
            
            //查询订单明细集合
            List<CheckOutOrderDetailDTO> detailList = GetOrderDetailByOrderTableId(orderTableIdList);
            checkoutOrder.OrderTableList = orderTableList;

            var paidList = _payRep.GetPaidRecordListByOrderId(orderId);

            checkoutOrder.PaidRecordList = paidList;
            checkoutOrder.Fraction = paidList.Where(x => x.CyddJzType == CyddJzType.四舍五入 
                            && x.CyddJzStatus == CyddJzStatus.已付)
                        .Sum(x => x.PayAmount);
            checkoutOrder.RealAmount = paidList.Where(x => (x.CyddJzType == CyddJzType.消费 
                                || x.CyddJzType == CyddJzType.转结 || x.CyddJzType == CyddJzType.定金)
                            && x.CyddJzStatus == CyddJzStatus.已付)
                        .Sum(x => x.PayAmount);
            checkoutOrder.GiveChange = paidList.Where(x => x.CyddJzType == CyddJzType.找零 && x.CyddPayType == 1).Sum(x => x.PayAmount);

            foreach (var item in detailList)
            {
                item.Amount = item.Price * item.Num;

                //计算拓展费用
                if (item.OrderDetailAllExtends != null)
                {
                    var extendTotal = item.OrderDetailAllExtends.Sum(x => x.Price * item.Num);
                    item.Amount += extendTotal;                    
                }

                //数量减去 转出，赠菜，退菜
                if (item.R_OrderDetailRecord_List != null && item.R_OrderDetailRecord_List.Count > 0)
                {
                    item.ODRecordCountList = item.R_OrderDetailRecord_List
                        .GroupBy(x => x.CyddMxCzType)
                        .Select(g => new OrderDetailRecordCountDTO
                        {
                            CyddMxCzType = g.Key,
                            Num = g.Sum(x => x.Num)
                        }).ToList();

                    var returnOrMoveOutItem = item.R_OrderDetailRecord_List.Where(
                        x => x.CyddMxCzType == CyddMxCzType.转出 
                        || x.CyddMxCzType == CyddMxCzType.退菜
                        || x.CyddMxCzType == CyddMxCzType.赠菜).ToList();

                    if(returnOrMoveOutItem != null)
                    {
                        var removeNum = returnOrMoveOutItem.Sum(x => x.Num);

                        item.Amount = item.Amount - (item.Price * removeNum);//剔除菜品价格

                        var extendTotal = item.OrderDetailAllExtends.Sum(x => x.Price * removeNum);
                        item.Amount = item.Amount - extendTotal; //剔除扩展（配菜、做法等）价格
                    }                    
                }

                item.DiscountedAmount = MoneyFormant(item.Amount * item.DiscountRate);
            }

            #region 查询订单主结列表
            checkoutOrder.MainPayList = _payRep.GetMainPayListByOrderId(orderId);
            #endregion

            #region 预结
            var orderPeOrderMainPay = _checkOutRepository.GetPreOrderMainPay(orderId);
            if (orderPeOrderMainPay != null)
            {
                checkoutOrder.IsPreCheckOut = true;
                checkoutOrder.DiscountMethod = orderPeOrderMainPay.DiscountType;
                checkoutOrder.SchemeDiscountId = orderPeOrderMainPay.DiscountId;
                checkoutOrder.DiscountRate = orderPeOrderMainPay.DiscountRate;
                checkoutOrder.PreOrderMainPayId = orderPeOrderMainPay.Id;
            }
            #endregion


            decimal sumAmount = detailList.Sum(x => x.Amount);
            //decimal sumDicountedAmount = detailList.Sum(x => x.DiscountedAmount);
            decimal sumDicountedAmount = paidList.Where(p => p.CyddJzType == CyddJzType.折扣).Sum(p => p.PayAmount);
            decimal serviceAmonut = 0;
            decimal fraction = 0;
            decimal oripay = 0;
            foreach (var table in orderTableList)
            {
                var tableDetailList= detailList.Where(x => x.R_OrderTable_Id == table.Id).ToList();
                table.OrderDetailList = tableDetailList;
                var oriAmount = tableDetailList.Where(p => p.IsServiceCharge > 0).Sum(p => p.Amount) - tableDetailList.Where(p => p.IsServiceCharge > 0).Sum(p => p.DiscountedAmount);

                #region 处理菜品服务费
                if (table.IsCheckOut)
                {
                    if (serviceAmonut<=0)
                    {
                        serviceAmonut += paidList.Where(p => p.CyddJzType == CyddJzType.服务费).Sum(p => p.PayAmount);
                    }
                }
                else
                {
                    var money = Convert.ToDecimal(oriAmount * table.ServerRate);
                    serviceAmonut += Math.Round(money, 2);
                }
                #endregion
                //var money = Convert.ToDecimal(oriAmount * table.ServerRate);
                //serviceAmonut += Math.Round(money,2);
            }

            sumAmount = MoneyFormant(sumAmount);//四舍五入保留两位小数
            checkoutOrder.OriginalAmount = sumAmount;
            checkoutOrder.ConAmount = sumAmount;
            //checkoutOrder.DiscountAmount = sumAmount - sumDicountedAmount;
            checkoutOrder.DiscountAmount = sumDicountedAmount;
            checkoutOrder.CheckoutRound = Round;    //四舍五入设置
            checkoutOrder.ServiceAmount = serviceAmonut;
            //oripay = Math.Round(Math.Round(checkoutOrder.OriginalAmount - checkoutOrder.DiscountAmount, 2) + serviceAmonut,2);
            //switch (Round)
            //{
            //    case 1:
            //        fraction = oripay - (Math.Round(oripay, 0));
            //        break;
            //    case 2:
            //        fraction = oripay - (Math.Floor(oripay));
            //        break;
            //    case 3:
            //        fraction = oripay - (Math.Ceiling(oripay));
            //        break;
            //    default:
            //        fraction = oripay - (Math.Round(oripay, 0));
            //        break;
            //}
            //if (fraction>0)
            //{
            //    fraction = -fraction;
            //}
            //checkoutOrder.Fraction = fraction;
            return checkoutOrder;
        }

        public CheckOutOrderDTO GetWeixinPayDTO(CheckOutOrderDTO model)
        {
            decimal fraction = 0;
            decimal oripay = Math.Round(Math.Round(model.OriginalAmount - model.DiscountAmount, 2) + model.ServiceAmount, 2);
            switch (Round)
            {
                case 1:
                    var r = Math.Round(oripay, 0, MidpointRounding.AwayFromZero);
                    fraction = oripay > r ? -(oripay - r) : r - oripay;
                    break;
                case 2:
                    fraction = Math.Floor(oripay) - oripay;
                    break;
                case 3:
                    fraction = Math.Ceiling(oripay) - oripay;
                    break;
                default:
                    break;
            }
            model.Fraction = fraction;
            return model;
        }

        /// <summary>
        /// 将decimal值四舍五入保留两位小数
        /// </summary>
        /// <param name="value">需要转换的值</param>
        /// <returns>转换后的值</returns>
        public static decimal MoneyFormant(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 判断挂账用户本次挂账总金额是否大于限额
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CheckoutVaild(List<OrderPayRecordDTO> req,int restaurant)
        {
            bool result = true;
            if (req.Any())
            {
                var group = req.Where(p => p.CyddPayType == (int)CyddPayType.挂账 ||
                    p.CyddPayType == (int)CyddPayType.会员卡 ||
                    p.CyddPayType == (int)CyddPayType.转客房).GroupBy(p => new { p.SourceId, p.SourceName, p.CyddPayType,p.Pwd }).Select(p => new
                {
                    SourceId = p.Key.SourceId,
                    TotalMoney = p.Sum(g => g.PayAmount),
                    SourceName = p.Key.SourceName,
                    PayMethod = p.Key.CyddPayType,
                    Pwd=p.Key.Pwd
                }).ToList();
                if (group.Any())
                {
                    foreach (var item in group)
                    {
                        var verifyInfo = new VerifySourceInfoDTO();
                        verifyInfo.SourceId = item.SourceId;
                        verifyInfo.PayMethod = item.PayMethod;
                        verifyInfo.OperateValue = item.TotalMoney;
                        verifyInfo.SourceName = item.SourceName;
                        verifyInfo.RestaruantId = restaurant;
                        if (item.PayMethod==(int)CyddPayType.转客房)
                        {
                            if (VerifyOutsideInfoRoom(verifyInfo,null) == null)
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            verifyInfo.SourceName = string.IsNullOrEmpty(item.Pwd)?"":item.Pwd;
                            if (VerifyOutsideInfo(verifyInfo) == null)
                            {
                                result = false;
                                break;
                            }
                        }

                    }
                }
            }
            return result;
        }

        public bool BeforeWholeOrPartialCheckout(WholeOrPartialCheckoutDto req)
        {
            bool res = true;
            CheckOutResultDTO resultDto = new CheckOutResultDTO();

            VerifyOrderInfo(req);

            var checkOutOrderDTO = GetCheckOutOrderDTO(req.OrderId, req.TableIds);

            VerifyAndCalcDetailInfo(req, checkOutOrderDTO);

            using (var db= new SqlSugarClient(Connection))
            {
                try
                {
                    db.BeginTran();
                    var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();

                    if (dateItem == null)
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

                    DateTime accDate = DateTime.Today;

                    if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                        throw new Exception("餐饮账务日期设置错误，请联系管理员");
                    R_ReOrderMainPay reMainPayRecord = db.Queryable<R_ReOrderMainPay>().FirstOrDefault(p => p.R_Order_Id == req.OrderId);
                    if (reMainPayRecord!=null && reMainPayRecord.Id>0)
                    {
                        reMainPayRecord.BillDate = accDate;
                        reMainPayRecord.CreateDate = DateTime.Now;
                        reMainPayRecord.CreateUser = req.OperateUser;
                        reMainPayRecord.R_Market_Id = req.CurrentMarketId;
                        reMainPayRecord.R_Order_Id = req.OrderId;
                        reMainPayRecord.DiscountType = (int)req.DiscountMethod;
                        reMainPayRecord.R_Discount_Id = req.SchemeDiscountId;
                        reMainPayRecord.Remark = req.Remark;
                        if (req.DiscountMethod == DiscountMethods.全单折)
                        {
                            var allDiscountRate = req.ListOrderDetailDTO.Min(p => p.DiscountRate);
                            reMainPayRecord.DiscountRate = allDiscountRate;
                        }
                        else
                        {
                            reMainPayRecord.DiscountRate = 1;
                        }
                        db.Update<R_ReOrderMainPay>(reMainPayRecord);
                    }
                    else
                    {
                        R_ReOrderMainPay newReMainPayRecord = new R_ReOrderMainPay();
                        newReMainPayRecord.BillDate = accDate;
                        newReMainPayRecord.CreateDate = DateTime.Now;
                        newReMainPayRecord.CreateUser = req.OperateUser;
                        newReMainPayRecord.R_Market_Id = req.CurrentMarketId;
                        newReMainPayRecord.R_Order_Id = req.OrderId;
                        newReMainPayRecord.DiscountType = (int)req.DiscountMethod;
                        newReMainPayRecord.R_Discount_Id = req.SchemeDiscountId;
                        newReMainPayRecord.Remark = req.Remark;
                        if (req.DiscountMethod == DiscountMethods.全单折)
                        {
                            var allDiscountRate = req.ListOrderDetailDTO.Min(p => p.DiscountRate);
                            newReMainPayRecord.DiscountRate = allDiscountRate;
                        }
                        else
                        {
                            newReMainPayRecord.DiscountRate = 1;
                        }
                        var reMainId = db.Insert<R_ReOrderMainPay>(newReMainPayRecord);
                      }
                    foreach (var item in req.ListOrderDetailDTO)
                    {
                        db.Update<R_OrderDetail>(new { DiscountRate=item.DiscountRate, PayableTotalPrice=item.DiscountedAmount }, p => p.Id == item.Id);
                    }
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    res = false;
                    throw ex;
                }
            }
            return res;
        }


        public void VerifyOrderDetail(WholeOrPartialCheckoutDto req,CheckOutOrderDTO verifyObj)
        {
            List<CheckOutOrderDetailDTO> detailList = new List<CheckOutOrderDetailDTO>();

            foreach (var item in verifyObj.OrderTableList)
            {
                detailList = detailList.Union(item.OrderDetailList).ToList();
            }
            var logicDetailList = req.ListOrderDetailDTO;
            if (logicDetailList.Count != detailList.Count)
            {
                throw new Exception("结账和点餐的菜品数量不一致,请返回该台号点餐界面重新进行结账操作");
            }
            foreach (var reqItem in logicDetailList)
            {
                var verifyItem = detailList.First(p => p.Id == reqItem.Id);
                if (verifyItem == null)
                {
                    throw new Exception("结账和点餐的菜品数量不一致,请返回该台号点餐界面重新进行结账操作");
                }
                if (verifyItem.Amount != reqItem.Amount)
                {
                    throw new Exception(string.Format("结账和点餐的应收价不一致,请返回该台号点餐界面重新进行结账操作", verifyItem.CyddMxName,verifyItem.Unit));
                }
            }
        }

        /// <summary>
        /// 整单全部结账或部分结账
        /// </summary>
        public CheckOutResultDTO WholeOrPartialCheckout(WholeOrPartialCheckoutDto req,CyddCzjlUserType userType=CyddCzjlUserType.员工)
        {
            CheckOutResultDTO resultDto = new CheckOutResultDTO();

            VerifyOrderInfo(req);

            var checkOutOrderDTO = GetCheckOutOrderDTO(req.OrderId, req.TableIds);

            VerifyOrderDetail(req, checkOutOrderDTO);

            //VerifyAndCalcDetailInfo(req, checkOutOrderDTO);

            //待结账台号ID集合
            List<int> unpaidTableIds = _checkOutRepository.GetOrderTableListBy(req.OrderId)
                .Where(x => !x.IsCheckOut && x.IsOpen).Select(x => x.R_Table_Id).ToList();

            //取餐饮账务日期 TypeId=10003
            var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();
                    
            if (dateItem == null)
                throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

            DateTime accDate = DateTime.Today;

            if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                throw new Exception("餐饮账务日期设置错误，请联系管理员");

            List<UserInfo> authUsers = null;
            if (req.AuthUserList != null && req.AuthUserList.Count > 0)
                authUsers = _oldUserRepository.GetByUserIds(req.AuthUserList.Select(x => x.AuthUserId).ToList());
            var tablesName = checkOutOrderDTO.OrderTableList.Select(p => p.Name).ToArray().Join(",");
            string memberRemark = string.Format("消费点:{0}-{1},订单号:{2},台号:{3}", checkOutOrderDTO.RestaurantName, checkOutOrderDTO.MarketName, checkOutOrderDTO.OrderNo, tablesName);
            List<MemberInfoDTO> members= SaveMemberConsumeInfo(req.ListOrderPayRecordDTO, req.OperateUserCode, true, accDate,memberRemark, checkOutOrderDTO.R_Restaurant_Id);


            using (var db = new SqlSugarClient(Connection))
            {
                try
                {
                    db.BeginTran();
                    var orderTableIds = checkOutOrderDTO.OrderTableList.Select(p => p.Id).ToList();
                    _orderRep.UpdateOrderTableIsControl(orderTableIds, false);
                    db.Delete<R_ReOrderMainPay>(p => p.R_Order_Id == req.OrderId);
                    var orderModel = db.Queryable<R_Order>()
                        .Where(x => x.Id == req.OrderId).FirstOrDefault();
                    var resObj = db.Queryable<R_Restaurant>().Where(x => x.Id == orderModel.R_Restaurant_Id).FirstOrDefault();
                    var marketObj = db.Queryable<R_Market>().Where(x => x.Id == req.CurrentMarketId).FirstOrDefault();

                    if (!req.IsReCheckout)
                    {
                        orderModel.RealAmount += req.Money; //本次支付金额
                        orderModel.ServiceAmount += req.ServiceAmount; //本次支付服务费金额
                        orderModel.DiscountAmount += req.DiscountAmount; //本次支付折扣金额
                        orderModel.GiveAmount += req.GiveAmount; //本次支付赠送金额
                        orderModel.ClearAmount += req.ClearAmount; //本次支付抹零金额
                        orderModel.OriginalAmount += req.OriginalAmount; //本次支付应收金额

                        orderModel.ConAmount += checkOutOrderDTO.ConAmount; //消费金额
                    }

                    List<R_OrderTable> list = new List<R_OrderTable>();

                    #region 根据订单Id 及餐台数取得相关的订单台号 记录

                    var result = unpaidTableIds.Except(req.TableIds);//获取传入的台Id 与 查询到的台Id 的差集
                    if (result.Count() == 0 && unpaidTableIds.Count == req.TableIds.Count) //整单结账
                    {
                        orderModel.CyddStatus = CyddStatus.结账;
                    }

                    //取指定的台号结账
                    list = db.Queryable<R_OrderTable>()
                        .Where(x => x.R_Order_Id == req.OrderId && req.TableIds.Contains(x.R_Table_Id) && !x.IsCheckOut)
                        .ToList();
                    #endregion

                    #region 生成订单当次主结记录
                    int mainPayId = 0;
                    
                    R_OrderMainPay mainPayRecord = new R_OrderMainPay();
                    mainPayRecord.BillDate = accDate;
                    mainPayRecord.CreateDate = DateTime.Now;
                    mainPayRecord.CreateUser = req.OperateUser;
                    mainPayRecord.R_Market_Id = req.CurrentMarketId;
                    mainPayRecord.R_Order_Id = req.OrderId;
                    mainPayRecord.DiscountType = (int)req.DiscountMethod;
                    mainPayRecord.R_Discount_Id = req.SchemeDiscountId;
                    mainPayRecord.Remark = req.Remark;
                    if (req.DiscountMethod==DiscountMethods.全单折 || req.DiscountMethod==DiscountMethods.强制折)
                    {
                        var allDiscountRate = req.ListOrderDetailDTO.Min(p => p.DiscountRate);
                        mainPayRecord.DiscountRate = allDiscountRate;
                    }
                    else
                    {
                        mainPayRecord.DiscountRate = 1;
                    }
                    var mainId = db.Insert<R_OrderMainPay>(mainPayRecord);
                    mainPayId = mainId.ToInt();
                    
                    #endregion

                    #region 订单台号，订单记录及餐台状态处理

                    var tableIdList = list.Select(x => x.R_Table_Id).ToList();
                    var originalTabOrderList = db.Queryable<R_OrderTable>()
                        .Where(x => !x.IsCheckOut //尚未结账
                            && x.IsOpen //已经开台
                            && x.R_Order_Id != req.OrderId //其它的订单
                            && tableIdList.Contains(x.R_Table_Id)) //当前台号
                        .ToList();

                    List<R_OrderRecord> ordRecordList = new List<R_OrderRecord>();
                    List<int> needUpdateTableIds = new List<int>();

                    foreach (var item in list)
                    {
                        if (!req.IsReCheckout)
                        {
                            item.IsCheckOut = true;
                            item.IsOpen = false;
                            item.BillDate = accDate;
                            item.R_OrderMainPay_Id = mainPayId;
                            item.R_Market_Id = req.CurrentMarketId;
                        }

                        #region 订单操作记录  R_OrderRecord
                        var recordModel = new R_OrderRecord()
                        {
                            CreateUser = req.OperateUser,
                            CreateDate = DateTime.Now,
                            CyddCzjlStatus = CyddStatus.结账,
                            CyddCzjlUserType = userType,
                            R_Order_Id = req.OrderId,
                            R_OrderTable_Id = item.Id,
                            Remark = "此餐台账单已结清 "
                        };
                        ordRecordList.Add(recordModel);
                        #endregion

                        //被其它订单占用，不能修改台号状态（跳过非当天订单的错误旧数据）
                        if (originalTabOrderList.Where(x => x.R_Table_Id == item.R_Table_Id).Any())
                            continue;// 若包含则跳过
        //                if (originalTabOrderList.Where(x => x.R_Table_Id == item.R_Table_Id
        //&& x.CreateDate.Value.AddHours(18) >= DateTime.Now).Any())
                            needUpdateTableIds.Add(item.R_Table_Id);
                    }
                    
                    var updateTableList = db.Queryable<R_Table>()
                        .Where(x => needUpdateTableIds.Contains(x.Id))
                        .ToList();

                    string tableNames = updateTableList.Select(x => x.Name).ToList().Join(",");

                    if (!req.IsReCheckout)
                    {
                        db.UpdateRange(list);//更新订单台号的结账状态

                        updateTableList.ForEach(x => { x.CythStatus = CythStatus.空置; });
                        db.UpdateRange(updateTableList); //更新餐台状态
                    }

                    db.Update(orderModel);//更新订单信息 
                    #endregion

                    #region 订单额外操作记录
                    
                    BuildOtherRecords(ordRecordList, authUsers, req);
                    db.InsertRange(ordRecordList);//插入操作记录
                    #endregion

                    #region 订单明细更新

                    var needUpdateDetailList = req.ListOrderDetailDTO.ToList();
                    var detailIds = needUpdateDetailList.Select(x => x.Id).ToList();
                    var projectDetailIds = needUpdateDetailList
                        .Where(p => p.CyddMxType == CyddMxType.餐饮项目).GroupBy(p => p.CyddMxId).Select(p => p.Key).ToArray();
                    var ordDetailList = db.Queryable<R_OrderDetail>().Where(x => detailIds.Contains(x.Id)).ToList();
                    var orderProjectList = db.Queryable<R_Project>()
                        .JoinTable<R_ProjectDetail>((s1, s2) => s1.Id == s2.R_Project_Id, JoinType.Left)
                        .Where<R_ProjectDetail>((s1, s2) => projectDetailIds.Contains(s2.Id))
                        .Select("s1.*").ToList();
                    var orderProjectDetailList = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).ToList();
                    var ordDetailRebackList = db.Queryable<R_OrderDetailRecord>()
                        .Where(p => detailIds.Contains(p.R_OrderDetail_Id) && p.CyddMxCzType == CyddMxCzType.退菜 && p.IsCalculation)
                        .ToList();
                    foreach (var item in ordDetailList)
                    {
                        var detailObj = needUpdateDetailList.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (detailObj != null || detailObj.Id > 0)
                        {
                            if (detailObj.CyddMxType==CyddMxType.餐饮项目)
                            {
                                item.DiscountRate = detailObj.DiscountRate;
                                item.PayableTotalPrice = detailObj.DiscountedAmount;
                                var projectId = orderProjectDetailList.Where(p => p.Id == detailObj.CyddMxId).Select(p => p.R_Project_Id).FirstOrDefault();
                                var projectObj = orderProjectList.Where(p => p.Id == projectId).FirstOrDefault();
                                if (projectObj != null)
                                {
                                    switch (projectObj.ExtractType)
                                    {
                                        case 1:
                                            var totalNum = item.Num - ordDetailRebackList.Where(p => p.R_OrderDetail_Id == item.Id).Sum(p => p.Num);
                                            item.ExtractPrice = totalNum * projectObj.ExtractPrice;
                                            break;
                                        case 2:
                                            if (ExtractType==1)
                                            {
                                                item.ExtractPrice = Math.Round(item.OriginalTotalPrice * projectObj.ExtractPrice, 2);
                                            }
                                            else if (ExtractType==2)
                                            {
                                                item.ExtractPrice = Math.Round(item.PayableTotalPrice * projectObj.ExtractPrice, 2);
                                            }
                                            break;
                                        default:
                                            item.ExtractPrice = 0;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    
                    db.UpdateRange(ordDetailList);
                    #endregion

                    #region 订单付款记录  R_OrderPayRecord

                    List<R_OrderPayRecord> newPayRecords = new List<R_OrderPayRecord>();

                    List<R_OrderPayRecord> updatePayRecords = new List<R_OrderPayRecord>();
                    foreach (var item in req.ListOrderPayRecordDTO)
                    {
                        R_OrderPayRecord payRecordModel = null;
                        if (item.Id > 0)
                        {
                            if (item.CyddJzType != CyddJzType.定金)
                            {

                            }
                            else if (item.CyddJzType == CyddJzType.定金 && item.CyddJzStatus != CyddJzStatus.已结)
                            {
                                payRecordModel = db.Queryable<R_OrderPayRecord>().Where(x => x.Id == item.Id).FirstOrDefault();
                                payRecordModel.CyddJzStatus = CyddJzStatus.已结;//更改定金记录状态为已结
                                updatePayRecords.Add(payRecordModel);
                            }
                            continue;
                        }

                        payRecordModel = AutoMapper.Mapper.Map<R_OrderPayRecord>(item);
                        payRecordModel.R_Order_Id = orderModel.Id;
                        payRecordModel.CyddPayType = item.CyddPayType;
                        payRecordModel.PayAmount = item.PayAmount;
                        payRecordModel.CyddJzStatus = item.CyddJzStatus;
                        payRecordModel.CyddJzType = item.CyddJzType;
                        payRecordModel.CreateDate = DateTime.Now;
                        payRecordModel.CreateUser = req.OperateUser;
                        payRecordModel.BillDate = accDate;
                        payRecordModel.R_Market_Id = req.CurrentMarketId;
                        payRecordModel.Remark = item.Remark;
                        payRecordModel.R_OrderMainPay_Id = mainPayId;
                        payRecordModel.SourceName = item.SourceName;
                        payRecordModel.PId = item.PId;
                        payRecordModel.R_Restaurant_Id = item.R_Restaurant_Id;

                        #region 挂账、转客房、会员卡操作
                        if (item.CyddPayType == (int)CyddPayType.挂账 || item.CyddPayType == (int)CyddPayType.转客房
                            || item.CyddPayType == (int)CyddPayType.会员卡)
                        {
                            if (item.CyddPayType == (int)CyddPayType.会员卡)
                            {
                                if (!EnabelGroupFlag)
                                {//若已启用集团会员库，里面则不再执行，本地库会员消费记录已在插入集团库时一并插入到本地库
                                    try
                                    {
                                        //var memberInfo = ApplyChangeMemberToDb(item.SourceId, item.Pwd, req.OperateUserCode,item.PayAmount, string.Format("{0}-{1} {2}", resObj.Name, marketObj.Name, memberRemark + item.Remark), false, db,accDate);
                                        var memberInfo = ApplyChangeMemberToDb(item.SourceId, item.Pwd, req.OperateUserCode, item.PayAmount, memberRemark, false, db, accDate,orderModel.R_Restaurant_Id);
                                        members.Add(memberInfo);
                                        string remark = string.Format("会员信息-卡号{0}- 姓名：{1}", memberInfo.MemberCardNo, memberInfo.MemberName);
                                        payRecordModel.SourceId = item.SourceId;
                                        payRecordModel.SourceName = string.Format("{0}-{1}",memberInfo.MemberCardNo,memberInfo.MemberName);
                                        payRecordModel.Remark = remark;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("本地库记录会员卡消费信息操作失败：" + ex.Message);
                                    }
                                }
                            }
                            else if (item.CyddPayType == (int)CyddPayType.挂账)
                            {
                                var verifyInfo = new VerifySourceInfoDTO();
                                verifyInfo.SourceId = item.SourceId;
                                verifyInfo.SourceName = item.SourceName;
                                verifyInfo.RestaruantId = orderModel.R_Restaurant_Id;
                                verifyInfo.PayMethod = (int)CyddPayType.挂账;
                                verifyInfo.OperateValue = item.PayAmount;

                                List<string> resultList = VerifyOutsideInfo(verifyInfo, db);

                                payRecordModel.SourceId = item.SourceId;
                                string code = resultList[0].Trim();
                                string remark = string.Format("挂账客户【{0}】- 代码：({1})", item.SourceName, code);
                                if (req.IsReCheckout)
                                    remark = " --> " + remark;
                                payRecordModel.Remark = remark;
                                try
                                {
                                    var paras = SqlSugarTool.GetParameters(new
                                    {
                                        xh = req.OrderId, //餐饮单序号
                                        dh = orderModel.R_Restaurant_Id + "." + req.OrderId, //餐厅代码+'.'+餐饮单单号
                                        lx = code, //协议单位代码(lxdmdm00)
                                        je = item.PayAmount,//金额
                                        cz = req.OperateUserCode, //操作员代码
                                        ctmc = resObj.Name, //餐厅名称
                                        fsmc = marketObj.Name, //分市名称
                                        th = req.OrderId,
                                        rs = orderModel.PersonNum,
                                        bz = payRecordModel.Remark,
                                        mz = "",
                                        atr = 0
                                    });
                                    db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                                    db.ExecuteCommand("p_po_toys_newCY", paras);
                                    db.CommandType = System.Data.CommandType.Text;//还原回默认
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("挂账操作失败：" + ex.Message);
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

                                //List<string> resultList = VerifyOutsideInfo(verifyInfo, db);
                                List<string> resultList = VerifyOutsideInfoRoom(verifyInfo, db);
                                payRecordModel.SourceName = item.SourceName;
                                try
                                {
                                    var paras = SqlSugarTool.GetParameters(new
                                    {
                                        zh00 = resultList[1].ToInt(), //客人账号(krzlzh00)
                                        zwdm = resultList[0], //账项代码
                                        hsje = item.PayAmount,//金额
                                        ckhm = item.SourceName, //房号(krzlfh00)
                                        czdm = req.OperateUserCode, //操作员代码
                                        xfje = 1,
                                        bz00 = "餐厅转客房,单号:"+orderModel.OrderNo,
                                        bc00 = "",
                                    });
                                    db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                                    db.ExecuteCommand("p_zw_addx", paras);
                                    db.CommandType = System.Data.CommandType.Text;//还原回默认
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("转客房操作失败：" + ex.Message);
                                }
                                #endregion

                                string remark = string.Format("转客房房号【{0}】- 客人帐号Id：({1})", item.SourceName, resultList[1]);
                                if (req.IsReCheckout)
                                    remark = " --> " + remark;
                                payRecordModel.Remark = remark;
                            }
                        }
                        #endregion

                        newPayRecords.Add(payRecordModel);
                    }

                    if (updatePayRecords.Count > 0)
                        db.UpdateRange(updatePayRecords);

                    db.InsertRange(newPayRecords);
                    #endregion

                    #region
                    if (members.Any())
                    {
                        foreach (var member in members)
                        {
                            if (!string.IsNullOrEmpty(member.MemberPhoneNo))
                            {
                                decimal totalMoney = req.ListOrderPayRecordDTO.Where(p => p.SourceId == member.Id).Sum(p => p.PayAmount);
                                string totalYe = (member.CardBalance + totalMoney).ToString();
                                string insertSql = string.Format(
        "INSERT sms1(sms0lx00,sms0dh00,sms0nr00,sms0bz00,sms0czsj) " +
        "VALUES('1', '" + member.MemberPhoneNo + "', '" + member.MemberName + ",您的会员卡(" + member.MemberCardNo + ")本次消费" + totalMoney + "元!欢迎消费目前您的余额为" + totalYe + "元', 'S', '" + DateTime.Now + "') ",
        member.MemberName, member.MemberCardNo, member.MemberGender,
        member.MemberGPID, member.MemberGUID, member.MemberPhoneNo, member.MemberIdentityNo);
                                db.CommandType = System.Data.CommandType.Text;
                                db.ExecuteCommand(insertSql);
                            }
                        }
                    }
                    #endregion
                    db.CommitTran();

                    resultDto = new CheckOutResultDTO()
                    {
                        OrderId = req.OrderId,
                        OrderMainPayId = mainPayId,
                        OrderTables = list.Select(p => p.Id).ToList()
                    };
                }
                catch (Exception e)
                {
                    db.RollbackTran();
                    SaveMemberConsumeInfo(req.ListOrderPayRecordDTO, req.OperateUserCode, false,accDate,"", checkOutOrderDTO.R_Restaurant_Id);
                    
                    throw e;
                }
                return resultDto;
            }
        }

        public void ReCheckout(WholeOrPartialCheckoutDto req)
        {
            VerifyOrderInfo(req);

            var checkOutOrderDTO = GetCheckOutOrderDTO(req.OrderId, req.TableIds);

            VerifyAndCalcDetailInfo(req, checkOutOrderDTO);
            
            //取餐饮账务日期 TypeId=10003
            var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();

            if (dateItem == null)
                throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

            DateTime accDate = DateTime.Today;

            if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                throw new Exception("餐饮账务日期设置错误，请联系管理员");

            List<UserInfo> authUsers = null;
            if (req.AuthUserList != null && req.AuthUserList.Count > 0)
                authUsers = _oldUserRepository.GetByUserIds(req.AuthUserList.Select(x => x.AuthUserId).ToList());
            string memberRemark = string.Format("消费点:{0}-{1}", checkOutOrderDTO.RestaurantName, checkOutOrderDTO.MarketName);
            SaveMemberConsumeInfo(req.ListOrderPayRecordDTO, req.OperateUserCode, true, accDate, memberRemark, checkOutOrderDTO.R_Restaurant_Id);

            using (var db = new SqlSugarClient(Connection))
            {
                try
                {
                    db.BeginTran();

                    var orderModel = db.Queryable<R_Order>()
                        .Where(x => x.Id == req.OrderId).FirstOrDefault();
                    
                    #region 根据订单Id 及餐台数取得相关的订单台号 记录
                    List<R_OrderTable> list = new List<R_OrderTable>();

                    //选中的台号反结结账
                    list = db.Queryable<R_OrderTable>()
                        .Where(x => x.R_Order_Id == req.OrderId && req.TableIds.Contains(x.R_Table_Id))
                        .ToList();
                    
                    #endregion

                    //获取订单指定的主结记录Id
                    int mainPayId = list.GroupBy(x => x.R_OrderMainPay_Id).Select(x => x.Key).FirstOrDefault();
                    
                    #region 订单台号，订单记录及餐台状态处理

                    var tableIdList = list.Select(x => x.R_Table_Id).ToList();
                    
                    List<R_OrderRecord> ordRecordList = new List<R_OrderRecord>();
                    List<int> needUpdateTableIds = new List<int>();

                    foreach (var item in list)
                    {
                        item.BillDate = accDate;
                        item.R_Market_Id = req.CurrentMarketId;
                        

                        #region 订单操作记录  R_OrderRecord
                        var recordModel = new R_OrderRecord()
                        {
                            CreateUser = req.OperateUser,
                            CreateDate = DateTime.Now,
                            CyddCzjlStatus = CyddStatus.结账,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            R_Order_Id = req.OrderId,
                            R_OrderTable_Id = item.Id,
                            Remark = "此餐台账单反结成功！"
                        };
                        ordRecordList.Add(recordModel);
                        #endregion

                        needUpdateTableIds.Add(item.R_Table_Id);
                    }

                    var updateTableList = db.Queryable<R_Table>()
                        .Where(x => needUpdateTableIds.Contains(x.Id))
                        .ToList();

                    string tableNames = updateTableList.Select(x => x.Name).ToList().Join(",");

                    if (req.IsReCheckout)
                    {
                        db.UpdateRange(list);//更新订单台号的结账状态                        
                    }
                    #endregion

                    #region 订单额外操作记录
                    BuildOtherRecords(ordRecordList, authUsers, req);
                    db.InsertRange(ordRecordList);//插入操作记录
                    #endregion

                    #region 订单明细更新

                    var needUpdateDetailList = req.ListOrderDetailDTO.ToList();
                    var detailIds = needUpdateDetailList.Select(x => x.Id).ToList();
                    var ordDetailList = db.Queryable<R_OrderDetail>().Where(x => detailIds.Contains(x.Id)).ToList();

                    foreach (var item in ordDetailList)
                    {
                        var detailObj = needUpdateDetailList.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (detailObj != null && detailObj.Id > 0)
                        {
                            item.DiscountRate = detailObj.DiscountRate;
                            item.PayableTotalPrice = detailObj.DiscountedAmount;
                        }
                    }

                    db.UpdateRange(ordDetailList);
                    #endregion

                    #region 订单付款记录  R_OrderPayRecord

                    List<R_OrderPayRecord> newPayRecords = new List<R_OrderPayRecord>();
                    List<R_OrderPayRecord> updatePayRecords = new List<R_OrderPayRecord>();

                    #region 为旧的四舍五入，抹零，服务费冲账生成相反金额的对应记录

                    var types = new[] { (int)CyddJzType.四舍五入, (int)CyddJzType.抹零, (int)CyddJzType.服务费, (int)CyddJzType.折扣 };
                    var otherPayRecords = db.Queryable<R_OrderPayRecord>()
                        .Where(x => x.R_OrderMainPay_Id == mainPayId && x.CyddJzStatus == CyddJzStatus.已付
                                && types.Contains((int)x.CyddJzType)).ToList();

                    foreach (var item in otherPayRecords)
                    {
                        R_OrderPayRecord payRecordModel = new R_OrderPayRecord();
                        payRecordModel.Id = 0;
                        payRecordModel.R_Order_Id = orderModel.Id;
                        payRecordModel.CyddPayType = item.CyddPayType;
                        payRecordModel.PayAmount = -item.PayAmount;
                        payRecordModel.CyddJzStatus = CyddJzStatus.已结;
                        payRecordModel.CyddJzType = item.CyddJzType;
                        payRecordModel.CreateDate = DateTime.Now;
                        payRecordModel.CreateUser = req.OperateUser;
                        payRecordModel.BillDate = accDate;
                        payRecordModel.R_Market_Id = req.CurrentMarketId;
                        payRecordModel.Remark = "反结对应源支付记录Id：" + item.Id;
                        payRecordModel.R_OrderMainPay_Id = mainPayId;

                        newPayRecords.Add(payRecordModel);

                        item.CyddJzStatus = CyddJzStatus.已结;
                        updatePayRecords.Add(item);
                    }
                    #endregion

                    //取对应反结的Id
                    var ids = req.ListOrderPayRecordDTO.Where(x => x.Id < 0).Select(x => Math.Abs(x.Id)).ToList();
                    var earmarkRecords = db.Queryable<R_OrderPayRecord>()
                        .Where(x => x.R_OrderMainPay_Id == mainPayId && x.CyddJzType == CyddJzType.消费 && ids.Contains(x.Id)).ToList();

                    foreach (var item in earmarkRecords)
                    {
                        item.CyddJzStatus = CyddJzStatus.已结;//更新反结对应的支付记录状态为已结
                        updatePayRecords.Add(item);
                    }

                    #region 保存页面新的反结支付记录

                    foreach (var item in req.ListOrderPayRecordDTO)
                    {
                        if (item.Id > 0)
                            continue;

                        R_OrderPayRecord payRecordModel = null;

                        payRecordModel = new R_OrderPayRecord();
                        payRecordModel.R_Order_Id = orderModel.Id;
                        payRecordModel.CyddPayType = item.CyddPayType;
                        payRecordModel.PayAmount = item.PayAmount;
                        payRecordModel.CyddJzStatus = item.CyddJzStatus;
                        payRecordModel.CyddJzType = item.CyddJzType;
                        payRecordModel.CreateDate = DateTime.Now;
                        payRecordModel.CreateUser = req.OperateUser;
                        payRecordModel.BillDate = accDate;
                        payRecordModel.R_Market_Id = req.CurrentMarketId;
                        payRecordModel.Remark = item.Remark;
                        payRecordModel.R_OrderMainPay_Id = mainPayId;

                        #region 挂账、转客房操作
                        if (item.CyddPayType == (int)CyddPayType.挂账 || 
                            item.CyddPayType == (int)CyddPayType.转客房 ||
                            item.CyddPayType == (int)CyddPayType.会员卡)
                        {
                            var resObj = db.Queryable<R_Restaurant>().Where(x => x.Id == orderModel.R_Restaurant_Id).FirstOrDefault();

                            if (item.CyddPayType == (int)CyddPayType.会员卡)
                            {
                                if (!EnabelGroupFlag)
                                {//若已启用集团会员库，里面则不再执行，本地库会员消费记录已在插入集团库时一并插入到本地库
                                    try
                                    {
                                        ApplyChangeMemberToDb(item.SourceId, item.SourceName, req.OperateUserCode,
                                            item.PayAmount, memberRemark + item.Remark, false, db,accDate, checkOutOrderDTO.R_Restaurant_Id);
                                        payRecordModel.SourceId = item.SourceId;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("本地库记录会员卡消费信息操作失败：" + ex.Message);
                                    }
                                }
                            }
                            else if (item.CyddPayType == (int)CyddPayType.挂账)
                            {
                                var verifyInfo = new VerifySourceInfoDTO();
                                verifyInfo.SourceId = item.SourceId;
                                verifyInfo.SourceName = item.SourceName;
                                verifyInfo.RestaruantId = orderModel.R_Restaurant_Id;
                                verifyInfo.PayMethod = (int)CyddPayType.挂账;
                                verifyInfo.OperateValue = item.PayAmount;

                                List<string> resultList = VerifyOutsideInfo(verifyInfo, db);

                                var marketObj = db.Queryable<R_Market>().Where(x => x.Id == req.CurrentMarketId).FirstOrDefault();
                                payRecordModel.SourceId = item.SourceId;
                                string code = resultList[0].Trim();
                                string remark = string.Format("挂账客户【{0}】- 代码：({1})", item.SourceName, code);
                                if (req.IsReCheckout)
                                    remark = " --> " + remark;
                                payRecordModel.Remark += remark;

                                string sql = "exec p_po_toys_newCY @xh, @dh, @lx, @je, @cz, @ctmc, @fsmc, @th, @rs, @bz, @mz, @atr";
                                try
                                {
                                    db.ExecuteCommand(sql,
                                        new
                                        {
                                            xh = req.OrderId, //餐饮单序号
                                            dh = orderModel.R_Restaurant_Id + "." + req.OrderId, //餐厅代码+'.'+餐饮单单号
                                            lx = code, //协议单位代码(lxdmdm00)
                                            je = item.PayAmount,//金额
                                            cz = req.OperateUserCode, //操作员代码
                                            ctmc = resObj.Name, //餐厅名称
                                            fsmc = marketObj.Name, //分市名称
                                            th = tableNames,
                                            rs = orderModel.PersonNum,
                                            bz = payRecordModel.Remark,
                                            mz = "",
                                            atr = 0
                                        });
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("挂账操作失败：" + ex.Message);
                                }
                            }
                            else if (item.CyddPayType == (int)CyddPayType.转客房)
                            {
                                #region 转客房处理

                                var verifyInfo = new VerifySourceInfoDTO();
                                verifyInfo.SourceId = 0;
                                verifyInfo.SourceName = item.SourceName;
                                verifyInfo.RestaruantId = orderModel.R_Restaurant_Id;
                                verifyInfo.PayMethod = (int)CyddPayType.转客房;
                                verifyInfo.OperateValue = item.PayAmount;

                                List<string> resultList = VerifyOutsideInfo(verifyInfo, db);

                                payRecordModel.SourceName = item.SourceName;
                                try
                                {
                                    var paras = SqlSugarTool.GetParameters(new
                                    {
                                        zh00 = resultList[1].ToInt(), //客人账号(krzlzh00)
                                        zwdm = resultList[0], //账项代码
                                        hsje = item.PayAmount,//金额
                                        ckhm = item.SourceName, //房号(krzlfh00)
                                        czdm = req.OperateUserCode, //操作员代码
                                        xfje = item.PayAmount,
                                        bz00 = "餐厅转客房",
                                        bc00 = "",
                                    });
                                    db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                                    db.ExecuteCommand("p_zw_addx", paras);
                                    db.CommandType = System.Data.CommandType.Text;//还原回默认
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("转客房操作失败：" + ex.Message);
                                }
                                #endregion

                                string remark = string.Format("转客房房号【{0}】- 客人帐号Id：({1})", item.SourceName, resultList[1]);
                                if (req.IsReCheckout)
                                    remark = " --> " + remark;
                                payRecordModel.Remark += remark;
                            }
                        }
                        #endregion

                        newPayRecords.Add(payRecordModel);
                    } 
                    #endregion

                    if (updatePayRecords.Count > 0)
                        db.UpdateRange(updatePayRecords);

                    db.InsertRange(newPayRecords);
                    #endregion

                    var allPaids = db.Queryable<R_OrderPayRecord>().Where(x => x.R_Order_Id == orderModel.Id).ToList();
                    var allDetails = db.Queryable<R_OrderDetail>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.R_OrderTable_Id == s2.Id, JoinType.Inner)
                        .Where<R_OrderTable>((s1, s2) => s2.R_Order_Id == orderModel.Id)
                        .Select<R_OrderDetail>("s1.*").ToList();

                    decimal totalConAmount = allDetails.Sum(x => x.OriginalTotalPrice);
                    decimal totalRealAmount = allPaids.Where(x => x.R_Order_Id == orderModel.Id
                                && (x.CyddJzType == CyddJzType.消费 || x.CyddJzType == CyddJzType.转结))
                        .Sum(x => x.PayAmount);
                    decimal totalDiscountAmount = allPaids
                        .Where(x => x.R_Order_Id == orderModel.Id
                                && x.CyddJzType == CyddJzType.折扣 && x.CyddJzStatus == CyddJzStatus.已付)
                        .Sum(x => x.PayAmount);
                    decimal totalServiceAmount = allPaids
                        .Where(x => x.R_Order_Id == orderModel.Id 
                                && x.CyddJzType == CyddJzType.服务费 && x.CyddJzStatus == CyddJzStatus.已付)
                        .Sum(x => x.PayAmount);
                    decimal totalClearAmount = allPaids
                        .Where(x => x.R_Order_Id == orderModel.Id
                                && x.CyddJzType == CyddJzType.抹零 && x.CyddJzStatus == CyddJzStatus.已付)
                        .Sum(x => x.PayAmount);
                    decimal totalFraction = allPaids
                        .Where(x => x.R_Order_Id == orderModel.Id
                                && x.CyddJzType == CyddJzType.四舍五入 && x.CyddJzStatus == CyddJzStatus.已付)
                        .Sum(x => x.PayAmount);

                    //orderModel.GiveAmount = req.GiveAmount; //本次支付赠送金额
                    orderModel.OriginalAmount = totalConAmount - totalDiscountAmount + totalServiceAmount + totalFraction; //总计应收金额
                    orderModel.ConAmount = totalConAmount; //消费金额
                    orderModel.RealAmount = totalRealAmount; //更新总计实收金额
                    orderModel.DiscountAmount = totalDiscountAmount; //更新总计折扣金额
                    orderModel.ServiceAmount = totalServiceAmount; //更新总计服务费金额
                    orderModel.ClearAmount = totalClearAmount; //更新总计抹零金额

                    db.Update(orderModel);//更新订单信息 

                    db.CommitTran();
                }
                catch (Exception e)
                {
                    db.RollbackTran();
                    SaveMemberConsumeInfo(req.ListOrderPayRecordDTO, req.OperateUserCode, false,accDate,"", checkOutOrderDTO.R_Restaurant_Id);
                    throw e;
                }
            }

        }

        /// <summary>
        /// 构造订单操作记录
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="req"></param>
        private void BuildOtherRecords(List<R_OrderRecord> sourceList, List<UserInfo> authUsers, WholeOrPartialCheckoutDto req)
        {
            if (req.AuthUserList != null && req.AuthUserList.Count > 0)
            {
                foreach (var item in req.AuthUserList)
                {
                    var user = authUsers.Where(x => x.UserId == item.AuthUserId).FirstOrDefault();
                    string authRemark = null;
                    if (item.OperateType == AuthOperateTypes.折扣授权)
                    {
                        authRemark = string.Format("本次结账含有打折授权操作，授权用户-({0}) Id:({1})", user.UserName, user.UserId);
                        if(item.AuthUserId == req.OperateUser)
                            authRemark = "本次结账有打折操作，操作用户：" + user.UserName;
                    }
                    else if (item.OperateType == AuthOperateTypes.抹零授权)
                    {
                        authRemark = string.Format("本次结账含有抹零授权操作，授权用户-({0}) Id:({1})", user.UserName, user.UserId);
                        if (item.AuthUserId == req.OperateUser)
                            authRemark = "本次结账有抹零操作，操作用户：" + user.UserName;
                    }

                    var recordOrder = new R_OrderRecord()
                    {
                        CreateUser = item.AuthUserId,
                        CreateDate = DateTime.Now,
                        CyddCzjlStatus = CyddStatus.结账,
                        CyddCzjlUserType = CyddCzjlUserType.员工,
                        R_Order_Id = req.OrderId,
                        Remark = authRemark
                    };
                    sourceList.Add(recordOrder);

                }
            }

            if (req.DiscountMethod != DiscountMethods.无折扣)
            {
                var userId = req.AuthUserList.Where(x => x.OperateType == AuthOperateTypes.折扣授权)
                    .Select(x => x.AuthUserId).FirstOrDefault();
                var discountRecord = new R_OrderRecord()
                {
                    CreateUser = userId > 0 ? userId : req.OperateUser,
                    CreateDate = DateTime.Now,
                    CyddCzjlStatus = CyddStatus.结账,
                    CyddCzjlUserType = CyddCzjlUserType.员工,
                    R_Order_Id = req.OrderId,
                    Remark = "本次账单结账折扣操作类型：" + Enum.GetName(typeof(DiscountMethods), req.DiscountMethod),
                };
                sourceList.Add(discountRecord);
            }

            if (req.IsReCheckout)
            {
                var userId = req.AuthUserList.Where(x => x.OperateType == AuthOperateTypes.反结操作).Select(x => x.AuthUserId).FirstOrDefault();
                var discountRecord = new R_OrderRecord()
                {
                    CreateUser = userId > 0 ? userId : req.OperateUser,
                    CreateDate = DateTime.Now,
                    CyddCzjlStatus = CyddStatus.结账,
                    CyddCzjlUserType = CyddCzjlUserType.员工,
                    R_Order_Id = req.OrderId,
                    Remark = "反结账单操作",
                };
                sourceList.Add(discountRecord);
            }

        }

        public List<string> VerifyOutsideInfo(VerifySourceInfoDTO verifySource)
        {
            return VerifyOutsideInfo(verifySource, null);
        }

        public List<string> VerifyOutsideInfoRoom(VerifySourceInfoDTO verifySource, SqlSugarClient db)
        {
            bool isOutside = false;
            if (db == null)
            {
                db = new SqlSugarClient(Connection);
                isOutside = true;
            }

            List<string> infoList = new List<string>();
            #region 转客房验证
            if (verifySource.SourceName.IsEmpty())
                throw new Exception("请输入房号！");

            string sqlRoom = string.Format("SELECT TOP 1 krzlfh00 " +
                                        "FROM krzl WHERE krzlzt00 = 'I' AND (krzlzh00 = {0} or (krzlkrlx='G' and krzlth00='{0}'))", verifySource.SourceId);

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
            return infoList;
        }

        public List<string> VerifyOutsideInfo(VerifySourceInfoDTO verifySource, SqlSugarClient db)
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

                if (info[0].LimitAmount != 0)    //挂账限额不等于0则需判断
                {
                    if ((info[0].RemainAmount + verifySource.OperateValue) > info[0].LimitAmount)
                    {
                        throw new Exception(string.Format("此客户（{0}）可挂账限额大于当前输入金额,无法挂账", verifySource.SourceName));
                    }
                }

                //decimal remainAmount = info[0].RemainAmount - info[0].LimitAmount;
                //if(info[0].RemainAmount < 0 || remainAmount <= 0)
                //    throw new Exception(string.Format("此客户（{0}）可挂账余额为0 无法操作挂账, 请重新确认挂账客户！", verifySource.SourceName));
                
                //if(remainAmount < verifySource.OperateValue)
                //    throw new Exception(string.Format("此客户（{0}）可挂账余额小于当前输入金额无法挂账, 请重新确认！", verifySource.SourceName));
                

                infoList.Add(info[0].Code.Trim());
            }
            else if(verifySource.PayMethod == (int)CyddPayType.转客房)
            {
                #region 转客房验证
                if (verifySource.SourceName.IsEmpty())
                    throw new Exception("请输入房号！");

                string sqlRoom = string.Format("SELECT TOP 1 krzlfh00 " +
                                            "FROM krzl WHERE krzlzt00 = 'I' AND (krzlfh00 = '{0}' or (krzlkrlx='G' and krzlth00='{0}'))", verifySource.SourceName);

                var rooms = db.SqlQuery<string>(sqlRoom);
                if (rooms == null || rooms.Count == 0)
                    throw new Exception(string.Format("无此在住房号（{0}）,请重新确认转客房房号！", verifySource.SourceName));
                
                //取在住主结人
                string customerSql = string.Format("SELECT TOP 1 krzlzh00 AS CustomerId, ISNULL(krzlgzxe, 0) AS LastAmount, isnull(krzlye00, 0) as LimitAmount, " +
                    "(select sum(ysq0je00) from ysq0 where ysq0zh00=k.krzlzh00) as PreAmount FROM krzl k " +
                    "WHERE krzlzt00 = 'I' AND (krzlfh00 = '{0}' or (krzlkrlx='G' and krzlth00='{0}')) AND krzlzh00 = krzltzxh", verifySource.SourceName);
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
                if(hasCode == null || hasCode.Count == 0)
                    throw new Exception("当前餐厅账务代码设置不正确，请重新确认！");

                infoList.Add(codes[0]);
                infoList.Add(customers[0].CustomerId.ToString());
                #endregion
            }
            else if(verifySource.PayMethod == (int)CyddPayType.会员卡)
            {
                var member = VerifyMemberInfo(verifySource.SourceId, verifySource.OperateValue, verifySource.SourceName);
                if(member != null && member.Id > 0)
                    infoList.Add(member.Id.ToString());
            }

            if (isOutside)
                db.Dispose();

            return infoList;
        }

        //public MemberInfoDTO GetMemberInfo(int sourceid)
        //{
        //    SqlSugarClient client = null;
        //    if (EnabelGroupFlag)
        //    {
        //        client = new SqlSugarClient(ConnentionGroup);
        //    }
        //    else
        //    {
        //        client = new SqlSugarClient(Connection);
        //    }
        //    using (client)
        //    {
        //        var data=client.Sqlable().From("krls","s1")
        //            .Where("s1.krlsxh00="+sourceid+"")
        //            .SelectToList<MemberInfoDTO>("")
        //    }
        //}

        /// <summary>
        /// 保存会员卡消费记录
        /// </summary>
        /// <param name="listOrderPayRecordDTO"></param>
        /// <param name="userCode">用户代码</param>
        /// <param name="isNormal">true：正常保存，false ：回滚金额</param>
        private List<MemberInfoDTO> SaveMemberConsumeInfo(List<OrderPayRecordDTO> listOrderPayRecordDTO, string userCode, bool isNormal,DateTime billDate,string remark="",int restaurantId=0)
        {
            List<MemberInfoDTO> res = new List<MemberInfoDTO>();
            if (EnabelGroupFlag)
            {
                using (var groupDB = new SqlSugarClient(ConnentionGroup))
                {
                    try
                    {
                        var payRecords = listOrderPayRecordDTO.Where(x => x.Id <= 0 && x.CyddPayType == (int)CyddPayType.会员卡).ToList();
                        if (payRecords != null && payRecords.Count > 0)
                        {
                            groupDB.BeginTran();
                            foreach (var item in payRecords)
                            {
                                if (item.Id > 0)
                                    continue;

                                if (item.CyddPayType == (int)CyddPayType.会员卡)
                                {
                                    res.Add(ApplyChangeMemberToDb(item.SourceId, item.Pwd, userCode, (isNormal ? item.PayAmount : -item.PayAmount), remark + item.Remark, true, groupDB,billDate,restaurantId));
                                }
                            }
                            groupDB.CommitTran();
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg1 = isNormal? "会员库记录会员卡消费信息操作失败" : "会员库回滚会员卡消费信息操作失败";
                        groupDB.RollbackTran();
                        throw new Exception(msg1 + "：" + ex.Message);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 保存会员卡消费信息到目标库
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="pwd">会员密码</param>
        /// <param name="userCode">当前操作员代码</param>
        /// <param name="amount">消费金额</param>
        /// <param name="remark">备注</param>
        /// <param name="isGroup">是否集团连接标识</param>
        /// <param name="db">目标库连接</param>
        private MemberInfoDTO ApplyChangeMemberToDb(int memberId, string pwd, string userCode, decimal amount, string remark,
            bool isGroup, SqlSugarClient db,DateTime billDate,int restaurantId)
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
                GUID = guid,
                rq = billDate
            });
            db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
            db.ExecuteCommand("p_zw_gbxf", paras);
            db.CommandType = System.Data.CommandType.Text;//还原回默认

            #region 积分
            string hyjfSql = string.Format("select top 1 xtdmbz01 from krls left join xtdm on xtdmlx00='KS' and krlskrlx = xtdmdm00 where krlsxh00 = {0} AND xtdmzs00 & 4 > 0", memberId);
            string hyjfGz = db.GetString(hyjfSql);
            if (!string.IsNullOrEmpty(hyjfGz) && hyjfGz!="0")
            {
                int memberJf = 0;
                int amountJf = 0;
                string[] strGz = hyjfGz.Split('/');
                if (strGz!=null && strGz.Length>0)
                {
                    switch (strGz[1])
                    {
                        case "1":
                            amountJf = (int)Math.Round(amount);
                            break;
                        case "2":
                            amountJf = (int)Math.Ceiling(amount);
                            break;
                        case "3":
                            amountJf = (int)Math.Floor(amount);
                            break;
                        default:
                            break;
                    }
                    memberJf = amountJf * Convert.ToInt32(strGz[0]);
                    string hyjfInsertSql = string.Format("INSERT INTO krxf(krxfkhid,krxfzwrq,krxflb00,krxfxfd0,krxfrq00,krxfczdm,krxfbz00,krxfzh00,krxfattr,krxfje01,krxfGPID) VALUES({0}, '{1}','{2}', {3}, '{4}', '{5}', '{6}', {7}, {8},{9},'{10}')", member.Id, billDate, 'Z', 1, DateTime.Now, userCode, remark, 0, 1, memberJf, list[0]);
                    db.CommandType = System.Data.CommandType.Text;
                    db.ExecuteCommand(hyjfInsertSql);
                }
            }
            #endregion

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
                    GUID = guid,
                    rq = billDate
                });
                localDb.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                localDb.ExecuteCommand("p_zw_gbxf", paras);
                localDb.CommandType = System.Data.CommandType.Text;//还原回默认
                localDb.Dispose();//销毁本地连接
            }
            return member;
        }

        /// <summary>
        /// 验证会员卡信息
        /// </summary>
        /// <param name="memberId">会员信息记录Id</param>
        /// <param name="amount">当前操作金额</param>
        /// <param name="memberPwd">会员密码</param>
        /// <param name="db"></param>
        /// <returns></returns>
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
                memberPwd = string.IsNullOrEmpty(memberPwd) ? "" : memberPwd;
                memberPwd = memberPwd ?? string.Empty;
                if (member.MemberPwd != DESEncrypt.Rc4PassHex(memberPwd))
                    throw new Exception("客户密码验证不正确！");

                if (member.CardBalance + amount > 0)
                    throw new Exception("会员消费金额已超过会员卡的剩余余额！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (isOutside)
                db.Dispose();

            return member;
        }

        public List<MemberInfoDTO> SearchMemberBy(string text)
        {
            string connString = Connection;
            if (EnabelGroupFlag)
                connString = ConnentionGroup;
            using (var db = new SqlSugarClient(connString))
            {
                try
                {
                    //搜索会员信息
                    string customerSql = string.Format(@"
                    SELECT TOP 10 krlsxh00 AS Id, krlsvpkh AS MemberCardNo, krlszjhm AS MemberIdentityNo, krlsmm00 AS MemberPwbByte, krlsye00 as CardBalance,
                           krlsdh00 AS MemberPhoneNo,LTRIM(RTRIM(krlszwxm)) AS MemberName
                    FROM krls WHERE (krlskrlx <> 'A' AND krlskrlx <> 'B') AND (krlsvpkh = '{0}' OR CAST(krlskhid AS varchar)='{0}' OR krlsdh00 = '{0}' OR krlszjhm = '{0}' OR krlszwxm LIKE '%{0}%') "
                    , text);

                    var customers = db.SqlQuery<MemberInfoDTO>(customerSql);

                    return customers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<SearchKrzlInfo> SearchRoomBy(string text)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                List<SearchKrzlInfo> infoList = new List<SearchKrzlInfo>();
                if (text.IsEmpty())
                    throw new Exception("请输入房号！");

                string sqlRoom = string.Format("SELECT TOP 1 krzlfh00 " +
                                            "FROM krzl WHERE krzlzt00 = 'I' AND (krzlfh00 = '{0}' or (krzlkrlx='G' and krzlth00='{0}'))", text);

                var rooms = db.SqlQuery<string>(sqlRoom);
                if (rooms == null || rooms.Count == 0)
                    throw new Exception(string.Format("无此在住房号（{0}）,请重新确认转客房房号！", text));

                //取在住主结人
                string customerSql = string.Format("SELECT TOP 1 krzlzh00 AS CustomerId, ISNULL(krzlgzxe,0) AS LastAmount,isnull(krzlzwxm,'') as CustomerName,isnull(krzlye00,0) as LimitAmount,krzlfh00 AS RoomNo, " +
                    "(select sum(ysq0je00) from ysq0 where ysq0zh00=k.krzlzh00) as PreAmount FROM krzl k " +
                    "WHERE krzlzt00 = 'I' AND (krzlfh00 = '{0}' or (krzlkrlx='G' and krzlth00='{0}')) AND krzlzh00 = krzltzxh", text);
                infoList = db.SqlQuery<SearchKrzlInfo>(customerSql);

                if (infoList == null || infoList.Count == 0)
                    throw new Exception(string.Format("此房号（{0}）无在住客人！", text));

                //infoList.Add(codes[0]);
                //infoList.Add(customers[0].CustomerId.ToString());
                return infoList;
            }
            
        }



        /// <summary>
        /// 验证订单信息
        /// </summary>
        /// <param name="requestDto"></param>
        public void VerifyOrderInfo(WholeOrPartialCheckoutDto requestDto)
        {
            #region 基本验证
            //查询订单台号集合
            List<CheckOutOrderTableDTO> orderTableList = _checkOutRepository.GetOrderTableListBy(requestDto.OrderId, requestDto.TableIds, requestDto.OrderTableStatus);
            
            List<int> selectedTableIds = orderTableList.Select(x => x.R_Table_Id).ToList();

            if (selectedTableIds.Count != requestDto.TableIds.Count)
                throw new Exception(string.Format("当前订单与餐台关联信息不匹配，请重新确认此订单！"));

            var checkoutTableList = orderTableList.Where(x => requestDto.TableIds.Contains(x.R_Table_Id)).ToList();
            if (checkoutTableList == null || checkoutTableList.Count == 0)
                throw new Exception(string.Format("此订单({0})下未包含相关餐台（餐台Id：{1}）信息！", requestDto.OrderId, requestDto.TableIds.Join()));

            var tableNames = checkoutTableList.Where(x => x.IsCheckOut).Select(x => x.Name).ToList();

            //判断当前台是否已经结过账
            if (tableNames != null && tableNames.Count > 0 && !requestDto.IsReCheckout)
                throw new Exception(string.Format("当前订单下的餐台（{0}）已结过账！请重新确认此单信息！", tableNames.Join()));

            #endregion
        }

        /// <summary>
        /// 验证页面的订单明细项金额、数量等是否被篡改
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="verifyObj"></param>
        public void VerifyAndCalcDetailInfo(WholeOrPartialCheckoutDto requestDto, CheckOutOrderDTO verifyObj)
        {
            if (requestDto.IsReCheckout)
                return;

            var sumPayAmount = requestDto.ListOrderPayRecordDTO
                .Where(x => (x.CyddJzType == CyddJzType.消费 || x.CyddJzType == CyddJzType.转结)).Sum(x => x.PayAmount);

            if (requestDto.Money != sumPayAmount)
                throw new Exception(string.Format("本次实收结账金额({0})数据不正确！", sumPayAmount.ToString("f2")));
            
            var reqIdList = requestDto.ListOrderDetailDTO.Select(x => x.Id).ToList();

            List<CheckOutOrderDetailDTO> detailList = new List<CheckOutOrderDetailDTO>();

            foreach (var item in verifyObj.OrderTableList)
            {
                detailList = detailList.Union(item.OrderDetailList).ToList();
            }

            var verifyIdList = detailList.Select(x => x.Id).ToList();

            var verifyIdResult = reqIdList.Except(verifyIdList);

            //判断页面请求的数据和后台查询的数据量是否相同
            if (verifyIdResult != null && verifyIdResult.Count() > 0)
                throw new Exception("非法数据操作，请勿篡改数据！");

            if (requestDto.AuthUserList != null && requestDto.AuthUserList.Count > 0)
            {
                var obj = requestDto.AuthUserList.Where(x => x.OperateType == AuthOperateTypes.折扣授权).FirstOrDefault();
                if (obj != null)
                {
                    var authUser = _oldUserRepository.GetByUserId(obj.AuthUserId);
                    requestDto.AuthPermissionDiscount = authUser.Discount / 100;
                }
            }

            List<SearchTableForCheckout> ordTableInfoList = new List<SearchTableForCheckout>();
            if (requestDto.DiscountMethod == DiscountMethods.方案折)
            {
                var list = _discountRep.GetSchemeDiscountList(
                new SchemeDiscountSearchDTO()
                {
                    MarketId = requestDto.CurrentMarketId,
                    RestaurantId = verifyObj.R_Restaurant_Id,
                    OrderId=requestDto.OrderId
                });

                var discountInfo = list.Where(x => x.Id == requestDto.SchemeDiscountId).FirstOrDefault();
                if (discountInfo == null || discountInfo.Id <= 0)
                    throw new Exception("本次账单结账所选方案折在当前餐厅分市下无效！");

                ordTableInfoList = verifyObj.OrderTableList
                    .Select(x => new SearchTableForCheckout()
                    {
                        AreaId = x.R_Area_Id,
                        OrderTableId = x.Id,
                        TableId = x.R_Table_Id,
                    }).ToList();

                var planList = discountInfo.DetailList;
                var areaId = planList.Select(x => x.AreaId).FirstOrDefault();

                foreach (var item in ordTableInfoList)
                {
                    if (item.AreaId == areaId || areaId == 0)
                        item.DiscountDetailList = planList;
                }
            }

            foreach (var reqItem in requestDto.ListOrderDetailDTO)
            {
                foreach (var verifyItem in detailList)
                {
                    if (reqItem.Id == verifyItem.Id)//验证订单明细项信息是否正确
                    {
                        if (reqItem.Price != verifyItem.Price || reqItem.Num != verifyItem.Num)
                        {
                            throw new Exception(string.Format("非法数据: 当前结账订单的明细项（{0}）数据后台验证未通过，请勿篡改数据！",
                                reqItem.ProjectName));
                        }
                        string invalidDiscountMsg1 = "非法操作: 当前的明细项（{0}）不可打折，请勿篡改数据！";
                        string invalidDiscountMsg2 = "非法操作: 当前的明细项（{0}）折扣率超过限制，请勿篡改数据！";

                        var calcDiscountAmount = MoneyFormant(verifyItem.Amount * reqItem.DiscountRate);//折后金额
                        if(requestDto.DiscountMethod == DiscountMethods.方案折)
                        {
                            foreach (var planItem in ordTableInfoList)
                            {
                                if (verifyItem.R_OrderTable_Id == planItem.OrderTableId)
                                {
                                    if (requestDto.DiscountMethod != DiscountMethods.方案折)
                                    {
                                         var plan = planItem.DiscountDetailList
                                            .Where(x => x.CategoryId == verifyItem.CategoryId).FirstOrDefault();

                                        //包含此菜品类型时判断折扣率是否相同
                                        if(plan != null && plan.Id > 0 && reqItem.DiscountRate == plan.DiscountRate)
                                        {

                                        }
                                        else if((plan == null || plan.Id == 0) && reqItem.DiscountRate == 1)//无此菜品类型判断折扣率是否为1
                                        {

                                        }
                                        else
                                        {
                                            throw new Exception(string.Format(invalidDiscountMsg2, reqItem.ProjectName));
                                        }
                                    }


                                    if (calcDiscountAmount != reqItem.DiscountedAmount)
                                        throw new Exception(string.Format("数据验证失败: 明细项({0})数据后台验证未通过，" +
                                            "此项金额({1})不正确！", reqItem.ProjectName, calcDiscountAmount));

                                    verifyItem.DiscountedAmount = calcDiscountAmount;
                                    verifyItem.DiscountRate = reqItem.DiscountRate;
                                    break;
                                }
                            }
                            break;
                        }

                        #region 验证明细项折扣率是否有效
                        
                        string msg = null;
                        if (verifyItem.CyddMxType == reqItem.CyddMxType && reqItem.CyddMxType == CyddMxType.餐饮项目)
                        {
                            if (verifyItem.IsDiscount == 0 && verifyItem.IsForceDiscount == 0 && reqItem.DiscountRate < 1)
                            {
                                msg = invalidDiscountMsg1;
                            }
                            else if (reqItem.DiscountRate > 1 || reqItem.DiscountRate < requestDto.AuthPermissionDiscount)
                            {
                                msg = invalidDiscountMsg2;
                            }
                        }
                        else if (verifyItem.CyddMxType == reqItem.CyddMxType && reqItem.CyddMxType == CyddMxType.餐饮套餐)
                        {
                            //套餐不能打强制折
                            if (verifyItem.IsDiscount == 0 && reqItem.DiscountRate < 1)
                            {
                                msg = invalidDiscountMsg1;
                            }
                            else if (reqItem.DiscountRate > 1 || reqItem.DiscountRate < requestDto.AuthPermissionDiscount)
                            {
                                msg = invalidDiscountMsg2;
                            }
                        }

                        if (!msg.IsEmpty())
                            throw new Exception(string.Format(msg, reqItem.ProjectName)); 
                        #endregion

                        //if (calcDiscountAmount != reqItem.DiscountedAmount)
                        //    throw new Exception(string.Format("数据验证失败: 明细项({0})数据后台验证未通过，此项金额({1})不正确！", reqItem.ProjectName, calcDiscountAmount));

                        verifyItem.DiscountedAmount = calcDiscountAmount;
                        verifyItem.DiscountRate = reqItem.DiscountRate;

                        break;//验证通过，跳出当前循环
                    }
                }
            }

            decimal serviceAmount = 0;
            decimal originalAmount = 0;

            //验证主订单费用信息
            foreach (var item in verifyObj.OrderTableList)
            {
                var tableOrderDetails = detailList.Where(x => x.R_OrderTable_Id == item.Id).ToList();
                decimal currTableAmount = tableOrderDetails.Sum(x => x.DiscountedAmount);
                serviceAmount += (item.ServerRate ?? 0) * currTableAmount;
                originalAmount += (currTableAmount);
            }

            //originalAmount += MoneyFormant(serviceAmount) + requestDto.Fraction;
            
            //if (originalAmount != (requestDto.Money + requestDto.ClearAmount) || originalAmount != requestDto.OriginalAmount)
            //    throw new Exception(string.Format("本次结账应收金额（{0}）数据不正确！", originalAmount.ToString("f2")));
        }        

        public List<OrderPaymentDTO> GetOrderPaymentInfo(int orderId)
        {
            var model = _orderRep.GetR_OrderById(orderId);
            if (model == null || model.Id == 0)
            {
                throw new Exception("此订单不存在!");
            }

            if (model.CyddStatus != CyddStatus.结账)
            {
                throw new Exception("此订单未结完账!");
            }

            var restaruant = _resRep.GetModel(model.R_Restaurant_Id);
            var ordTabList = _orderRep.GetOrderTableListBy(orderId, SearchTypeBy.订单Id);
            var payBatchRecords = _payRep.GetMainPayListByOrderId(orderId);

            //取餐饮账务日期 TypeId=10003
            var dateItem = _extendItemRepository.GetModelList(restaruant.R_Company_Id, 10003).FirstOrDefault();

            if (dateItem == null)
                throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

            DateTime accDate = DateTime.Today;

            if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                throw new Exception("餐饮账务日期设置错误，请联系管理员");
            
            
            //判断结账记录是否包含当前账务日期
            if (payBatchRecords.Select(x => x.BillDate).Contains(accDate))
            {
                payBatchRecords = payBatchRecords.Where(x => x.BillDate <= accDate).ToList();
            }
            else
            {
                throw new Exception("未找到此账单在当前账务日期下的结账记录，请确认！");
            }
            
            List<OrderPaymentDTO> list = new List<OrderPaymentDTO>();
            
            foreach (var payItem in payBatchRecords)
            {
                OrderPaymentDTO payment = new OrderPaymentDTO();
                var ordTableList = ordTabList.Where(x => x.R_OrderMainPay_Id == payItem.Id).ToList();
                payment.TabIdList = ordTableList.Select(x => x.R_Table_Id).ToList();
                payment.BillDate = payItem.BillDate.ToString("yyyy-MM-dd");
                payment.OrderId = orderId;
                payment.OrderMainPayId = payItem.Id;
                payment.MarketName = payItem.MarketName;
                list.Add(payment);
            }
            return list;
        }

        /// <summary>
        /// 账单反结并重置账单相关
        /// </summary>
        /// <param name="mainPayId"></param>
        /// <returns></returns>
        public CheckOutResultDTO ReverseOrder(ReverseOrderDTO req)
        {
            return _checkOutRepository.ReverseOrder(req);
        }

        public List<ProjectCheckOutStaticsDTO> GetCheckOutStatics(List<CheckOutOrderTableDTO> req)
        {
            List<ProjectCheckOutStaticsDTO> res = new List<ProjectCheckOutStaticsDTO>();
            if (req.Any())
            {
                var categoryList = _categoryRepository.GetAllCategoryList(true);
                var categoryChildList = _categoryRepository.GetChildList(true);
                List<CheckOutOrderDetailDTO> detailList = new List<CheckOutOrderDetailDTO>();
                foreach (var item in req)
                {
                    foreach (var detail in item.OrderDetailList)
                    {
                        ProjectCheckOutStaticsDTO model = new ProjectCheckOutStaticsDTO();
                        var parentCategoryId = categoryChildList.First(p => p.Id == detail.CategoryId).Pid;
                        var parentCategory = categoryList.First(p => p.Id == parentCategoryId);
                        #region
                        //decimal totalPrice = 0;
                        //decimal totalNum = 0;
                        //decimal totalOtherNum = 0;
                        //if (detail.R_OrderDetailRecord_List.Any())
                        //{
                        //    totalOtherNum = detail.R_OrderDetailRecord_List.Where(p => (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.退菜 || p.CyddMxCzType == CyddMxCzType.转出) && p.IsCalculation==true).Sum(p => p.Num);
                        //}
                        //totalNum = detail.Num - totalOtherNum;
                        //if (detail.OrderDetailAllExtends.Any())
                        //{

                        //}
                        #endregion
                        model.TotalPrice = detail.DiscountedAmount;
                        model.ParentCategoryId = parentCategoryId;
                        model.ParentCategoryName = parentCategory.Name;
                        res.Add(model);
                    }
                }
                if (res.Any())
                {
                    res = res.GroupBy(p => new { p.ParentCategoryId, p.ParentCategoryName })
                        .Select(p => new ProjectCheckOutStaticsDTO()
                        {
                            ParentCategoryId = p.Key.ParentCategoryId,
                            ParentCategoryName = p.Key.ParentCategoryName,
                            TotalPrice = p.Sum(x => x.TotalPrice)
                        }).OrderByDescending(p => p.TotalPrice).ToList();
                }
            }
            return res;
        }
    }

}