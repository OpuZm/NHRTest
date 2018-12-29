using System;
using System.Reflection;
using System.Reflection.Emit;

namespace OPUPMS.Web.Framework.Core.Emit
{
    internal class ConstructorEmit
    {
        readonly ConstructorInfo _constructorInfo;
        readonly Func<object> _invoker;

        public ConstructorEmit(ConstructorInfo constructorInfo)
        {
            _constructorInfo = constructorInfo;
        }

        protected virtual Func<object> CreateInvoker()
        {
            var dynamicMethod = new DynamicMethod($"invoker-{Guid.NewGuid()}", 
                typeof(object), null, _constructorInfo.Module, true);
            var ilGen = dynamicMethod.GetILGenerator();

            var parameterobjectypes = _constructorInfo.GetParameterTypes();
            if (parameterobjectypes.Length == 0)
            {
                ilGen.Emit(OpCodes.Newobj, _constructorInfo);
                return CreateDelegate();
            }

            return null;

            Func<object> CreateDelegate()
            {
                ilGen.Emit(OpCodes.Ret);
                return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
            }
        }

        public virtual object Invoke()
        {
            return _invoker();
        }
    }
}
