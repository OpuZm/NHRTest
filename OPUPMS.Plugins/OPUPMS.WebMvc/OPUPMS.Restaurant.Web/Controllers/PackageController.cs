using System;
using System.Collections.Generic;
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
    [CustomerAuthorize(Permission.套餐管理)]
    public class PackageController : AuthorizationController
    {
        readonly IPackageRepository _packageRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IProjectRepository _projectRepository;

        public PackageController(
            IPackageRepository packageRepository,
            ICategoryRepository categoryRepository,
            IProjectRepository projectRepository)
        {
            _packageRepository = packageRepository;
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPackages(PackageSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _packageRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var categorys = _categoryRepository.GetChildList();
            var model = _packageRepository.GetModel(id);
            ViewBag.Package = model;
            ViewBag.Categorys = categorys;
            return View();
        }

        public ActionResult GetModel(int id)
        {
            var model = _packageRepository.GetModel(id);
            return Json(new { Data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNewModel(int id)
        {
            var model = _packageRepository.GetModel(id);
            var projects = _projectRepository.GetDetailList(0, false);
            var images = _projectRepository.GetProjectImages(id, 2);
            return Json(new { Package = model, Projects = projects, Images = images }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                req.IsCustomer = false;
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _packageRepository.Update(req);
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.R_Company_Id = currentUser.CompanyId.ToInt();
                        res.Data = _packageRepository.Create(req);
                    }
                }
                catch (Exception ex)
                {
                    res.Data = 0;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitExtend(PackageCreateDTO model, List<PackageDetailCreateDTO> req)
        {
            Response res = new Response();

            try
            {
                res.Data = _packageRepository.DetailCreate(model, req);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
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
            var categorys = _categoryRepository.GetChildList();
            var model = _packageRepository.GetModel(id);
            ViewBag.Package = model;
            ViewBag.Categorys = categorys;
            return View();
        }
        public ActionResult NewEdit2(int id = 0)
        {
            var categorys = _categoryRepository.GetChildList();
            var model = _packageRepository.GetModel(id);
            ViewBag.Package = model;
            ViewBag.Categorys = categorys;

            //菜品类别明细
            var Projects = _projectRepository.GetProjectAndDetailList(0, false);
            ViewBag.Projects = Projects;

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
                Data = _packageRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}