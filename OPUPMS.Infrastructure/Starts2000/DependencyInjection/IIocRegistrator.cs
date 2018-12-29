using System;

namespace Starts2000.DependencyInjection
{
    public interface IIocRegistrator
    {
        /// <summary>
        /// Registers a type as self registration.
        /// </summary>
        /// <typeparam name="T">Type of the class</typeparam>
        /// <param name="lifeTime">Lifestyle of the objects of this type</param>
        void Register<T>(DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
            where T : class;

        /// <summary>
        /// Registers a type as self registration.
        /// </summary>
        /// <param name="type">Type of the class</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type.</param>
        void Register(Type type, DependencyLifeTime lifeTime = DependencyLifeTime.Transient);

        /// <summary>
        /// Registers a type with factory.
        /// </summary>
        /// <typeparam name="TService">The type of the service to regist.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <param name="lifeTime">Lifestyle of the objects of this type.</param>
        void Register<TService>(Func<IIocResolver, TService> implementationFactory, 
            DependencyLifeTime lifeTime = DependencyLifeTime.Singleton) 
            where TService : class;

        /// <summary>
        /// Registers a type with it's implementation.
        /// </summary>
        /// <typeparam name="TService">Registering type</typeparam>
        /// <typeparam name="TImplementation">The type that implements <see cref="TType"/></typeparam>
        /// <param name="lifeTime">Lifestyle of the objects of this type</param>
        void Register<TService, TImplementation>(
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Registers a type with it's implementation.
        /// </summary>
        /// <param name="serviceType">Type of the class</param>
        /// <param name="implementationType">The type that implements <paramref name="serviceType"/></param>
        /// <param name="lifeTime">Lifestyle of the objects of this type</param>
        void Register(Type serviceType, Type implementationType, 
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient);

        /// <summary>
        /// Registers a type with it's implementation instacne.
        /// </summary>
        /// <typeparam name="TService">Registering type</typeparam>
        /// <param name="instance">Implementation instacne.</param>
        void RegisterInstance<TService>(TService instance);

        /// <summary>
        /// Registers a type with it's implementation instacne.
        /// </summary>
        /// <param name="serviceType">Type of the class</param>
        /// <param name="instance">Implementation instacne.</param>
        void RegisterInstance(Type serviceType, object instance);

        /// <summary>
        /// Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        bool IsRegistered(Type type);

        /// <summary>
        /// Checks whether given type is registered before.
        /// </summary>
        /// <typeparam name="TType">Type to check</typeparam>
        bool IsRegistered<TType>();
    }
}
