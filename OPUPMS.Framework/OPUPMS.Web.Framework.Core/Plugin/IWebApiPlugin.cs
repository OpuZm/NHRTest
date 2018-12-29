using System.Web.Http;

namespace OPUPMS.Web.Framework.Core.Plugin
{
    public interface IWebApiPlugin
    {
        void Register(HttpConfiguration config);
    }
}
