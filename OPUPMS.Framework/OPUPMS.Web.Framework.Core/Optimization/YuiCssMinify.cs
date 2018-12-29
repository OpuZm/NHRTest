using System;
using System.Web.Optimization;
using Yahoo.Yui.Compressor;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public class YuiCssMinify : IBundleTransform
    {
        internal static string CssContentType = "text/css";

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
                CssCompressor compressor = new CssCompressor();
                string str = compressor.Compress(response.Content);
                response.Content = str;
            }
            response.ContentType = CssContentType;
        }
    }
}
