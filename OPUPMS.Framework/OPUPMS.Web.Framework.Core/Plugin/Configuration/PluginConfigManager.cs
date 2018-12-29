using System.IO;
using System.Linq;
using System.Web.Optimization;
using System.Xml.Linq;
using OPUPMS.Web.Framework.Core.Optimization;

namespace OPUPMS.Web.Framework.Core.Plugin.Configuration
{
    /// <summary>
    /// 插件资源、路由配置管理。
    /// </summary>
    public class PluginConfigManager
    {
        /// <summary>
        /// 加载资源配置文件。
        /// </summary>
        /// <param name="bundles"></param>
        /// <param name="configFile"></param>
        public void LoadResource(BundleCollection bundles, string configFile, bool isFramework = false)
        {
            var document = Load(configFile);
            if (document == null)
            {
                return;
            }

            var bundleElement = document.Element("bundle");
            if (bundleElement != null)
            {
                var plugins = bundleElement.Elements("plugin");
                foreach (var plugin in plugins)
                {
                    var pluginName = plugin.Attribute("Name").Value;
                    var scriptQuery = from script in plugin.Elements("script")
                                      select new
                                      {
                                          Name = script.Attribute("Name").Value,
                                          VirtualPath = script.Attribute("VirtualPath").Value,
                                          Includes = from include in script.Elements("include")
                                                     select new
                                                     {
                                                         FileBasePath = (string)include.Attribute("FileBasePath"),
                                                         Files = from file in include.Elements("file")
                                                                 select file.Attribute("Path").Value
                                                     }
                                      };

                    foreach (var item in scriptQuery)
                    {
                        var bundle = new ScriptBundle(item.VirtualPath);
                        foreach (var include in item.Includes)
                        {
                            var files = include.Files.Select(
                                file => UriCombine(
                                    isFramework ? "~" : WebPluginManager.WebPluginsVirtualPath,
                                    isFramework ? string.Empty : pluginName,
                                    include.FileBasePath, file)).ToArray();
                            bundle.Include(files);
                        }
                        bundles.Add(pluginName, item.Name, bundle);
                    }

                    var styleQuery = from style in plugin.Elements("style")
                                     select new
                                     {
                                         Name = style.Attribute("Name").Value,
                                         VirtualPath = style.Attribute("VirtualPath").Value,
                                         Includes = from include in style.Elements("include")
                                                    select new
                                                    {
                                                        FileBasePath = (string)include.Attribute("FileBasePath"),
                                                        RewriteUrl = (bool?)include.Attribute("RewriteUrl") ?? true,
                                                        Files = from file in include.Elements("file")
                                                                select file.Attribute("Path").Value
                                                    }
                                     };
                    foreach (var item in styleQuery)
                    {
                        var bundle = new YuiStyleBundle(item.VirtualPath);
                        foreach (var include in item.Includes)
                        {
                            var files = include.Files.Select(
                                file => UriCombine(
                                    isFramework ? "~" : WebPluginManager.WebPluginsVirtualPath,
                                    isFramework ? string.Empty : pluginName,
                                    include.FileBasePath, file)).ToArray();
                            if (include.RewriteUrl)
                            {
                                bundle.IncludeCssRewriteUrl(files);
                            }
                            else
                            {
                                bundle.Include(files);
                            }
                        }

                        bundles.Add(pluginName, item.Name, bundle);
                    }
                }
            }
        }

        XDocument Load(string configFile)
        {
            if (File.Exists(configFile))
            {
                return XDocument.Load(configFile);
            }

            return null;
        }

        string UriCombine(string url1, string url2, string url3, string url4)
        {
            if (url1.Equals("~"))
            {
                return string.Format("{0}/{1}/{2}", url1, url3, url4).Replace("//", "/");
            }

            return string.Format("{0}/{1}/{2}/{3}", url1, url2, url3, url4).Replace("//", "/");
        }
    }
}