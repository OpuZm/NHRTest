using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class AreaPrintController : AuthorizationController
    {
        readonly IAreaRepository _areaRepository;
        readonly IRestaurantRepository _restaurantRepository;
        readonly IPrinterRepository _printerRepository;

        public AreaPrintController(
    IAreaRepository areaRepository, IRestaurantRepository restaurantRepository,
    IPrinterRepository printerRepository)
        {
            _areaRepository = areaRepository;
            _restaurantRepository = restaurantRepository;
            _printerRepository = printerRepository;
        }
        // GET: AreaPrint
        public ActionResult NewIndex()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        public ActionResult GetList(WeixinPrintSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _areaRepository.GetWeixinPrints(req, out int total);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(WeixinPrintDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _areaRepository.UpdateWeixinPrint(req);
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

        public ActionResult GetModel(int id)
        {
            var model = _areaRepository.GetWeixinPrint(id);
            var restaurants = _restaurantRepository.GetList();
            var prints = _printerRepository.GetList();
            var areas = _areaRepository.GetList(0);
            ViewBag.AreaPrint = model;
            ViewBag.Restaurants = restaurants;
            ViewBag.Prints = prints;
            ViewBag.Areas = areas;
            return Json(new { Model=model,Restaurants= restaurants ,Prints= prints ,Areas= areas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IsDelete(int id = 0)
        {
            Response res = new Response
            {
                Data = _areaRepository.IsDeleteWeixinPrint(id)
            };
            return Json(res);
        }

        public ActionResult ChangeTableIndex()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        public ActionResult GeneralOrderIndex()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }
    }
}