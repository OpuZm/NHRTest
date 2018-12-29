using System.Data.Entity;
using Starts2000.Domain.Entities;
using Starts2000.Domain.Repositories;

namespace Starts2000.EntityFramework.Repositories
{
    public class EfRepositoryBase<TDbContext, TEntity> 
        : EfRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
