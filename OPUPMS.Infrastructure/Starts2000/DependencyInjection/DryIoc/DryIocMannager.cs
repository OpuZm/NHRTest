using System;
using DryIoc;

namespace Starts2000.DependencyInjection.DryIoc
{
    public class DryIocMannager : IDryIocManager
    {
        public IContainer IocContainer { get; private set; }

        public DryIocMannager(IContainer container)
        {
            IocContainer = container;
        }

        public bool IsRegistered(Type type)
        {
            return IocContainer.IsRegistered(type);
        }

        public bool IsRegistered<TType>()
        {
            return IocContainer.IsRegistered<TType>();
        }

        public void Register<T>(
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient) 
            where T : class
        {
            IocContainer.Register<T>(reuse: ConvertLifetimeToReuse(lifeTime));
        }

        public void Register(Type type,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            IocContainer.Register(type, reuse: ConvertLifetimeToReuse(lifeTime));
        }

        public void Register<TService>(Func<IIocResolver, TService> implementationFactory,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
            where TService : class
        {
            IocContainer.RegisterDelegate(
                resolver => implementationFactory(resolver.Resolve<IIocResolver>()),
                ConvertLifetimeToReuse(lifeTime));
        }

        public void Register(Type serviceType, Type implementationType,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            IocContainer.Register(serviceType, 
                implementationType, ConvertLifetimeToReuse(lifeTime));
        }

        public void Register<TService, TImplementation>(
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
            where TService : class
            where TImplementation : class, TService
        {
            IocContainer.Register<TService, TImplementation>(ConvertLifetimeToReuse(lifeTime));
        }

        public void RegisterInstance<TService>(TService instance)
        {
            IocContainer.UseInstance(instance);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            IocContainer.UseInstance(serviceType, instance);
        }

        public object Resolve(Type type)
        {
            return IocContainer.Resolve(type);
        }

        public T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }

        #region IDisposable Support

        bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if(IocContainer != null)
                    {
                        IocContainer.Dispose();
                    }
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        internal static IReuse ConvertLifetimeToReuse(DependencyLifeTime lifeTime)
        {
            switch (lifeTime)
            {
                case DependencyLifeTime.Transient:
                    return Reuse.Transient;
                case DependencyLifeTime.Scoped:
                    return Reuse.ScopedOrSingleton;
                case DependencyLifeTime.Singleton:
                    return Reuse.Singleton;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(lifeTime), lifeTime, "Not supported lifetime");
            }
        }
    }
}