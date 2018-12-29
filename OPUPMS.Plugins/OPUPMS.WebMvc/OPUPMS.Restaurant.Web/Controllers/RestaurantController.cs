using System;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class RestaurantController : AuthorizationController
    {
        readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRestaurants(RestaurantSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            req.CompanyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            var list = _restaurantRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var model = _restaurantRepository.GetModel(id);
            ViewBag.Restaurant = model;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RestaurantCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _restaurantRepository.Update(req);
                    }
                    else
                    {
                        var operatorUser = OperatorProvider.Provider.GetCurrent();
                        req.R_Company_Id = operatorUser.CompanyId.ToInt();
                        res.Data = _restaurantRepository.Create(req);
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
            return View();
        }

        [HttpGet]
        public ActionResult NewEdit(int id = 0)
        {
            var model = _restaurantRepository.GetModel(id);
            ViewBag.Restaurant = model;
            return View();
        } 
        #endregion
    }
}