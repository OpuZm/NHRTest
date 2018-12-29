using System.Text;
using System.Web.Mvc;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Domain.AuthorizeService
{
    /// <summary>
    /// 权限认证，只要登录成功就可以进行任何操作。
    /// </summary>
    public class PermissionAuthorizeService : IAuthorizeService
    {
        public bool Authorize(AuthorizationContext filterContext)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            //if (operatorProvider == null || filterContext.HttpContext.Request.UrlReferrer == null)
            //{
            //    var result = new JsonResult
            //    {
            //        ContentType = "NoPermission"
            //    };
            //    filterContext.Result = result;

            //    return false;
            //}

            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //var controllerName = filterContext.RouteData.Values["controller"].ToString();
                //var actionName = filterContext.RouteData.Values["action"].ToString();
                //var nameSpace = NamespaceHelpers.GetNameSpace(filterContext.RouteData);
                //var completeControllerName = string.Concat(nameSpace, controllerName);

                var attribute = GetCustomerAuthorizeAttribute(filterContext);
                if(attribute != null)
                {
                    if ((operatorProvider.Permission & (int)attribute.Permission) == 0)
                    {
                        var result = new JsonResult
                        {
                            ContentType = "NoPermission",
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        filterContext.Result = result;
                        return false;
                    }
                }

                return true;
            }
            else
            {
                var result = new JsonResult
                {
                    ContentType = "LoginTimeOut"
                };
                filterContext.Result = result;
                return false;
            }
        }

        public void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            JsonResult result = filterContext.Result as JsonResult;
            StringBuilder sbScript = new StringBuilder();
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                switch (result.ContentType)
                {
                    case "LoginTimeOut":
                        sbScript.Append("<script type='text/javascript'>alert('访问超时，请重新登录！');top.location='/Res/Login';</script>");
                        //filterContext.Result = new JavaScriptResult() { Script = sbScript.ToString() };
                        break;
                    case "NoPermission":
                        sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！'); if(window.parent){var index = parent.layer.getFrameIndex(window.name); parent.layer.close(index);} else{history.go(-1);}</script>");
                        //filterContext.Result = new JavaScriptResult() { Script = sbScript.ToString() };
                        break;
                    default:
                        break;
                }
                filterContext.HttpContext.Response.Write(sbScript.ToString());
            }
            else
            {
                Response res = new Response()
                {
                    Data = null,
                    Successed = false,
                    Message = "您的权限不足，访问被拒绝"
                };
                result.Data = res;
                filterContext.Result = result;
            }
        }

        CustomerAuthorizeAttribute GetCustomerAuthorizeAttribute(AuthorizationContext filterContext)
        {
            object[] actionAuthorizeAttributes = filterContext.ActionDescriptor
                    .GetCustomAttributes(typeof(CustomerAuthorizeAttribute), false);
            if(actionAuthorizeAttributes.Length > 0)
            {
                return actionAuthorizeAttributes[0] as CustomerAuthorizeAttribute;
            }

            object[] controllerAuthorizeAttributes = filterContext
                        .ActionDescriptor
                        .ControllerDescriptor
                        .GetCustomAttributes(typeof(CustomerAuthorizeAttribute), false);

            if (controllerAuthorizeAttributes.Length > 0)
            {
                return controllerAuthorizeAttributes[0] as CustomerAuthorizeAttribute;
            }

            return null;
        }
    }
}