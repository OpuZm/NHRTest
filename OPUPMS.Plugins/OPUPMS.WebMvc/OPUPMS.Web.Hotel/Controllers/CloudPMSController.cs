using Newtonsoft.Json;
using OPUPMS.Domain.Base.Services;
using OPUPMS.Domain.Hotel.Model.Dtos;
using OPUPMS.Web.Hotel.Models.Dtos;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common.Web;
using OPUPMS.Web.Framework.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OPUPMS.Web.Hotel.Controllers
{
    public class CloudPMSController : AuthorizationController
    {
        readonly IMenuManageService<HotelMenuDto> _menuService;
        public CloudPMSController(IMenuManageService<HotelMenuDto> menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CurrentDate = DateTime.Today.ToString("yyyy-MM-dd");
            return View();
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        
        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        

        public ActionResult Home()
        {
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm");
            ViewBag.Weekday = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            ViewBag.MonthDate = DateTime.Today.ToString(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern);
            return View();
        }

        #region 系统菜单处理

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetClientMenuJson()
        {
            var currentOperator = OperatorProvider.Provider.GetCurrent();
            var sourceList = _menuService.GetMenuList(currentOperator.ConnectToken, currentOperator.UserId);
            List<MenuJsonDto> jsonList = ConvertToMenuJsonObject(sourceList);
            var menuList = ToMenuJson(jsonList, 0);

            return Content(menuList);
        }
        
        private List<MenuJsonDto> ConvertToMenuJsonObject(List<HotelMenuDto> sourceMenuList)
        {
            List<MenuJsonDto> jsonList = new List<MenuJsonDto>();
            foreach (var item in sourceMenuList)
            {
                MenuJsonDto json = new MenuJsonDto();
                json.id = item.MenuId;
                json.parentId = item.ParentMenuId;
                json.icon = item.IconName;
                json.href = item.MenuUrl;
                json.menuvalue = item.MenuValue;
                //json.spread = false;
                json.title = item.MenuName;
                if (item.SubMenus != null && item.SubMenus.Count > 0)
                {
                    json.submenus = ConvertToMenuJsonObject(item.SubMenus);
                }
                jsonList.Add(json);
            }
            return jsonList;
        }

        /// <summary>
        /// json 数据转换
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private string ToMenuJson(List<MenuJsonDto> menuList, int parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("[");
            List<MenuJsonDto> entitys = menuList.FindAll(x => x.parentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    string strJson = item.ToJson();
                    if (item.submenus != null && item.submenus.Count > 0)
                        strJson = strJson.Insert(strJson.Length - 1, ",\"children\":" + ToMenuJson(item.submenus, item.id) + "");
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

        #endregion
    }
}
