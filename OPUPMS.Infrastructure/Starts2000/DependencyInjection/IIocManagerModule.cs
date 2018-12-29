namespace Starts2000.DependencyInjection
{
    public interface IIocManagerModule : IHaveIocManager
    {
        string Name { get; }
        void OnLoad(IIocManager iocManager);
    }
}
