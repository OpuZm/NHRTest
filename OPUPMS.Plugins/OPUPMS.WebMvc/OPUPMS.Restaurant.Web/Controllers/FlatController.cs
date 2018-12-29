using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Infrastructure.Common.Operator;
using Microsoft.AspNet.SignalR;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Restaurant.Web.Models;
using System.Threading.Tasks;
using OPUPMS.Domain.Base;
using System.Web.Security;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using System.Web.Script.Serialization;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class FlatController : BaseController
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
        private readonly IAreaRepository _areaRepository;
        private readonly IUserService _userService;
        private readonly ICustomerRepository _oldCustRepository; //(旧系统)Lxdm 客户表

        public FlatController(IRestaurantRepository restaurantRepository,
            IRestaurantService restaurantService, IMarketRepository marketRepository,
            ICategoryRepository categoryRepository, IRestaurantCategoryRepository restaurantCategoryRepository,
            IProjectRepository projectRepository, IOrderRepository orderRepository,
            ITableService tableHandlerSers, IExtendItemRepository extendItemRepository,
            IPackageRepository packageRepository, ICheckOutService checkOutService, IPayMethodRepository payMethodRepository,
            IAreaRepository areaRepository, IUserService userService, ICustomerRepository customerRepository)
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
            _areaRepository = areaRepository;
            _userService = userService;
            _oldCustRepository = customerRepository;
        }
        // GET: Flat
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChoseProject()
        {
            return View();
        }

        /// <summary>
        /// 获取所有餐饮项目分类和餐饮项目类别
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public ActionResult GetAllCategoryProject(int restaurantId)
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var categorys = _categoryRepository.GetAllCategoryList();
                    var projects = _projectRepository.GetProjectAndDetailList(0, true);
                    var projectExtendSplitList = _projectRepository.GetProjectExtendSplitListNew();
                    projects = _restaurantCategoryRepository.FilterRestaurantCategorys(projects, restaurantId);
                    categorys = _restaurantCategoryRepository.FilterRestaurantCategorys(categorys, restaurantId);
                    var info = new
                    {
                        CategoryList = categorys,
                        ProjectAndDetails = projects,
                        ProjectExtendSplitList = projectExtendSplitList
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
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(res),
                ContentType = "application/json"
            };
            //return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取餐厅和分市信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRestaurants()
        {
            var res = new Response() { Data = null, Successed = false };
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
        /// 获取换台数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitAllChangeTableInfo (int restaurantId, OperatorModel user)
        {
            //获取当前餐厅下的区域
            var areas = _areaRepository.GetList(restaurantId);
            //获取当前餐厅下所有台号
            TableSearchDTO req = new TableSearchDTO()
            {
                RestaurantId = restaurantId,
                CythStatus = CythStatus.在用
            };
            var tables = _tableHandlerSers.GetTableListForApi(req);
            var customerSources = _extendItemRepository
                .GetModelList(user.CompanyId.ToInt(), 10002);
            var orderTypes = _extendItemRepository
                .GetModelList(user.CompanyId.ToInt(), 10001);
            var customerList = _oldCustRepository.GetListByStatusSimple(null);
            var markets = _marketRepository.GetList(restaurantId);

            if (markets.Where(x => x.IsDefault).Count() > 1)
            {
                int firstId = markets.Where(x => x.IsDefault).Select(x => x.Id).FirstOrDefault();
                markets.ForEach(x =>
                {
                    if (x.Id != firstId)
                        x.IsDefault = false;
                });
            }
            var info = new
            {
                Areas = areas,
                Tables = tables,
                CustomerList = customerList,
                CustomerSources = customerSources,
                Markets = markets,
                OrderTypes = orderTypes
            };
            //return new ContentResult
            //{
            //    Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(info),
            //    ContentType = "application/json",
            //};
            return Json(info);
        }

        [HttpPost]
        public async Task<JsonResult> SubmitOrder(ReserveCreateDTO req, List<int> tableIds, List<OrderDetailDTO> list, OperatorModel user, CyddMxStatus status)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    req.CreateUser = user.UserId;
                    req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    req.CreateDate = DateTime.Now;
                    req.CyddStatus = CyddStatus.开台;
                    req.CurrentMarketId = user.LoginMarketId;
                    req.CompanyId = user.CompanyId.ToInt();
                    req.UserType = CyddCzjlUserType.员工;
                    req.CyddStatus = CyddStatus.开台;
                    res.Data = _orderRepository.FlatOrderSubmit(req,tableIds,list,user,status);
                    res.Successed = Convert.ToBoolean(res.Data);
                    if (Convert.ToBoolean(res.Data)==true)
                    {
                        var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                        await hub.Clients.All.callResServiceRefersh(true);
                    }
                }
                catch (Exception ex)
                {
                    res.Data = false;
                    res.Message = ex.Message;
                    res.Successed = false;
                }
            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        //[CustomerAuthorize(Permission.点餐)]
        [HttpPost]
        public async Task<JsonResult> SubmitOrderAdd(List<OrderDetailDTO> req, List<int> orderTableIds, CyddMxStatus status, OperatorModel user)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    string msg = string.Empty;
                    if (!Enum.IsDefined(typeof(CyddMxStatus), status))
                    {
                        res.Message = "非法参数！";
                        return Json(res);
                    }
                    bool result = _orderRepository.OrderDetailCreate(req, orderTableIds, status, user, out msg);
                    res.Data = result;
                    res.Message = msg;
                    res.Successed = result;
                    if (result)
                    {
                        var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                        await hub.Clients.All.callResServiceRefersh(true);
                    }
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
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        public ActionResult JoinTable()
        {
            return View();
        }
    }
}