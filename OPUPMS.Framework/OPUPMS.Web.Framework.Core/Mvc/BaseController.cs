using System.Text;
using System.Web.Mvc;
using log4net;
using OPUPMS.Infrastructure.Common.Web;
using OPUPMS.Web.Framework.Core.Json;
using OPUPMS.Web.Framework.Core.Mvc.Filter;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    /// <summary>
    /// 提供用于响应对 ASP.NET MVC 网站所进行的 HTTP 请求的方法，并记录执行方法出现的错误日志。
    /// </summary>
    [HandleAndLogExceptionFilter]
    public abstract class BaseController : Controller
    {
        public ILog Logger
        {
            get;
            set;
        }

        protected BaseController() : base()
        {
        }

        protected internal NewtonSoftJsonResult NewtonSoftJson(
            object data, bool ignoreNullValue = true)
        {
            return NewtonSoftJson(data, null /* contentType */,
                null /* contentEncoding */, JsonRequestBehavior.DenyGet, ignoreNullValue);
        }

        protected internal NewtonSoftJsonResult NewtonSoftJson(
            object data, string contentType, bool ignoreNullValue = true)
        {
            return NewtonSoftJson(data, contentType, 
                null /* contentEncoding */, JsonRequestBehavior.DenyGet, ignoreNullValue);
        }

        protected internal virtual NewtonSoftJsonResult NewtonSoftJson(
            object data, string contentType, 
            Encoding contentEncoding, bool ignoreNullValue = true)
        {
            return NewtonSoftJson(data, contentType, 
                contentEncoding, JsonRequestBehavior.DenyGet, ignoreNullValue);
        }

        protected internal NewtonSoftJsonResult NewtonSoftJson(
            object data, JsonRequestBehavior behavior, bool ignoreNullValue = true)
        {
            return NewtonSoftJson(data, null /* contentType */, 
                null /* contentEncoding */, behavior, ignoreNullValue);
        }

        protected internal NewtonSoftJsonResult NewtonSoftJson(object data, 
            string contentType, JsonRequestBehavior behavior, bool ignoreNullValue = true)
        {
            return NewtonSoftJson(data, contentType, 
                null /* contentEncoding */, behavior, ignoreNullValue);
        }

        protected internal virtual NewtonSoftJsonResult NewtonSoftJson(
            object data, string contentType, Encoding contentEncoding,
            JsonRequestBehavior behavior, bool ignoreNullValue = true)
        {
            var result = new NewtonSoftJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };

            if(ignoreNullValue)
            {
                result.Settings = JsonDefaultSerializerSettings.IgnoreNullValue;
            }

            return result;
        }

        protected override JsonResult Json(object data, string contentType,
            Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new MyJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                FormateStr = "yyyy-MM-dd HH:mm:ss",
                MaxJsonLength=int.MaxValue
            };
        }
    }
}
