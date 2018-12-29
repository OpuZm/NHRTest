using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class AreaController : AuthorizationController
    {
        readonly IAreaRepository _areaRepository;
        readonly IRestaurantRepository _restaurantRepository;

        public AreaController(
            IAreaRepository areaRepository, 
            IRestaurantRepository restaurantRepository)
        {
            _areaRepository = areaRepository;
            _restaurantRepository = restaurantRepository;
        }

        /// <summary>
        /// 根据餐厅获取下面的区域信息列表
        /// </summary>
        /// <param name="restaurantId">餐厅Id</param>
        /// <returns></returns>
        public JsonResult GetAreasByRestaurantId(int restaurantId)
        {          
            List<AreaListDTO> areaList = _areaRepository.GetList(restaurantId);
            return Json(areaList, JsonRequestBehavior.AllowGet);
        }

        // GET: Area
        public ActionResult Index()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;

            return View();
        }

        public ActionResult GetAreas(AreaSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _areaRepository.GetList(out int total, req);
            return NewtonSoftJson(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            ViewBag.Area = _areaRepository.GetModel(id);
            ViewBag.Restaurants = _restaurantRepository.GetList(); ;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaCreateDTO req)
        {
            Response res = new Response();

            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _areaRepository.Update(req);
                    }
                    else
                    {
                        res.Data = _areaRepository.Create(req);
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

        // GET: Area
        public ActionResult NewIndex()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;

            return View();
        }

        public ActionResult NewEdit(int id = 0)
        {
            ViewBag.Area = _areaRepository.GetModel(id);
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
                Data = _areaRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}