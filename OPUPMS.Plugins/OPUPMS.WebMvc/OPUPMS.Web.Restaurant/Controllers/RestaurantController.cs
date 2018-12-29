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
    public class RestaurantController : BaseController
    {
        readonly IRestaurantRepository RestaurantRepository;
        public RestaurantController(IRestaurantRepository _RestaurantRepository)
        {
            RestaurantRepository = _RestaurantRepository;
        }
        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRestaurants(RestaurantSearchDTO req)
        {
            int total = 0;
            var list = RestaurantRepository.GetList(out total,req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id=0)
        {
            var model = RestaurantRepository.GetModel(id);
            ViewBag.Restaurant = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(RestaurantCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = RestaurantRepository.Update(req);
                    }
                    else
                    {
                        res.Data = RestaurantRepository.Create(req);
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