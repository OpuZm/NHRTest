namespace OPUPMS.Web.Framework.Core.Mvc
{
    internal interface IResolver<T>
    {
        T Current { get; }
    }
}
