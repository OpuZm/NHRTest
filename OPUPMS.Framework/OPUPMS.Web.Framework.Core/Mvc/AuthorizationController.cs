namespace OPUPMS.Web.Framework.Core.Mvc
{
    /// <summary>
    /// 提供用于响应对 ASP.NET MVC 网站所进行的 HTTP 请求的方法，
    /// 并对执行方法进行权限验证和错误处理。
    /// </summary>
    [Filter.Authorize]
    public abstract class AuthorizationController : BaseController
    {
        protected AuthorizationController() : base()
        {
            
        }
    }
}
