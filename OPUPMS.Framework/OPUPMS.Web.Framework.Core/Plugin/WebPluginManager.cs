using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Optimization;
using System.Web.Routing;
using OPUPMS.Web.Framework.Core.Plugin.Configuration;

namespace OPUPMS.Web.Framework.Core.Plugin
{
    public class WebPluginManager : IDisposable
    {
        readonly static string _webPluginsVirtualPath = "~/Plugins/Web";
        readonly static string _webPluginsPath = HostingEnvironment.MapPath(_webPluginsVirtualPath);
        readonly IList<Assembly> _webPluginAssemblys = new List<Assembly>(64);
        bool _isDisposed;

        public static string WebPluginsVirtualPath
        {
            get { return _webPluginsVirtualPath; }
        }

        public WebPluginManager()
        {
        }

        public void Load()
        {
            try
            {
                if (!Directory.Exists(_webPluginsPath))
                {
                    Debug.WriteLine("Load Plugins: Web Plugins directory dose not exist.");
                    return;
                }

                var pluginFolder = new DirectoryInfo(_webPluginsPath);
                foreach (var plugin in pluginFolder.GetFiles("*.dll", SearchOption.AllDirectories))
                {
                    Assembly asm = Assembly.LoadFrom(plugin.FullName);
                    _webPluginAssemblys.Add(asm);
                    BuildManager.AddReferencedAssembly(asm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Concat(
                    "Framework Load Web Plugin Error!!!!", Environment.NewLine, ex.ToString()));
            }
        }

        public IEnumerable<Assembly> WebPluginAssemblys
        {
            get { return _webPluginAssemblys; }
        }

        public void RegisterWebPluginRoutesAndBundles(RouteCollection routes, BundleCollection bundles)
        {
            Type iPluginType = typeof(IWebPlugin);
            PluginConfigManager bundleConfig = new PluginConfigManager();

            foreach (Assembly asm in _webPluginAssemblys)
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (iPluginType.IsAssignableFrom(type))
                    {
                        IWebPlugin plugin = Activator.CreateInstance(type) as IWebPlugin;
                        bundleConfig.LoadResource(bundles, Path.Combine(
                            _webPluginsPath, plugin.Name, @"Configs\Bundle.config"));
                        plugin.RegisterRoutes(routes);
                        plugin.RegisterBundles(bundles);
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _webPluginAssemblys.Clear();
                }

                _isDisposed = true;
            }
        }
    }
}
