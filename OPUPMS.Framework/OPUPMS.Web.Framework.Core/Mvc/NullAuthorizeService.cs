using System.Web.Mvc;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    /// <summary>
    /// 空权限认证，只要登录成功就可以进行任何操作。
    /// </summary>
    public class NullAuthorizeService : IAuthorizeService
    {
        public bool Authorize(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //var controllerName = filterContext.RouteData.Values["controller"].ToString();
                //var actionName = filterContext.RouteData.Values["action"].ToString();
                //var nameSpace = NamespaceHelpers.GetNameSpace(filterContext.RouteData);
                //var completeControllerName = string.Concat(nameSpace, controllerName);
                //TODO:这里加上判断权限的业务逻辑。

                return true;
            }

            return false;
        }

        public void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}