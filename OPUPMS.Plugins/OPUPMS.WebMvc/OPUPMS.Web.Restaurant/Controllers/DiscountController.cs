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
    public class DiscountController : BaseController
    {
        readonly IDiscountRepository DiscountRepository;
        readonly IRestaurantRepository RestaurantRepository;
        readonly IAreaRepository AreaRepository;
        readonly ICategoryRepository CategoryRepository;
        readonly IMarketRepository MarketRepository;

        public DiscountController(IDiscountRepository _DiscountRepository, IRestaurantRepository _RestaurantRepository, IAreaRepository _AreaRepository, ICategoryRepository _CategoryRepository, IMarketRepository _MarketRepository)
        {
            DiscountRepository = _DiscountRepository;
            RestaurantRepository = _RestaurantRepository;
            AreaRepository = _AreaRepository;
            CategoryRepository = _CategoryRepository;
            MarketRepository = _MarketRepository;
        }
        // GET: Discount
        public ActionResult Index()
        {
            var restaurants = RestaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        public ActionResult GetDiscounts(DiscountSearchDTO req)
        {
            int total = 0;
            var list = DiscountRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var categorys = CategoryRepository.GetChildList();
            var restaurants= RestaurantRepository.GetList();
            //var markets = MarketRepository.GetList();
            //var areas = AreaRepository.GetList();
            var model = DiscountRepository.GetModel(id);
            ViewBag.Discount = model;
            ViewBag.Categorys = categorys;
            ViewBag.Restaurants = restaurants;
            //ViewBag.Markets = markets;
            //ViewBag.Areas = areas;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(DiscountCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = DiscountRepository.Update(req);
                    }
                    else
                    {
                        res.Data = DiscountRepository.Create(req);
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
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}