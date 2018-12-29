using System.Web.Optimization;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public class YuiStyleBundle : Bundle
    {
        public YuiStyleBundle(string virtualPath)
            : base(virtualPath, new IBundleTransform[] { new YuiCssMinify() })
        {
        }

        public YuiStyleBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, new IBundleTransform[] { new YuiCssMinify() })
        {
        }
    }
}
