using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class CommonExtendController : AuthorizationController
    {
        readonly IExtendTypeRepository _extendTypeRepository;//可扩展类型表 
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        readonly IOrderRepository _orderRepository;// 

        public CommonExtendController(
            IExtendTypeRepository extendTypeRepository,
            IExtendItemRepository extendItemRepository,
            IOrderRepository orderRepository)
        {
            _extendTypeRepository = extendTypeRepository;
            _extendItemRepository = extendItemRepository;
            _orderRepository = orderRepository;
        }

        public ActionResult Index(int typeId)
        {
            var extendModel = _extendTypeRepository.GetModel(typeId);
            ViewBag.TypeName = extendModel.Name;
            ViewBag.TypeId = extendModel.Id;
            return View();
        }

        public ActionResult GetExtendItemList(ExtendItemSearchDTO req)
        {
            if (req.TypeId == 0)
                throw new Exception("无效类型!");
            var operatorUser = OperatorProvider.Provider.GetCurrent();

            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            List<ExtendItemDto> list = _extendItemRepository
                .GetModelList(operatorUser.CompanyId.ToInt(), req.TypeId);
            int total = 0;
            total = list.Count;
            list = list.Skip(req.offset).Take(req.limit).ToList();
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditExtendItem(int id, int typeId = 0)
        {
            ExtendItemModel model = _extendItemRepository.GetModel(id);
            ViewBag.ExtendItemModel = model;
            ViewBag.TypeId = typeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExtendItemSave(ExtendItemDto req)
        {
            ExtendItemModel model = new ExtendItemModel();

            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    var operatorUser = OperatorProvider.Provider.GetCurrent();
                    if (req.Id > 0)//修改
                    {
                        model = _extendItemRepository.GetModel(req.Id);
                        model.Code = req.Code;
                        model.Name = req.Name;
                        model.ItemValue = req.ItemValue;
                        model.Sort = req.Sort;
                        model.TypeId = req.TypeId;
                    }
                    else//新增
                    {
                        //判断code是否重复
                        var list = _extendItemRepository.GetModelList(operatorUser.CompanyId.ToInt(), req.TypeId).Where(x => x.Code == req.Code).ToList();
                        if (list.Count > 0)
                        {
                            res.Data = false;
                            res.Message = req.Code + "该代码已经存在，请重新输入!";
                            return Json(res, JsonRequestBehavior.AllowGet);
                        }


                        model.Code = req.Code;
                        model.Name = req.Name;
                        model.ItemValue = req.ItemValue;
                        model.Sort = req.Sort;
                        model.TypeId = req.TypeId;
                        model.CompanyId = Convert.ToInt32(operatorUser.CompanyId);
                    }
                    res.Data = _extendItemRepository.AddModel(model);
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

        [HttpPost]
        public JsonResult DeleteExtendItem(int id, int typeId)
        {
            Response res = new Response();
            try
            {
                if (typeId == 0)
                {
                    res.Data = false;
                    res.Message = "无效类型!";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                if (typeId == 10001)//订单类型
                {
                    var list = _orderRepository.GetListByOrderType(id);
                    if (list != null && list.Count > 0)
                    {
                        res.Data = false;
                        res.Message = "该订单类型，不可删除，与订单有关联!";
                    }
                    else
                    {
                        res.Data = _extendItemRepository.DelModel(id);
                    }

                }
                else if (typeId == 10002)//客源类型
                {
                    var list = _orderRepository.GetListByCustomerSource(id);
                    if (list != null && list.Count > 0)
                    {
                        res.Data = false;
                        res.Message = "该客源类型，不可删除，与订单有关联!";
                    }
                    else
                    {
                        res.Data = _extendItemRepository.DelModel(id);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExtendTypeItemsIndex()
        {
            return View();
        }
        
        #region 新界面Action

        public ActionResult NewIndex(int typeId)
        {
            var extendModel = _extendTypeRepository.GetModel(typeId);
            ViewBag.TypeName = extendModel.Name;
            ViewBag.TypeId = extendModel.Id;
            return View();
        }

        public ActionResult NewEdit(int id, int typeId = 0)
        {
            ExtendItemModel model = _extendItemRepository.GetModel(id);
            ViewBag.ExtendItemModel = model;
            ViewBag.TypeId = typeId;
            return View();
        } 

        public ActionResult CustomConfig()
        {
            return View();
        }
        #endregion
    }
}