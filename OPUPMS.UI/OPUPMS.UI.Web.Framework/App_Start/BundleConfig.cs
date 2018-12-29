using System.Web.Optimization;
using OPUPMS.Web.Framework.Core;
using OPUPMS.Web.Framework.Core.Plugin;

namespace OPUPMS.UI.Web.Framework
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //注意，压缩后的路径如果与现有路径相同，必须加上后缀名，否者会出现404错误。
            //所以为了避免出错，所有的压缩路径都应该加上后缀名（.js或者.css）
            //bundles.Add(FrameworkInfo.Name, (int)FrameworkBundleFlag.CommonScripts,
            //    new ScriptBundle("~/scripts/common.js").Include(
            //    "~/Scripts/jquery/jquery-{version}.js",
            //    "~/Scripts/jquery/jquery*",
            //    "~/MiniUi/miniuicr.js",
            //    "~/MiniUi/miniui.js")
            //);

            //bundles.Add(FrameworkInfo.Name, (int)FrameworkBundleFlag.CommonStyles,
            //    new YuiStyleBundle("~/Content/commonall.css").IncludeCssRewriteUrl(
            //    "~/Content/reset.css",
            //    "~/Content/common.css")
            //);

            //bundles.Add(FrameworkInfo.Name, (int)FrameworkBundleFlag.MiniUIDefaultStyles,
            //    new YuiStyleBundle("~/Content/MiniUiDefaultStyle.css").IncludeCssRewriteUrl(
            //    "~/MiniUi/themes/default/*.css")
            //);


            //bundles.Add(FrameworkInfo.Name, (int)FrameworkBundleFlag.MiniUIBlueStyles,
            //    new YuiStyleBundle("~/Content/MiniUiBuleStyle.css").IncludeCssRewriteUrl(
            //    "~/MiniUi/themes/blue/*.css")
            //);

            //bundles.Add(FrameworkInfo.Name, (int)FrameworkBundleFlag.MiniUIIconStyles,
            //    new YuiStyleBundle("~/MiniUi/themes/iconsstyle.css").IncludeCssRewriteUrl(
            //    "~/MiniUi/themes/icons.css")
            //);
            
            //BundleTable.EnableOptimizations = false;
        }
    }
}