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
    public class ExtendTypeController : AuthorizationController
    {
        readonly IExtendRepository _extendRepository;

        public ExtendTypeController(IExtendRepository extendRepository)
        {
            _extendRepository = extendRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetExtendTypes(ExtendTypeSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _extendRepository.GetExtendTypeList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var model = _extendRepository.GetExtendTypeModel(id);
            ViewBag.ExtendType = model;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExtendTypeCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _extendRepository.UpdateExtendType(req);
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.R_Company_Id = currentUser.CompanyId.ToInt();
                        res.Data = _extendRepository.CreateExtendType(req);
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

        public ActionResult NewEdit(int id = 0)
        {
            var model = _extendRepository.GetExtendTypeModel(id);
            ViewBag.ExtendType = model;
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
                Data = _extendRepository.IsDeleteExtendType(id)
            };
            return Json(res);
        }
        #endregion
    }
}