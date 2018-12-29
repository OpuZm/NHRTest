using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DryIoc;
using DryIoc.Mvc;
using DryIoc.SignalR;
using DryIoc.Web;
using DryIoc.WebApi;
using log4net;
using OPUPMS.Web.Framework.Core.Log;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Web.Framework.Core.Plugin;
using OPUPMS.Web.Framework.Core.Plugin.Configuration;
using OPUPMS.Web.Framework.Core.WebApi;
using Starts2000.DependencyInjection;
using Starts2000.DependencyInjection.DryIoc;

[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(OPUPMS.Web.Framework.Core.Startup), "Init", Order = int.MinValue)]
[assembly: WebActivatorEx.ApplicationShutdownMethod(
    typeof(OPUPMS.Web.Framework.Core.Startup), "Stop")]

namespace OPUPMS.Web.Framework.Core
{
    internal static class Startup
    {
        static IContainer _container;
        static readonly WebPluginManager _webPluginManager = new WebPluginManager();
        static readonly WebApiPluginManager _webApiPluginManager = new WebApiPluginManager();
        static readonly PluginConfigManager _pluginConfigManager = new PluginConfigManager();

        public static void Init()
        {
            DryIocHttpModuleInitializer.Initialize();
            _webPluginManager.Load();
            _webApiPluginManager.Load();
            CreateContainer();
        }

        public static void Config(Func<IContainer, IContainer> configure)
        {
            RegistServices(_container);
            _container = configure?.Invoke(_container) ?? _container;
            _container = _container.WithSignalR(
                _webPluginManager.WebPluginAssemblys.ToArray());
            _container.RegisterHubs(AppDomain.CurrentDomain.GetAssemblies());
            _container = _container.WithMvc(
                _webPluginManager.WebPluginAssemblys,
                throwIfUnresolved: type => type.IsController());
        }

        public static void Start()
        {
            _pluginConfigManager.LoadResource(BundleTable.Bundles,
                HostingEnvironment.MapPath("~/Configs/Bundle.config"), true);
            _webPluginManager.RegisterWebPluginRoutesAndBundles(
                RouteTable.Routes, BundleTable.Bundles);
            GlobalConfiguration.Configure(
                _webApiPluginManager.RegisterWebApiPlugin);
            _container = _container.WithWebApi(GlobalConfiguration.Configuration,
                _webApiPluginManager.WebApiPluginAssemblys,
                throwIfUnresolved: type => type.IsApiController());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new PluginsRazorViewEngine());

            log4net.Config.XmlConfigurator.ConfigureAndWatch(
                new FileInfo(HostingEnvironment.MapPath("~/Configs/Log4Net.xml")));
        }

        public static void Stop()
        {
            try
            {
                _webPluginManager.Dispose();
                _webApiPluginManager.Dispose();
                _container.Dispose();
            }
            catch
            {

            }
        }

        static void CreateContainer()
        {
            _container = new Container()
                .With(rules => rules
                    .With(FactoryMethod.ConstructorWithResolvableArguments)
                    .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                    .WithTrackingDisposableTransients()
                    .With(propertiesAndFields: PropertiesAndFields.Auto));
        }

        static void RegistServices(IContainer container)
        {
            container.RegisterMany<DryIocMannager>(Reuse.Singleton);
            container.Register<ILog, Logger>(made:
               Parameters.Of.Type(request => request.Parent.ImplementationType));
            container.RegisterInstance(_webApiPluginManager, Reuse.Singleton);
            container.RegisterInstance(_webPluginManager, Reuse.Singleton);
            container.RegisterInstance(_pluginConfigManager, Reuse.Singleton);
            container.Register<IAuthorizeService, NullAuthorizeService>();
            container.Register<IWebApiTokenService, DefaultWebApiTokenService>();
            LoadIocManagerModule(container);
            container.AddAutoMapper(true, AppDomain.CurrentDomain.GetAssemblies());
        }

        static void LoadIocManagerModule(IContainer container)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(_ => _.DefinedTypes)
                .Where(_ => _.ImplementsServiceType(typeof(IIocManagerModule)));
            var iocManager = container.Resolve<IIocManager>();
            foreach(var type in types)
            {
                var module = Activator.CreateInstance(type) as IIocManagerModule;
                module.OnLoad(iocManager);
            }
        }
    }

    public class FrameworkBuilder : IFrameworkBuilder
    {
        static readonly IFrameworkBuilder _instace = new FrameworkBuilder();

        public static IFrameworkBuilder Instance
        {
            get { return _instace; }
        }

        public IFrameworkBuilder Config(Func<IContainer, IContainer> configure)
        {
            Startup.Config(configure);
            return this;
        }

        public IFrameworkBuilder Start()
        {
            Startup.Start();
            return this;
        }
    }

    public interface IFrameworkBuilder
    {
        IFrameworkBuilder Config(Func<IContainer, IContainer> configure = null);
        IFrameworkBuilder Start();
    }
}