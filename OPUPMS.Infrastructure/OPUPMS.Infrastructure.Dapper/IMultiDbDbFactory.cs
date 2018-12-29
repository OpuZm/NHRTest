using System.Data;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public interface IMultiDbDbFactory : IDbFactory
    {
        T Create<T>(string dbToken = null) where T : class, ISession;
        TUnitOfWork Create<TUnitOfWork, TSession>(string dbToken = null,
            IsolationLevel isolationLevel = IsolationLevel.Serializable)
            where TUnitOfWork : class, IUnitOfWork
            where TSession : class, ISession;

        void SetConnectionCache(string token, string connectionString);
    }
}
