using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DryIoc;

namespace Starts2000.DependencyInjection.DryIoc
{
    public static class IocRegistratorExtensions
    {
        public static IDryIocManager AsDryIocManager(this IIocRegistrator registrator)
        {
            if (registrator is IDryIocManager iocManager)
            {
                return iocManager;
            }

            Check.NotSupported("Application not supported DryIoc!");
            return null;
        }

        public static IIocRegistrator RegisterTransient<TService, TImplementation>(
            this IIocRegistrator registrator, bool autoInjectProperty = false)
            where TImplementation : TService
        {
            return registrator.Register<TService, TImplementation>(
                autoInjectProperty, DependencyLifeTime.Transient);
        }

        public static IIocRegistrator RegisterScoped<TService, TImplementation>(
            this IIocRegistrator registrator, bool autoInjectProperty = false)
            where TImplementation : TService
        {
            return registrator.Register<TService, TImplementation>(
                autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterSingleton<TService, TImplementation>(
           this IIocRegistrator registrator, bool autoInjectProperty = false)
           where TImplementation : TService
        {
            return registrator.Register<TService, TImplementation>(
                autoInjectProperty, DependencyLifeTime.Singleton);
        }

        public static IIocRegistrator RegisterTransient(
            this IIocRegistrator registrator,
            Type serviceType,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            return registrator.Register(serviceType, implementationType,
                autoInjectProperty, DependencyLifeTime.Transient);
        }

        public static IIocRegistrator RegisterScoped(
            this IIocRegistrator registrator,
            Type serviceType,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            return registrator.Register(serviceType, implementationType,
                autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterSingleton(
            this IIocRegistrator registrator,
            Type serviceType,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            return registrator.Register(serviceType, implementationType,
                autoInjectProperty, DependencyLifeTime.Singleton);
        }

        public static IIocRegistrator RegisterManyTransient<TImplementation>(
            this IIocRegistrator registrator, bool autoInjectProperty = false)
        {
            return registrator.RegisterMany<TImplementation>(
                autoInjectProperty, DependencyLifeTime.Transient);
        }

        public static IIocRegistrator RegisterManyScoped<TImplementation>(
            this IIocRegistrator registrator, bool autoInjectProperty = false)
        {
            return registrator.RegisterMany<TImplementation>(
                autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterManySingleton<TImplementation>(
            this IIocRegistrator registrator, bool autoInjectProperty = false)
        {
            return registrator.RegisterMany<TImplementation>(
                autoInjectProperty, DependencyLifeTime.Singleton);
        }

        public static IIocRegistrator RegisterManyTransient(
            this IIocRegistrator registrator,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterMany(
                implementationType, autoInjectProperty, DependencyLifeTime.Transient);
        }

        public static IIocRegistrator RegisterManyScoped(
            this IIocRegistrator registrator,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterMany(
                implementationType, autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterManySingleton(
            this IIocRegistrator registrator,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterMany(
                implementationType, autoInjectProperty, DependencyLifeTime.Singleton);
        }

        public static IIocRegistrator RegisterManyTransient(
            this IIocRegistrator registrator,
            IEnumerable<Type> implementationTypes,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterMany(
                implementationTypes, autoInjectProperty, DependencyLifeTime.Transient);
        }
        public static IIocRegistrator RegisterManyScoped(
            this IIocRegistrator registrator,
            IEnumerable<Type> implementationTypes,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterMany(
                implementationTypes, autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterManySingleton(
            this IIocRegistrator registrator,
            IEnumerable<Type> implementationTypes,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterMany(
                implementationTypes, autoInjectProperty, DependencyLifeTime.Singleton);
        }

        public static IIocRegistrator RegisterManyTransient(
            this IIocRegistrator registrator,
            IEnumerable<Type> seviceTypes,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            registrator.RegisterMany(seviceTypes,
                implementationType, autoInjectProperty, DependencyLifeTime.Transient);
            return registrator;
        }

        public static IIocRegistrator RegisterManyScoped(
            this IIocRegistrator registrator,
            IEnumerable<Type> seviceTypes,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            registrator.RegisterMany(seviceTypes,
                implementationType, autoInjectProperty, DependencyLifeTime.Scoped);
            return registrator;
        }

        public static IIocRegistrator RegisterManySingleton(
            this IIocRegistrator registrator,
            IEnumerable<Type> seviceTypes,
            Type implementationType,
            bool autoInjectProperty = false)
        {
            registrator.RegisterMany(seviceTypes,
                implementationType, autoInjectProperty, DependencyLifeTime.Singleton);
            return registrator;
        }

        public static IIocRegistrator RegisterAssembliesTransient(
            this IIocRegistrator registrator,
            IEnumerable<Assembly> assemblies = null,
            Func<Type, bool> typeSelector = null,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterAssemblies(assemblies, 
                typeSelector, autoInjectProperty, DependencyLifeTime.Transient);
        }

        public static IIocRegistrator RegisterAssembliesScoped(
            this IIocRegistrator registrator,
            IEnumerable<Assembly> assemblies = null,
            Func<Type, bool> typeSelector = null,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterAssemblies(assemblies,
                typeSelector, autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterAssembliesSingleton(
            this IIocRegistrator registrator,
            IEnumerable<Assembly> assemblies = null,
            Func<Type, bool> typeSelector = null,
            bool autoInjectProperty = false)
        {
            return registrator.RegisterAssemblies(assemblies,
                typeSelector, autoInjectProperty, DependencyLifeTime.Singleton);
        }

        public static IIocRegistrator RegisterAssemblyTransient(
            this IIocRegistrator registrator,
            Assembly assembly,
            Func<Type, bool> typeSelector = null,
            bool autoInjectProperty = false)
        {
            Check.NotNull(assembly, nameof(assembly));
            return registrator.RegisterAssemblies(new[] { assembly },
                typeSelector, autoInjectProperty, DependencyLifeTime.Transient);
        }

        public static IIocRegistrator RegisterAssemblyScoped(
            this IIocRegistrator registrator,
            Assembly assembly,
            Func<Type, bool> typeSelector,
            bool autoInjectProperty = false)
        {
            Check.NotNull(assembly, nameof(assembly));
            return registrator.RegisterAssemblies(new[] { assembly },
                typeSelector, autoInjectProperty, DependencyLifeTime.Scoped);
        }

        public static IIocRegistrator RegisterAssemblySingleton(
            this IIocRegistrator registrator,
            Assembly assembly,
            Func<Type, bool> typeSelector = null,
            bool autoInjectProperty = false)
        {
            Check.NotNull(assembly, nameof(assembly));
            return registrator.RegisterAssemblies(new[] { assembly },
                typeSelector, autoInjectProperty, DependencyLifeTime.Singleton);
        }

        internal static IIocRegistrator RegisterAssemblies(
            this IIocRegistrator registrator,
            IEnumerable<Assembly> assemblies,
            Func<Type, bool> typeSelector,
            bool autoInjectProperty,
            DependencyLifeTime lifeTime)
        {
            Check.NotNull(registrator, nameof(registrator));
            assemblies = assemblies ?? AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies
                .SelectMany(asm => asm.DefinedTypes)
                .Select(typeInfo => typeInfo.AsType())
                .Where(type => typeSelector?.Invoke(type) ?? true);
            foreach(var type in types)
            {
                registrator.RegisterMany(type, autoInjectProperty, lifeTime);
            }

            return registrator;
        }

        internal static IIocRegistrator Register<TService, TImplementation>(
            this IIocRegistrator registrator,
            bool autoInjectProperty = false,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
            where TImplementation : TService
        {
            registrator.AsDryIocManager().IocContainer
                .Register<TService, TImplementation>(
                    DryIocMannager.ConvertLifetimeToReuse(lifeTime),
                    autoInjectProperty ? PropertiesAndFields.Auto : null);
            return registrator;
        }

        internal static IIocRegistrator Register(
            this IIocRegistrator registrator,
            Type serviceType,
            Type implementationType,
            bool autoInjectProperty = false,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            registrator.AsDryIocManager().IocContainer.Register(
                serviceType, implementationType,
                DryIocMannager.ConvertLifetimeToReuse(lifeTime),
                autoInjectProperty ? PropertiesAndFields.Auto : null);
            return registrator;
        }

        internal static IIocRegistrator RegisterMany<TImplementation>(
            this IIocRegistrator registrator,
            bool autoInjectProperty = false,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            registrator.AsDryIocManager().IocContainer
                .RegisterMany<TImplementation>(
                    DryIocMannager.ConvertLifetimeToReuse(lifeTime),
                    autoInjectProperty ? PropertiesAndFields.Auto : null);
            return registrator;
        }

        internal static IIocRegistrator RegisterMany(
            this IIocRegistrator registrator,
            Type implementationType,
            bool autoInjectProperty = false,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            registrator.RegisterMany(
                new[] { implementationType }, autoInjectProperty, lifeTime);
            return registrator;
        }

        internal static IIocRegistrator RegisterMany(
            this IIocRegistrator registrator,
            IEnumerable<Type> implementationTypes,
            bool autoInjectProperty = false,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            registrator.AsDryIocManager().IocContainer.RegisterMany(
                implementationTypes,
                DryIocMannager.ConvertLifetimeToReuse(lifeTime),
                autoInjectProperty ? PropertiesAndFields.Auto : null);
            return registrator;
        }

        internal static IIocRegistrator RegisterMany(
            this IIocRegistrator registrator,
            IEnumerable<Type> seviceTypes,
            Type implementationType,
            bool autoInjectProperty = false,
            DependencyLifeTime lifeTime = DependencyLifeTime.Transient)
        {
            registrator.AsDryIocManager().IocContainer.RegisterMany(
                seviceTypes.ToArray(),
                implementationType,
                DryIocMannager.ConvertLifetimeToReuse(lifeTime),
                autoInjectProperty ? PropertiesAndFields.Auto : null);
            return registrator;
        }
    }
}