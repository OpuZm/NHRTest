using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    public interface IMultiDbRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IComparable
    {
        int Count(ISession session);
        int Count(IUnitOfWork uow);
        int Count<TSession>(string dbToken) where TSession : class, ISession;
        int Count<TSession>() where TSession : class, ISession;
        Task<int> CountAsync(ISession session);
        Task<int> CountAsync(IUnitOfWork uow);
        Task<int> CountAsync<TSession>(string dbToken) where TSession : class, ISession;
        Task<int> CountAsync<TSession>() where TSession : class, ISession;

        bool DeleteKey(TPk key, ISession session);
        bool DeleteKey(TPk key, IUnitOfWork uow);
        bool DeleteKey<TSession>(string dbToken, TPk key) where TSession : class, ISession;
        bool DeleteKey<TSession>(TPk key) where TSession : class, ISession;
        Task<bool> DeleteKeyAsync(TPk key, ISession session);
        Task<bool> DeleteKeyAsync(TPk key, IUnitOfWork uow);
        Task<bool> DeleteKeyAsync<TSession>(string dbToken, TPk key) where TSession : class, ISession;
        Task<bool> DeleteKeyAsync<TSession>(TPk key) where TSession : class, ISession;

        bool Delete(TEntity entity, ISession session);
        bool Delete(TEntity entity, IUnitOfWork uow);
        bool Delete<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        bool Delete<TSession>(TEntity entity) where TSession : class, ISession;
        Task<bool> DeleteAsync(TEntity entity, ISession session);
        Task<bool> DeleteAsync(TEntity entity, IUnitOfWork uow);
        Task<bool> DeleteAsync<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        Task<bool> DeleteAsync<TSession>(TEntity entity) where TSession : class, ISession;


        TEntity GetKey(TPk key, ISession session);
        TEntity GetKey(TPk key, IUnitOfWork uow);
        TEntity GetKey<TSession>(string dbToken, TPk key) where TSession : class, ISession;
        TEntity GetKey<TSession>(TPk key) where TSession : class, ISession;
        Task<TEntity> GetKeyAsync(TPk key, ISession session);
        Task<TEntity> GetKeyAsync(TPk key, IUnitOfWork uow);
        Task<TEntity> GetKeyAsync<TSession>(string dbToken, TPk key) where TSession : class, ISession;
        Task<TEntity> GetKeyAsync<TSession>(TPk key) where TSession : class, ISession;

        TEntity Get(TEntity entity, ISession session);
        TEntity Get<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        TEntity Get<TSession>(TEntity entity) where TSession : class, ISession;
        TEntity Get(TEntity entity, IUnitOfWork uow);
        Task<TEntity> GetAsync(TEntity entity, ISession session);
        Task<TEntity> GetAsync<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        Task<TEntity> GetAsync<TSession>(TEntity entity) where TSession : class, ISession;
        Task<TEntity> GetAsync(TEntity entity, IUnitOfWork uow);

        IEnumerable<TEntity> GetAll(ISession session);
        IEnumerable<TEntity> GetAll(IUnitOfWork uow);
        IEnumerable<TEntity> GetAll<TSession>(string dbToken) where TSession : class, ISession;
        IEnumerable<TEntity> GetAll<TSession>() where TSession : class, ISession;
        Task<IEnumerable<TEntity>> GetAllAsync(ISession session);
        Task<IEnumerable<TEntity>> GetAllAsync(IUnitOfWork uow);
        Task<IEnumerable<TEntity>> GetAllAsync<TSession>(string dbToken) where TSession : class, ISession;
        Task<IEnumerable<TEntity>> GetAllAsync<TSession>() where TSession : class, ISession;

        TPk SaveOrUpdate(TEntity entity, IUnitOfWork uow);
        TPk SaveOrUpdate<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        TPk SaveOrUpdate<TSession>(TEntity entity) where TSession : class, ISession;
        Task<TPk> SaveOrUpdateAsync(TEntity entity, IUnitOfWork uow);
        Task<TPk> SaveOrUpdateAsync<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        Task<TPk> SaveOrUpdateAsync<TSession>(TEntity entity) where TSession : class, ISession;

        void Add(TEntity entity, ISession session);
        void Add(TEntity entity, IUnitOfWork uow);
        void Add<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        void Add<TSession>(TEntity entity) where TSession : class, ISession;
        Task AddAsync(TEntity entity, ISession session);
        Task AddAsync(TEntity entity, IUnitOfWork uow);
        Task AddAsync<TSession>(string dbToken, TEntity entity) where TSession : class, ISession;
        Task AddAsync<TSession>(TEntity entity) where TSession : class, ISession;
    }
}
