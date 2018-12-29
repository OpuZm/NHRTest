using System.Linq;
using DryIoc;
using Starts2000.DependencyInjection;
using Starts2000.DependencyInjection.DryIoc;

namespace OPUPMS.Domain.Services.IocManagerMoudles
{
    public class DomainServiceIocManagerModule : IocManagerModule
    {
        public override void Load()
        {
            IocManager.RegisterAssemblyTransient(
                typeof(DomainServiceIocManagerModule).Assembly,
                type => (type.IsClass && type.IsPublic && !type.IsAbstract) &&
                        type.FullName.EndsWith("DomainService") &&
                        type.GetInterfaces().Length > 0);
        }
    }
}
