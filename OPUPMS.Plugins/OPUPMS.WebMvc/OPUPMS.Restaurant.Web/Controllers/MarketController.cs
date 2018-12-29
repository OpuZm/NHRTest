using System;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class MarketController : AuthorizationController
    {
        readonly IMarketRepository _marketRepository;
        readonly IRestaurantRepository _restaurantRepository;

        public MarketController(
            IMarketRepository marketRepository,
            IRestaurantRepository restaurantRepository)
        {
            _marketRepository = marketRepository;
            _restaurantRepository = restaurantRepository;
        }

        public ActionResult Index()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;

            return View();
        }

        public ActionResult GetMarkets(MarketSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _marketRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var restaurants = _restaurantRepository.GetList();
            var model = _marketRepository.GetModel(id);
            ViewBag.Restaurants = restaurants;
            ViewBag.Market = model;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MarketCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _marketRepository.Update(req);
                    }
                    else
                    {
                        res.Data = _marketRepository.Create(req);
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
        
        #region 新界面Action

        public ActionResult NewIndex()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;

            return View();
        }

        public ActionResult NewEdit(int id = 0)
        {
            var restaurants = _restaurantRepository.GetList();
            var model = _marketRepository.GetModel(id);
            ViewBag.Restaurants = restaurants;
            ViewBag.Market = model;
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
                Data = _marketRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}