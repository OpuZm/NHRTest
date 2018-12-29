using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using System.Web.Mvc;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Infrastructure.Common.Operator;
using Microsoft.AspNet.SignalR;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class WeiXinController : BaseController
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantService _restaurantService;
        private readonly IMarketRepository _marketRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRestaurantCategoryRepository _restaurantCategoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITableService _tableHandlerSers;
        private readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        private readonly IPackageRepository _packageRepository;
        private readonly ICheckOutService _checkOutService;
        private readonly IPayMethodRepository _payMethodRepository;

        public WeiXinController(IRestaurantRepository restaurantRepository, 
            IRestaurantService restaurantService, IMarketRepository marketRepository,
            ICategoryRepository categoryRepository, IRestaurantCategoryRepository restaurantCategoryRepository,
            IProjectRepository projectRepository, IOrderRepository orderRepository,
            ITableService tableHandlerSers, IExtendItemRepository extendItemRepository,
            IPackageRepository packageRepository, ICheckOutService checkOutService, IPayMethodRepository payMethodRepository)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantService = restaurantService;
            _marketRepository = marketRepository;
            _categoryRepository = categoryRepository;
            _restaurantCategoryRepository = restaurantCategoryRepository;
            _projectRepository = projectRepository;
            _orderRepository = orderRepository;
            _tableHandlerSers = tableHandlerSers;
            _extendItemRepository = extendItemRepository;
            _packageRepository = packageRepository;
            _checkOutService = checkOutService;
            _payMethodRepository = payMethodRepository;
        }

        /// <summary>
        /// 获取餐厅和分市信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRestaurants()
        {
            var res = new Response() { Data=null,Successed=false};
            if (ModelState.IsValid)
            {
                try
                {
                    var restaurants = _restaurantRepository.GetList();
                    var markets = _marketRepository.GetList();
                    foreach (var item in restaurants)
                    {
                        item.MarketList = markets.Where(x => x.RestaurantId == item.Id).ToList();
                    }
                    res.Successed = true;
                    res.Data = restaurants;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取餐厅区域,台号实时状态信息
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public JsonResult GetResInfos(int restaurantId)
        {
            var res= new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data=_restaurantService.LoadPlatformInfo(restaurantId);
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取餐厅菜品类别
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public JsonResult GetCategorys(int restaurantId)
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var categorys = _categoryRepository.GetAllCategoryList();
                    categorys = _restaurantCategoryRepository.FilterRestaurantCategorys(categorys, restaurantId);
                    res.Data = categorys;
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取餐厅点餐菜品
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public JsonResult GetProjects(int restaurantId)
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var projects = _projectRepository.GetProjectAndDetailList(0, true);
                    projects = projects.Where(p => p.IsUpStore > 0).ToList();
                    projects= _restaurantCategoryRepository.FilterRestaurantCategorys(projects, restaurantId);
                    res.Data = projects;
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有餐饮项目分类和餐饮项目类别
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public JsonResult GetAllCategoryProject()
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var categorys = _categoryRepository.GetAllCategoryList();
                    var projects = _projectRepository.GetList(0);
                    var packages = _packageRepository.GetList();
                    var info = new
                    {
                        Categorys = categorys,
                        Projects= projects.Where(p=>p.IsOnSale),
                        Packages=packages.Where(p=>p.IsOnSale)
                    };
                    res.Data = info;
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单相关信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult GetOrderInfo(int orderId)
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var ordTabList = _orderRepository.GetOrderTableListBy(orderId, SearchTypeBy.订单Id);
                    var tabIdList = ordTabList.Select(x => x.R_Table_Id).ToList();
                    CheckOutOrderDTO order = _checkOutService.GetCheckOutOrderDTO(orderId, tabIdList, OrderTableStatus.所有);
                    OrderSearchDTO orderSearchDTO = new OrderSearchDTO(order);
                    res.Data = orderSearchDTO;
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取订单台号下点餐菜品
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public JsonResult GetOrderTableInfo(int orderTableId)
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var orderTableInfo = _orderRepository.GetOrderAndTablesByOrderTableId(orderTableId);
                    var orderTableProjects= _orderRepository.GetOrderTableProjects(orderTableId);//已点菜品
                    var info = new
                    {
                        OrderAndTables = orderTableInfo,
                        OrderTableProjects = orderTableProjects
                    };
                    res.Data = info;
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        /// <summary>
        /// 开台
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<JsonResult> OpenTableSubmit(ReserveCreateDTO req, int tableId)
        {
            var res = new Response() { Data = null, Successed = false };
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    List<int> tableIds = new List<int>{ tableId };
                    var restaurant = _restaurantRepository.GetModel(req.R_Restaurant_Id);
                    var orderTypes = _extendItemRepository
                        .GetModelList(restaurant.R_Company_Id, 10001);
                    var customerSources = _extendItemRepository
                        .GetModelList(restaurant.R_Company_Id, 10002);
                    req.OrderType = orderTypes.First().Id;
                    req.CyddOrderSource = customerSources.First().Id;
                    req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    req.CreateDate = DateTime.Now;
                    req.CyddStatus = CyddStatus.开台;
                    req.CurrentMarketId = req.R_Market_Id;
                    req.CompanyId = restaurant.R_Company_Id;
                    req.UserType = CyddCzjlUserType.会员;
                    var model = _tableHandlerSers.OpenTableHandle(req, tableIds, out msg);

                    if (model != null)
                    {
                        res.Data = model;
                        res.Successed = true;
                    }
                    res.Message = msg;
                    var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    await hub.Clients.All.callResServiceRefersh(true);
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        [HttpPost]
        /// <summary>
        /// 点菜
        /// </summary>
        /// <param name="req"></param>
        /// <param name="orderTableId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<JsonResult> OrderDetailSubmit(List<OrderDetailDTO> req, OperatorModel member, int orderTableId, CyddMxStatus status)
        {
            var res = new Response() { Data = null, Successed = false };
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    List<int> orderTableIds = new List<int> { orderTableId};
                    var userInfo = new OperatorModel()
                    {
                        UserId = member.UserId,
                        UserName= member.UserName,
                        UserCode=member.UserCode,
                        
                    };
                    if (!Enum.IsDefined(typeof(CyddMxStatus), status))
                    {
                        res.Message = "非法参数！";
                        return Json(res);
                    }
                    bool result = _orderRepository.OrderDetailCreate(req, orderTableIds, status, userInfo, out msg,CyddCzjlUserType.会员,true);
                    if (result)
                    {
                        result = _orderRepository.WeixinPrint(req, orderTableIds, status, userInfo, CyddCzjlUserType.会员);
                    }
                    res.Data = result;
                    res.Message = msg;
                    res.Successed = result;
                    var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    await hub.Clients.All.callResServiceRefersh(true);
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        /// <summary>
        /// 获取结账订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult GetOrderCheckOut(CheckoutReqDTO checkoutReqDTO)
        {
            var res = new Response() { Data = null, Successed = false };
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    checkoutReqDTO.OrderTableStatus = OrderTableStatus.未结;
                    CheckOutOrderDTO checkoutOrder = _checkOutService.GetCheckOutOrderDTO(checkoutReqDTO.OrderId, checkoutReqDTO.TableIds, checkoutReqDTO.OrderTableStatus);
                    checkoutOrder = _checkOutService.GetWeixinPayDTO(checkoutOrder);
                    var payTypeList = _payMethodRepository.GetList();
                    checkoutOrder.PayTypeList = payTypeList;
                    res.Data = checkoutOrder;
                    res.Successed = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> CheckOutOrder(WholeOrPartialCheckoutDto req, OperatorModel member)
        {
            var res = new Response() { Data = false, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var orderInfo = _orderRepository.GetOrderDTO(req.OrderId);
                    var user = new VerifyUserDTO() { UserId = member.UserId };
                    //var discount = user.MinDiscountValue;
                    req.OrderTableStatus = OrderTableStatus.未结;
                    req.CompanyId = orderInfo.R_Restaurant_Id;
                    req.OperateUser = member.UserId;
                    req.CurrentMarketId = orderInfo.R_Market_Id;
                    req.AuthPermissionDiscount = 0;
                    req.OperateUserCode = member.UserCode;
                    CheckOutResultDTO resultDto = _checkOutService.WholeOrPartialCheckout(req, CyddCzjlUserType.会员);
                    if (resultDto != null)
                    {
                        res.Successed = true;
                        res.Data = true;
                        var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                        await hub.Clients.All.callResServiceRefersh(true);
                    }
                    else
                    {
                        res.Message = "结账失败,请联系管理员";
                    }
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }
    }
}
