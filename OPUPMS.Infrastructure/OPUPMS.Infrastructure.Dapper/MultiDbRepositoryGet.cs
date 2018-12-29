using System;
using System.Threading.Tasks;
using Dapper;
using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork.Extensions;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public abstract partial class MultiDbRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {

        public virtual TEntity GetKey(TPk key, ISession session)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                var sql = OrmConfiguration.GetSqlBuilder<TEntity>();
                return session.QuerySingleOrDefault<TEntity>(
                    $@"SELECT {sql.ConstructColumnEnumerationForSelect()} 
                       FROM {sql.GetTableName()} 
                       WHERE {sql.GetColumnName("Id")} = @Id",
                    new { Id = key });
            }

            var entity = CreateEntityAndSetKeyValue(key);
            return session.Get(entity);
        }

        public virtual TEntity GetKey<TSession>(string dbToken, TPk key) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return GetKey(key, session);
            }
        }

        public virtual TEntity GetKey<TSession>(TPk key) where TSession : class, ISession
        {
            return GetKey<TSession>(null, key);
        }

        public virtual TEntity GetKey(TPk key, IUnitOfWork uow)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                var sql = OrmConfiguration.GetSqlBuilder<TEntity>();
                return uow.Connection.QuerySingleOrDefault<TEntity>(
                    $@"SELECT {sql.ConstructColumnEnumerationForSelect()} 
                       FROM {sql.GetTableName()} 
                       WHERE {sql.GetColumnName("Id")} = @Id",
                    new { Id = key }, uow.Transaction);
            }

            var entity = CreateEntityAndSetKeyValue(key);
            return uow.Get(entity);
        }

        public virtual Task<TEntity> GetKeyAsync(TPk key, ISession session)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                var sql = OrmConfiguration.GetSqlBuilder<TEntity>();
                return session.QuerySingleOrDefaultAsync<TEntity>(
                    $@"SELECT {sql.ConstructColumnEnumerationForSelect()} 
                       FROM {sql.GetTableName()} 
                       WHERE {sql.GetColumnName("Id")} = @Id",
                    new { Id = key });
            }

            var entity = CreateEntityAndSetKeyValue(key);
            return GetAsync(entity, session);
        }

        public virtual async Task<TEntity> GetKeyAsync<TSession>(string dbToken, TPk key) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return await GetKeyAsync(key, session);
            }
        }

        public virtual async Task<TEntity> GetKeyAsync<TSession>(TPk key) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>())
            {
                return await GetKeyAsync(key, session);
            }
        }

        public virtual Task<TEntity> GetKeyAsync(TPk key, IUnitOfWork uow)
        {
            if (_container.IsIEntity<TEntity, TPk>())
            {
                var sql = OrmConfiguration.GetSqlBuilder<TEntity>();
                return uow.Connection.QuerySingleOrDefaultAsync<TEntity>(
                    $@"SELECT {sql.ConstructColumnEnumerationForSelect()} 
                       FROM {sql.GetTableName()} 
                       WHERE {sql.GetColumnName("Id")} = @Id",
                    new { Id = key }, uow.Transaction);
            }

            var entity = CreateEntityAndSetKeyValue(key);
            return uow.GetAsync(entity);
        }

        public virtual TEntity Get(TEntity entity, ISession session)
        {
            return session.Get(entity);
        }

        public virtual TEntity Get<TSession>(string dbToken, TEntity entity) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return Get(entity, session);
            }
        }

        public virtual TEntity Get<TSession>(TEntity entity) where TSession : class, ISession
        {
            return Get<TSession>(null, entity);
        }

        public virtual TEntity Get(TEntity entity, IUnitOfWork uow)
        {
            return uow.Get(entity);
        }

        public virtual Task<TEntity> GetAsync(TEntity entity, ISession session)
        {
            return session.GetAsync(entity);
        }

        public virtual Task<TEntity> GetAsync(TEntity entity, IUnitOfWork uow)
        {
            return uow.GetAsync(entity);
        }

        public virtual async Task<TEntity> GetAsync<TSession>(string dbToken, TEntity entity) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return await GetAsync(entity, session);
            }
        }

        public virtual async Task<TEntity> GetAsync<TSession>(TEntity entity) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>())
            {
                return await GetAsync(entity, session);
            }
        }
    }
}
