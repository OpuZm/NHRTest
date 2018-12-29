using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Infrastructure.Common.Net;
using OPUPMS.Web.Restaurant.Models;
using OPUPMS.Domain.Restaurant.Services;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Web.Restaurant.Controllers
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
        // GET: Api
        public ApiController(IDiscountRepository _DiscountRepository, IRestaurantRepository _RestaurantRepository, IAreaRepository _AreaRepository, ICategoryRepository _CategoryRepository, IMarketRepository _MarketRepository, IBoxRepository _BoxRepository, IProjectRepository _ProjectRepository, ITableRepository _TableRepository, IOrderRepository _OrderRepository, IExtendRepository _ExtendRepository)
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
        }

        public ActionResult GetCategorys()
        {
            Response res = new Response();
            res.Data = CategoryRepository.GetChildList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRestaurants()
        {
            Response res = new Response();
            res.Data = RestaurantRepository.GetList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMarkets()
        {
            Response res = new Response();
            res.Data = MarketRepository.GetList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAreas()
        {
            Response res = new Response();
            res.Data = AreaRepository.GetList(0);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBoxs()
        {
            Response res = new Response();
            res.Data = BoxRepository.GetList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjects(int category)
        {
            Response res = new Response();
            res.Data = ProjectRepository.GetList(category);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTableStatus()
        {
            Response res = new Response();
            res.Data = TableRepository.GetStatus();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTables(TableSearchDTO req)
        {
            Response res = new Response();
            res.Data = TableRepository.GetList(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChoseTables(TableChoseSearchDTO req)
        {
            Response res = new Response();
            res.Data = TableRepository.GetReseverChoseList(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOpenChoseTables(TableSearchDTO req)
        {
            Response res = new Response();
            res.Data = TableRepository.GetOpenTableChoseList(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderTableProjects(int orderTableId)
        {
            Response res = new Response();
            res.Data = OrderRepository.GetOrderTableProjects(orderTableId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectExtend(int projectId)
        {
            Response res = new Response();
            res.Data = ProjectRepository.GetProjectExtends(projectId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReserveOrdersByTable(int tableId)
        {
            Response res = new Response();
            Nullable<DateTime> minDate = null;
            res.Data = OrderRepository.GetReserveOrdersByTable(tableId, minDate);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}