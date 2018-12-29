using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace OPUPMS.Tests
{
    class TestSetup
    {
        public static IKernel Setup()
        {
            var setting = new NinjectSettings
            {
                ExtensionSearchPatterns = new[]
                {
                    //"Ninject.Extensions.*.dll",
                    //"Ninject.Web*.dll",
                    //@"..\Configs\NinjectModule.xml"
                    "*.dll"
                },
                LoadExtensions = true
            };
            var kernel = new StandardKernel(setting);
            try
            {
                //kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                //kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                //kernel.Bind<WebPluginManager>().ToConstant(_webPluginManager);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            //kernel.Load(new MySql.RepositoryNinjectModule());
            //kernel.Load(@"..\Configs\NinjectModule.xml");
        }
    }
}
