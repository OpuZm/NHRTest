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
    public class TableController : BaseController
    {
        readonly IBoxRepository BoxRepository;
        readonly IRestaurantRepository RestaurantRepository;
        readonly IAreaRepository AreaRepository;
        readonly ITableRepository TableRepository;
        
        public TableController(IBoxRepository _BoxRepository, IRestaurantRepository _RestaurantRepository, IAreaRepository _AreaRepository, ITableRepository _TableRepository)
        {
            BoxRepository = _BoxRepository;
            RestaurantRepository = _RestaurantRepository;
            AreaRepository = _AreaRepository;
            TableRepository = _TableRepository;
        }

        // GET: Table
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTables(TableSearchDTO req)
        {
            int total = 0;
            var list = TableRepository.GetList(out total, req);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var restaurants = RestaurantRepository.GetList();
            var model = TableRepository.GetModel(id);
            //var areas = AreaRepository.GetList();
            //var boxs = BoxRepository.GetList();
            ViewBag.Table = model;
            ViewBag.Restaurants = restaurants;
            //ViewBag.Areas = areas;
            //ViewBag.Boxs = boxs;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(TableCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = TableRepository.Update(req);
                    }
                    else
                    {
                        res.Data = TableRepository.Create(req);
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

        public ActionResult ChoseTable(TableChoseSearchDTO req)
        {
            var areas = AreaRepository.GetList(req.RestaurantId);
            ViewBag.Paras = req;
            ViewBag.Areas = areas;
            return View();
        }

        public ActionResult OpenChoseTable(TableChoseSearchDTO req)
        {
            var areas = AreaRepository.GetList(req.RestaurantId);
            req.CythStatus = Domain.Restaurant.Model.CythStatus.空置;
            ViewBag.Paras = req;
            ViewBag.Areas = areas;
            return View();
        }
    }
}