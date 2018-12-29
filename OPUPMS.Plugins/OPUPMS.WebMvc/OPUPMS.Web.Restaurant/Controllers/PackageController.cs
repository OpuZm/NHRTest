using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Infrastructure.Common.Net;
using OPUPMS.Web.Restaurant.Models;
using OPUPMS.Domain.Restaurant.Services;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class PackageController : BaseController
    {
        readonly IPackageRepository PackageRepository;
        readonly ICategoryRepository CategoryRepository;
        readonly IProjectRepository ProjectRepository;
        public PackageController(IPackageRepository _PackageRepository, ICategoryRepository _CategoryRepository, IProjectRepository _ProjectRepository)
        {
            PackageRepository = _PackageRepository;
            CategoryRepository = _CategoryRepository;
            ProjectRepository = _ProjectRepository;
        }
        // GET: Package
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPackages(PackageSearchDTO req)
        {
            int total = 0;
            var list = PackageRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var categorys = CategoryRepository.GetChildList();
            var model = PackageRepository.GetModel(id);
            ViewBag.Package = model;
            ViewBag.Categorys = categorys;
            return View();
        }

        public ActionResult GetModel(int id)
        {
            var model = PackageRepository.GetModel(id);
            return Json(new { Data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
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
                        res.Data = PackageRepository.Update(req);
                    }
                    else
                    {
                        res.Data = PackageRepository.Create(req);
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
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitExtend(PackageCreateDTO model,List<PackageDetailCreateDTO> req)
        {
            Response res = new Response();
            try
            {
                res.Data = PackageRepository.DetailCreate(model, req);
            }
            catch (Exception ex)
            {
                res.Message = ex.InnerException.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}