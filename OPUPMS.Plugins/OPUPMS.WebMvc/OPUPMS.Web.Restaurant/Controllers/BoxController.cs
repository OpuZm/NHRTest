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

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class BoxController : BaseController
    {
        readonly IBoxRepository BoxRepository;
        readonly IRestaurantRepository RestaurantRepository;
        readonly IAreaRepository AreaRepository;
        public BoxController(IBoxRepository _BoxRepository, IRestaurantRepository _RestaurantRepository, IAreaRepository _AreaRepository)
        {
            BoxRepository = _BoxRepository;
            RestaurantRepository = _RestaurantRepository;
            AreaRepository = _AreaRepository;
        }
        // GET: Box
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBoxs(BoxSearchDTO req)
        {
            int total = 0;
            var list = BoxRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var restaurants = RestaurantRepository.GetList();
            var model = BoxRepository.GetModel(id);
            //var areas = AreaRepository.GetList();
            ViewBag.Box = model;
            ViewBag.Restaurants = restaurants;
            //ViewBag.Areas = areas;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(BoxCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = BoxRepository.Update(req);
                    }
                    else
                    {
                        res.Data = BoxRepository.Create(req);
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
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}