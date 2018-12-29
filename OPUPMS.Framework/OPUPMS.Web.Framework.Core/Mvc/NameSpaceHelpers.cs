using System;
using System.Web.Routing;
using System.Web.Mvc;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    public static class NamespaceHelpers
    {
        static readonly string NameSpacesTokens = "namespaces";

        public static string GetNameSpace(RouteBase route)
        {
            Route castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                string[] nameSpaces = castRoute.DataTokens[NameSpacesTokens] as string[];
                if (nameSpaces != null && nameSpaces.Length > 0)
                {
                    return nameSpaces[0];
                }
            }

            return null;
        }

        public static string GetNameSpace(RouteData routeData)
        {
            object nameSpace;
            if (routeData.DataTokens.TryGetValue(NameSpacesTokens, out nameSpace))
            {
                string[] nameSpaces = nameSpace as string[];
                if (nameSpaces != null && nameSpaces.Length > 0)
                {
                    return nameSpaces[0];
                }
            }

            return GetNameSpace(routeData.Route);
        }

        public static string GetNameSpaceWithNotControllers(RouteBase route)
        {
            Route castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                string[] nameSpaces = castRoute.DataTokens[NameSpacesTokens] as string[];
                if(nameSpaces != null && nameSpaces.Length > 0)
                {
                    string nameSpace = nameSpaces[0];
                    return nameSpace.Substring(0, nameSpace.LastIndexOf('.'));
                }
            }

            return null;
        }

        public static string GetNameSpaceWithNotControllers(RouteData routeData)
        {
            object nameSpace;
            if (routeData.DataTokens.TryGetValue(NameSpacesTokens, out nameSpace))
            {
                string[] nameSpaces = nameSpace as string[];
                if (nameSpaces != null && nameSpaces.Length > 0)
                {
                    string firstNamespace = nameSpaces[0];
                    return firstNamespace.Substring(0, firstNamespace.LastIndexOf('.'));
                }
            }

            return GetNameSpaceWithNotControllers(routeData.Route);
        }

        public static string GetNameSpaceWithControllers(ControllerContext context)
        {
            var nameSpace = GetNameSpaceWithNotControllers(context.RouteData);
            if(string.IsNullOrEmpty(nameSpace))
            {
                nameSpace = context.Controller.GetType().Namespace;
            }

            return nameSpace;
        }
    }
}
