using System;
using System.Web.Mvc;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    public class PluginsRazorViewEngine : PluginBuildManagerViewEngine
    {
        public PluginsRazorViewEngine()
            : this(null)
        {
        }

        public PluginsRazorViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            PluginNameSpaceViewLocationFormats = new[]
            {
                "~/Plugins/Web/{2}/Views/{1}/{0}.cshtml",
                "~/Plugins/Web/{2}/Views/{1}/{0}.vbhtml",
                "~/Plugins/Web/{2}/Views/Shared/{0}.cshtml",
                "~/Plugins/Web/{2}/Views/Shared/{0}.vbhtml"
            };
            PluginNameSpaceMasterLocationFormats = new[]
            {
                "~/Plugins/Web/{2}/Views/{1}/{0}.cshtml",
                "~/Plugins/Web/{2}/Views/{1}/{0}.vbhtml",
                "~/Plugins/Web/{2}/Views/Shared/{0}.cshtml",
                "~/Plugins/Web/{2}/Views/Shared/{0}.vbhtml"
            };
            PluginNameSpacePartialViewLocationFormats = new[]
            {
                "~/Plugins/Web/{2}/Views/{1}/{0}.cshtml",
                "~/Plugins/Web/{2}/Views/{1}/{0}.vbhtml",
                "~/Plugins/Web/{2}/Views/Shared/{0}.cshtml",
                "~/Plugins/Web/{2}/Views/Shared/{0}.vbhtml"
            };

            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            MasterLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            PartialViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };

            FileExtensions = new[]
            {
                "cshtml",
                "vbhtml",
            };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new RazorView(controllerContext, partialPath,
                layoutPath: null, runViewStartPages: false, 
                viewStartFileExtensions: FileExtensions, viewPageActivator: ViewPageActivator)
            {
                //DisplayModeProvider = DisplayModeProvider
            };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var view = new RazorView(controllerContext, viewPath,
                layoutPath: masterPath, runViewStartPages: true, 
                viewStartFileExtensions: FileExtensions, viewPageActivator: ViewPageActivator)
            {
                //DisplayModeProvider = DisplayModeProvider
            };
            return view;
        }
    }
}
