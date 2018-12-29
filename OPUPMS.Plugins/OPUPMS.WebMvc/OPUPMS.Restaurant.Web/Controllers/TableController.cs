using System;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class TableController : AuthorizationController
    {
        readonly IBoxRepository _boxRepository;
        readonly IRestaurantRepository _restaurantRepository;
        readonly IAreaRepository _areaRepository;
        readonly ITableRepository _tableRepository;

        public TableController(
            IBoxRepository boxRepository,
            IRestaurantRepository restaurantRepository,
            IAreaRepository areaRepository,
            ITableRepository tableRepository)
        {
            _boxRepository = boxRepository;
            _restaurantRepository = restaurantRepository;
            _areaRepository = areaRepository;
            _tableRepository = tableRepository;
        }

        [CustomerAuthorize(Permission.餐饮系统设置)]
        public ActionResult Index()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        [CustomerAuthorize(Permission.餐饮系统设置)]
        public ActionResult GetTables(TableSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _tableRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [CustomerAuthorize(Permission.餐饮系统设置)]
        public ActionResult Edit(int id = 0)
        {
            var restaurants = _restaurantRepository.GetList();
            var model = _tableRepository.GetModel(id);

            ViewBag.Table = model;
            ViewBag.Restaurants = restaurants;

            return View();
        }

        [CustomerAuthorize(Permission.餐饮系统设置)]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TableCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _tableRepository.Update(req);
                    }
                    else
                    {
                        res.Data = _tableRepository.Create(req);
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

        public ActionResult ChoseTable(TableChoseSearchDTO req)
        {
            var areas = _areaRepository.GetList(req.RestaurantId);

            ViewBag.Paras = req;
            ViewBag.Areas = areas;
            return View();
        }

        public ActionResult OpenChoseTable(TableChoseSearchDTO req)
        {
            var areas = _areaRepository.GetList(req.RestaurantId);
            req.CythStatus = Domain.Restaurant.Model.CythStatus.空置;
            ViewBag.Paras = req;
            ViewBag.Areas = areas;
            return View();
        }

        #region 新界面Action

        [CustomerAuthorize(Permission.餐饮系统设置)]
        public ActionResult NewIndex()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        [CustomerAuthorize(Permission.餐饮系统设置)]
        public ActionResult NewEdit(int id = 0)
        {
            var restaurants = _restaurantRepository.GetList();
            var model = _tableRepository.GetModel(id);

            ViewBag.Table = model;
            ViewBag.Restaurants = restaurants;

            return View();
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpPost]
        public ActionResult IsDelete(int id = 0)
        {
            Response res = new Response
            {
                Data = _tableRepository.IsDelete(id, out string msg),
                Message = msg
            };
            return Json(res);
        }
        #endregion
    }
}