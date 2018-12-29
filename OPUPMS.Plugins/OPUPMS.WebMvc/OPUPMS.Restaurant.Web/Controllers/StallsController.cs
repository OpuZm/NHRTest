using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class StallsController : AuthorizationController
    {
        readonly IStallsRepository _stallsRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IPrinterRepository _printerRepository;
        readonly IProjectRepository _projectRepository;

        public StallsController(
            IStallsRepository stallRepository,
            ICategoryRepository categoryRepository,
            IPrinterRepository printerRepository,
            IProjectRepository projectRepository)
        {
            _stallsRepository = stallRepository;
            _categoryRepository = categoryRepository;
            _printerRepository = printerRepository;
            _projectRepository = projectRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStalls(StallsSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            var list = _stallsRepository.GetList(out int total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            var model = _stallsRepository.GetModel(id);
            var printList = _printerRepository.GetList();
            ViewBag.Stalls = model;
            ViewBag.PrinterList = printList;
            return View();
        }

        public ActionResult GetModel(int id, int billType)
        {
            Response res = new Response();
            res.Data = _stallsRepository.GetModel(id, billType);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StallsCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _stallsRepository.Update(req);
                    }
                    else
                    {
                        res.Data = _stallsRepository.Create(req);
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
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelProject(int id)
        {
            var model = _stallsRepository.GetModel(id);
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
            var model = _stallsRepository.GetModel(id);
            var categorys = _categoryRepository.GetChildList();
            var projects = _projectRepository.GetList(0);

            ViewBag.Stall = model;
            ViewBag.Categorys = categorys;
            ViewBag.Projects = projects;

            return View();
        }

        [HttpPost]
        public ActionResult EditProject(int id, int billType, List<R_ProjectStall> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _stallsRepository.ProjectStallSubmit(id, billType, req);
                }
                catch (Exception ex)
                {
                    res.Message = ex.InnerException.Message;
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

        [HttpPost]
        public ActionResult EditProjectNew(int id,  List<R_ProjectStall> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _stallsRepository.ProjectStallSubmitNew(id,  req);
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
            var model = _stallsRepository.GetModel(id);
            var printList = _printerRepository.GetList();
            ViewBag.Stalls = model;
            ViewBag.PrinterList = printList;
            return View();
        }

        public ActionResult NewEditProject(int id)
        {
            var model = _stallsRepository.GetModel(id);
            var categorys = _categoryRepository.GetChildList();
            //var projects = _projectRepository.GetList(0);

            ViewBag.Stall = model;
            ViewBag.Categorys = categorys;
            //ViewBag.Projects = projects;

            return View();
        }

        /// <summary>
        /// 获取档口出品关联菜品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetStallProjects(int id)
        {
            var itemAndDetails = _projectRepository.GetProjectAndDetailList(0, false);//菜品明细
            var categories = _categoryRepository.GetAllCategoryList();//菜品分类 
            var model= _stallsRepository.GetModel(id, null);
            var info = new
            {
                ProjectAndDetails = itemAndDetails,
                CategoryList = categories,
                model= model
            };
            return Json(info);
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
                Data = _stallsRepository.IsDelete(id)
            };
            return Json(res);
        }
        #endregion
    }
}