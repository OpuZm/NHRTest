using System;
using System.Collections.Generic;
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
        public virtual IEnumerable<TEntity> GetAll(ISession session)
        {
            return session.Find<TEntity>();
        }
        public virtual IEnumerable<TEntity> GetAll(IUnitOfWork uow)
        {
            return uow.Find<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll<TSession>(string dbToken) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return GetAll(session);
            }
        }

        public virtual IEnumerable<TEntity> GetAll<TSession>() where TSession : class, ISession
        {
            return GetAll<TSession>(null);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(ISession session)
        {
            return session.FindAsync<TEntity>();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(IUnitOfWork uow)
        {
            return uow.FindAsync<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TSession>(string dbToken) where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>(dbToken))
            {
                return await GetAllAsync(session);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TSession>() where TSession : class, ISession
        {
            using (var session = Factory.Create<TSession>())
            {
                return await GetAllAsync(session);
            }
        }
    }
}
