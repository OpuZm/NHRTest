using System.Web.Optimization;
using System.Web.Routing;

namespace OPUPMS.Web.Framework.Core.Plugin
{
    /// <summary>
    ///  OPUPMS.Web.Framework 框架 Web 插件接口。
    /// </summary>
    public interface IWebPlugin
    {
        /// <summary>
        /// 插件名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 插件控制器命名控件。
        /// </summary>
        string ControllerNamespace { get; }

        /// <summary>
        /// 插件CSS、图片资源虚拟路径。
        /// </summary>
        string ContentVirtualPath { get; }

        /// <summary>
        /// 插件脚本资源虚拟路径。
        /// </summary>
        string ScriptsVirtualPath { get; }

        /// <summary>
        /// 路由注册。
        /// </summary>
        /// <param name="routes"></param>
        void RegisterRoutes(RouteCollection routes);

        /// <summary>
        /// 资源注册。
        /// </summary>
        /// <param name="bundles"></param>
        void RegisterBundles(BundleCollection bundles);
    }
}
