using System;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.Repository.UnitOfWork.Extensions;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public abstract partial class MultiDbRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {
        public virtual int Count(ISession session)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                return
                    session.QuerySingleOrDefault<int>(
                        $"SELECT count(*) FROM {Sql.Table<TEntity>(session.SqlDialect)}");
            }
            return session.Count<TEntity>();
        }

        public virtual int Count(IUnitOfWork uow)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                return
                    uow.Connection.QuerySingleOrDefault<int>(
                        $"SELECT count(*) FROM {Sql.Table<TEntity>(uow.SqlDialect)}", uow.Transaction);
            }
            return uow.Count<TEntity>();
        }

        public virtual int Count<TSession>(string dbToken) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return Count(session);
            }
        }

        public virtual int Count<TSession>() where TSession : class, ISession
        {
            return Count<TSession>(null);
        }

        public virtual Task<int> CountAsync(ISession session)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                return session.QuerySingleOrDefaultAsync<int>(
                            $"SELECT count(*) FROM {Sql.Table<TEntity>(session.SqlDialect)}");
            }
            return session.CountAsync<TEntity>();
        }

        public virtual Task<int> CountAsync(IUnitOfWork uow)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                return uow.Connection.QuerySingleOrDefaultAsync<int>(
                            $"SELECT count(*) FROM {Sql.Table<TEntity>(uow.SqlDialect)}");
            }
            return uow.CountAsync<TEntity>();
        }

        public virtual async Task<int> CountAsync<TSession>(string dbToken) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return await CountAsync(session);
            }
        }

        public virtual async Task<int> CountAsync<TSession>() where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>())
            {
                return await CountAsync(session);
            }
        }
    }
}
