using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class OrderController : BaseController
    {
        readonly IOrderRepository OrderRepository;

        public OrderController(IOrderRepository _OrderRepository)
        {
            OrderRepository = _OrderRepository;
        }
        public ActionResult CancelReserveOrder(int orderId)
        {
            return View();
        }

        public ActionResult EditReserve(int orderId)
        {
            var order = OrderRepository.GetOrderModel(orderId);
            ViewBag.Order = order;
            return View();
        }

        public ActionResult DepositManager(int orderId)
        {
            ViewBag.Order = OrderRepository.GetOrderModel(orderId);
            return View();
        }

        public ActionResult GetOrderDeposits(int orderId)
        {
            Response res = new Response();
            var data = OrderRepository.GetOrderPayList(orderId, CyddJzType.定金);
            res.Data = data;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderDepositCreate(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost]
        public ActionResult OrderDepositCreate(OrderPayHistoryDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                req.CreateUser = 1;
                req.CreateDate = DateTime.Now;
                req.CyddJzStatus = CyddJzStatus.已付;
                req.CyddJzType = CyddJzType.定金;
                var result = OrderRepository.OrderDepositCreate(req);
                res.Data = result;
                res.Message = result ? "操作成功" : "操作失败，请联系管理员";
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