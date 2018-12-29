using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OPUPMS.Web.Framework.Core.WebApi
{
    public class TokenHandler : DelegatingHandler
    {
        readonly IWebApiTokenService _service;

        public TokenHandler(IWebApiTokenService service)
        {
            _service = service;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> result = _service.CheckToken(request, cancellationToken);
            if (result != null)
            {
                return result;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
