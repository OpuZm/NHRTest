using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace OPUPMS.Web.Framework.Core.Mvc.Filter
{
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is AllowMultiple = true and users might want to override behavior.")]
    [AttributeUsage(AttributeTargets.Class |
        AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HandleAndLogExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public HandleAndLogExceptionFilterAttribute()
            : base()
        {
        }

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            int httpCode = new HttpException(null, filterContext.Exception).GetHttpCode();

            ILog logger = null;
            if (filterContext.Controller is BaseController baseController)
            {
                logger = baseController.Logger;
            }

            if (logger == null)
            {
                logger = LogManager.GetLogger(filterContext.Controller.GetType());
            }

            Trace.Assert(logger != null, "Logger is null!");

            if (logger != null)
            {
                logger.ErrorFormat(
                    "Controller：{0}， Action：{1}，HttpCode：{2}, Exception:\n\t{3}",
                    controllerName, actionName, httpCode, filterContext.Exception);
            }

            if (filterContext.IsChildAction)
            {
                return;
            }

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // If this is not an HTTP 500 (
            // for example, if somebody throws an HTTP 404 from an action method),
            // ignore it.
            if (httpCode != 500)
            {
                return;
            }

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new NewtonSoftJsonResult
                {
                    ContentEncoding = Encoding.UTF8,
                    Data = new
                    {
                        ControllerName = controllerName,
                        ActionName = actionName,
                        filterContext.Exception.Message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                HandleErrorInfo model = new HandleErrorInfo(
                    filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ContentResult
                {
                    Content = string.Format(
                        Resources.Resource.Error_Handle_Html,
                        controllerName, actionName, filterContext.Exception.ToString()),
                    ContentEncoding = Encoding.UTF8,
                    ContentType = "text/html; charset=utf-8"
                };
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}