using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Infrastructure.Common.Net;
using OPUPMS.Domain.Restaurant.Services;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class ApiController : BaseController
    {
        readonly IDiscountRepository DiscountRepository;
        readonly IRestaurantRepository RestaurantRepository;
        readonly IAreaRepository AreaRepository;
        readonly ICategoryRepository CategoryRepository;
        readonly IMarketRepository MarketRepository;
        readonly IBoxRepository BoxRepository;
        readonly IProjectRepository ProjectRepository;
        readonly ITableRepository TableRepository;
        readonly IOrderRepository OrderRepository;
        readonly IExtendRepository ExtendRepository;

        readonly ITableService TableServices;
        // GET: Api
        public ApiController(IDiscountRepository _DiscountRepository, IRestaurantRepository _RestaurantRepository, IAreaRepository _AreaRepository, ICategoryRepository _CategoryRepository, IMarketRepository _MarketRepository, IBoxRepository _BoxRepository, IProjectRepository _ProjectRepository, ITableRepository _TableRepository, IOrderRepository _OrderRepository, IExtendRepository _ExtendRepository, ITableService tableService)
        {
            DiscountRepository = _DiscountRepository;
            RestaurantRepository = _RestaurantRepository;
            AreaRepository = _AreaRepository;
            CategoryRepository = _CategoryRepository;
            MarketRepository = _MarketRepository;
            BoxRepository = _BoxRepository;
            ProjectRepository = _ProjectRepository;
            TableRepository = _TableRepository;
            OrderRepository = _OrderRepository;
            ExtendRepository = _ExtendRepository;

            TableServices = tableService;
        }

        /// <summary>
        /// 获取所有二级分类
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCategorys()
        {
            Response res = new Response();
            res.Data = CategoryRepository.GetChildList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllCategorys()
        {
            Response res = new Response();
            res.Data = CategoryRepository.GetList(0);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有餐厅
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRestaurants()
        {
            Response res = new Response();
            res.Data = RestaurantRepository.GetList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有分市
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMarkets()
        {
            //Response res = new Response();
            //res.Data = MarketRepository.GetList();
            //return Json(res, JsonRequestBehavior.AllowGet);
            return NewtonSoftJson(new
            {
                Data = MarketRepository.GetList()
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAreas()
        {
            return NewtonSoftJson(new
            {
                Data = AreaRepository.GetList(0)
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有包厢
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBoxs()
        {
            Response res = new Response();
            res.Data = BoxRepository.GetList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据分类获取餐饮项目
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult GetProjects(int category)
        {
            Response res = new Response();
            res.Data = ProjectRepository.GetList(category);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有台号状态
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTableStatus()
        {
            Response res = new Response();
            res.Data = TableRepository.GetStatus();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据查询条件获取台号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult GetTables(TableSearchDTO req)
        {
            Response res = new Response();
            res.Data = TableRepository.GetList(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据查询条件获取当天预定列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult GetChoseTables(TableChoseSearchDTO req)
        {
            Response res = new Response();
            res.Data = TableRepository.GetReseverChoseList(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReseverChoseTables(TableChoseSearchDTO req)
        {
            var tables = TableRepository.GetReseverChoseList(req);
            var areas = AreaRepository.GetList(req.RestaurantId);
            return Json(new
            {
                areas=areas,
                tables=tables
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据条件获取台号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult GetOpenChoseTables(TableSearchDTO req)
        {
            Response res = new Response();
            var currentUser = OperatorProvider.Provider.GetCurrent();
            req.CompanyId = currentUser.CompanyId.ToInt();
            res.Data = TableServices.GetTableList(req);
            //res.Data = TableRepository.GetOpenTableChoseList(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单台号下所选菜品
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public ActionResult GetOrderTableProjects(int orderTableId)
        {
            Response res = new Response();
            res.Data = OrderRepository.GetOrderTableProjects(orderTableId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取菜品特殊要求
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult GetProjectExtend(int projectId)
        {
            Response res = new Response();
            res.Data = ProjectRepository.GetProjectExtends(projectId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据台号获取订单列表
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public ActionResult GetReserveOrdersByTable(int tableId)
        {
            Response res = new Response();
            Nullable<DateTime> minDate = null;
            res.Data = OrderRepository.GetReserveOrdersByTable(tableId, minDate);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据订单台号id获取订单下所有台号列表
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public ActionResult GetTablesByOrderTableId(int orderTableId)
        {
            Response res = new Response();
            res.Data = OrderRepository.GetTablesByOrderTableId(orderTableId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}