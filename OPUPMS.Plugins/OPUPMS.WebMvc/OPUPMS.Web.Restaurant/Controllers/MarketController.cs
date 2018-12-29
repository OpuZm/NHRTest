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
    public class MarketController : BaseController
    {
        readonly IMarketRepository MarketRepository;
        readonly IRestaurantRepository RestaurantRepository;
        public MarketController(IMarketRepository _MarketRepository, IRestaurantRepository _RestaurantRepository)
        {
            MarketRepository = _MarketRepository;
            RestaurantRepository = _RestaurantRepository;
        }
        // GET: Stalls
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMarkets(MarketSearchDTO req)
        {
            int total = 0;
            var list = MarketRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var restaurants = RestaurantRepository.GetList();
            var model = MarketRepository.GetModel(id);
            ViewBag.Restaurants = restaurants;
            ViewBag.Market = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(MarketCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = MarketRepository.Update(req);
                    }
                    else
                    {
                        res.Data = MarketRepository.Create(req);
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