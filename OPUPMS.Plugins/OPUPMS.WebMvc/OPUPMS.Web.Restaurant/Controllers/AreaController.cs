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

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class AreaController : BaseController
    {
        readonly IAreaRepository AreaRepository;
        readonly IRestaurantRepository RestaurantRepository;
        public AreaController(IAreaRepository _AreaRepository,IRestaurantRepository _RestaurantRepository)
        {
            AreaRepository = _AreaRepository;
            RestaurantRepository = _RestaurantRepository;
        }

        // GET: Area
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAreas(AreaSearchDTO req)
        {
            int total = 0;
            var list = AreaRepository.GetList(out total, req);
            return NewtonSoftJson(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var restaurants = RestaurantRepository.GetList();
            var model = AreaRepository.GetModel(id);
            ViewBag.Area = model;
            ViewBag.Restaurants = restaurants;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(AreaCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = AreaRepository.Update(req);
                    }
                    else
                    {
                        res.Data = AreaRepository.Create(req);
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.InnerException.Message;
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