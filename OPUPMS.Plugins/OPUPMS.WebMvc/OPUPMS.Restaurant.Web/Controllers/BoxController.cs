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
    public class BoxController : AuthorizationController
    {
        readonly IBoxRepository _boxRepository;
        readonly IRestaurantRepository _restaurantRepository;
        readonly IAreaRepository _areaRepository;

        public BoxController(
            IBoxRepository boxRepository,
            IRestaurantRepository restaurantRepository,
            IAreaRepository areaRepository)
        {
            _boxRepository = boxRepository;
            _restaurantRepository = restaurantRepository;
            _areaRepository = areaRepository;
        }

        public ActionResult Index()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        public ActionResult GetBoxs(BoxSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _boxRepository.GetList(out int total, req);
            return NewtonSoftJson(new
            {
                rows = list,
                total = total,
                code = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            ViewBag.Box = _boxRepository.GetModel(id); ;
            ViewBag.Restaurants = _restaurantRepository.GetList();

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BoxCreateDTO req)
        {
            Response res = new Response();

            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _boxRepository.Update(req);
                    }
                    else
                    {
                        res.Data = _boxRepository.Create(req);
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
            var box = _boxRepository.GetModel(id);
            ViewBag.Box = box;
            ViewBag.Restaurants = _restaurantRepository.GetList();

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
                Data = _boxRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}