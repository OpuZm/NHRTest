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
    public class ExtendController : BaseController
    {
        readonly IExtendRepository ExtendRepository;
        public ExtendController(IExtendRepository _ExtendRepository)
        {
            ExtendRepository = _ExtendRepository;
        }
        // GET: Extend
        public ActionResult Index()
        {
            var category = ExtendRepository.GetCategory();
            ViewBag.Category = category;
            return View();
        }

        public ActionResult GetExtends(ExtendSearchDTO req)
        {
            int total = 0;
            var list = ExtendRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var model = ExtendRepository.GetModel(id);
            var category = ExtendRepository.GetCategory();
            ViewBag.Category = category;
            ViewBag.Extend = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ExtendCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = ExtendRepository.Update(req);
                    }
                    else
                    {
                        res.Data = ExtendRepository.Create(req);
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