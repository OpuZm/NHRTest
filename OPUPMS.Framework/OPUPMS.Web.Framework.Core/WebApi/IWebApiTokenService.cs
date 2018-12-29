using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OPUPMS.Web.Framework.Core.WebApi
{
    public interface IWebApiTokenService
    {
        Task<HttpResponseMessage> CheckToken(
            HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
