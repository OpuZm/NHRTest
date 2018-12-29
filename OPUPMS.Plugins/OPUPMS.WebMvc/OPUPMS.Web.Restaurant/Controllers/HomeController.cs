using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class HomeController : BaseController
    {
        readonly IOrderRepository OrderRepository;
        readonly ITableRepository TableRepository;
        readonly IMarketRepository MarketRepository;
        readonly ICategoryRepository CategoryRepository;
        readonly IAreaRepository AreaRepository;
        public HomeController(IOrderRepository _OrderRepository, ICategoryRepository _CategoryRepository, ITableRepository _TableRepository,IMarketRepository _MarketRepository, IAreaRepository _AreaRepository)
        {
            OrderRepository = _OrderRepository;
            TableRepository = _TableRepository;
            MarketRepository = _MarketRepository;
            CategoryRepository = _CategoryRepository;
            AreaRepository = _AreaRepository;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Reserve(int tableId)
        {
            var orderSources = OrderRepository.GetOrderSources();
            var table = TableRepository.GetModel(tableId);
            var markets = MarketRepository.GetList(table.Restaurant);
            ViewBag.OrderSources = orderSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;
            return View();
        }

        [HttpPost]
        public ActionResult ReserveCreate(ReserveCreateDTO req, List<int> TableIds)
        {
            Response res = new Response();
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req.CreateDate = DateTime.Now;
                req.CyddStatus = CyddStatus.预定;
                var model = OrderRepository.ReserveCreate(req, TableIds, out msg);
                if (model!=null)
                {
                    res.Data = model.Id;
                }
                res.Message = msg;
            }
            else
            {
                res.Data = 0;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenTable(int tableId)
        {
            var orderSources = OrderRepository.GetOrderSources();
            var table = TableRepository.GetModel(tableId);
            var markets = MarketRepository.GetList(table.Restaurant);
            ViewBag.OrderSources = orderSources;
            ViewBag.Table = table;
            ViewBag.Markets = markets;
            return View();
        }

        public ActionResult OpenTableCreate(ReserveCreateDTO req, List<int> TableIds)
        {
            Response res = new Response();
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                req.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req.CreateDate = DateTime.Now;
                req.CyddStatus = CyddStatus.开台;
                var model = OrderRepository.OpenTableCreate(req, TableIds, out msg);
                if (model != null)
                {
                    res.Data = model;
                }
                res.Message = msg;
            }
            else
            {
                res.Data = 0;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChoseProject(int orderTableId)
        {
            var categorys = CategoryRepository.GetChildList();
            var Table = OrderRepository.GetTableByOrderTableId(orderTableId);
            ViewBag.Categorys = categorys;
            ViewBag.OrderTableId = orderTableId;
            ViewBag.Table = Table;
            return View();
        }

        public ActionResult BatchChoseProject(string orderTableIds)
        {
            var categorys = CategoryRepository.GetChildList();
            ViewBag.Categorys = categorys;
            ViewBag.OrderTableIds = orderTableIds;
            return View();
        }

        public ActionResult ReserveHistory(int tableId)
        {
            var table = TableRepository.GetModel(tableId);
            ViewBag.Table = table;
            return View();
        }

        public ActionResult Order(int tableId, int orderId = 0,bool print=true)
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderDetailCreate(List<OrderDetailDTO> req,List<int> orderTableIds, bool print=true)
        {
            Response res = new Response();
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                string message = string.Empty;
                bool result = OrderRepository.OrderDetailCreate(req, orderTableIds, out msg, print);
                res.Data = result;
                res.Message = msg;
            }
            else
            {
                res.Data = 0;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeTable(ChangeTableDTO req)
        {
            req.CythStatus = CythStatus.空置;
            var areas = AreaRepository.GetList(req.RestaurantId);
            var table = TableRepository.GetModel(req.TableId);
            ViewBag.Areas = areas;
            ViewBag.Paras = req;
            ViewBag.Table = table;
            return View();
        }

        [HttpPost]
        public ActionResult ChangeTableSubmit(ChangeTableSubmitDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                res.Data = OrderRepository.ChangeTableSubmit(req);
                if (Convert.ToBoolean(res.Data)==false)
                {
                    res.Message = "操作失败";
                }
            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JoinTable(int orderTableId)
        {
            return View();
        }

        public ActionResult CancelTable(int tableId,int orderId)
        {
            return View();
        }

        public ActionResult CheckOrder(int tableId,int orderId)
        {
            return View();
        }

        public ActionResult CancelOrder(int tableId,int orderId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetTableEmpty(int tableId)
        {
            return View();
        }
    }
}