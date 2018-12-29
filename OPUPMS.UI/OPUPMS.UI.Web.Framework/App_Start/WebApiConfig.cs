using System.Web.Http;
using OPUPMS.Web.Framework.Core.WebApi;

namespace OPUPMS.UI.Web.Framework
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.MessageHandlers.Add(new TokenHandler(
            //    config.DependencyResolver.GetService(typeof(IWebApiTokenService)) as IWebApiTokenService));
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        }
    }
}