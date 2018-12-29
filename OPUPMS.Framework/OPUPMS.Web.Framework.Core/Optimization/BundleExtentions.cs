using System.Web.Optimization;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public static class BundleExtentions
    {
        /// <summary>
        /// 对 CSS 资源打包压缩，并替换 CSS 中的相对路径为绝对路径，以避免 CSS 中引用的资源（如：图像）路径不对，导致失效。
        /// </summary>
        /// <param name="bundle"><see cref="System.Web.Optimization.Bundle"/></param>
        /// <param name="virtualPaths">资源的虚拟路径。</param>
        /// <returns><see cref="System.Web.Optimization.Bundle"/></returns>
        public static Bundle IncludeCssRewriteUrl(this Bundle bundle, params string[] virtualPaths)
        {
            CssRewriteUrlTransform transform = new CssRewriteUrlTransform();
            foreach (var path in virtualPaths)
            {
                bundle.Include(path, transform);
            }
            return bundle;
        }
    }
}
