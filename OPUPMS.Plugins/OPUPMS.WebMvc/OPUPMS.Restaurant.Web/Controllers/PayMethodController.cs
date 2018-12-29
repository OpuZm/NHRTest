using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Operator;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class PayMethodController : AuthorizationController
    {
        private readonly IPayMethodRepository _payMethodRepository;

        public PayMethodController(IPayMethodRepository payMethodRepository)
        {
            _payMethodRepository = payMethodRepository;
        }


        public ActionResult Index()
        {
            ViewBag.Parents= _payMethodRepository.GetParents();
            return View();
        }

        public ActionResult Edit(int id=0)
        {
            ViewBag.Parents = _payMethodRepository.GetParents();
            ViewBag.Model = _payMethodRepository.GetModel(id);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PayMethodCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _payMethodRepository.Update(req);
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.CreateUser = currentUser.UserId;
                        req.R_Company_Id = Convert.ToInt32(currentUser.CompanyId);
                        res.Data = _payMethodRepository.Create(req);
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

            return Json(res);
        }

        public ActionResult GetPayMethods(PayMethodSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _payMethodRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParents()
        {
            var res= _payMethodRepository.GetParents();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _payMethodRepository.Delete(id);
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
            return Json(res);
        }
    }
}