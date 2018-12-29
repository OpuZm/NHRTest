using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class MHomeController : AuthorizationController
    {
        readonly IOrderRepository _orderRepository;
        readonly ITableRepository _tableRepository;
        readonly IMarketRepository _marketRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IAreaRepository _areaRepository;
        readonly ITableService _tableHandlerSers;
        readonly IOrderService _orderHandlerSers;
        readonly IRestaurantService _restaurantHandlerSers;

        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        readonly ICustomerRepository _oldCustRepository; //(旧系统)Lxdm 客户表
        readonly IUserRepository_Old _oldUserRepository; //(Czdm) 用户表
        readonly ICheckOutRepository _checkOutRepository;
        readonly IRestaurantRepository _resRepository;
        readonly IExtendTypeRepository _extendTypeRepository;
        readonly IOrderPayRecordRepository _orderPayRecordRepository;
        readonly IRestaurantRepository _restaurantRepository;//餐饮餐厅
        readonly IMarketRepository _marketRep;//餐饮分市
        readonly ITableRepository _tableRep;//餐饮台号

        readonly IPrintService _printService;

        public MHomeController(
            IOrderRepository orderRepository,
            ICategoryRepository categoryRepository,
            ITableRepository tableRepository,
            IMarketRepository marketRepository,
            IAreaRepository areaRepository,
            IExtendItemRepository extendItemRepository,
            ICustomerRepository customerRepository,
            IUserRepository_Old oldUserRepository,
            ITableService tableService,
            IOrderService orderService,
            IRestaurantService restaurantService,
            ICheckOutRepository checkOutRepository,
            IRestaurantRepository resRepository,
            IExtendTypeRepository extendTypeRepository,
            IOrderPayRecordRepository orderPayRecordRepository,
            IPrintService printService,
            IRestaurantRepository restaurantRepository,
            IMarketRepository marketRep,
            ITableRepository tableRep)
        {
            _orderRepository = orderRepository;
            _tableRepository = tableRepository;
            _marketRepository = marketRepository;
            _categoryRepository = categoryRepository;
            _areaRepository = areaRepository;
            _extendItemRepository = extendItemRepository;
            _oldCustRepository = customerRepository;
            _oldUserRepository = oldUserRepository;

            _tableHandlerSers = tableService;
            _orderHandlerSers = orderService;
            _restaurantHandlerSers = restaurantService;
            _checkOutRepository = checkOutRepository;
            _resRepository = resRepository;
            _extendTypeRepository = extendTypeRepository;
            _orderPayRecordRepository = orderPayRecordRepository;
            _printService = printService;
            _restaurantRepository = restaurantRepository;
            _marketRep = marketRep;
            _tableRep = tableRep;
        }

        // GET: MHome
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新开台操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.开台)]
        public ActionResult OpenTable(int Id)
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var customerSources = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10002);
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            var table = _tableRepository.GetModel(Id);
            var markets = _marketRepository.GetList(table.RestaurantId);

            if (markets.Where(x => x.IsDefault).Count() > 1)
            {
                int firstId = markets.Where(x => x.IsDefault).Select(x => x.Id).FirstOrDefault();
                markets.ForEach(x =>
                {
                    if (x.Id != firstId)
                        x.IsDefault = false;
                });
            }
            var sellerList = _orderRepository.GetDepartList();

            var customerList = _oldCustRepository.GetListByStatus(null);
            var areaList = _areaRepository.GetList(operatorUser.DepartmentId.ToInt());

            var emptyTableList = _tableHandlerSers.GetTableList(
                new TableSearchDTO()
                {
                    CythStatus = CythStatus.空置,
                    RestaurantId = operatorUser.DepartmentId.ToInt(),
                    CompanyId = operatorUser.CompanyId.ToInt(),
                });

            ViewBag.SellerList = sellerList;
            ViewBag.TableList = emptyTableList;
            ViewBag.CustomerList = customerList;
            ViewBag.CustomerSources = customerSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;
            ViewBag.Areas = areaList;
            ViewBag.UserName = operatorUser.UserName;
            return View();
        }

        /// <summary>
        /// 打开新点餐界面
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult BatchChoseProject()
        {
            return View();
        }

        /// <summary>
        /// 菜品转台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.换桌)]
        public ActionResult ChangeTable()
        {
            return View();
        }

        /// <summary>
        /// 多桌点餐
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult ChoseTable()
        {
            return View();
        }

        /// <summary>
        /// 换台
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.换桌)]
        public ActionResult AllChangeTable()
        {
            return View();
        }

        /// <summary>
        /// 加台
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.开台)]
        public ActionResult AddTable()
        {
            return View();
        }

        [CustomerAuthorize(Permission.并台)]
        public ActionResult JoinTable()
        {
            return View();
        }

        /// <summary>
        /// 拼台界面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.拼台)]
        public ActionResult SpellTable(int Id)
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var customerSources = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10002);
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            var table = _tableRepository.GetModel(Id);
            var markets = _marketRepository.GetList(table.RestaurantId);

            if (markets.Where(x => x.IsDefault).Count() > 1)
            {
                int firstId = markets.Where(x => x.IsDefault).Select(x => x.Id).FirstOrDefault();
                markets.ForEach(x =>
                {
                    if (x.Id != firstId)
                        x.IsDefault = false;
                });
            }

            //var sellerResult = _oldUserRepository.GetAll<Smooth.IoC.UnitOfWork.ISession>();
            //var sellerList = AutoMapperExtend<CzdmModel, UserInfo>.ConvertToList(sellerResult, new List<UserInfo>());
            var sellerList = _orderRepository.GetDepartList();

            var customerList = _oldCustRepository.GetListByStatus(null);
            var areaList = _areaRepository.GetList(operatorUser.DepartmentId.ToInt());

            var emptyTableList = _tableHandlerSers.GetTableList(
                new TableSearchDTO()
                {
                    CythStatus = CythStatus.空置,
                    RestaurantId = operatorUser.DepartmentId.ToInt(),
                    CompanyId = operatorUser.CompanyId.ToInt(),
                });

            ViewBag.SellerList = sellerList;
            ViewBag.TableList = emptyTableList;
            ViewBag.CustomerList = customerList;
            ViewBag.CustomerSources = customerSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;
            ViewBag.Areas = areaList;
            ViewBag.UserName = operatorUser.UserName;
            return View();
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult Reserve(int id = 0)
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

            var restaurantId = currentUser.DepartmentId.ToInt();
            var resModel = _restaurantRepository.GetModel(restaurantId);

            var markets = _marketRep.GetList(restaurantId);
            var customerList = _oldCustRepository.GetListByStatus(null);
            var areaList = _areaRepository.GetList(currentUser.DepartmentId.ToInt());

            //var sellerResult = _oldUserRepository.GetAll<Smooth.IoC.UnitOfWork.ISession>();
            //var sellerList = AutoMapperExtend<CzdmModel, UserInfo>.ConvertToList(sellerResult, new List<UserInfo>());
            var sellerList = _orderRepository.GetDepartList();
            var sales = _oldUserRepository.GetByUsersSql("ZZ");
            List<TableListDTO> conditionList = null;
            if (orderInfo != null)
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
            ViewBag.OrderTableIds = orderInfo != null ? string.Join(",", orderInfo.OrderTableIds) : "";
            return View();
        }
    }
}