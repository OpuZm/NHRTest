using System;
using System.Web;
using System.Web.Optimization;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public static class ScriptsEx
    {
        /// <summary>
        /// 输出使用的压缩资源的 HTML 标记。
        /// </summary>
        /// <param name="pluginName">插件名称。</param>
        /// <param name="bundleNames">压缩资源的标识。</param>
        /// <returns>使用的压缩资源的Html，<see cref="System.Web.IHtmlString"/>。</returns>
        public static IHtmlString Render(string pluginName, params string[] bundleNames)
        {
            if (string.IsNullOrEmpty(pluginName))
            {
                throw new ArgumentException("name 不能为空或 null");
            }

            if (bundleNames == null)
            {
                throw new ArgumentNullException("bundleFlags");
            }

            if (bundleNames.Length == 0)
            {
                throw new ArgumentOutOfRangeException("bundleFlags");
            }

            string[] fullBundleNames = new string[bundleNames.Length];

            for (int i = 0; i < bundleNames.Length; i++)
            {
                fullBundleNames[i] = GetVirtualPath(pluginName, bundleNames[i]);
            }

            return Scripts.Render(fullBundleNames);
        }

        /// <summary>
        /// 根据插件名称和压缩资源的标识获取资源的Url。
        /// </summary>
        /// <param name="pluginName">插件名称。</param>
        /// <param name="bundleName">压缩资源的标识。</param>
        /// <returns>资源的Url。</returns>
        public static string GetVirtualPath(string pluginName, string bundleName)
        {
            return BundleCollectionExtensions.BundleInfos[pluginName, bundleName];
        }
    }
}
