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

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class CategoryController : BaseController
    {
        readonly ICategoryRepository CategoryRepository;

        public CategoryController(ICategoryRepository _CategoryRepository)
        {
            CategoryRepository = _CategoryRepository;
        }
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategorys(CategorySearchDTO req)
        {
            int total = 0;
            var list = CategoryRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var categorys = CategoryRepository.GetList(0);
            var model = CategoryRepository.GetModel(id);
            ViewBag.Categorys = categorys;
            ViewBag.Category = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CategoryCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = CategoryRepository.Update(req);
                    }
                    else
                    {
                        res.Data = CategoryRepository.Create(req);
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
    }
}