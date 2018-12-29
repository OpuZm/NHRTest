using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Domain.Base.Repositories;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class ProjectController : BaseController
    {
        readonly IProjectRepository _projectRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IExtendRepository _extendRepository;
        readonly IOrderRepository _orderRepository;
        readonly ITableService _tableServices;
        readonly IAreaRepository _areaRepository;
        readonly IExtendItemRepository _extendItemRepository;
        readonly IRestaurantCategoryRepository _restaurantCategoryRepository;
        readonly IPrinterRepository _printerRepository;
        readonly IOrderDetailCauseRepository _orderDetailCauseRepository;
        readonly ICustomConfigRepository _customConfigRepository;

        public ProjectController(
            IProjectRepository projectRepository,
            ICategoryRepository categoryRepository,
            IExtendRepository extendRepository,
            IOrderRepository orderRepository,
            ITableService tableService,
            IAreaRepository areaRepository,
            IExtendItemRepository extendItemRepository,
            IRestaurantCategoryRepository restaurantCategoryRepository,
            IPrinterRepository printerRepository,
            IOrderDetailCauseRepository orderDetailCauseRepository,
            ICustomConfigRepository customConfigRepository)
        {
            _projectRepository = projectRepository;
            _categoryRepository = categoryRepository;
            _extendRepository = extendRepository;
            _orderRepository = orderRepository;
            _tableServices = tableService;
            _areaRepository = areaRepository;
            _extendItemRepository = extendItemRepository;
            _restaurantCategoryRepository = restaurantCategoryRepository;
            _printerRepository = printerRepository;
            _orderDetailCauseRepository = orderDetailCauseRepository;
            _customConfigRepository = customConfigRepository;
        }

        [CustomerAuthorize(Permission.餐饮项目管理)]
        public ActionResult Index()
        {
            var categorys = _categoryRepository.GetChildList();
            ViewBag.Categorys = categorys;
            return View();
        }

        [CustomerAuthorize(Permission.餐饮项目管理)]
        public ActionResult GetProjects(ProjectSearchDTO req)
        {
            if (req.ListType == 1)
                req.offset = (req.offset - 1) * req.limit;

            int total = 0;
            var list = _projectRepository.GetList(out total, req);
            return Json(new { rows = list, total = total, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        [CustomerAuthorize(Permission.餐饮项目管理)]
        public ActionResult Edit(int id = 0)
        {
            var categorys = _categoryRepository.GetChildList();
            var model = _projectRepository.GetModel(id);
            var extend = _extendRepository.GetList();
            ViewBag.Project = model;
            ViewBag.Categorys = categorys;
            ViewBag.Extend = extend;
            return View();
        }

        [ValidateInput(false)]
        [CustomerAuthorize(Permission.餐饮项目管理)]
        [HttpPost]
        public ActionResult Edit(ProjectCreateDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    if (req.Id > 0)
                    {
                        res.Data = _projectRepository.Update(req, out string msg);
                        res.Message = msg;
                    }
                    else
                    {
                        var currentUser = OperatorProvider.Provider.GetCurrent();
                        req.R_Company_Id = currentUser.CompanyId.ToInt();
                        req.CreateDate = DateTime.Now;
                        res.Data = _projectRepository.Create(req, out string msg);
                        res.Message = msg;
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
            return Json(res, JsonRequestBehavior.DenyGet);
        }

        [CustomerAuthorize(Permission.餐饮项目管理)]
        [HttpPost]
        public ActionResult SubmitExtend(ProjectExtendCreateDTO req)
        {
            Response res = new Response();
            try
            {
                res.Data = _projectRepository.ExtendCreate(req);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModel(int id)
        {
            var model = _projectRepository.GetModel(id);
            return Json(new { Data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Specification(int id = 0)
        {
            var model = _projectRepository.GetModel(id);
            ViewBag.Project = model;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult SpecificationSubmit(int cyxmId, List<R_ProjectDetail> list)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _projectRepository.SpecificationSubmit(cyxmId, list);
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
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectDetails(int category = 0, bool hasPackage = false)
        {
            Response res = new Response
            {
                Data = _projectRepository.GetDetailList(category, hasPackage)
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectAndDetails(int category = 0, bool hasPackage = false)
        {
            Response res = new Response
            {
                Data = _projectRepository.GetProjectAndDetailList(category, hasPackage)
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #region 新界面Action

        #region 点餐 菜品转台 多桌点餐 打厨单页面初始化 结账判断 换台
        /// <summary>
        /// 点餐页面初始化
        /// </summary>
        /// <param name="orderTableId">多个订单台号列表</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitFormInfo(List<int> orderTableId)
        {
            var itemAndDetails = _projectRepository.GetProjectAndDetailList(0, true);//菜品明细
            var categories = _categoryRepository.GetAllCategoryList();//菜品分类 
            var orderAndTables = _orderRepository.GetOrderAndTablesByOrderTableId(orderTableId[0]);
            //var projectExtendSplitList = _projectRepository.GetProjectExtendSplitList();
            var projectExtendSplitList = _projectRepository.GetProjectExtendSplitListNew();
            var projects = _projectRepository.GetDetailList(0, false);
            List<OrderDetailDTO> orderTableProjects = new List<OrderDetailDTO>();
            if (orderTableId.Count == 1)//
            {
                orderTableProjects = _orderRepository.GetOrderTableProjects(orderTableId[0]);//已点菜品
            }
            else if (orderTableId.Count > 1)
            {
                var Tables = _orderRepository.GetTablesByOrderTableIds(orderTableId);
                orderAndTables.TableName = Tables.Count > 1 ? string.Join(",", Tables.Select(p => p.Name)) : orderAndTables.TableName;
                orderTableProjects = _orderRepository.GetOrderTableProjects(orderTableId[0]);//已点菜品
            }
            itemAndDetails = _restaurantCategoryRepository.FilterRestaurantCategorys(itemAndDetails, orderAndTables.R_Restaurant_Id);
            categories = _restaurantCategoryRepository.FilterRestaurantCategorys(categories, orderAndTables.R_Restaurant_Id);
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var permissions = new Dictionary<string, bool>();
            permissions.Add("IsGive",(currentUser.Permission & (int)Permission.赠菜) ==0?false:true);
            permissions.Add("IsReturn", (currentUser.Permission & (int)Permission.退菜) == 0 ? false : true);
            var orderDetailCauses = _orderDetailCauseRepository.GetAllList();
            var customConfigs = _customConfigRepository.GetList(new CustomConfigDTO() { PageModule = (int)PageModule.点餐界面PC端 });
            var customConfigsFlat= _customConfigRepository.GetList(new CustomConfigDTO() { PageModule = (int)PageModule.点餐界面平板端 });
            var info = new
            {
                OrderAndTables = orderAndTables,
                ProjectAndDetails = itemAndDetails,
                CategoryList = categories,
                OrderTableProjects = orderTableProjects,
                ProjectExtendSplitList = projectExtendSplitList,
                Projects = projects,
                PrintModel = _printerRepository.GetPrintModel(),
                UserPermission = permissions,
                GiveCauses = orderDetailCauses.Where(p => p.CauseType == CauseType.赠菜).ToList(),
                ReturnCauses= orderDetailCauses.Where(p => p.CauseType == CauseType.退菜).ToList(),
                OrderDetailTest=_projectRepository.GetOrderDetailIsTeset(),
                CustomConfigs = customConfigs,
                CustomConfigsFlat= customConfigsFlat,
                AutoListPrint=_orderRepository.GetAutoListPrint(),
                DefaultPromptly=_orderRepository.GetDefaultPromptly()
            };
            return Json(info);
        }

        /// <summary>
        /// 菜品转台页面初始化
        /// </summary>
        /// <param name="orderTableId">订单台号Id</param>
        /// <param name="restaurantId">餐厅Id</param>
        [HttpPost]
        /// <returns></returns>
        public ActionResult InitChangeTableInfo(int orderTableId, int restaurantId)
        {
            //获取当前台号已点菜品列表
            var orderTableProjects = _orderRepository.GetOrderTableProjects(orderTableId).Where(p => p.CyddMxStatus != CyddMxStatus.保存).ToList() ?? new List<OrderDetailDTO>();//已点菜品,除保存状态的
            //获取当前餐厅下其他开台状态的列表
            var req = new TableSearchDTO()
            {
                CompanyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt(),
                RestaurantId = restaurantId,
                CythStatus = CythStatus.在用,
                OrderTableId = orderTableId
            };
            var areas = _areaRepository.GetList(req.RestaurantId);
            var tables = _tableServices.GetTableList(req);
            var info = new
            {
                Areas=areas,
                OrderTableProjects = orderTableProjects,
                Tables = tables
            };
            return Json(info);
        }

        /// <summary>
        /// 多桌点餐页面初始化
        /// </summary>
        /// <param name="orderTableId">订单台号Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitChoseTableInfo(int orderTableId)
        {
            //获取当前台号已点菜品列表
            var orderTableProjects = _orderRepository.GetOrderTableProjects(orderTableId).Where(p => p.CyddMxStatus != CyddMxStatus.保存).ToList() ?? new List<OrderDetailDTO>();//已点菜品,除保存状态的
            //获取当前台号订单下其他台号的列表
            var tables = _orderRepository.GetTablesByOrderTableId(orderTableId).Where(p => p.Id != orderTableId).ToList() ?? new List<OrderTableDTO>();
            var info = new
            {
                OrderTableProjects = orderTableProjects,
                Tables = tables
            };
            return Json(info);
        }

        /// <summary>
        /// 换台页面初始化
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitAllChangeTableInfo(TableSearchDTO req)
        {
            //获取当前餐厅下的区域
            var areas = _areaRepository.GetList(req.RestaurantId);
            //获取当前餐厅下所有区域的空置台号
            var currentUser = OperatorProvider.Provider.GetCurrent();
            req.CompanyId = currentUser.CompanyId.ToInt();
            req.CythStatus = CythStatus.空置;
            req.InCludVirtual = false;
            var tables = _tableServices.GetTableList(req);
            var orderAndTables = _orderRepository.GetOrderAndTablesByOrderTableId(req.OrderTableId);
            var info = new
            {
                Areas = areas,
                Tables = tables,
                OrderAndTables = orderAndTables
            };
            return Json(info);
        }

        /// <summary>
        /// 打厨单页面初始化
        /// </summary>
        /// <param name="orderTableId">订单台号Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitCookOrderInfo(int orderTableId)
        {
            //获取当前台号已点菜品状态为保存的列表
            Response res = new Response
            {
                Data = _orderRepository.GetOrderTableProjects(orderTableId).ToList() ?? new List<OrderDetailDTO>()
            };
            return Json(res);
        }

        /// <summary>
        /// 判断结账时当前是否还存在保存状态的数据
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JudgeOrderPay(List<OrderTableDTO> orderTableInfos)
        {
            //获取当前台号已点菜品状态为保存的列表
            Response res = new Response();
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            //取餐饮账务日期 TypeId=10003
            var dateItem = _extendItemRepository.GetModelList(operatorUser.CompanyId.ToInt(), 10003).FirstOrDefault();
            if (dateItem == null)
                throw new Exception("餐饮账务日期尚未初始化，请联系管理员");
            DateTime accDate = DateTime.Today;
            if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                throw new Exception("餐饮账务日期设置错误，请联系管理员");
            if (orderTableInfos == null || !orderTableInfos.Any())
            {
                res.Data = false;
                res.Message = "请选择要打厨的数据！";
                return Json(res);
            }
            var data = _orderRepository.JudgeOrderPay(orderTableInfos, operatorUser.LoginMarketId, dateItem.ItemValue, out string msg);
            res.Data = data;
            res.Message = msg;
            return Json(res);
        }
        #endregion

        [CustomerAuthorize(Permission.餐饮项目管理)]
        public ActionResult NewIndex()
        {
            var categorys = _categoryRepository.GetAllCategoryList();
            ViewBag.Categorys = categorys;
            return View();
        }

        [CustomerAuthorize(Permission.餐饮项目管理)]
        public ActionResult NewEdit(int id = 0)
        {
            var categorys = _categoryRepository.GetChildList();
            var model = _projectRepository.GetModel(id);
            var extend = _extendRepository.GetList();
            ViewBag.Project = model;
            ViewBag.Categorys = categorys;
            ViewBag.Extend = extend;
            return View();
        }

        public ActionResult NewEditExtend(int id)
        {
            Response res = new Response();
            var projectExtendSplitList = _projectRepository.GetProjectExtendSplitListByProjectId(id);

            var projectImages= _projectRepository.GetProjectImages(id,1);
            res.Data = new { Extends=projectExtendSplitList,Images= projectImages };
            return Json(res);
        }

        public ActionResult NewSpecification(int id = 0)
        {
            var model = _projectRepository.GetModel(id);
            ViewBag.Project = model;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [CustomerAuthorize(Permission.估清)]
        public ActionResult ProjectStockSubmit(List<ProjectClearDTO> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _projectRepository.ProjectClearSubmit(req);
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

        /// <summary>
        /// 菜品估清初始化数据
        /// </summary>
        /// <returns></returns>
        [CustomerAuthorize(Permission.估清)]
        public ActionResult ProjectClearInit()
        {
            var itemAndDetails = _projectRepository.GetProjectAndDetailList(0, false);//菜品明细
            var categories = _categoryRepository.GetAllCategoryList();//菜品分类 
            List<OrderDetailDTO> orderTableProjects = new List<OrderDetailDTO>();
            var info = new
            {
                ProjectAndDetails = itemAndDetails,
                CategoryList = categories
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
                Data = _projectRepository.IsDelete(id)
            };
            return Json(res);
        }

        [HttpPost]
        public ActionResult IsEnable(List<int> ids,bool enable)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data =  _projectRepository.IsEnable(ids, enable);
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

        [CustomerAuthorize(Permission.菜品推荐设置)]
        [HttpPost]
        public ActionResult RecomandConfigSubmit()
        {
            return View();
        }

        //[CustomerAuthorize(Permission.餐饮系统设置)]
        [HttpPost]
        public ActionResult ImageUpdateSingle(HttpPostedFileBase file)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = ConfigurationManager.AppSettings["ProjectImagePath"];   //图片上传地址
                    int maxImgSize = int.Parse(ConfigurationManager.AppSettings["UploadImgMaxSize"]);   //图片尺寸
                    int maxImgMB = maxImgSize / 1000000;
                    ImageThumbnailMaker imgHelp = new ImageThumbnailMaker();
                    if (file.ContentLength > maxImgSize)
                    {
                        res = new Response() { Successed = false, Data = null, Message = string.Format("图片大小超出尺寸,系统规定为{0}M", maxImgMB) };
                    }
                    else
                    {
                        string mediaPath = imgHelp.UploadImage(file, 320, 0, imagePath, true);
                        if (string.IsNullOrEmpty(mediaPath))
                        {
                            res = new Response() { Successed = false, Data = null, Message = string.Format("图片上传失败,请联系管理员") };
                        }
                        else
                        {
                            res = new Response() { Data = mediaPath, Successed = true, Message = "" };
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Data = null;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Data = null;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        [HttpPost]
        public ActionResult ImageUpload(HttpPostedFileBase file, ProjectImageUploadDTO req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = ConfigurationManager.AppSettings["ProjectImagePath"];   //图片上传地址
                    int maxImgSize = int.Parse(ConfigurationManager.AppSettings["UploadImgMaxSize"]);   //图片尺寸
                    int maxImgMB = maxImgSize / 1000000;
                    ImageThumbnailMaker imgHelp = new ImageThumbnailMaker();
                    if (file.ContentLength > maxImgSize)
                    {
                        res = new Response() { Successed = false, Data = null, Message = string.Format("图片大小超出尺寸,系统规定为{0}M",maxImgMB) };
                    }
                    else
                    {
                        string mediaPath = imgHelp.UploadImage(file, 320, 0, imagePath, true);
                        if (string.IsNullOrEmpty(mediaPath))
                        {
                            res = new Response() { Successed = false, Data = null, Message = string.Format("图片上传失败,请联系管理员") };
                        }
                        else
                        {
                            ProjectImageDTO model = new ProjectImageDTO()
                            {
                                Url = mediaPath,
                                CyxmTpSourceType = req.CyxmTpSourceType,
                                Sorted = req.Sorted,
                                IsCover = req.IsCover,
                                Source_Id = req.Source_Id
                            };
                            //req.Url = mediaPath;
                            var result = _projectRepository.EditProjectImage(model);
                            res = new Response() { Data = result, Successed = result != null ? true : false, Message = "" };
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Data = null;
                    res.Message = ex.Message;
                }
            }
            else
            {
                res.Data = null;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        public ActionResult UploadImage(int id)
        {
            return View();
        }

        public ActionResult EditImage(int id)
        {
            return View();
        }

        public ActionResult GetProjectImage(int id)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                res.Data = _projectRepository.GetProjectImages(id,1);
            }
            else
            {
                res.Data = null;
                res.Message = string.Join(",", ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        [CustomerAuthorize(Permission.餐饮系统设置)]
        [HttpPost]

        public ActionResult DeleteProjectImage(int id)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    var projectImage = _projectRepository.GetProjectImageModel(id);
                    bool result = _projectRepository.DeleteProjectImage(id);
                    if (result == true && projectImage != null)
                    {
                        FileInfo file = new FileInfo(Server.MapPath(projectImage.Url));
                        if (file.Exists)
                        {
                            file.Attributes = FileAttributes.Normal;
                            file.Delete();
                        }
                    }
                    res.Data = result;
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
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

        [CustomerAuthorize(Permission.餐饮系统设置)]
        [HttpPost]
        public ActionResult BathUpdateProjectImage(List<ProjectImageDTO> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _projectRepository.BathUpdateProjectImage(req);
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
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
        #endregion

        public ActionResult ProjectRecommend()
        {
            return View();
        }

        [HttpPost]
        [CustomerAuthorize(Permission.菜品推荐设置)]
        public ActionResult ProjectRecommendSubmit(List<ProjectRecomandDTO> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _projectRepository.ProjectRecommendSubmit(req);
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
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

        [HttpPost]
        public ActionResult OrderDetailPrintTesting(List<OrderDetailDTO> req)
        {
            Response res = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    res.Data = _projectRepository.OrderDetailPrintTesting(req);
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
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