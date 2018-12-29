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
    [CustomerAuthorize(Permission.自定义折扣管理)]
    public class DiscountController : AuthorizationController
    {
        readonly IDiscountRepository _discountRepository;
        readonly IRestaurantRepository _restaurantRepository;
        readonly IAreaRepository _areaRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IMarketRepository _marketRepository;

        public DiscountController(
            IDiscountRepository discountRepository,
            IRestaurantRepository restaurantRepository,
            IAreaRepository areaRepository,
            ICategoryRepository categoryRepository,
            IMarketRepository marketRepository)
        {
            _discountRepository = discountRepository;
            _restaurantRepository = restaurantRepository;
            _areaRepository = areaRepository;
            _categoryRepository = categoryRepository;
            _marketRepository = marketRepository;
        }

        public ActionResult Index()
        {
            var restaurants = _restaurantRepository.GetList();
            ViewBag.Restaurants = restaurants;
            return View();
        }

        public ActionResult GetDiscounts(DiscountSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _discountRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDiscountsForPay(PayDiscountSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _discountRepository.GetList(out int total, req);
           
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var categorys = _categoryRepository.GetChildList();
            var restaurants = _restaurantRepository.GetList();
            var model = _discountRepository.GetModel(id);

            ViewBag.Discount = model;
            ViewBag.Categorys = categorys;
            ViewBag.Restaurants = restaurants;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiscountCreateDTO req)
        {
            Response res = new Response();

            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _discountRepository.Update(req);
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.CompanyId = currentUser.CompanyId.ToInt();
                        res.Data = _discountRepository.Create(req);
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
            var categorys = _categoryRepository.GetChildList();
            var restaurants = _restaurantRepository.GetList();
            var model = _discountRepository.GetModel(id);

            ViewBag.Discount = model;
            ViewBag.Categorys = categorys;
            ViewBag.Restaurants = restaurants;

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
                Data = _discountRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion

        public ActionResult GetSchemeDiscountList(int orderId=0)
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var list = _discountRepository.GetSchemeDiscountList(
                new SchemeDiscountSearchDTO()
                {
                    MarketId = currentUser.LoginMarketId,
                    RestaurantId = currentUser.DepartmentId.ToInt(),
                    OrderId=orderId
                });

            return Json(list);
        }
    }
}