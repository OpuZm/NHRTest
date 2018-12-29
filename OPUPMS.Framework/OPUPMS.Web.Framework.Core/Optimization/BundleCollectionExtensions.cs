using System.Collections.Generic;
using System.Web.Optimization;

namespace OPUPMS.Web.Framework.Core.Optimization
{
    public static class BundleCollectionExtensions
    {
        static readonly BundleInfo _bundleInfos = new BundleInfo();

        /// <summary>
        /// 添加插件中的压缩的资源信息。
        /// </summary>
        /// <param name="bundles">此方法扩展的 <see cref="System.Web.Optimization.BundleCollection"/> 实例。</param>
        /// <param name="pluginName">插件名称。</param>
        /// <param name="bundleName">压缩的资源标识。</param>
        /// <param name="bundle"><see cref="System.Web.Optimization.Bundle"/></param>
        public static void Add(this BundleCollection bundles,
            string pluginName, string bundleName, Bundle bundle)
        {
            _bundleInfos.Add(pluginName, bundleName, bundle.Path);
            bundles.Add(bundle);
        }

        /// <summary>
        /// 添加插件中的压缩的资源信息。
        /// </summary>
        /// <param name="bundles">此方法扩展的 <see cref="System.Web.Optimization.BundleCollection"/> 实例。</param>
        /// <param name="pluginName">插件名称。</param>
        /// <param name="bundleKeyValues">压缩的资源标识信息。</param>
        public static void Add(this BundleCollection bundles,
            string pluginName, params KeyValuePair<string, Bundle>[] bundleKeyValues)
        {
            foreach (var bundle in bundleKeyValues)
            {
                _bundleInfos.Add(pluginName, bundle.Key, bundle.Value.Path);
                bundles.Add(bundle.Value);
            }
        }

        internal static BundleInfo BundleInfos
        {
            get { return _bundleInfos; }
        }

        internal class BundleInfo
        {
            readonly IDictionary<string, IDictionary<string, string>> _infos =
                new Dictionary<string, IDictionary<string, string>>(97);

            internal void Add(string pluginName, string bundleName, string bundleVirtualPath)
            {
                IDictionary<string, string> bundles;

                if (_infos.TryGetValue(pluginName, out bundles))
                {
                    bundles.Add(bundleName, bundleVirtualPath);
                }
                else
                {
                    bundles = new Dictionary<string, string>(13);
                    bundles.Add(bundleName, bundleVirtualPath);
                    _infos.Add(pluginName, bundles);
                }
            }

            internal string this[string pluginName, string bundleName]
            {
                get
                {
                    return _infos[pluginName][bundleName];
                }
            }
        }
    }
}
