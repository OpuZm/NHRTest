using System;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class PrinterController : AuthorizationController
    {
        readonly IPrinterRepository _printerRepository;

        public PrinterController(IPrinterRepository printerRepository)
        {
            _printerRepository = printerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(PrinterSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _printerRepository.GetList(req, out int total);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id = 0)
        {
            var model = _printerRepository.GetModel(id);
            ViewBag.Printer = model;
            return View();
        }

        public ActionResult GetModel(int id)
        {
            Response res = new Response();
            res.Data = _printerRepository.GetModel(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PrinterDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _printerRepository.Update(req);
                    }
                    else
                    {
                        res.Data = _printerRepository.Create(req);
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

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #region 新界面Action

        public ActionResult NewIndex()
        {
            return View();
        }

        public ActionResult NewEdit(int id = 0)
        {
            var model = _printerRepository.GetModel(id);
            ViewBag.Printer = model;
            return View();
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpPost]
        public ActionResult IsDelete(int id = 0)
        {
            Response res = new Response
            {
                Data = _printerRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}