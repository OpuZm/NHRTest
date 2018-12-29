using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OPUPMS.Web.Framework.Core.Plugin;

namespace OPUPMS.Web.Hotel
{
    public class Plugin : IWebPlugin
    {
        static readonly Plugin _instance = new Plugin();

        #region Properties

        public static Plugin Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// 插件名称。
        /// </summary>
        public string Name
        {
            get { return "OPUPMS.Web.Hotel"; }
        }

        /// <summary>
        /// 插件控制器命名控件。
        /// </summary>
        public string ControllerNamespace
        {
            get { return "OPUPMS.Web.Hotel.Controllers"; }
        }

        /// <summary>
        /// 插件CSS、图片资源虚拟路径。
        /// </summary>
        public string ContentVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Web.Hotel/Content/"; }
        }

        /// <summary>
        /// 插件脚本资源虚拟路径。
        /// </summary>
        public string ScriptsVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Web.Hotel/Scripts/"; }
        }

        /// <summary>
        /// 插件页面模版虚拟路径。
        /// </summary>
        public string LayoutVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Web.Hotel/Views/"; }
        }

        #endregion

        /// <summary>
        /// 路由注册。
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapMvcAttributeRoutes();
            //注册特性路由，顺序是从最精确的到模糊的匹配规则
            //TODO:注册路由。
            //routes.MapRoute(
            //    name: "Account",
            //    url: "account/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //    namespaces: new[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "Logout",
            //    url: "logout",
            //    defaults: new { controller = "Account", action = "Logout" },
            //    namespaces: new[] { ControllerNamespace });


            routes.MapRoute(
                name: "OPUPMS.Web.Hotel.Default",
                url: "{perfix}/{controller}/{action}/{id}",
                defaults: new { controller = "CloudPMS", action = "Index", id = UrlParameter.Optional },
                constraints: new { perfix = "^(?i)H$" },
                namespaces: new[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Hotel.Default",
            //    url: "H/CloudPMS/{action}",
            //    defaults: new { controller = "CloudPMS", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Hotel.RoomManage.Common",
            //    url: "H/RoomManage/{action}/{id}",
            //    defaults: new { controller = "RoomManage", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Hotel.BookingManage.Default",
            //    url: "H/BookingManage/{action}",
            //    defaults: new { controller = "BookingManage", action = "List", id = UrlParameter.Optional },
            //    namespaces: new[] { ControllerNamespace });

        }

        /// <summary>
        /// 资源注册。
        /// 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725。
        /// </summary>
        /// <param name="bundles"></param>
        public void RegisterBundles(BundleCollection bundles)
        {
            //TODO:注册 CSS 资源。
            //bundles.Add(PluginInfos.Name, (int)BundleFlag.CommonStyles,
            //    new YuiStyleBundle("~/Content/commonall.css").IncludeCssRewriteUrl(
            //    "~/Content/reset.css").IncludeCssRewriteUrl(
            //    "~/Content/common.css")
            //);

            //TODO:注册 JavaScript 资源。
            //bundles.Add(PluginInfos.Name, (int)BundleFlag.WebPluginTemplateScript,
            //   new ScriptBundle("~/WebPluginTemplate/WebPluginTemplate.js").Include(
            //   PluginInfos.GetScriptPath("WebPluginTemplate.js")));
        }

        /// <summary>
        /// 获取CSS、图片资源文件的完整虚拟路径。
        /// </summary>
        /// <param name="filePath">CSS、图片文件。</param>
        /// <returns></returns>
        public string GetContentPath(string filePath)
        {
            return string.Concat(ContentVirtualPath, filePath);
        }

        /// <summary>
        /// 获取脚本资源文件的完整虚拟路径。
        /// </summary>
        /// <param name="filePath">脚本文件。</param>
        /// <returns></returns>
        public string GetScriptPath(string filePath)
        {
            return string.Concat(ScriptsVirtualPath, filePath);
        }

        public string GetLayoutPath(string filePath)
        {
            return string.Concat(LayoutVirtualPath, filePath);
        }
    }
}