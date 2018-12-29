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
    public class RestaurantCategoryController : AuthorizationController
    {
        readonly ICategoryRepository _categoryRepository;
        readonly IRestaurantCategoryRepository _restaurantCategoryRepository;
        public RestaurantCategoryController(IRestaurantCategoryRepository restaurantCategoryRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _restaurantCategoryRepository = restaurantCategoryRepository;
        }
        // GET: RestaurantCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RestaurantCategoryCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _restaurantCategoryRepository.Update(req);
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

        public ActionResult EditInit(int restaurantId)
        {
            var parentCategorys = _categoryRepository.GetList(0)
    .Where(p => p.Pid == 0).ToList();
            var model = _restaurantCategoryRepository.GetModel(restaurantId);
            return Json(new { Categorys= parentCategorys,Model=model});
        }
    }
}