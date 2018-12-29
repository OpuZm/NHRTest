using System;

namespace Starts2000.DependencyInjection
{
    public abstract class IocManagerModule : IIocManagerModule
    {
        protected IocManagerModule()
        {

        }

        public virtual string Name { get; }

        public IIocManager IocManager { get; private set; }

        public abstract void Load();

        public void OnLoad(IIocManager iocManager)
        {
            IocManager = iocManager;
            try
            {
                Load();
            }
            catch(Exception ex)
            {
                var type = GetType();
                throw new IocManagerModuleLoadException(
                    $"Load IocManagerModule failed, from {type.Assembly.FullName} {type.FullName}", ex);
            }
        }
    }
}
