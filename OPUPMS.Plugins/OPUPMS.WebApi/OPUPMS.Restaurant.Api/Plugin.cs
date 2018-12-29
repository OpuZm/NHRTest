using System.Web;
using System.Web.Http;
using System.Web.Routing;
using OPUPMS.Web.Framework.Core.Plugin;

namespace OPUPMS.Restaurant.Api
{
    public class Plugin : IWebApiPlugin
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
            get { return "OPUPMS.Restaurant.Api"; }
        }

        /// <summary>
        /// 插件控制器命名控件。
        /// </summary>
        public string ControllerNamespace
        {
            get { return "OPUPMS.Restaurant.Api.Controllers"; }
        }
        
        /// <summary>
        /// 插件CSS、图片资源虚拟路径。
        /// </summary>
        public string ContentVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Restaurant.Api/Content/"; }
        }

        /// <summary>
        /// 插件脚本资源虚拟路径。
        /// </summary>
        public string ScriptsVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Restaurant.Api/Scripts/"; }
        }

        /// <summary>
        /// 插件页面模版虚拟路径。
        /// </summary>
        public string LayoutVirtualPath
        {
            get { return "~/Plugins/Web/OPUPMS.Restaurant.Api/Views/"; }
        }

        #endregion
        /*
        /// <summary>
        /// 路由注册。
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapMvcAttributeRoutes();//注册特性路由，顺序是从最精确的到模糊的匹配规则
            //TODO:注册路由。
            routes.MapRoute(
                name: "Login",
                url: "",
                defaults: new { controller = "Account", action = "NewLogin", id = UrlParameter.Optional },
                namespaces: new[] { ControllerNamespace });

            routes.MapRoute(
                name: "OPUPMS.Web.Restaurant",
                url: "{perfix}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { perfix = "^(?i)Res$" },
                namespaces: new string[] { ControllerNamespace });
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
*/
        public void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "{perfix}/{controller}/{action}/{id}",
                 defaults: new { controller = "ApiPlugin", action = "Get", id = RouteParameter.Optional },
                 constraints: new { perfix = "^(?i)ResApi$" }
             );
        }
    }
}