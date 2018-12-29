﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FastCrud;
using Dapper.FastCrud.Mappings;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Infrastructure.Dapper
{
    internal sealed class MultiDbRepositoryContainer
    {
        private static volatile MultiDbRepositoryContainer _instance;
        private static readonly object SyncRoot = new object();

        private readonly ConcurrentDictionary<Type, PropertyMapping[]> _keys =
            new ConcurrentDictionary<Type, PropertyMapping[]>();
        private readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _properties =
            new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
        private readonly ConcurrentDictionary<Type, bool> _isIEntity =
            new ConcurrentDictionary<Type, bool>();

        private MultiDbRepositoryContainer() { }

        public static MultiDbRepositoryContainer Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MultiDbRepositoryContainer();
                }
                return _instance;
            }
        }

        public PropertyMapping[] GetKeys<TEntity>()
        where TEntity : class
        {
            return _keys.GetOrAdd(typeof(TEntity), GetKeyPropertyMembers<TEntity>());
        }

        public IEnumerable<PropertyInfo> GetProperties<TEntity>(PropertyMapping[] keys)
        where TEntity : class
        {
            if (keys == null || !keys.Any())
            {
                return new PropertyInfo[0];
            }
            return _properties.GetOrAdd(typeof(TEntity), GetKeyPropertyInfo(typeof(TEntity), keys));
        }

        public bool IsIEntity<TEntity, TPk>()
        where TEntity : class
        where TPk : IComparable
        {
            return _isIEntity.GetOrAdd(typeof(TEntity), typeof(TEntity).GetInterfaces().Any(x => x == typeof(IEntity<TPk>)));
        }

        private static PropertyMapping[] GetKeyPropertyMembers<TEntity>()
        {
            return OrmConfiguration.GetDefaultEntityMapping<TEntity>().GetProperties(PropertyMappingOptions.KeyProperty);
        }

        private static IEnumerable<PropertyInfo> GetKeyPropertyInfo(Type entity, PropertyMapping[] keys)
        {
            return entity.GetProperties().Where(property => keys.Any(key => property.Name.Equals(key.PropertyName, StringComparison.Ordinal)));
        }
    }
}
