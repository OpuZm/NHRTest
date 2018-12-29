using System;
using System.Threading.Tasks;
using Smooth.IoC.Repository.UnitOfWork.Extensions;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public abstract partial class MultiDbRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {
        public virtual TPk SaveOrUpdate(TEntity entity, IUnitOfWork uow)
        {
            if (TryAllKeysDefault(entity))
            {
                uow.Insert(entity, options => options.AttachToTransaction(uow.Transaction));
            }
            else
            {
                uow.Update(entity, options => options.AttachToTransaction(uow.Transaction));
            }

            var primaryKeyValue = GetPrimaryKeyValue(entity);
            return primaryKeyValue != null ? primaryKeyValue : default(TPk);
        }

        public virtual TPk SaveOrUpdate<TSesssion>(string dbToken, TEntity entity) where TSesssion : class, ISession
        {
            using (var uow = Factory.Create<IUnitOfWork, TSesssion>(dbToken))
            {
                return SaveOrUpdate(entity, uow);
            }
        }

        public virtual TPk SaveOrUpdate<TSesssion>(TEntity entity) where TSesssion : class, ISession
        {
            return SaveOrUpdate<TSesssion>(null, entity);
        }

        public async virtual Task<TPk> SaveOrUpdateAsync(TEntity entity, IUnitOfWork uow)
        {
            if (TryAllKeysDefault(entity))
            {
                await uow.InsertAsync(entity, options => options.AttachToTransaction(uow.Transaction));
            }
            else
            {
                await uow.UpdateAsync(entity, options => options.AttachToTransaction(uow.Transaction));
            }
            var primaryKeyValue = GetPrimaryKeyValue(entity);
            return primaryKeyValue != null ? primaryKeyValue : default(TPk);
        }

        public virtual async Task<TPk> SaveOrUpdateAsync<TSesssion>(string dbToken, TEntity entity) where TSesssion : class, ISession
        {
            using (var uow = Factory.Create<IUnitOfWork, TSesssion>(dbToken))
            {
                return await SaveOrUpdateAsync(entity, uow);
            }
        }

        public virtual async Task<TPk> SaveOrUpdateAsync<TSesssion>(TEntity entity) where TSesssion : class, ISession
        {
            using (var uow = Factory.Create<IUnitOfWork, TSesssion>())
            {
                return await SaveOrUpdateAsync(entity, uow);
            }
        }
    }
}
