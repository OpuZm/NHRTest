﻿using Starts2000.Domain.Entities;

namespace Starts2000.Domain.Repositories
{
    /// <summary>
    /// A shortcut of <see cref="IRepository{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IRepository<TEntity>
        : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {

    }
}
