using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Http;

namespace OPUPMS.Web.Framework.Core.Plugin
{
    public class WebApiPluginManager : IDisposable
    {
        readonly static string _webApiPluginsPath = HostingEnvironment.MapPath("~/Plugins/WebApi");
        readonly IList<Assembly> _webApiPluginAssemblys = new List<Assembly>(64);
        bool _isDisposed;

        public void Load()
        {
            try
            {
                if(!Directory.Exists(_webApiPluginsPath))
                {
                    Debug.WriteLine("Load Plugins: Web Api Plugins directory dose not exist.");
                    return;
                }

                var pluginFolder = new DirectoryInfo(_webApiPluginsPath);
                foreach (var plugin in pluginFolder.GetFiles("*.dll", SearchOption.AllDirectories))
                {
                    Assembly asm = Assembly.LoadFrom(plugin.FullName);
                    _webApiPluginAssemblys.Add(asm);
                    BuildManager.AddReferencedAssembly(asm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Concat(
                    "Framework Load WebApi Plugin Error!!!!", Environment.NewLine, ex.ToString()));
            }
        }

        public IEnumerable<Assembly> WebApiPluginAssemblys
        {
            get { return _webApiPluginAssemblys; }
        }

        public void RegisterWebApiPlugin(HttpConfiguration config)
        {
            Type iPluginType = typeof(IWebApiPlugin);

            foreach (Assembly asm in _webApiPluginAssemblys)
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (iPluginType.IsAssignableFrom(type))
                    {
                        IWebApiPlugin plugin = Activator.CreateInstance(type) as IWebApiPlugin;
                        plugin.Register(config);
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
                    _webApiPluginAssemblys.Clear();
                }

                _isDisposed = true;
            }
        }
    }
}