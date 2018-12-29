using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Base.Repositories;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class StatisticsController : AuthorizationController
    {
        readonly IStatisticsRepository _statisticsRepository;
        readonly IExtendItemRepository _extendItemRepository;
        public StatisticsController(IStatisticsRepository statisticsRepository, IExtendItemRepository extendItemRepository)
        {
            _statisticsRepository = statisticsRepository;
            _extendItemRepository = extendItemRepository;
        }
        // GET: Statistics
        public ActionResult Produced()
        {
            ViewBag.BeginDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult GetProduced(ProducedSearchDTO req)
        {
            Response res = new Response();
            try
            {
                var currentUser = OperatorProvider.Provider.GetCurrent();
                req.RestaurantId = Convert.ToInt32(currentUser.DepartmentId);
                var list = _statisticsRepository.Produced(req);
                res.Data = list;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TurnDuty()
        {
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            var dateItem = _extendItemRepository.GetModelList(Convert.ToInt32(operatorUser.CompanyId), 10003).FirstOrDefault();
            ViewBag.BeginDate = dateItem!=null? dateItem.ItemValue:DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult GetTurnDuty(TurnDutySearchDTO req)
        {
            Response res = new Response();
            try
            {
                var operatorUser = OperatorProvider.Provider.GetCurrent();
                req.RestaurantId = Convert.ToInt32(operatorUser.DepartmentId);
                res.Data = _statisticsRepository.GetTurnDuty(req);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportList()
        {
            return View();
        }

        public ActionResult GetReports()
        {
            var Data = _statisticsRepository.GetReportList();
            return NewtonSoftJson(new
            {
                rows = Data,
                total = Data.Count(),
                code = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }
    }
}