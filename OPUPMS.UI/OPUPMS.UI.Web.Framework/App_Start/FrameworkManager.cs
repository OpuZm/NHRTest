[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(OPUPMS.UI.Web.Framework.FrameworkManager), "Config")]
[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(OPUPMS.UI.Web.Framework.FrameworkManager), "Start")]

namespace OPUPMS.UI.Web.Framework
{
    using DryIoc;
    using OPUPMS.Domain.AuthorizeService;
    using OPUPMS.Web.Framework.Core;
    using OPUPMS.Web.Framework.Core.Mvc;

    public class FrameworkManager
    {
        public static void Config()
        {
            FrameworkBuilder.Instance.Config(container =>
            {
                container.Register<IAuthorizeService, PermissionAuthorizeService>(
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
                return container;
            });
        }

        public static void Start()
        {
            FrameworkBuilder.Instance.Start();
        }
    }
}