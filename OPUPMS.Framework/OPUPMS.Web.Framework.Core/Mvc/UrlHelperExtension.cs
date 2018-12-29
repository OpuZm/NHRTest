using System.Web.Mvc;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    public static class UrlHelperExt
    {
        /// <summary>
        /// 将 WEB 插件的虚拟（相对）路径转换为应用程序绝对路径。
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="pluginName">WEB 插件的名称。</param>
        /// <param name="contentPath">内容的虚拟路径。</param>
        /// <returns>应用程序绝对路径。</returns>
        public static string Content(
            this UrlHelper urlHelper, string pluginName, string contentPath)
        {
            if (contentPath.StartsWith("~"))
            {
                contentPath = contentPath.Remove(0);
            }

            return urlHelper.Content(string.Format(
                "~/plugins/web/{0}/{1}", pluginName, contentPath));
        }
    }
}
