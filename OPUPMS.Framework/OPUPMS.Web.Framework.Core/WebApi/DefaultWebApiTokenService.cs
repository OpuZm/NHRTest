using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OPUPMS.Web.Framework.Core.WebApi
{
    public class DefaultWebApiTokenService : IWebApiTokenService
    {
        readonly string Token = "OPUPMS.Web.Framework-WebApi-Ver1-2013-08-20";

        public Task<HttpResponseMessage> CheckToken(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //string cookieToken = string.Empty;
            //string formToken = string.Empty;

            //IEnumerable<string> tokenHeaders;
            //if (request.Headers.TryGetValues("Token", out tokenHeaders))
            //{
            //    string[] tokens = tokenHeaders.First().Split(':');
            //    if (tokens.Length == 2)
            //    {
            //        cookieToken = tokens[0].Trim();
            //        formToken = tokens[1].Trim();
            //        try
            //        {
            //            AntiForgery.Validate(cookieToken, formToken);
            //            return null;
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //}

            try
            {
                IEnumerable<string> tokenHeaders;
                if (request.Headers.TryGetValues("Token", out tokenHeaders))
                {
                    string token = tokenHeaders.First();
                    if (token.Equals(Token))
                    {
                        return null;
                    }
                }
            }
            catch
            {
            }

            HttpResponseMessage responseMessage = request.CreateErrorResponse(
                HttpStatusCode.Unauthorized, "Request is missing authorization token.");
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(responseMessage);
            return tsc.Task;
        }
    }
}