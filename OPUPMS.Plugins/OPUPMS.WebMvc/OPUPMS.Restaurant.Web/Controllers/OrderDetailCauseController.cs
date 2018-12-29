using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class OrderDetailCauseController : AuthorizationController
    {
        private readonly IOrderDetailCauseRepository _orderDetailCauseRepository;

        public OrderDetailCauseController(IOrderDetailCauseRepository orderDetailCauseRepository)
        {
            _orderDetailCauseRepository = orderDetailCauseRepository;
        }

        // GET: OrderDetailCause
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetModel(int id=0)
        {
            Response res = new Response();
            try
            {
                res.Data = _orderDetailCauseRepository.GetModel(id);
            }
            catch (Exception e)
            {
                res.Data = null;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(OrderDetailCauseDTO req)
        {
            Response res = new Response();
            try
            {
                res.Data = _orderDetailCauseRepository.Edit(req);
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }
            return Json(res);
        }

        [HttpGet]
        public ActionResult GetList(OrderDetailCauseSearch req)
        {
            if (req.ListType == 1)
            {
                req.offset = (req.offset - 1) * req.limit;
            }
            var list = _orderDetailCauseRepository.GetList(out int total, req);
            return NewtonSoftJson(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            Response res = new Response();
            try
            {
                res.Data = _orderDetailCauseRepository.Delete(ids);
            }
            catch (Exception e)
            {
                res.Data = false;
                res.Message = e.Message;
            }
            return Json(res);
        }
    }
}