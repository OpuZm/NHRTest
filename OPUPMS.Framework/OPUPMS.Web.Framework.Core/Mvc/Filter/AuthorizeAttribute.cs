using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace OPUPMS.Web.Framework.Core.Mvc.Filter
{
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", 
        Justification = "Unsealed so that subclassed types can set properties in the default constructor or override our behavior.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public IAuthorizeService AuthorizeService { get; set; }

        //This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
        protected bool AuthorizeCore(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            return AuthorizeService.Authorize(filterContext);
        }

        private void CacheValidateHandler(HttpContext context, 
            object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(
                new HttpContextWrapper(context), data as AuthorizationContext);
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException(
                    "AuthorizeAttribute cannot be used within a child action caching block.");
            }

            Type allowAnonymousAttributeType = typeof(AllowAnonymousAttribute);
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(
                    allowAnonymousAttributeType, inherit: true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                    allowAnonymousAttributeType, inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            if (AuthorizeCore(filterContext))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized user
                // to cause the page to be cached, then an unauthorized user would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.

                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, filterContext);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            //filterContext.Result = new HttpUnauthorizedResult();
            AuthorizeService.HandleUnauthorizedRequest(filterContext);
        }

        // This method must be thread-safe since it is called by the caching module.
        protected HttpValidationStatus OnCacheAuthorization(
            HttpContextBase httpContext, AuthorizationContext filterContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool isAuthorized = AuthorizeCore(filterContext);
            return isAuthorized ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }
    }
}