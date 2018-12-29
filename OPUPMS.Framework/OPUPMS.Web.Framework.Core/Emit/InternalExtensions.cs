using System;
using System.Linq;
using System.Reflection;

namespace OPUPMS.Web.Framework.Core.Emit
{
    internal static class InternalExtensions
    {
        internal static Type[] GetParameterTypes(this MethodBase method)
        {
            return method.GetParameters().Select(x => x.ParameterType).ToArray();
        }
    }
}
