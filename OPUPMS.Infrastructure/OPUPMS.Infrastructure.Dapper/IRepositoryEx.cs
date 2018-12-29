using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smooth.IoC.Repository.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public interface IRepositoryEx<TEntity, TPk> : IRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {
        TEntity Get(TEntity entity);
        Task<TEntity> GetAsync(TEntity entity);

        TEntity GetKey(TPk key);
        Task<TEntity> GetKeyAsync(TPk key);

        int Count();
        Task<int> CountAsync();

        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        bool Delete(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);

        bool DeleteKey(TPk key);
        Task<bool> DeleteKeyAsync(TPk key);

        TPk SaveOrUpdate(TEntity entity);
        Task<TPk> SaveOrUpdateAsync(TEntity entity);
    }
}