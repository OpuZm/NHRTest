using System;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork.Extensions;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public abstract partial class MultiDbRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {
        public void Add(TEntity entity, ISession session)
        {
            session.Insert(entity);
        }

        public void Add(TEntity entity, IUnitOfWork uow)
        {
            uow.Connection.Insert(entity, options => options.AttachToTransaction(uow.Transaction));
        }

        public void Add<TSession>(string dbToken, TEntity entity) where TSession : class, ISession
        {
            using (var session = Factory.Create<ISession>(dbToken))
            {
                Add(entity, session);
            }
        }

        public void Add<TSession>(TEntity entity) where TSession : class, ISession
        {
            Add<TSession>(null, entity);
        }

        public Task AddAsync(TEntity entity, ISession session)
        {
            return session.InsertAsync(entity);
        }

        public Task AddAsync(TEntity entity, IUnitOfWork uow)
        {
            return uow.Connection.InsertAsync(entity, options => options.AttachToTransaction(uow.Transaction));
        }

        public async Task AddAsync<TSession>(string dbToken, TEntity entity) where TSession : class, ISession
        {
            using (var session = Factory.Create<ISession>(dbToken))
            {
                await AddAsync(entity, session);
            }
        }

        public async Task AddAsync<TSession>(TEntity entity) where TSession : class, ISession
        {
            using (var session = Factory.Create<ISession>())
            {
                await AddAsync(entity, session);
            }
        }
    }
}
