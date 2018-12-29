using System;
using System.Web.Optimization;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public class YuiScriptBundle : Bundle
    {
        public YuiScriptBundle(string virtualPath)
            : this(virtualPath, null)
        {
        }

        public YuiScriptBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, new IBundleTransform[] { new YuiJsMinify() })
        {
            base.ConcatenationToken = ";" + Environment.NewLine;
        }
    }
}
