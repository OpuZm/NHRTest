using System;

namespace Starts2000.DependencyInjection
{
    public interface IIocManager : IIocRegistrator, IIocResolver, IDisposable
    {
    }
}
