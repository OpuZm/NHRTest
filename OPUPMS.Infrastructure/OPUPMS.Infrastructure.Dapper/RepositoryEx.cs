using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smooth.IoC.Repository.UnitOfWork;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public abstract class RepositoryEx<TEntity, TPk> : 
        Repository<TEntity, TPk>, IRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {
        protected RepositoryEx(IDbFactory factory) : base(factory)
        {
        }

        public TEntity Get(TEntity entity)
        {
            return Get<ISession>(entity);
        }

        public Task<TEntity> GetAsync(TEntity entity)
        {
            return GetAsync<ISession>(entity);
        }

        public TEntity GetKey(TPk key)
        {
            return GetKey<ISession>(key);
        }

        public Task<TEntity> GetKeyAsync(TPk key)
        {
            return GetKeyAsync<ISession>(key);
        }

        public int Count()
        {
            return Count<ISession>();
        }

        public Task<int> CountAsync()
        {
            return CountAsync<ISession>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return GetAll<ISession>();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return GetAllAsync<ISession>();
        }

        public bool Delete(TEntity entity)
        {
            return Delete<ISession>(entity);
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            return DeleteAsync<ISession>(entity);
        }

        public bool DeleteKey(TPk key)
        {
            return DeleteKey<ISession>(key);
        }

        public Task<bool> DeleteKeyAsync(TPk key)
        {
            return DeleteKeyAsync<ISession>(key);
        }

        public TPk SaveOrUpdate(TEntity entity)
        {
            return SaveOrUpdate<ISession>(entity);
        }

        public Task<TPk> SaveOrUpdateAsync(TEntity entity)
        {
            return SaveOrUpdateAsync<ISession>(entity);
        }
    }
}