using System;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class OrderController : AuthorizationController
    {
        readonly IOrderRepository _orderRepository;//餐饮订单
        readonly IRestaurantRepository _restaurantRepository;//餐饮餐厅
        readonly IAreaRepository _areaRepository;//区域        
        readonly ITableRepository _tableRep;//餐饮台号
        readonly IMarketRepository _marketRep;//餐饮分市
        readonly IOrderRecordRepository _orderRecordRepository;//操作记录
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        readonly ICustomerRepository _oldCustRepository;//Old system Lxdm 
        readonly IUserRepository_Old _oldUserRepository; //(Czdm) 用户表
        readonly ITableService _tableHandlerSers;
        readonly IOrderPayRecordRepository _payRep;
        readonly IOrderService _orderService;
        readonly IPrinterRepository _printerRepository;

        public OrderController(
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            IAreaRepository areaRepository,
            IMarketRepository marketRepository,
            ITableRepository tableRepository,
            IOrderRecordRepository orderRecordRepository,
            IExtendItemRepository extendItemRepository,
            ICustomerRepository customerRepository,
            IUserRepository_Old oldUserRepository,
            IOrderPayRecordRepository payRecordRepository,
            ITableService tableHandlerSers,
            IOrderService orderService,
            IPrinterRepository printerRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
            _areaRepository = areaRepository;
            _marketRep = marketRepository;
            _tableRep = tableRepository;
            _orderRecordRepository = orderRecordRepository;
            _extendItemRepository = extendItemRepository;
            _oldCustRepository = customerRepository;
            _oldUserRepository = oldUserRepository;
            _payRep = payRecordRepository;
            _tableHandlerSers = tableHandlerSers;
            _orderService = orderService;
            _printerRepository = printerRepository;
        }

        public ActionResult CancelReserveOrder(int orderId)
        {
            return View();
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult EditReserve(int orderId)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var order = _orderRepository.GetOrderModel(orderId);
            var markets = _marketRep.GetList(order.R_Restaurant_Id);

            var tableList = order.Tables;
            var ids = tableList.Select(x => x.Id).ToList();
            var names = tableList.Select(x => x.Name).ToArray().Join();
            var customerList = _oldCustRepository.GetListByStatus(null);
            ViewBag.CustomerList = customerList;

            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            ViewBag.TableIds = ids.Join(",");
            ViewBag.TableName = names;
            ViewBag.Markets = markets;
            ViewBag.Order = order;
            return View();
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult DepositManager(int orderId)
        {
            ViewBag.Order = _orderRepository.GetOrderModel(orderId);
            return View();
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult GetOrderDeposits(int orderId)
        {
            Response res = new Response();
            var data = _payRep.GetPaidRecordListByOrderId(orderId);
            data = data.Where(x => x.CyddJzType == CyddJzType.定金).ToList();
            res.Data = data;

            return Json(new
            {
                rows = data,
                total = data.Count,
                code = 0,
                msg = "",
                PrintModel = _printerRepository.GetPrintModel()
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult OrderDepositCreate(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        [CustomerAuthorize(Permission.预定)]
        [HttpPost]
        public ActionResult OrderDepositCreate(OrderPayHistoryDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();

                var order = _orderRepository.GetOrderModel(req.OrderId);
                if (order == null || order.Id <= 0)
                {
                    res.Successed = false;
                    res.Message = "无法找到此预订订单，请重新确认！";
                }
                else if (order.ReserveDate < DateTime.Now)
                {
                    res.Successed = false;
                    res.Message = "此预订订单预订日期已过期！";
                }
                else
                {
                    req.CreateUser = currentUser.UserId;
                    req.CreateDate = DateTime.Now;
                    req.CyddJzStatus = CyddJzStatus.已付;
                    req.CyddJzType = CyddJzType.定金;
                    var dateItem = _extendItemRepository.GetModelList(currentUser.CompanyId.ToInt(), 10003).FirstOrDefault();
                    if (dateItem != null)
                    {
                        var billDateNow = Convert.ToDateTime(dateItem.ItemValue);
                        if (billDateNow > DateTime.Today)
                        {
                            res.Successed = false;
                            res.Message = "账务日期不能超过当前日期！";
                        }
                        else
                        {
                            req.MarketId = currentUser.LoginMarketId;
                            req.BillDate = billDateNow;
                            req.R_Restaurant_Id = currentUser.DepartmentId.ToInt();
                            var result = _orderRepository.OrderDepositCreate(req);
                            res.Successed = result;
                            res.Message = result ? "操作成功" : "操作失败，请联系管理员";
                        }
                    }
                    else
                    {
                        res.Successed = false;
                        res.Message = "餐饮账务日期尚未初始化，请联系管理员";
                    }
                }
            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult OrderList()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            //餐厅
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;

            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;

            //订单状态
            var statusList = EnumToList.ConvertEnumToList(typeof(CyddStatus));
            statusList = statusList.Where(x => x.Key != (int)CyddStatus.预定).ToList();

            ViewBag.StatusList = statusList;

            return View();
        }

        public ActionResult OrderListSearch(OrderListSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var currentUser = OperatorProvider.Provider.GetCurrent();
            req.ManagerRestaurant = currentUser.ManagerRestaurant;

            var list = _orderRepository.GetOrderList(out int total, req);
            return Json(new
            {
                rows = list,
                total = total,
                code = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预定订单
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定)]
        public ActionResult OrderReserveList()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            //餐饮
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;

            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            return View();
        }

        /// <summary>
        /// 预定订单查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定)]
        public ActionResult OrderReserveListSearch(OrderListSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var currentUser = OperatorProvider.Provider.GetCurrent();
            req.ManagerRestaurant = currentUser.ManagerRestaurant;

            var list = _orderRepository.GetOrderList(out int total, req);
            return Json(new
            {
                rows = list,
                total = total,
                code = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult OrderEdit(int id)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            var customerList = _oldCustRepository.GetListByStatus(null);
            ViewBag.CustomerList = customerList;

            ReserveCreateDTO model = _orderRepository.GetOrder(id);
            var restaurant = _restaurantRepository.GetModel(model.R_Restaurant_Id);
            model.RestaurantName = restaurant.Name;
            ViewBag.Order = model;
            return View();
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderRecord(int id)
        {
            ViewBag.id = id;
            //var list = _orderRecordRepository.GetList(id).OrderByDescending(x => x.Id);
            var order = _orderRepository.GetOrder(id);
            //var tables = _tableRep.GetTables(new int[] { order.R_Restaurant_Id }, null, 0);
            var tables = _tableRep.GetTables(id);

            var restaurant = _restaurantRepository.GetModel(order.R_Restaurant_Id);
            ViewBag.RestaurantName = restaurant.Name;
            ViewBag.TableList = tables;
            ViewBag.OrderId = order.Id;
            //ViewBag.TableId = 0;

            return View();
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult OrderRecordSearch(OrderRecordSearchDTO searchDTO)
        {
            var list = _orderRecordRepository
                .GetList(searchDTO, out int total)
                .OrderByDescending(x => x.Id);

            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        [CustomerAuthorize(Permission.订单管理)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult OrderEditSave(ReserveCreateDTO req)
        {
            Response res = new Response();
            res.Data = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)//修改
                    {
                        res.Data = _orderRepository.UpdateOrderInfo(req);
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {

                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Message = string.Join(",",
                    ModelState.SelectMany(m => m.Value.Errors).
                    Select(e => e.ErrorMessage));
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预定预测
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定记录)]
        public ActionResult Forecast()
        {
            //餐饮名称
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var resModel= _restaurantRepository.GetModel(Convert.ToInt32(operatorUser.DepartmentId));
            ViewBag.RestaurantName = resModel.Name;

            //默认预定日期
            string beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            string endDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            ViewBag.beginDate = beginDate;
            ViewBag.endDate = endDate;

            return View();
        }
        public JsonResult ForecastSearch(ForecastSearchDTO req)
        {
            Response res = new Response();
            res.Successed = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.BeginDate == null || req.BeginDate < DateTime.Today)
                        throw new Exception("起始日期错误，最小起始日期不能小于当前日期！");
                    if (req.BeginDate > req.EndDate)
                        throw new Exception("无效日期条件，起始日期不能大于截止日期！");
                    if (req.EndDate > DateTime.Today.AddDays(30))
                        throw new Exception("最大查询截止日期不能超过当前日期一个月！");
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.Restaurant = operatorUser.DepartmentId.ToInt();

                    var info = _orderService.ForecastSearch(req);
                    res.Data = info;
                    res.Successed = true;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        #region 新界面Action

        /// <summary>
        /// 预定预测
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定记录)]
        public ActionResult NewForecast()
        {
            //餐饮名称
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var resModel = _restaurantRepository.GetModel(Convert.ToInt32(operatorUser.DepartmentId));
            ViewBag.RestaurantName = resModel.Name;

            //默认预定日期
            string beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            string endDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            ViewBag.beginDate = beginDate;
            ViewBag.endDate = endDate;

            return View();
        }

        /// <summary>
        /// 预定订单
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定)]
        public ActionResult NewOrderReserveList()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var resIds = currentUser.ManagerRestaurant.Join(",").Split(',');

            //餐厅
            var restaurants = _restaurantRepository.GetList(resIds);
            ViewBag.Restaurants = restaurants;

            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;

            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);

            ViewBag.OrderTypes = orderTypes;
            ViewBag.RestaurantId = currentUser.DepartmentId.ToInt();
            return View();
        }

        /// <summary>
        /// 预定
        /// </summary>
        /// <param name="tableId">桌号</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定)]
        public ActionResult NewReserve(int id = 0)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            ReserveCreateDTO orderInfo = null;
            if (id > 0)
                orderInfo = _orderRepository.GetOrderModel(id);

            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;

            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            var restaurantId = orderInfo==null? currentUser.DepartmentId.ToInt():orderInfo.R_Restaurant_Id;
            var resModel = _restaurantRepository.GetModel(restaurantId);

            //var markets = _marketRep.GetList(restaurantId);
            var markets = _marketRep.GetList();
            var customerList = _oldCustRepository.GetListByStatus(null);
            var areaList = _areaRepository.GetList(restaurantId);
            var restaurantList = _restaurantRepository.GetList();
            restaurantList = restaurantList.Where(p => currentUser.ManagerRestaurant.Contains(p.Id)).ToList();

            //var sellerResult = _oldUserRepository.GetAll<Smooth.IoC.UnitOfWork.ISession>();
            //var sellerList = AutoMapperExtend<CzdmModel, UserInfo>.ConvertToList(sellerResult, new List<UserInfo>());
            var sellerList = _orderRepository.GetDepartList();
            var sales = _oldUserRepository.GetByUsersSql("ZZ");
            List<TableListDTO> conditionList = null;
            if(orderInfo != null)
            {
                conditionList = _tableRep.GetReseverChoseList(
                    new TableChoseSearchDTO()
                    {
                        Market = orderInfo.R_Market_Id,
                        RestaurantId = orderInfo.R_Restaurant_Id,
                        ReverDate = (DateTime)orderInfo.ReserveDate
                    });

                orderInfo.Tables.ForEach(
                    x =>
                    {
                        conditionList.Add(new TableListDTO()
                        {
                            IsSelected = true,
                            AreaId = x.AreaId,
                            Name = x.Name,
                            Id = x.Id,
                            RestaurantId = x.RestaurantId,
                            Description = x.Description
                        });

                    });

                conditionList = conditionList.OrderBy(x => x.Id).ToList();
                orderInfo.Tables = orderInfo.Tables.OrderBy(x => x.Id).ToList();

                markets.ForEach(
                    x =>
                    {
                        x.IsDefault = false;
                        if (x.Id == orderInfo.R_Market_Id)
                            x.IsDefault = true;
                    });
            }

            ViewBag.Markets = markets;
            ViewBag.CustomerList = customerList;
            ViewBag.Restaurant = resModel;

            var emptyTableList = _tableHandlerSers.GetTableList(
                new TableSearchDTO()
                {
                    CythStatus = CythStatus.空置,
                    RestaurantId = currentUser.DepartmentId.ToInt(),
                    CompanyId = currentUser.CompanyId.ToInt(),
                });

            ViewBag.SearchTableList = conditionList;

            ViewBag.OrderInfo = orderInfo;
            ViewBag.TableList = emptyTableList;
            ViewBag.SellerList = sellerList;
            ViewBag.CustomerSources = customerSources;
            ViewBag.restaurantId = restaurantId;
            ViewBag.Areas = areaList;
            ViewBag.UserName = currentUser.UserName;
            ViewBag.LoginUserId = currentUser.UserId;
            ViewBag.Sales = sales;
            ViewBag.OrderTableIds = orderInfo!=null? string.Join(",",orderInfo.OrderTableIds):"";
            ViewBag.RestaurantList = restaurantList;
            return View();
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult NewOrderList()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var resIds = currentUser.ManagerRestaurant.Join(",").Split(',');
            
            //餐厅
            var restaurants = _restaurantRepository.GetList(resIds);
            ViewBag.Restaurants = restaurants;

            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;

            //订单状态
            var statusList = EnumToList.ConvertEnumToList(typeof(CyddStatus));
            statusList = statusList.Where(x => x.Key != (int)CyddStatus.预定).ToList();

            ViewBag.StatusList = statusList;
            ViewBag.RestaurantId = currentUser.DepartmentId.ToInt();

            return View();
        }

        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult NewOrderEdit(int id,bool isReview=false)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            //客源类型
            var customerSources = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10002);
            ViewBag.CustomerSources = customerSources;
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            var customerList = _oldCustRepository.GetListByStatus(null);
            ViewBag.CustomerList = customerList;

            //var sellerResult = _oldUserRepository.GetAll<Smooth.IoC.UnitOfWork.ISession>();
            //var sellerList = AutoMapperExtend<CzdmModel, UserInfo>.ConvertToList(sellerResult, new List<UserInfo>());
            var sellerList = _orderRepository.GetDepartList();

            ViewBag.SellerList = sellerList;
            ReserveCreateDTO model = _orderRepository.GetOrder(id);
            var restaurant = _restaurantRepository.GetModel(model.R_Restaurant_Id);
            model.RestaurantName = restaurant.Name;
            ViewBag.Order = model;

            var markets = _marketRep.GetList(model.R_Restaurant_Id);
            var sales = _oldUserRepository.GetByUsersSql("ZZ");
            ViewBag.Markets = markets;
            ViewBag.IsReview = isReview;
            ViewBag.Sales = sales;
            return View();
        }

        [CustomerAuthorize(Permission.订单管理)]
        public ActionResult OrderSearchList()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(currentUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            //订单状态
            var statusList = EnumToList.ConvertEnumToList(typeof(CyddStatus));
            statusList = statusList.Where(x => x.Key < 8 || x.Key==(int)CyddStatus.反结).ToList();
            var marketList = _marketRep.GetList(currentUser.DepartmentId.ToInt());
            var tableList = _tableRep.GetList(new TableSearchDTO() { RestaurantId= currentUser.DepartmentId.ToInt() });

            ViewBag.MarketList = marketList;
            ViewBag.StatusList = statusList;
            ViewBag.RestaurantId = currentUser.DepartmentId.ToInt();
            ViewBag.IsDelete = (currentUser.Permission & (int)Permission.删除订单) > 0 ? true : false;
            ViewBag.IsSearchDelete = (currentUser.Permission & (int)Permission.查看删除订单) > 0 ? true : false;
            ViewBag.CustomerList= _oldCustRepository.GetListByStatus(null);
            ViewBag.TableList = tableList;
            return View();
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult DepositList(int orderId)
        {
            var includes = new [] { (int)CyddPayType.现金, (int)CyddPayType.微信, (int)CyddPayType.支付宝, (int)CyddPayType.银行卡 };

            //支付类型
            var payTypeList = EnumToList.ConvertEnumToList(typeof(CyddPayType));
            payTypeList = payTypeList.Where(x => includes.Contains(x.Key)).ToList();
            ViewBag.PayTypeList = payTypeList;
            ViewBag.OrderId = orderId;
            return View();
        }


        [CustomerAuthorize(Permission.预定)]
        [HttpPost]
        public ActionResult RefundDepositAmount(int id)
        {
            Response res = new Response();
            res.Successed = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = OperatorProvider.Provider.GetCurrent();
                    bool result = _orderService.RefundDepositHandler(
                        new RefundDepositDTO()
                        {
                            CompanyId = currentUser.CompanyId.ToInt(),
                            CurrentMarketId = currentUser.LoginMarketId,
                            CurrentUserId = currentUser.UserId,
                            OrigianlDepositId = id,
                            RestaurantId = currentUser.DepartmentId.ToInt()
                        });

                    res.Message = result ? "退定金操作成功" : "退定金操作失败，请稍后重拾或类型管理员！"; ;
                    res.Successed = result;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                }                
            }
            else
            {
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchOrder(OrderListSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var currentUser = OperatorProvider.Provider.GetCurrent();
            req.ManagerRestaurant = currentUser.ManagerRestaurant;

            var info = _orderRepository.GetOrderStatisticalList(req);
            return Json(new
            {
                rows = info.OrderList,
                total = info.ListSummaryObj.TotalRecords,
                summaryObj = info.ListSummaryObj,
                code = 0,
                msg = "",
                PrintModel=_printerRepository.GetPrintModel()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除(恢复)订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.删除订单)]
        [HttpPost]
        public ActionResult DeleteOrder(OrderDeleteDTO req)
        {
            Response res = new Response();
            res.Data = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = OperatorProvider.Provider.GetCurrent();
                    req.CompanyId = currentUser.CompanyId.ToInt();
                    res.Data = _orderRepository.OrderDelete(req);
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        /// <summary>
        /// 创建发票
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.订单管理)]
        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = OperatorProvider.Provider.GetCurrent();
                    req.CreateUser = currentUser.UserId;
                    req.CreateDate = DateTime.Now;
                    res.Data = _orderService.CreateOrderInvoice(req);
                }
                catch (Exception ex)
                {
                    res.Data = false;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
                res.Data = false;
            }
            return Json(res);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.订单管理)]
        [HttpPost]
        public ActionResult DeleteInvoice(int id)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _orderRepository.DeleteInvoice(id);
                }
                catch (Exception ex)
                {
                    res.Data = false;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
                res.Data = false;
            }
            return Json(res);
        }

        public ActionResult GetInvoice(int id)
        {
            Response res = new Response();
            try
            {
                res.Data = _orderService.GetInvoice(id);
                res.Successed = true;
            }
            catch (Exception ex)
            {
                res.Successed = false;
                res.Message = ex.Message;
            }
            return Json(res);
        }



        [HttpPost]
        public ActionResult UpdateOrderTableIsControl(List<int> ordertableIds,bool isControl)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    bool result= _orderRepository.UpdateOrderTableIsControl(ordertableIds, isControl);
                    if (result)
                    {
                        var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                        hub.Clients.All.callResServiceRefersh(true);
                    }
                    res.Data = result;
                }
                catch (Exception ex)
                {
                    res.Data = false;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
                res.Data = false;
            }
            return Json(res);
        }

        [CustomerAuthorize(Permission.夜审)]
        public ActionResult BeforeNightTrial()
        {
            Response res = new Response() { Data = true };
            try
            {
                int count = _orderRepository.GetOrderCountBeforeNightTrial();
                if (count > 0)
                {
                    res.Message = string.Format("当前有 {0} 个订单未结账,",count);
                }
            }
            catch (Exception e)
            {
                res.Data = false;
                res.Message = e.Message;
            }
            return Json(res);
        }

        [CustomerAuthorize(Permission.夜审)]
        [HttpPost]
        public ActionResult NightTrial()
        {
            Response res = new Response();
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                res.Data = _orderRepository.NightTrial(currentUser.CompanyId.ToInt(),currentUser.UserCode);
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.Message = ex.Message;
            }
            return Json(res);
        }

        [CustomerAuthorize(Permission.点餐)]
        [HttpPost]
        public ActionResult RemindOrder(int orderTableId,List<int> detailIds)
        {
            Response res = new Response();
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                res.Data = _orderRepository.RemindOrder(orderTableId, detailIds, currentUser);
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.Message = ex.Message;
            }
            return Json(res);
        }
        #endregion
    }
}