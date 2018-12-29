using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OPUPMS.Web.Framework.Core.Plugin;

namespace OPUPMS.Web.Restaurant
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
            get { return "OPUPMS.Web.Restaurant"; }
        }

        /// <summary>
        /// 插件控制器命名控件。
        /// </summary>
        public string ControllerNamespace
        {
            get { return "OPUPMS.Web.Restaurant.Controllers"; }
        }

        /// <summary>
        /// 插件CSS、图片资源虚拟路径。
        /// </summary>
        public string ContentVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Web.Restaurant/Content/"; }
        }

        /// <summary>
        /// 插件脚本资源虚拟路径。
        /// </summary>
        public string ScriptsVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Web.Restaurant/Scripts/"; }
        }

        /// <summary>
        /// 插件页面模版虚拟路径。
        /// </summary>
        public string LayoutVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Web.Restaurant/Views/"; }
        }

        #endregion

        /// <summary>
        /// 路由注册。
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapMvcAttributeRoutes();//注册特性路由，顺序是从最精确的到模糊的匹配规则
                                           //TODO:注册路由。

            routes.MapRoute(
                name: "OPUPMS.Web.Restaurant.Api.Common",
                url: "{perfix}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { perfix = "^(?i)Res$" },
                namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Api.Common",
            //    url: "Res/Api/{action}/{id}",
            //    defaults: new { controller = "Api", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Area.Common",
            //    url: "Res/Area/{action}/{id}",
            //    defaults: new { controller = "Area", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Box.Common",
            //    url: "Res/Box/{action}/{id}",
            //    defaults: new { controller = "Box", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Category.Common",
            //    url: "Res/Category/{action}/{id}",
            //    defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Discount.Common",
            //    url: "Res/Discount/{action}/{id}",
            //    defaults: new { controller = "Discount", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Extend.Common",
            //    url: "Res/Extend/{action}/{id}",
            //    defaults: new { controller = "Extend", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Home.Common",
            //    url: "Res/Home/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Market.Common",
            //    url: "Res/Market/{action}/{id}",
            //    defaults: new { controller = "Market", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Package.Common",
            //    url: "Res/Package/{action}/{id}",
            //    defaults: new { controller = "Package", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Plugin.Common",
            //    url: "Res/Plugin/{action}/{id}",
            //    defaults: new { controller = "Plugin", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Project.Common",
            //    url: "Res/Project/{action}/{id}",
            //    defaults: new { controller = "Project", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Restaurant.Common",
            //    url: "Res/Restaurant/{action}/{id}",
            //    defaults: new { controller = "Restaurant", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Stalls.Common",
            //    url: "Res/Stalls/{action}/{id}",
            //    defaults: new { controller = "Stalls", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });

            //routes.MapRoute(
            //    name: "OPUPMS.Web.Restaurant.Table.Common",
            //    url: "Res/Table/{action}/{id}",
            //    defaults: new { controller = "Table", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { ControllerNamespace });
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