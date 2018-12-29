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
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;
using Microsoft.AspNet.SignalR;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class HomeController : AuthorizationController
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

        readonly IPrintService _printService;
        readonly IProjectRepository _projectRepository;
        readonly IPrinterRepository _printerRepository;
        public HomeController(
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
            IProjectRepository projectRepository,
            IPrinterRepository printerRepository)
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
            _projectRepository = projectRepository;
            _printerRepository = printerRepository;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// 预定
        /// </summary>
        /// <param name="tableId">桌号</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定)]
        public ActionResult Reserve(int tableId)
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

            var table = _tableRepository.GetModel(tableId);
            var markets = _marketRepository.GetList(table.RestaurantId);
            var customerList = _oldCustRepository.GetListByStatus(null);

            ViewBag.Table = table;
            ViewBag.Markets = markets;
            ViewBag.CustomerList = customerList;

            return View();
        }

        /// <summary>
        /// 预定提交
        /// </summary>
        /// <param name="req">订单数据</param>
        /// <param name="TableIds">台号列表</param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.预定)]
        public async Task<ActionResult> ReserveInfoSave(ReserveCreateDTO req, List<int> TableIds)
        {
            Response res = new Response();
            string msg = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.CreateUser = operatorUser.UserId;
                    req.TableNum = TableIds.Count;
                    req.CurrentMarketId = operatorUser.LoginMarketId;
                    req.CurrentRestaurantId = operatorUser.DepartmentId.ToInt();
                    if (req.Id == 0)
                    {
                        req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        req.CreateDate = DateTime.Now;
                        req.CyddStatus = CyddStatus.预定;
                    }
                    //取餐饮账务日期 TypeId=10003
                    var dateItem = _extendItemRepository.GetModelList(operatorUser.CompanyId.ToInt(), 10003).FirstOrDefault();

                    if (dateItem == null)
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

                    DateTime accDate = DateTime.Today;

                    if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                        throw new Exception("餐饮账务日期设置错误，请联系管理员");

                    req.AccountingDate = accDate;

                    var model = _orderHandlerSers.SaveReserveOrderHandle(req, TableIds, out msg);
                    if (model != null)
                    {
                        var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                        await hub.Clients.All.callResServiceRefersh(true);
                        res.Data = model;
                    }
                    res.Message = msg;
                }
                catch (Exception ex)
                {
                    res.Data = 0;
                    res.Message = "编辑预订操作失败: " + ex.Message;
                }
            }
            else
            {
                res.Data = 0;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 开台
        /// </summary>
        /// <param name="tableId">台号</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.开台)]
        public ActionResult OpenTable(int tableId)
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var customerSources = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10002);
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            var table = _tableRepository.GetModel(tableId);
            var markets = _marketRepository.GetList(table.RestaurantId);
            var customerList = _oldCustRepository.GetListByStatus(null);

            ViewBag.CustomerList = customerList;
            ViewBag.CustomerSources = customerSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;

            return View();
        }

        /// <summary>
        /// 开台提交
        /// </summary>
        /// <param name="req">订单数据</param>
        /// <param name="TableIds">台号列表</param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.开台)]
        public ActionResult OpenTableCreate(ReserveCreateDTO req, List<int> TableIds)
        {
            Response res = new Response();
            string msg = string.Empty;

            if (ModelState.IsValid)
            {
                var operatorUser = OperatorProvider.Provider.GetCurrent();
                req.CreateUser = operatorUser.UserId;
                req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req.CreateDate = DateTime.Now;
                req.CyddStatus = CyddStatus.开台;
                req.CurrentMarketId = operatorUser.LoginMarketId;
                req.CompanyId = operatorUser.CompanyId.ToInt();
                req.UserType = CyddCzjlUserType.员工;
                var model = _tableHandlerSers.OpenTableHandle(req, TableIds, out msg);

                if (model != null)
                {
                    res.Data = model;
                }
                res.Message = msg;
            }
            else
            {
                res.Data = 0;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 选菜
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult ChoseProject(int orderTableId)
        {
            var categorys = _categoryRepository.GetChildList();
            var Table = _orderRepository.GetTableByOrderTableId(orderTableId);

            ViewBag.Categorys = categorys;
            ViewBag.OrderTableId = orderTableId;
            ViewBag.Table = Table;

            return View();
        }

        /// <summary>
        /// 多桌选菜
        /// </summary>
        /// <param name="orderTableIds">台号列表</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult BatchChoseProject(string orderTableIds, string tablesName)
        {
            var categorys = _categoryRepository.GetChildList();
            ViewBag.Categorys = categorys;
            ViewBag.OrderTableIds = orderTableIds;
            ViewBag.TablesName = tablesName;
            return View();
        }

        /// <summary>
        /// 预定纪录
        /// </summary>
        /// <param name="tableId">台号</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.预定记录)]
        public ActionResult ReserveHistory(int tableId)
        {
            var table = _tableRepository.GetModel(tableId);
            ViewBag.Table = table;
            return View();
        }

        public ActionResult Order(int tableId, int orderId = 0, bool print = true)
        {
            return View();
        }

        /// <summary>
        /// 订单明细提交
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="orderTableIds">台号列表</param>
        /// <param name="print">是否打印厨单</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        [HttpPost]
        public ActionResult OrderDetailCreate(List<OrderDetailDTO> req, List<int> orderTableIds, CyddMxStatus status)
        {
            Response res = new Response();
            if (req == null || !req.Any())
            {
                res.Data = false;
                res.Message = "请点菜后在提交！";
                return Json(res);
            }
            if (orderTableIds == null || !orderTableIds.Any())
            {
                res.Data = false;
                res.Message = "请提交点餐的台号信息！";
                return Json(res);
            }
            if (!Enum.IsDefined(typeof(CyddMxStatus), status))
            {
                res.Data = false;
                res.Message = "请联系管理员！";
                return Json(res);
            }
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = OperatorProvider.Provider.GetCurrent();
                    bool result = _orderRepository.OrderDetailCreate(req, orderTableIds, status, userInfo, out msg);
                    res.Data = result;
                    res.Message = msg;
                    if (result)
                    {
                        var hub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                        hub.Clients.All.callResServiceRefersh(true);
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
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        /// <summary>
        /// 换台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.换桌)]
        public ActionResult ChangeTable(ChangeTableDTO req)
        {
            req.CythStatus = CythStatus.空置;
            var areas = _areaRepository.GetList(req.RestaurantId);
            var table = _tableRepository.GetModel(req.TableId);
            ViewBag.Areas = areas;
            ViewBag.Paras = req;
            ViewBag.Table = table;
            return View();
        }

        /// <summary>
        /// 换台提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeTableSubmit(ChangeTableSubmitDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.CreateUser = operatorUser.UserId;
                    //res.Data = OrderRepository.ChangeTableSubmit(req);
                    res.Data = _tableHandlerSers.ChangeTableHandle(req);
                    if (Convert.ToBoolean(res.Data) == false)
                    {
                        res.Message = "操作失败";
                    }
                }
                catch (Exception ex)
                {
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

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 并台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.并台)]
        public ActionResult JoinTable(JoinTableDTO req)
        {
            req.CythStatus = CythStatus.在用;
            var areas = _areaRepository.GetList(req.RestaurantId);
            var table = _tableRepository.GetModel(req.TableId);
            ViewBag.Areas = areas;
            ViewBag.Paras = req;
            ViewBag.Table = table;
            return View();
        }

        /// <summary>
        /// 并台提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.并台)]
        [HttpPost]
        public ActionResult JoinTableSubmit(JoinTableSubmitDTO req)
        {
            Response res = new Response() { Successed=false,Data=false};
            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.UserId = operatorUser.UserId;
                    var result = _tableHandlerSers.JoinTableHandle(req);
                    if (result)
                    {
                        res.Data = true;
                        res.Successed = true;
                    }
                    else
                    {
                        res.Message = "并台操作失败";
                    }
                }
                catch (Exception ex)
                {
                    res.Message = "并台操作失败: " + ex.Message;
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

        public ActionResult OpenUsingTable(int tableId)
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var customerSources = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10002);
            //订单类型
            var orderTypes = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), 10001);
            ViewBag.OrderTypes = orderTypes;

            customerSources.OrderBy(x => x.Sort).OrderBy(x => x.Id);

            var table = _tableRepository.GetModel(tableId);
            var markets = _marketRepository.GetList(table.RestaurantId);
            var customerList = _oldCustRepository.GetListByStatus(null);

            ViewBag.CustomerList = customerList;
            ViewBag.CustomerSources = customerSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;

            return View();
        }

        /// <summary>
        /// 拼台提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [CustomerAuthorize(Permission.拼台)]
        [HttpPost]
        public ActionResult OpenUsingTableCreate(ReserveCreateDTO req, int[] tableIds)
        {
            Response res = new Response();
            string msg = string.Empty;

            if (ModelState.IsValid)
            {
                var operatorUser = OperatorProvider.Provider.GetCurrent();
                req.CreateUser = operatorUser.UserId;
                req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req.CreateDate = DateTime.Now;
                req.CyddStatus = CyddStatus.开台;
                req.TableNum = tableIds.Length;
                req.R_Market_Id = operatorUser.LoginMarketId;
                req.CompanyId = operatorUser.CompanyId.ToInt();

                var model = _tableHandlerSers.OpenTableHandle(req, tableIds.ToList(), out msg, true);
                if (model != null)
                {
                    res.Data = model;
                }
                res.Message = msg;
            }
            else
            {
                res.Data = 0;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomerAuthorize(Permission.预定)]
        public ActionResult ReserveToOpen(int orderId)
        {
            Response res = new Response();
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    ReserveCreateDTO req = _orderRepository.GetOrderDTO(orderId);
                    req.CurrentMarketId = operatorUser.LoginMarketId;

                    var tableList = _checkOutRepository.GetOrderTableListBy(orderId);
                    var model = _tableHandlerSers.OpenTableHandle(req, tableList.Select(x => x.R_Table_Id).ToList(), out msg, false);
                    if (model != null)
                    {
                        res.Data = model;
                    }
                    res.Message = msg;
                }
                else
                {
                    res.Data = 0;
                    res.Message = string.Join(",", ModelState
                        .SelectMany(ms => ms.Value.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOrder(int tableId, int orderId)
        {
            return View();
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.取消订单)]
        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            Response res = new Response();

            try
            {
                var operatorUser = OperatorProvider.Provider.GetCurrent();
                CancelOrderOperateDTO info = new CancelOrderOperateDTO();
                info.CompanyId = operatorUser.CompanyId.ToInt();
                info.OperateUserId = operatorUser.UserId;
                info.OrderId = orderId;

                var result = _orderHandlerSers.CancelOrderHandle(info);

                if (result)
                    res.Data = true;
                else
                    res.Message = "取消订单操作失败";

            }
            catch (Exception ex)
            {
                res.Message = "取消订单操作失败: " + ex.Message;
            }
            return Json(res);
        }

        /// <summary>
        /// 设餐台状态为空置
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.设为空置)]
        public ActionResult SetTableEmpty(List<int> tableIds)
        {
            Response res = new Response();

            res.Successed = false;
            try
            {
                var result = _tableHandlerSers.UpdateTablesStatus(tableIds, CythStatus.空置);

                if (result)
                    res.Successed = true;
                else
                    res.Message = "更新餐台状态-空置操作失败";

            }
            catch (Exception ex)
            {
                res.Message = "更新餐台状态操作失败: " + ex.Message;
            }
            return Json(res);
        }

        /// <summary>
        /// 换桌提交
        /// </summary>
        /// <param name="req"></param>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.换桌)]
        public ActionResult ChangeProjectTable(List<OrderDetailDTO> req, int orderTableId)
        {
            Response res = new Response();
            if (req == null || !req.Any())
            {
                res.Data = false;
                res.Message = "请选择菜品！";
                return Json(res);
            }
            if (orderTableId <= 0)
            {
                res.Data = false;
                res.Message = "请联系管理员！";
                return Json(res);
            }
            string msg = string.Empty;
            var userInfo = OperatorProvider.Provider.GetCurrent();
            res.Data = _orderRepository.ChangeProjectToTable(req, orderTableId, userInfo, out msg);
            if (Convert.ToBoolean(res.Data) == false)
            {
                res.Message = msg;
            }
            return Json(res);
        }

        /// <summary>
        /// 多桌点餐
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        [HttpPost]
        public ActionResult ChoseProjectTable(List<OrderDetailDTO> req, List<OrderTableDTO> tables)
        {
            Response res = new Response();
            if (req == null || !req.Any())
            {
                res.Data = false;
                res.Message = "请联系管理员！";
                return Json(res);
            }
            if (tables == null || !tables.Any())
            {
                res.Data = false;
                res.Message = "请联系管理员！";
                return Json(res);
            }
            string msg = string.Empty;
            var userInfo = OperatorProvider.Provider.GetCurrent();
            res.Data = _orderRepository.ChoseProjectToTable(req, tables, userInfo, out msg);
            if (Convert.ToBoolean(res.Data) == false)
            {
                res.Message = msg;
            }
            return Json(res);
        }

        /// <summary>
        /// 拆台
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.拆台)]
        public ActionResult SeparateTable(SeparateTableDTO dto)
        {
            dto.CythStatus = CythStatus.空置;
            var areas = _areaRepository.GetList(dto.RestaurantId);
            var table = _tableRepository.GetModel(dto.TableId);
            var details = _orderRepository.GetOrderTableProjects(dto.OrderTableId);

            ViewBag.Areas = areas;
            ViewBag.Paras = dto;
            ViewBag.Table = table;
            ViewBag.OrderTableId = dto.OrderTableId;

            return View();
        }

        /// <summary>
        /// 拆台提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.拆台)]
        public ActionResult SeparateTableSubmit(SeparateTableSubmitDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.CreateUser = operatorUser.UserId;
                    res.Data = _tableHandlerSers.SeparateTableHandle(req);
                    if (Convert.ToBoolean(res.Data) == false)
                    {
                        res.Message = "操作失败";
                    }
                }
                catch (Exception ex)
                {
                    res.Message = "拆台操作失败: " + ex.Message;
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

        #region 新界面相关Action

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult NewIndex()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult NewWelcome()
        {
            return View();
        }

        /// <summary>
        /// 加载新工作台信息
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadPlatformInfo()
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var info = _restaurantHandlerSers.LoadPlatformInfo(operatorUser.DepartmentId.ToInt());
            info.LoginMarketId = operatorUser.LoginMarketId;

            return Json(info);
        }

        public ActionResult GetRestauantInfo()
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var list = _resRepository.GetList();
            list = list.Where(x => operatorUser.ManagerRestaurant.Contains(x.Id)).ToList();
            var currentResName = list
                .Where(x => x.Id == operatorUser.DepartmentId.ToInt())
                .Select(x => x.Name).FirstOrDefault();

            var markets = _marketRepository.GetList(operatorUser.ManagerRestaurant);
            foreach (var item in list)
            {
                item.MarketList = markets.Where(x => x.RestaurantId == item.Id).ToList();
            }

            var info = new
            {
                RestaurantName = currentResName,
                RestaurantList = list,
                UserName = operatorUser.UserName,
                MarketName = markets.Where(x => x.Id == operatorUser.LoginMarketId).Select(x => x.Name).FirstOrDefault(),
                NightTrial=_printerRepository.GetNightTrial()
            };

            return Json(info);
        }

        /// <summary>
        /// 新开台操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.开台)]
        public ActionResult NewOpenTable(int Id)
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
            var sales = _oldUserRepository.GetByUsersSql("ZZ");
            ViewBag.SellerList = sellerList;
            ViewBag.TableList = emptyTableList;
            ViewBag.CustomerList = customerList;
            ViewBag.CustomerSources = customerSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;
            ViewBag.Areas = areaList;
            ViewBag.UserName = operatorUser.UserName;
            ViewBag.Sales = sales;
            return View();
        }

        /// <summary>
        /// 打开新点餐界面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewBatchChoseProject(List<int> OrderTableIds)
        {
            return View();
        }

        /// <summary>
        /// 退菜
        /// </summary>
        /// <param name="req">orderdetail</param>
        /// <param name="table">(必须)Restaurant=餐厅名称,Name=台号名称</param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.退菜)]
        [HttpPost]
        public ActionResult ReturnOrderDetail(OrderDetailDTO req, TableListDTO table)
        {
            Response res = new Response();
            if (req == null)
            {
                res.Data = false;
                res.Message = "请选择菜品！";
                return Json(res);
            }
            if (table == null)
            {
                res.Data = false;
                res.Message = "非法参数！";
                return Json(res);
            }
            if (ModelState.IsValid)
            {
                string message = string.Empty;
                var userInfo = OperatorProvider.Provider.GetCurrent();
                var result = _orderRepository.ReturnOrderDetail(req, table, userInfo, out message);
                res.Data = result;
                res.Message = message;
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
        /// 打厨单
        /// </summary>
        /// <param name="req">已保存菜品明细列表</param>
        /// <param name="table">台号信息dto</param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult CookingMenu(List<OrderDetailDTO> req, TableListDTO table)
        {
            Response res = new Response();
            if (req == null || !req.Any())
            {
                res.Data = false;
                res.Message = "请选择菜品！";
                return Json(res);
            }
            if (table == null)
            {
                res.Data = false;
                res.Message = "请联系管理员！";
                return Json(res);
            }
            if (ModelState.IsValid)
            {
                string message = string.Empty;
                var userInfo = OperatorProvider.Provider.GetCurrent();
                bool result = _orderRepository.CookingMenu(req, table, userInfo, out message);
                res.Data = result;
                res.Message = message;
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
        /// 订单明细记录赠送
        /// </summary>
        /// <param name="req"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.赠菜)]
        public ActionResult CreateOrderDetailRecord(R_OrderDetailRecordDTO req)
        {
            Response res = new Response();
            if (req == null)
            {
                res.Data = false;
                res.Message = "请联系管理员！";
                return Json(res);
            }
            if (ModelState.IsValid)
            {
                string msg = string.Empty;
                var userInfo = OperatorProvider.Provider.GetCurrent();
                var result = _orderRepository.CreateOrderDetailRecord(req, userInfo, out msg);
                res.Data = result;
                res.Message = msg;
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
        /// 删除订单明细纪录
        /// </summary>
        /// <param name="req"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult DeleteOrderDetailRecord(List<R_OrderDetailRecordDTO> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    string msg = string.Empty;
                    var userInfo = OperatorProvider.Provider.GetCurrent();
                    bool result = _orderRepository.DeleteOrderDetailRecord(req, userInfo);
                    res.Data = result;
                    res.Message = msg;
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
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
        /// 更改餐饮账务日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        //[HttpPost]
        //[CustomerAuthorize(Permission.餐饮系统设置)]
        //public ActionResult UpdateBillDate(DateTime date)
        //{
        //    Response res = new Response();
        //    if (ModelState.IsValid)
        //    {
        //        res.Data = false;
        //        var operatorUser = OperatorProvider.Provider.GetCurrent();
        //        var dateItem = _extendItemRepository.GetModelList(operatorUser.CompanyId.ToInt(), 10003).FirstOrDefault();
        //        if (dateItem != null)
        //        {
        //            //餐饮账务日期

        //            if (date > DateTime.Today)
        //            {
        //                res.Message = "账务日期不能超过当前日期！";
        //            }
        //            else
        //            {
        //                var item = _extendItemRepository.GetModel(dateItem.Id);
        //                item.ItemValue = date.ToString("yyyy-MM-dd");
        //                bool result = _extendItemRepository.AddModel(item);
        //                res.Data = result;
        //                res.Message = result ? "提交成功" : "提交失败，请稍后再试";
        //            }
        //        }
        //        else
        //        {
        //            res.Message = "餐饮账务日期尚未初始化，请联系管理员";
        //        }
        //    }
        //    else
        //    {
        //        res.Data = false;
        //        res.Message = string.Join(",", ModelState
        //        .SelectMany(ms => ms.Value.Errors)
        //        .Select(e => e.ErrorMessage));
        //    }
        //    return Json(res);
        //}

        /// <summary>
        /// 日结
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[CustomerAuthorize(Permission.结账)]
        //public ActionResult BillDaySettlement()
        //{
        //    Response res = new Response();
        //    if (ModelState.IsValid)
        //    {
        //        var operatorUser = OperatorProvider.Provider.GetCurrent();

        //        //餐饮账务日期
        //        var dateItem = _extendItemRepository.GetModelList(operatorUser.CompanyId.ToInt(), 10003).FirstOrDefault();
        //        if (dateItem != null)
        //        {
        //            var item = _extendItemRepository.GetModel(dateItem.Id);

        //            var billDateNow = Convert.ToDateTime(item.ItemValue);
        //            if (billDateNow <= DateTime.Today)
        //                dateItem.Code = billDateNow.AddDays(1).ToString("yyyy-MM-dd");

        //            bool result = _extendItemRepository.AddModel(item);
        //            res.Data = result;
        //            res.Message = result ? "提交成功" : "提交失败，请稍后再试";
        //        }
        //        else
        //        {
        //            res.Data = false;
        //            res.Message = "餐饮账务日期尚未初始化，请联系管理员";
        //        }
        //    }
        //    else
        //    {
        //        res.Data = false;
        //        res.Message = string.Join(",", ModelState
        //        .SelectMany(ms => ms.Value.Errors)
        //        .Select(e => e.ErrorMessage));
        //    }
        //    return Json(res);
        //}

        /// <summary>
        /// 交班数据获取
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.结账)]
        public ActionResult DayStallStatistics(List<int> UserIds, DateTime? date)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                res.Data = _orderPayRecordRepository.GetDayStallStatistics(UserIds, date);
            }
            else
            {
                res.Data = new DayMarketStatistics();
                res.Message = string.Join(",", ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        /// <summary>
        /// 换台
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.换桌)]
        public ActionResult NewAllChangeTable()
        {
            return View();
        }

        /// <summary>
        /// 菜品转台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.换桌)]
        public ActionResult NewChangeTable()
        {
            return View();
        }

        /// <summary>
        /// 多桌点餐
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult NewChoseTable()
        {
            return View();
        }

        /// <summary>
        /// 打厨单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[CustomerAuthorize(Permission.None)]
        public ActionResult NewCookOrder()
        {
            return View();
        }

        /// <summary>
        /// 估清
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.估清)]
        public ActionResult BatchClearProject()
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

        /// <summary>
        /// 加台提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.开台)]
        public ActionResult AddTableSubmit(AddTableSubmitDTO req)
        {
            Response res = new Response();
            res.Successed = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.CreateUser = operatorUser.UserId;
                    req.CurrentMarketId = operatorUser.LoginMarketId;
                    req.CompanyId = Convert.ToInt32(operatorUser.CompanyId);
                    res.Data = _tableHandlerSers.AddTableHandle(req);
                    res.Successed = true;
                }
                catch (Exception ex)
                {
                    res.Data = null;
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

        /// <summary>
        /// 辙台提交
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult CancelOrderTable(CancelOrderTableSubmitDTO req)
        {
            Response res = new Response();
            res.Data = false;
            res.Successed = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    req.CreateUser = operatorUser.UserId;
                    res.Data = _tableHandlerSers.CancelOrderTable(req);
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
            return Json(res);
        }

        [CustomerAuthorize(Permission.菜品推荐设置)]
        public ActionResult RecomandConfig()
        {
            return View();
        }

        [HttpPost]
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult OrderTablePersonUpdate(OrderTableDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _orderRepository.UpdateOrderTablePerson(req);
                }
                catch (Exception ex)
                {
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

        [HttpPost]
        [CustomerAuthorize(Permission.点餐)]
        public ActionResult UpdateOrderTableListPrint(int orderTableId)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _orderRepository.UpdateOrderTableListPrint(orderTableId);
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
        #endregion
    }
}