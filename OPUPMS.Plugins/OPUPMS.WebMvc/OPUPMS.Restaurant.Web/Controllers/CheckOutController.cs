using OPUPMS.Web.Framework.Core.Mvc;
using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Infrastructure.Common;
using System;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using System.Web.Script.Serialization;
using System.Linq;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Repositories.OldRepositories;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class CheckOutController : AuthorizationController
    {
        readonly ICheckOutRepository _checkOutRepository;
        readonly IOrderRepository _orderRepository;
        readonly IUserRepository_Old _oldUserRepository; //(Czdm) 用户表
        readonly ICheckOutService _checkOutService;
        readonly IPrintService _printService;
        readonly IUserService _userService;
        readonly ICustomerRepository _oldCustRepository;//Old system Lxdm 
        readonly IOrderRecordRepository _orderRecordRepository;//操作记录
        readonly IPayMethodRepository _payMethodRepository;
        readonly IPrinterRepository _printerRepository;

        public CheckOutController(
            ICheckOutRepository checkOutRepository,
            IOrderRepository orderRepository,
            IOrderRecordRepository orderRecordRepository,
            IUserRepository_Old oldUserRepository,
            ICustomerRepository customerRepository,
            ICheckOutService checkOutService,
            IPrintService printService,
            IUserService userService,
            IPayMethodRepository payMethodRepository,
            IPrinterRepository printerRepository
            )
        {
            _checkOutRepository = checkOutRepository;
            _orderRepository = orderRepository;
            _orderRecordRepository = orderRecordRepository;
            _oldUserRepository = oldUserRepository;
            _oldCustRepository = customerRepository;
            _checkOutService = checkOutService;
            _printService = printService;
            _userService = userService;
            _payMethodRepository = payMethodRepository;
            _printerRepository = printerRepository;
        }

        /// <summary>
        /// 返回订单和订单下所有台号
        /// </summary>
        /// <param name="req">订单结账请求参数</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        public ActionResult Index(string req)
        {
            var list = EnumToList.ConvertEnumToList(typeof(CyddPayType));
            ViewBag.CyddPayTypeList = list.Where(x => x.Key > 0).ToList(); //EnumHelper.GetEnumDic<CyddPayType>(); //支付方式列表
            ViewBag.Req = req;
            return View();
        }


        /// <summary>
        /// 返回订单和订单下所有台号
        /// </summary>
        /// <param name="req">订单结账请求参数</param>
        /// <returns></returns>
        //[CustomerAuthorize(Permission.结账)]
        public ActionResult GetOrderInfo(CheckoutReqDTO checkoutReqDTO)
        {
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                checkoutReqDTO.OrderTableStatus = OrderTableStatus.未结;
                CheckOutOrderDTO checkoutOrder = GetCheckOutOrder(checkoutReqDTO);
                var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });
                //var payTypeList =  EnumToList.ConvertEnumToList(typeof(CyddPayType));
                var payTypeList = _payMethodRepository.GetList();

                checkoutOrder.OperateUser = currentUser.UserId;
                checkoutOrder.OperateUserName = currentUser.UserName;
                checkoutOrder.TableIds = checkoutReqDTO.TableIds;
                //checkoutOrder.PayTypeList = payTypeList.Where(x => x.Key > 0).ToList();
                checkoutOrder.PayTypeList = payTypeList;
                checkoutOrder.DiscountRate = user.MinDiscountValue;
                checkoutOrder.AuthClearValue = user.MaxClearValue;
                checkoutOrder.PaidRecordList = checkoutOrder.PaidRecordList
                        .Where(x => x.CyddJzType == CyddJzType.定金 
                                && x.CyddJzStatus == CyddJzStatus.已付).ToList();//只取当前订单已付的定金记录
                checkoutOrder.PrintModel = _printerRepository.GetPrintModel();
                checkoutOrder.CheckOutStaticsList = _checkOutService.GetCheckOutStatics(checkoutOrder.OrderTableList);
                checkoutOrder.CheckOutRemovePayType = _payMethodRepository.GetCheckOutRemovePayType();
                return Json(checkoutOrder);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// 返回反结订单和订单下所有台号
        /// </summary>
        /// <param name="req">订单订单反结请求参数</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        public ActionResult GetOrderInfoForReCheckout(CheckoutReqDTO checkoutReqDTO)
        {
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                checkoutReqDTO.OrderTableStatus = OrderTableStatus.已结;
                CheckOutOrderDTO checkoutOrder = GetCheckOutOrder(checkoutReqDTO);
                var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });

                //var payTypeList = EnumToList.ConvertEnumToList(typeof(CyddPayType));
                var payTypeList = _payMethodRepository.GetList();
                checkoutOrder.OperateUser = currentUser.UserId;
                checkoutOrder.OperateUserName = currentUser.UserName;
                checkoutOrder.TableIds = checkoutReqDTO.TableIds;
                checkoutOrder.PayTypeList = payTypeList;
                checkoutOrder.DiscountRate = user.MinDiscountValue;
                checkoutOrder.AuthClearValue = user.MaxClearValue;
                var mainPayId = checkoutOrder.OrderTableList.GroupBy(x => x.R_OrderMainPay_Id).Select(x => x.Key).FirstOrDefault();

                checkoutOrder.ClearAmount = checkoutOrder.PaidRecordList
                    .Where(x => x.CyddJzType == CyddJzType.抹零 && x.CyddJzStatus == CyddJzStatus.已付 
                                && x.OrderMainPayId == mainPayId)
                    .Sum(x => x.PayAmount);

                checkoutOrder.Fraction = checkoutOrder.PaidRecordList
                    .Where(x => x.CyddJzType == CyddJzType.四舍五入 && x.CyddJzStatus == CyddJzStatus.已付
                                && x.OrderMainPayId == mainPayId)
                    .Sum(x => x.PayAmount);

                checkoutOrder.ServiceAmount = checkoutOrder.PaidRecordList
                    .Where(x => x.CyddJzType == CyddJzType.服务费 && x.CyddJzStatus == CyddJzStatus.已付
                                && x.OrderMainPayId == mainPayId)
                    .Sum(x => x.PayAmount);

                checkoutOrder.DiscountAmount = checkoutOrder.PaidRecordList
                    .Where(x => x.CyddJzType == CyddJzType.折扣 && x.CyddJzStatus == CyddJzStatus.已付
                                && x.OrderMainPayId == mainPayId)
                    .Sum(x => x.PayAmount);
                
                //只取当次结账已付的消费记录
                checkoutOrder.PaidRecordList = checkoutOrder.PaidRecordList
                        .Where(x => (x.CyddJzType == CyddJzType.转结 || x.CyddJzType == CyddJzType.消费)
                                && x.OrderMainPayId == mainPayId).ToList();

                return Json(checkoutOrder);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        private CheckOutOrderDTO GetCheckOutOrder(CheckoutReqDTO checkoutReqDTO)
        {
            CheckOutOrderDTO checkoutOrder = _checkOutService.GetCheckOutOrderDTO(checkoutReqDTO.OrderId, checkoutReqDTO.TableIds, checkoutReqDTO.OrderTableStatus);
            return checkoutOrder;
        }

        #region 结账相关

        /// <summary>
        /// 单品打折弹出画面
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult SingleProductDiscount(String orderDetailIDs)
        {
            var jss = new JavaScriptSerializer();
            List<int> listOrderDetailID = jss.Deserialize<List<int>>(orderDetailIDs);

            var currentUser = OperatorProvider.Provider.GetCurrent();
            var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });
            //查询OrderDetail
            var listOrderDetail = _orderRepository.GetOrderDetails(listOrderDetailID);
            ViewBag.Discount = user.MinDiscountValue;
            return View(listOrderDetail);
        }

        /// <summary>
        /// 单品折扣率设置
        /// </summary>
        /// <param name="req">单品信息请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SingleProductDiscountSet(List<SingleProductDiscountSetRequestDto> req)
        {
            try
            {
                //修改单品折扣率
                this._orderRepository.UpdateOrderDetailDiscounts(req);

                return Json(new { result = true, Info = "操作成功" });
            }
            catch (Exception e)
            {
                return Json(new { result = false, Info = e.Message });
            }
        }



        /// <summary>
        /// 全单折弹出画面
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public ActionResult FullOrderDiscount()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });
            ViewBag.Discount = user.MinDiscountValue;
            return View();
        }


        /// <summary>
        /// 全单折折扣率设置
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public JsonResult FullOrderDiscountSet(int orderId, decimal discountRate)
        {
            try
            {
                var orderDto = this._orderRepository.GetOrderDTO(orderId);

                //修改单品折扣率
                orderDto.DiscountRate = discountRate;
                this._orderRepository.Update(orderDto);
                return Json(new { result = true, Info = "操作成功" });
            }
            catch (Exception e)
            {
                return Json(new { result = false, Info = e.Message });
            }
        }


        /// <summary>
        /// 方案折弹出画面
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public ActionResult SchemeDiscount(int orderId)
        {
            //获取订单所属的 餐厅，分市  是否启用 预定时间
            var orderDto = this._orderRepository.GetOrderDTO(orderId);
            //orderDto.R_Restaurant_Id；//所属餐厅
            //orderDto.R_Market_Id;//所属分市场

            //是否启用
            //orderDto.ConAmount =;
            return View(orderDto);
        }

        [HttpPost]
        [CustomerAuthorize(Permission.点餐)]
        public JsonResult BeforeCheckout(CheckOutBillDTO billDto, WholeOrPartialCheckoutDto req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = OperatorProvider.Provider.GetCurrent();
                    var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });
                    var discount = user.MinDiscountValue;
                    req.OrderTableStatus = OrderTableStatus.未结;
                    req.CompanyId = currentUser.CompanyId.ToInt();
                    req.OperateUser = currentUser.UserId;
                    req.CurrentMarketId = currentUser.LoginMarketId;
                    req.AuthPermissionDiscount = discount;
                    req.OperateUserCode = currentUser.UserCode;
                    res.Data = _checkOutService.BeforeWholeOrPartialCheckout(req);
                    _printService.CheckedOutBill(billDto);
                }
                catch (Exception ex)
                {
                    res.Data = false;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        /// <summary>
        /// 整单全部结账或部分结账
        /// </summary>
        /// <returns>JSON</returns>
        [HttpPost]
        [CustomerAuthorize(Permission.结账)]
        public JsonResult Checkout(WholeOrPartialCheckoutDto req)
        {
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });
                var discount = user.MinDiscountValue;
                req.OrderTableStatus = OrderTableStatus.未结;
                req.CompanyId = currentUser.CompanyId.ToInt();
                req.OperateUser = currentUser.UserId;
                req.CurrentMarketId = currentUser.LoginMarketId;
                req.AuthPermissionDiscount = discount;
                req.OperateUserCode = currentUser.UserCode;
                if (_checkOutService.CheckoutVaild(req.ListOrderPayRecordDTO, currentUser.DepartmentId.ToInt()))
                {
                    CheckOutResultDTO resultDto= _checkOutService.WholeOrPartialCheckout(req);
                    return Json(new { Result = true, Info = "操作成功",Data= resultDto });
                }
                else
                {
                    return Json(new { Result = false, Info = "可挂账限额不足" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Info = "结账操作失败 - " + e.Message });
            }
        }

        /// <summary>
        /// 订单反结结账
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult ReCheckout(WholeOrPartialCheckoutDto req)
        {
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                var user = _userService.GetUserInfo(new VerifyUserDTO() { UserId = currentUser.UserId });
                var discount = user.MinDiscountValue;
                req.OrderTableStatus = OrderTableStatus.已结;
                req.CompanyId = currentUser.CompanyId.ToInt();
                req.OperateUser = currentUser.UserId;
                req.CurrentMarketId = currentUser.LoginMarketId;
                req.AuthPermissionDiscount = discount;
                req.OperateUserCode = currentUser.UserCode;

                _checkOutService.ReCheckout(req);
                return Json(new { Result = true, Info = "操作成功 " });
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Info = "结账操作失败 - " + e.Message });
            }

        }


        /// <summary>
        /// 根据订单号，获取订单下面的台号集合
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult GetOrderTablesByOrderId(int orderId)
        {
            try
            {
                List<CheckOutOrderTableDTO> tableList = _checkOutRepository.GetOrderTableListBy(orderId);
                return Json(tableList);
            }
            catch (Exception e)
            {
                return Json(new { result = false, Info = "获取订单下面的台号出现异常 " + e.Message });
            }

          
        }

        /// <summary>
        /// 按台号结账
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult BillByTables()
        {
            return null;
        }

        /// <summary>
        /// 获取授权用户信息
        /// </summary>
        /// <param name="verifyUserDTO"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        [HttpPost]
        public ActionResult GetAuthUser(VerifyUserDTO verifyUserDTO)
        {
            Response res = new Response();
            try
            {
                verifyUserDTO.UserPwd = verifyUserDTO.UserPwd ?? "";
                verifyUserDTO.RestaurantId = OperatorProvider.Provider.GetCurrent().DepartmentId.ToInt();
                var user = _userService.GetUserInfo(verifyUserDTO);

                res.Data = user;

                if (user.State == Domain.Base.LoginState.InvalidAccount)
                    res.Message = "无效的用户！";
                else if (user.State == Domain.Base.LoginState.InvalidPassword)
                    res.Message = "用户密码错误！";
                else if (user.State == Domain.Base.LoginState.NoPermission)
                    res.Message = "用户无权限操作！请确认该用户是否可管理当前餐厅";
                else if (user.State == Domain.Base.LoginState.Successed)
                    res.Message = "";
                else
                    res.Message = "网络错误，请重新操作！";
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return Json(res);
        }

        /// <summary>
        /// 反结订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        public ActionResult GetOrderTableIds(int orderId)
        {
            Response res = new Response();
            res.Successed = false;
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                var payInfo = _checkOutService.GetOrderPaymentInfo(orderId);
                if (payInfo != null && payInfo.Count > 0)
                {
                    res.Successed = true;
                    res.Data = payInfo;
                }
                else
                    res.Message = "此订单当前账务日期无结账记录，请重新确认！";

                return Json(res);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return Json(res);
        }

        /// <summary>
        /// 反结订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        public ActionResult ReturnCheckout(string orderId, string tableIds)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();

            var userResult = _oldUserRepository.GetAll<Smooth.IoC.UnitOfWork.ISession>();
            var userList = AutoMapperExtend<CzdmModel, UserInfo>.ConvertToList(userResult, new List<UserInfo>());
            
            var order = _orderRepository.GetOrderDTO(orderId.ToInt());
            ViewBag.CustomerId = (order == null || order.Id <= 0) ? 0 : order.CustomerId;

            var customerList = _oldCustRepository.GetListByStatus(null);
            ViewBag.CustomerList = customerList;
            ViewBag.UserList = userList;

            return View();            
        }
        #endregion

        #region 新界面Action

        /// <summary>
        /// 按台号结账
        /// </summary>
        /// <returns>JSON</returns>
        public ActionResult OpenCheckout(string orderId, string tableIds)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            
            var userResult = _oldUserRepository.GetAll<Smooth.IoC.UnitOfWork.ISession>();
            var userList = AutoMapperExtend<CzdmModel, UserInfo>.ConvertToList(userResult, new List<UserInfo>());

            var order = _orderRepository.GetOrderDTO(orderId.ToInt());
            ViewBag.CustomerId = (order == null || order.Id <= 0) ? 0 : order.CustomerId;

            var customerList = _oldCustRepository.GetListByStatus("Y");
            ViewBag.CustomerList = customerList;
            ViewBag.UserList = userList;

            return View("Checkout");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchMemberInofBy(string text)
        {
            Response res = new Response();
            try
            {
                if (text.IsEmpty() || text.Trim().IsEmpty())
                    throw new Exception("请输入信息查询");
                var memberList = _checkOutService.SearchMemberBy(text);
                if (memberList == null || memberList.Count == 0)
                    throw new Exception("未查询到相关的客户信息！");
                res.Data = memberList;
                res.Successed = true;
            }
            catch (Exception ex)
            {
                res.Successed = false;
                res.Message = ex.Message;
            }

            return Json(res);
        }

        /// <summary>
        /// 新方案折弹出画面
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public ActionResult NewSchemeDiscount(int orderId)
        {
            //获取订单所属的 餐厅，分市  是否启用 预定时间
            var orderDto = this._orderRepository.GetOrderDTO(orderId);
            return View(orderDto);
        }

        /// <summary>
        /// 打结账单
        /// </summary>
        /// <param name="req">结账详情</param>
        /// <param name="isLocked">是否锁单</param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.结账)]
        public ActionResult CheckOutBill(CheckOutBillDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    bool result= _printService.CheckedOutBill(req);
                    res.Data = result;
                    res.Message = result ? "操作成功" : "打单台数有误或台号下存在保存菜品，请核对后再进行操作";
                    return Json(res);
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
                    return Json(res);
                }
                
            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
                return Json(res);
            }
        }

        /// <summary>
        /// 解锁台号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.结账)]
        public ActionResult Unlock(CheckOutBillDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = _printService.Unlock(req);
                    res.Data = result;
                    res.Message = result ? "操作成功" : "解锁台数有误，请核对后再进行操作";
                    return Json(res);
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
                    return Json(res);
                }

            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
                return Json(res);
            }
        }

        /// <summary>
        /// 反结订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        [HttpPost]
        public ActionResult VerifyByOutsideInfo(int id, string name, decimal money, CyddPayType payType)
        {
            Response res = new Response();
            res.Successed = false;
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                var verifyInfo = new VerifySourceInfoDTO();
                verifyInfo.SourceId = id;
                verifyInfo.SourceName = name;
                verifyInfo.RestaruantId = currentUser.DepartmentId.ToInt();
                verifyInfo.PayMethod = (int)payType;
                verifyInfo.OperateValue = money;
                var resultInfo = _checkOutService.VerifyOutsideInfo(verifyInfo);
                if (resultInfo != null && resultInfo.Count > 0)
                    res.Successed = true;
                else
                    res.Message = "网络故障，请稍后重新操作！";
                res.Data = resultInfo;

                return Json(res);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult SearchMemberInfo(string text)
        {
            Response res = new Response();
            res.Successed = false;
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                var verifyInfo = new VerifySourceInfoDTO();
                verifyInfo.RestaruantId = currentUser.DepartmentId.ToInt();
                var resultInfo = _checkOutService.VerifyOutsideInfo(verifyInfo);
                if (resultInfo != null && resultInfo.Count > 0)
                    res.Successed = true;
                else
                    res.Message = "网络故障，请稍后重新操作！";
                res.Data = resultInfo;

                return Json(res);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult SearchRoomInfoBy(string text)
        {
            Response res = new Response();
            try
            {
                if (text.IsEmpty() || text.Trim().IsEmpty())
                    throw new Exception("请输入房号查询");
                var memberList = _checkOutService.SearchRoomBy(text);
                if (memberList == null || memberList.Count == 0)
                    throw new Exception("未查询到相关的客户信息！");
                res.Data = memberList;
                res.Successed = true;
            }
            catch (Exception ex)
            {
                res.Successed = false;
                res.Message = ex.Message;
            }

            return Json(res);
        }

        /// <summary>
        /// 返回订单和订单下所有台号
        /// </summary>
        /// <param name="req">订单结账请求参数</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult OrderSearchById(int orderId)
        {
            try
            {
                var ordTabList = _orderRepository.GetOrderTableListBy(orderId, SearchTypeBy.订单Id);
                var tabIdList = ordTabList.Select(x => x.R_Table_Id).ToList();
                var currentUser = OperatorProvider.Provider.GetCurrent();

                var order = GetCheckOutOrder(new CheckoutReqDTO() { OrderId = orderId, TableIds = tabIdList });

                OrderSearchDTO orderSearchDTO = new OrderSearchDTO(order);

                var recordSearch = new OrderRecordSearchDTO();
                recordSearch.OrderId = orderId;
                recordSearch.RestaurantId = currentUser.DepartmentId.ToInt();

                var recordList = _orderRecordRepository
                    .GetList(recordSearch, out int total)
                    .OrderByDescending(x => x.Id);
                orderSearchDTO.OrderRecordList = recordList.ToList();
                orderSearchDTO.InvoiceList = _orderRepository.GetInvoiceList(orderId);
                var user = _oldUserRepository.GetByUserId(order.CreateUser);
                orderSearchDTO.CreateUserName = (user == null || user.UserId == 0) ? "Unknow" : user.UserName;

                return Json(orderSearchDTO);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// 反结账单
        /// </summary>
        /// <param name="mainPayId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        [HttpPost]
        public ActionResult ReverseOrder(int mainPayId)
        {
            Response res = new Response();
            res.Successed = true;
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = OperatorProvider.Provider.GetCurrent();
                    ReverseOrderDTO req = new ReverseOrderDTO()
                    {
                        MainPayId = mainPayId,
                        UserId = currentUser.UserId,
                        CompanyId=currentUser.CompanyId.ToInt(),
                        CurrentMarketId = currentUser.LoginMarketId,
                        UserCode=currentUser.UserCode
                };
                    res.Data = _checkOutService.ReverseOrder(req);
                }
                catch (Exception ex)
                {
                    res.Successed = false;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Data = null;
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }
        #endregion
    }
}