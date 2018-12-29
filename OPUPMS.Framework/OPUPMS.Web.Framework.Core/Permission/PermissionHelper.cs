using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Web.Framework.Core.Plugin;

namespace OPUPMS.Web.Framework.Core.Permission
{
    public static class PermissionHelper
    {
        static readonly string CacheKey = "Cache_Controller_Action_Permissions";
        static readonly Type AuthorizationControllerType = typeof(AuthorizationController);
        static readonly Type ChildActionOnlyAttributeType = typeof(ChildActionOnlyAttribute);
        static readonly Type AllowAnonymousAttributeType = typeof(AllowAnonymousAttribute);
        static readonly Type DescriptionAttributeType = typeof(DescriptionAttribute);

        /// <summary>
        /// 移除所有反射的Action权限缓存。
        /// </summary>
        public static void ClearCache()
        {
            HttpContext.Current.Cache.Remove(CacheKey);
        }

        /// <summary>
        /// 获取加载的插件中所有需要进行权限管理的Action。
        /// 如果缓存中存在，则直接获取缓存中的；否则直接从加载的插件中反射，并缓存。
        /// </summary>
        /// <returns></returns>
        public static IList<ActionPermissionInfo> GetAllActionPermissionFromPlugins()
        {
            var permissions = HttpContext.Current.Cache[CacheKey] as IList<ActionPermissionInfo>;

            if (permissions != null)
            {
                return permissions;
            }

            WebPluginManager pluginManager = DependencyResolver.Current.GetService<WebPluginManager>() as WebPluginManager;
            IEnumerable<Assembly> pluginAssemblys = pluginManager.WebPluginAssemblys;

            permissions = new List<ActionPermissionInfo>(128);

            foreach (Assembly pluginAssembly in pluginAssemblys)
            {
                foreach (Type type in pluginAssembly.GetTypes())
                {
                    if (AuthorizationControllerType.IsAssignableFrom(type))
                    {
                        string controller = type.FullName.Substring(0, type.FullName.Length - 10);

                        foreach (MethodInfo methodInfo in type.GetMethods(
                            BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                        {
                            if (methodInfo.GetCustomAttributes(ChildActionOnlyAttributeType, false).Length > 0 ||
                                methodInfo.GetCustomAttributes(AllowAnonymousAttributeType, false).Length > 0)
                            {
                                continue;
                            }

                            object[] attrs = methodInfo.GetCustomAttributes(DescriptionAttributeType, false);
                            string description = null;
                            if (attrs.Length > 0)
                            {
                                description = (attrs[0] as DescriptionAttribute).Description;
                            }

                            permissions.Add(new ActionPermissionInfo
                            {
                                Controller = controller,
                                Action = methodInfo.Name,
                                Description = description == null ? string.Empty : description
                            });
                        }
                    }
                }
            }

            HttpContext.Current.Cache.Insert(CacheKey, permissions,
                null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10));

            return permissions;
        }
    }
}