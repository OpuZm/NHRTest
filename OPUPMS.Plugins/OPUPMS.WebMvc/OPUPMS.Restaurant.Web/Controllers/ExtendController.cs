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
    [CustomerAuthorize(Permission.商品特殊要求管理)]
    public class ExtendController : AuthorizationController
    {
        readonly IExtendRepository _extendRepository;

        public ExtendController(IExtendRepository extendRepository)
        {
            _extendRepository = extendRepository;
        }

        public ActionResult Index()
        {
            var category = _extendRepository.GetCategory();
            var ExtendType = _extendRepository.GetExtendTypeList(0);
            ViewBag.Category = category;
            ViewBag.ExtendType = ExtendType;
            return View();
        }

        public ActionResult GetExtends(ExtendSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _extendRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExtend()
        {
            Response res = new Response();
            res.Data = _extendRepository.GetList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var model = _extendRepository.GetModel(id);
            var category = _extendRepository.GetCategory();
            var ExtendType = _extendRepository.GetExtendTypeList(0);
            ViewBag.ExtendType = ExtendType;
            ViewBag.Category = category;
            ViewBag.Extend = model;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExtendCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _extendRepository.Update(req);
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.R_Company_Id = currentUser.CompanyId.ToInt();
                        res.Data = _extendRepository.Create(req);
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
            var category = _extendRepository.GetCategory();
            var ExtendType = _extendRepository.GetExtendTypeList(0);
            ViewBag.Category = category;
            ViewBag.ExtendType = ExtendType;
            return View();
        }

        public ActionResult NewEdit(int id = 0)
        {
            var model = _extendRepository.GetModel(id);
            var category = _extendRepository.GetCategory();
            var ExtendType = _extendRepository.GetExtendTypeList(0);
            ViewBag.ExtendType = ExtendType;
            ViewBag.Category = category;
            ViewBag.Extend = model;
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
                Data = _extendRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}