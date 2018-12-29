using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Starts2000.Domain.Entities;
using Starts2000.Domain.Repositories;

namespace Starts2000.EntityFramework.Repositories
{
    public static class EfRepositoryExtensions
    {
        public static TDbContext GetDbContext<TDbContext, TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
            where TDbContext : DbContext
        {
            if (repository is IRepositoryWithDbContext repositoryWithDbContext)
            {
                return repositoryWithDbContext.GetDbContext() as TDbContext;
            }

            throw new ArgumentException(
                "Given repository does not implement IRepositoryWithDbContext",
                nameof(repository));
        }

        public static DbContext GetDbContext<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            if (repository is IRepositoryWithDbContext repositoryWithDbContext)
            {
                return repositoryWithDbContext.GetDbContext();
            }

            throw new ArgumentException(
                "Given repository does not implement IRepositoryWithDbContext",
                nameof(repository));
        }

        public static int SaveChanges<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.GetDbContext().SaveChanges();
        }

        public static Task<int> SaveChangesAsync<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.GetDbContext().SaveChangesAsync();
        }

        public static void DetachFromDbContext<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            repository.GetDbContext().Entry(entity).State = EntityState.Detached;
        }
    }
}
