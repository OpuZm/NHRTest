using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper.FastCrud;
using DryIoc;
using OPUPMS.Infrastructure.Dapper;
using Smooth.IoC.UnitOfWork;
using Starts2000.DependencyInjection;
using Starts2000.DependencyInjection.DryIoc;

namespace OPUPMS.Domain.Repository.IocManagerMoudles
{
    public class RepositoryIocManagerModule : IocManagerModule
    {
        public override void Load()
        {
            IocManager.AsDryIocManager().IocContainer
                .Register<ISession, MsSqlSession>(
                    made: FactoryMethod.ConstructorWithResolvableArguments);
            IocManager.AsDryIocManager().IocContainer
                .Register<IUnitOfWork, UnitOfWork>(
                    made: FactoryMethod.ConstructorWithResolvableArguments);
            IocManager.RegisterManySingleton<DbFactory>();

            IocManager.RegisterAssemblyTransient(
                typeof(RepositoryIocManagerModule).Assembly,
                type => (type.IsClass && type.IsPublic && !type.IsAbstract) &&
                        type.FullName.EndsWith("Repository") &&
                        type.GetInterfaces().Length > 0);

            //Kernel.Bind<IDbFactory, IMultiDbDbFactory>().To<DbFactory>().InSingletonScope();
            //Kernel.Bind<IUnitOfWork>().To<UnitOfWork>()
            //    .WithConstructorArgument(typeof(IDbFactory))
            //    .WithConstructorArgument(typeof(ISession))
            //    .WithConstructorArgument(typeof(IsolationLevel));
            //Kernel.Bind<ISession>().To<MsSqlSession>();

            ////Kernel.Bind<IUserRepository_Old>().To<UserRepository_Old>();
            //Kernel.Bind<IOperateLogRepository>().To<OperateLogRepository>();
            //Kernel.Bind<ICustomerRepository>().To<CustomerRepository>();


            //Kernel.Bind<IAreaCodeRepository>().To<AreaCodeRepository>();
            //Kernel.Bind<IAreaLogRepository>().To<AreaLogRepository>();
            //Kernel.Bind<ICompanyRepository>().To<CompanyRepository>();
            //Kernel.Bind<IDepartmentRepository>().To<DepartmentRepository>();
            //Kernel.Bind<IDepartmentUserRepository>().To<DepartmentUserRepository>();
            //Kernel.Bind<IDictionaryItemRepository>().To<DictionaryItemRepository>();
            //Kernel.Bind<IDictionaryTypeRepository>().To<DictionaryTypeRepository>();
            //Kernel.Bind<IExtendItemRepository>().To<ExtendItemRepository>();
            //Kernel.Bind<IExtendItemSettingRepository>().To<ExtendItemSettingRepository>();
            //Kernel.Bind<IExtendTypeRepository>().To<ExtendTypeRepository>();
            //Kernel.Bind<IGroupRepository>().To<GroupRepository>();
            //Kernel.Bind<IOrganizationUserRepository>().To<OrganizationUserRepository>();
            //Kernel.Bind<IPermissionRepository>().To<PermissionRepository>();
            //Kernel.Bind<IRolePermissionRepository>().To<RolePermissionRepository>();
            //Kernel.Bind<IRoleRepository>().To<RoleRepository>();
            //Kernel.Bind<ISettingRepository>().To<SettingRepository>();
            //Kernel.Bind<ISystemLogRepository>().To<SystemLogRepository>();
            //Kernel.Bind<IUserRepository>().To<UserRepository>();
            //Kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>();
            //Kernel.Bind<IVersionRepository>().To<VersionRepository>();
        }
    }

    sealed class DbFactory : IMultiDbDbFactory
    {
        public static readonly ConcurrentDictionary<string, string>
            ConnectCacheList = new ConcurrentDictionary<string, string>();
        readonly IDryIocManager _dryIocManager;

        public DbFactory(IDryIocManager dryIocManager)
        {
            _dryIocManager = dryIocManager;
        }

        public T Create<T>() where T : class, ISession
        {
            return Create<T>(null);
        }

        public T Create<T>(string token = null) where T : class, ISession
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                var func = _dryIocManager.IocContainer.Resolve<Func<IDbFactory, ISession>>();
                return func(this) as T;
            }

            if (ConnectCacheList.TryGetValue(token, out string connectionString))
            {
                var func = _dryIocManager.IocContainer.Resolve<Func<IDbFactory, string, ISession>>();
                return func(this, connectionString) as T;
            }

            throw new Exception("连接超时或已断开，请重新登录！");
        }

        public TUnitOfWork Create<TUnitOfWork, TSession>(string dbToken = null,
            IsolationLevel isolationLevel = IsolationLevel.Serializable)
            where TUnitOfWork : class, IUnitOfWork
            where TSession : class, ISession
        {
            return Create<TUnitOfWork>(this, Create<TSession>(dbToken), isolationLevel);
        }

        public TUnitOfWork Create<TUnitOfWork, TSession>(IsolationLevel isolationLevel)
            where TUnitOfWork : class, IUnitOfWork
            where TSession : class, ISession
        {
            return Create<TUnitOfWork, TSession>(null, isolationLevel);
        }

        public T Create<T>(IDbFactory factory, ISession session,
            IsolationLevel isolationLevel = IsolationLevel.Serializable)
            where T : class, IUnitOfWork
        {
            var func = _dryIocManager.IocContainer
                .Resolve<Func<IDbFactory, ISession, IsolationLevel, bool, IUnitOfWork>>();
            return func(this, session, isolationLevel, true) as T;
        }

        public void Release(IDisposable instance)
        {
            //_dryIocManager.re
        }

        public void SetConnectionCache(string token, string connectionString)
        {
            if (ConnectCacheList.ContainsKey(token))
            {
                return;
            }

            ConnectCacheList.TryAdd(token, connectionString);
        }
    }

    public class MsSqlSession : Session<SqlConnection>
    {
        //static readonly string GroupConnectString =
        //    ConfigurationManager.ConnectionStrings["OPUPMSConnGroup"].ConnectionString;
        static readonly string GroupConnectString =
            ConfigurationManager.ConnectionStrings["OPUPMSConn"].ConnectionString;

        static MsSqlSession()
        {
            OrmConfiguration.DefaultDialect = Dapper.FastCrud.SqlDialect.MsSql;
        }

        public MsSqlSession(IDbFactory factory) : this(factory, GroupConnectString)
        {
        }

        public MsSqlSession(IDbFactory factory, string connectionString)
            : base(factory, connectionString)
        {
        }
    }
}
