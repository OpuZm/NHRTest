using System;

namespace Starts2000.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IIocRegistrator"/> interface.
    /// </summary>
    public static class IocRegistratorExtensions
    {
        #region RegisterIfNot

        /// <summary>
        /// Registers a type as self registration if it's not registered before.
        /// </summary>
        /// <typeparam name="T">Type of the class</typeparam>
        /// <param name="iocRegistror">Registrar</param>
        /// <param name="lifeTime">Life time of the objects of this type</param>
        /// <returns>True, if registered for given implementation.</returns>
        public static bool RegisterIfNot<T>(this IIocRegistrator iocRegistror,
            DependencyLifeTime lifeTime = DependencyLifeTime.Singleton)
            where T : class
        {
            if (iocRegistror.IsRegistered<T>())
            {
                return false;
            }

            iocRegistror.Register<T>(lifeTime);
            return true;
        }

        /// <summary>
        /// Registers a type as self registration if it's not registered before.
        /// </summary>
        /// <param name="iocRegistror">Registrar</param>
        /// <param name="type">Type of the class</param>
        /// <param name="lifeTime">Life time of the objects of this type</param>
        /// <returns>True, if registered for given implementation.</returns>
        public static bool RegisterIfNot(this IIocRegistrator iocRegistror, 
            Type type, DependencyLifeTime lifeTime = DependencyLifeTime.Singleton)
        {
            if (iocRegistror.IsRegistered(type))
            {
                return false;
            }

            iocRegistror.Register(type, lifeTime);
            return true;
        }

        /// <summary>
        /// Registers a type with it's implementation if it's not registered before.
        /// </summary>
        /// <typeparam name="TService">Registering type</typeparam>
        /// <typeparam name="TImplementation">The type that implements <see cref="TType"/></typeparam>
        /// <param name="iocRegistror">Registrar</param>
        /// <param name="lifeTime">Life time of the objects of this type</param>
        /// <returns>True, if registered for given implementation.</returns>
        public static bool RegisterIfNot<TService, TImplementation>(
            this IIocRegistrator iocRegistror,
            DependencyLifeTime lifeTime = DependencyLifeTime.Singleton)
            where TService : class
            where TImplementation : class, TService
        {
            if (iocRegistror.IsRegistered<TService>())
            {
                return false;
            }

            iocRegistror.Register<TService, TImplementation>(lifeTime);
            return true;
        }


        /// <summary>
        /// Registers a type with it's implementation if it's not registered before.
        /// </summary>
        /// <param name="iocRegistror">Registrar</param>
        /// <param name="serviceType">Type of the class</param>
        /// <param name="implementationType">The type that implements <paramref name="serviceType"/></param>
        /// <param name="lifeTime">Life time of the objects of this type</param>
        /// <returns>True, if registered for given implementation.</returns>
        public static bool RegisterIfNot(
            this IIocRegistrator iocRegistror, 
            Type serviceType, Type implementationType, 
            DependencyLifeTime lifeTime = DependencyLifeTime.Singleton)
        {
            if (iocRegistror.IsRegistered(serviceType))
            {
                return false;
            }

            iocRegistror.Register(serviceType, implementationType, lifeTime);
            return true;
        }

        #endregion

        /// <summary>
        /// Registersa transient service of the type specified in TService with an implementation
        /// type specified in TImplementation.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterTransient<TService, TImplementation>(
            this IIocRegistrator registrator)
            where TService : class
            where TImplementation : class, TService
        {
            registrator.Register<TService, TImplementation>(DependencyLifeTime.Transient);
            return registrator;
        }

        /// <summary>
        /// Registersa scoped service of the type specified in TService with an implementation
        /// type specified in TImplementation.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterScoped<TService, TImplementation>(
            this IIocRegistrator registrator)
            where TService : class
            where TImplementation : class, TService
        {
            registrator.Register<TService, TImplementation>(DependencyLifeTime.Scoped);
            return registrator;
        }

        /// <summary>
        /// Registersa singleton service of the type specified in TService with an implementation
        /// type specified in TImplementation.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterSingleton<TService, TImplementation>(
            this IIocRegistrator registrator)
            where TService : class
            where TImplementation : class, TService
        {
            registrator.Register<TService, TImplementation>(DependencyLifeTime.Singleton);
            return registrator;
        }

        /// <summary>
        /// Registers a transient service of the type specified in TService as self registration.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterTransient<TService>(this IIocRegistrator registrator)
            where TService : class
        {
            registrator.Register<TService>(DependencyLifeTime.Transient);
            return registrator;
        }

        /// <summary>
        /// Registers a scoped service of the type specified in TService as self registration.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterScoped<TService>(this IIocRegistrator registrator)
            where TService : class
        {
            registrator.Register<TService>(DependencyLifeTime.Scoped);
            return registrator;
        }

        /// <summary>
        /// Registers a singleton service of the type specified in TService as self registration.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterSingleton<TService>(this IIocRegistrator registrator)
            where TService : class
        {
            registrator.Register<TService>(DependencyLifeTime.Singleton);
            return registrator;
        }

        /// <summary>
        /// Registers a transient service of the type specified in TService with factory.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterTransient<TService>(
            this IIocRegistrator registrator,
            Func<IIocResolver, TService> implementationFactory)
            where TService : class
        {
            registrator.Register(implementationFactory, DependencyLifeTime.Transient);
            return registrator;
        }

        /// <summary>
        /// Registers a scoped service of the type specified in TService with factory.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterScoped<TService>(
            this IIocRegistrator registrator,
            Func<IIocResolver, TService> implementationFactory)
            where TService : class
        {
            registrator.Register(implementationFactory, DependencyLifeTime.Scoped);
            return registrator;
        }

        /// <summary>
        /// Registers a singleton service of the type specified in TService with factory.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterSingleton<TService>(
            this IIocRegistrator registrator,
            Func<IIocResolver, TService> implementationFactory)
            where TService : class
        {
            registrator.Register(implementationFactory, DependencyLifeTime.Singleton);
            return registrator;
        }

        /// <summary>
        /// Registers a transient service of the type specified in serviceType with an implementation
        /// of the type specified in implementationType.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterTransient(
            this IIocRegistrator registrator, Type serviceType, Type implementationType)
        {
            registrator.Register(serviceType, implementationType, DependencyLifeTime.Transient);
            return registrator;
        }

        /// <summary>
        /// Registers a scoped service of the type specified in serviceType with an implementation
        /// of the type specified in implementationType.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterScoped(
            this IIocRegistrator registrator, Type serviceType, Type implementationType)
        {
            registrator.Register(serviceType, implementationType, DependencyLifeTime.Scoped);
            return registrator;
        }

        /// <summary>
        /// Registers a singleton service of the type specified in serviceType with an implementation
        /// of the type specified in implementationType.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterSingleton(
            this IIocRegistrator registrator, Type serviceType, Type implementationType)
        {
            registrator.Register(serviceType, implementationType, DependencyLifeTime.Singleton);
            return registrator;
        }

        /// <summary>
        /// Registers a transient service of the type specified in serviceType as self registration.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterTransient(
            this IIocRegistrator registrator, Type serviceType)
        {
            registrator.Register(serviceType, DependencyLifeTime.Transient);
            return registrator;
        }

        /// <summary>
        /// Registers a scoped service of the type specified in serviceType as self registration.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterScoped(
            this IIocRegistrator registrator, Type serviceType)
        {
            registrator.Register(serviceType, DependencyLifeTime.Scoped);
            return registrator;
        }

        /// <summary>
        /// Registers a singleton service of the type specified in serviceType as self registration.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterSingleton(
            this IIocRegistrator registrator, Type serviceType)
        {
            registrator.Register(serviceType, DependencyLifeTime.Singleton);
            return registrator;
        }

        /// <summary>
        /// Registers a open scope or singleton service of the type
        /// specified in TService with it's implementation instacne.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="instance">Implementation instacne.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterInstanceFluent<TService>(
            this IIocRegistrator registrator, TService instance)
        {
            registrator.RegisterInstance(instance);
            return registrator;
        }

        /// <summary>
        /// Registers a open scope or singleton service of the type
        /// specified in serviceType with it's implementation instacne.
        /// </summary>
        /// <param name="registrator">The <see cref="IIocRegistrator"/> to regist the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="instance">Implementation instacne.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IIocRegistrator RegisterInstanceFluent(
            this IIocRegistrator registrator, Type serviceType, object instance)
        {
            registrator.RegisterInstance(serviceType, instance);
            return registrator;
        }
    }
}