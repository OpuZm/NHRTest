using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Attributes;

namespace DryIoc
{
    public static class IContainerExtensions
    {
        static readonly Action<IMapperConfigurationExpression> DefaultConfig = cfg => { };

        public static IContainer AddAutoMapper(this IContainer container,
            bool useUseStaticMapper, params Assembly[] assemblies)
                => AddAutoMapperClasses(container, useUseStaticMapper, null, assemblies);

        public static IContainer AddAutoMapper(this IContainer container,
            bool useUseStaticMapper, IEnumerable<Assembly> assemblies)
                => AddAutoMapperClasses(container, useUseStaticMapper, null, assemblies);


        public static IContainer AddAutoMapper(
            this IContainer container,
            bool useUseStaticMapper,
            Action<IMapperConfigurationExpression> additionalInitAction,
            params Assembly[] assemblies)
                => AddAutoMapperClasses(container,
                    useUseStaticMapper, additionalInitAction, assemblies);

        public static IContainer AddAutoMapper(
            this IContainer container,
            bool useUseStaticMapper,
            Action<IMapperConfigurationExpression> additionalInitAction,
            IEnumerable<Assembly> assemblies)
                => AddAutoMapperClasses(container,
                    useUseStaticMapper, additionalInitAction, assemblies);

        public static IContainer AddAutoMapper(this IContainer container,
            bool useUseStaticMapper, params Type[] profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(container, useUseStaticMapper, null,
                profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));
        }

        public static IContainer AddAutoMapper(
            this IContainer container,
            bool useUseStaticMapper,
            Action<IMapperConfigurationExpression> additionalInitAction,
            params Type[] profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(container, useUseStaticMapper, additionalInitAction,
                profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));
        }

        public static IContainer AddAutoMapper(
            this IContainer container,
            bool useUseStaticMapper,
            Action<IMapperConfigurationExpression> additionalInitAction,
            IEnumerable<Type> profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(container, useUseStaticMapper, additionalInitAction,
                profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));
        }


        static IContainer AddAutoMapperClasses(
            IContainer container,
            bool useUseStaticMapper,
            Action<IMapperConfigurationExpression> additionalInitAction,
            IEnumerable<Assembly> assembliesToScan)
        {
            additionalInitAction = additionalInitAction ?? DefaultConfig;
            assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

            var allTypes = assembliesToScan
                .Where(a => a.GetName().Name != nameof(AutoMapper))
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

            var profiles =
                allTypes
                    .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t))
                    .Where(t => !t.IsAbstract);

            var openTypes = new[]
            {
                typeof(IValueResolver<,,>),
                typeof(IMemberValueResolver<,,,>),
                typeof(ITypeConverter<,>)
            };

            foreach (var type in openTypes.SelectMany(openType => allTypes
                .Where(t => t.IsClass)
                .Where(t => !t.IsAbstract)
                .Where(t => t.AsType().ImplementsGenericInterface(openType))))
            {
                container.Register(type, Reuse.Transient);
            }

            void configurer(IMapperConfigurationExpression configuration)
            {
                additionalInitAction(configuration);

                foreach (var ass in assembliesToScan)
                {
                    ass.MapTypes(configuration);
                }

                foreach (var profile in profiles.Select(t => t.AsType()))
                {
                    configuration.AddProfile(profile);
                }
            }

            if (useUseStaticMapper)
            {
                Mapper.Initialize(configurer);
                container.RegisterInstance(Mapper.Configuration);
                container.RegisterInstance(Mapper.Instance);
            }
            else
            {
                var config = new MapperConfiguration(configurer);
                container.RegisterInstance<IConfigurationProvider>(config);
                container.RegisterDelegate<IMapper>(resolver =>
                       new Mapper(
                           resolver.Resolve<IConfigurationProvider>(),
                           resolver.Resolve), Reuse.ScopedOrSingleton);
            }

            return container;
        }

        static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            return type.IsGenericType(interfaceType) ||
                   type.GetTypeInfo().ImplementedInterfaces.Any(
                       @interface => @interface.IsGenericType(interfaceType));
        }

        static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType &&
               type.GetGenericTypeDefinition() == genericType;
    }
}
