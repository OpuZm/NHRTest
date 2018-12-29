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
    public class ProjectController : BaseController
    {
        readonly IProjectRepository ProjectRepository;
        readonly ICategoryRepository CategoryRepository;
        readonly IExtendRepository ExtendRepository;

        public ProjectController(IProjectRepository _ProjectRepository, ICategoryRepository _CategoryRepository, IExtendRepository _ExtendRepository)
        {
            ProjectRepository = _ProjectRepository;
            CategoryRepository = _CategoryRepository;
            ExtendRepository = _ExtendRepository;
        }
        // GET: Project
        public ActionResult Index()
        {
            var categorys = CategoryRepository.GetChildList();
            ViewBag.Categorys = categorys;
            return View();
        }

        public ActionResult GetProjects(ProjectSearchDTO req)
        {
            int total = 0;
            var list = ProjectRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var categorys = CategoryRepository.GetChildList();
            var model = ProjectRepository.GetModel(id);
            var extend = ExtendRepository.GetList();
            ViewBag.Project = model;
            ViewBag.Categorys = categorys;
            ViewBag.Extend = extend;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ProjectCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = ProjectRepository.Update(req);
                    }
                    else
                    {
                        req.CreateDate = DateTime.Now;
                        res.Data = ProjectRepository.Create(req);
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.InnerException.Message;
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
        public ActionResult SubmitExtend(ProjectExtendCreateDTO req)
        {
            Response res = new Response();
            try
            {
                res.Data = ProjectRepository.ExtendCreate(req);
            }
            catch (Exception ex)
            {
                res.Message = ex.InnerException.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModel(int id)
        {
            var model = ProjectRepository.GetModel(id);
            return Json(new { Data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Specification(int id = 0)
        {
            var model = ProjectRepository.GetModel(id);
            ViewBag.Project = model;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult SpecificationSubmit(int cyxmId, List<R_ProjectDetail> list)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = ProjectRepository.SpecificationSubmit(cyxmId, list);
                }
                catch (Exception ex)
                {
                    res.Data = false;
                    res.Message = ex.InnerException.Message;
                }

            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectDetails(int category = 0,bool hasPackage=false)
        {
            Response res = new Response();
            res.Data= ProjectRepository.GetDetailList(category, hasPackage);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}