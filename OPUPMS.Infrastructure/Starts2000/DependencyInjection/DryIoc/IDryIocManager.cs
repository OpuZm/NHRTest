using DryIoc;

namespace Starts2000.DependencyInjection.DryIoc
{
    public interface IDryIocManager : IIocManager
    {
        IContainer IocContainer { get; }
    }
}