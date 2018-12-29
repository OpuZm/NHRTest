using System.Web.Mvc;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    /// <summary>
    /// 权限处理服务接口。
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 进行权限认证。
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool Authorize(AuthorizationContext filterContext);

        /// <summary>
        /// 处理权限认证失败的请求。
        /// </summary>
        /// <param name="filterContext"></param>
        void HandleUnauthorizedRequest(AuthorizationContext filterContext);
    }
}
