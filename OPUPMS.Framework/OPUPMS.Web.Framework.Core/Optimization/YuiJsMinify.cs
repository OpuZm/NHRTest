using System;
using System.Text;
using System.Web.Optimization;
using Yahoo.Yui.Compressor;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public class YuiJsMinify : IBundleTransform
    {
        internal static string JsContentType = "text/javascript";

        public virtual void Process(BundleContext context, BundleResponse response)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            if (!context.EnableInstrumentation)
            {
                JavaScriptCompressor compressor = new JavaScriptCompressor();
                compressor.Encoding = Encoding.UTF8;
                string str = compressor.Compress(response.Content);

                response.Content = str;
            }
            response.ContentType = JsContentType;
        }
    }
}
