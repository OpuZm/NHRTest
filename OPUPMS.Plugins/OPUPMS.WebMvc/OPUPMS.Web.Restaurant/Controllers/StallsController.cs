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
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class StallsController : BaseController
    {
        readonly IStallsRepository StallsRepository;
        readonly ICategoryRepository CategoryRepository;
        public StallsController(IStallsRepository _StallsRepository, ICategoryRepository _CategoryRepository)
        {
            StallsRepository = _StallsRepository;
            CategoryRepository = _CategoryRepository;
        }
        // GET: Stalls
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStalls(StallsSearchDTO req)
        {
            int total = 0;
            var list = StallsRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var model = StallsRepository.GetModel(id);
            ViewBag.Stalls = model;
            return View();
        }

        public ActionResult GetModel(int id)
        {
            Response res = new Response();
            res.Data = StallsRepository.GetModel(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(StallsCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = StallsRepository.Update(req);
                    }
                    else
                    {
                        res.Data = StallsRepository.Create(req);
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

        public ActionResult SelProject(int id)
        {
            var model = StallsRepository.GetModel(id);
            ViewBag.Stalls = model;
            return View();
        }

        [HttpPost]
        public ActionResult SelProjectSubmit(List<R_Stall> req)
        {
            return View();
        }

        public ActionResult GetStallsProject()
        {
            return View();
        }

        public ActionResult EditProject(int id)
        {
            var model = StallsRepository.GetModel(id);
            var categorys = CategoryRepository.GetChildList();
            ViewBag.Stall = model;
            ViewBag.Categorys = categorys;
            return View();
        }

        [HttpPost]
        public ActionResult EditProject(int id, List<R_ProjectStall> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = StallsRepository.CyxmDkSubmit(id, req);
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
    }
}