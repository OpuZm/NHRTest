using System;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class CategoryController : AuthorizationController
    {
        readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategorys(CategorySearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _categoryRepository.GetList(out int total, req);
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
            ViewBag.Categorys = _categoryRepository.GetList(0);
            ViewBag.Category = _categoryRepository.GetModel(id);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryCreateDTO req)
        {
            Response res = new Response();

            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _categoryRepository.Update(req);
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.R_Company_Id = currentUser.CompanyId.ToInt();
                        res.Data = _categoryRepository.Create(req);
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
            var parentCategorys = _categoryRepository.GetList(0);
            ViewBag.ParentCategorys = parentCategorys;
            return View();
        }

        public ActionResult NewEdit(int id = 0)
        {
            ViewBag.Categorys = _categoryRepository.GetList(0);
            ViewBag.Category = _categoryRepository.GetModel(id);
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
                Data = _categoryRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}