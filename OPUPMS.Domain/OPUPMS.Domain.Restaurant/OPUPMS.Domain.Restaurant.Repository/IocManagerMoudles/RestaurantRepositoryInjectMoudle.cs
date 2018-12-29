using System.Configuration;
using OPUPMS.Domain.Repository.IocManagerMoudles;
using SqlSugar;
using Starts2000.DependencyInjection.DryIoc;

namespace OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles
{
    public class RestaurantRepositoryIocManagerMoudle : RepositoryIocManagerModule
    {
        public override void Load()
        {
            base.Load();
            IocManager.RegisterAssemblyTransient(
                typeof(RestaurantRepositoryIocManagerMoudle).Assembly,
                type => (type.IsClass && type.IsPublic && !type.IsAbstract) &&
                        type.FullName.EndsWith("Repository") &&
                        type.GetInterfaces().Length > 0);

            //Kernel.Bind<INinjectDbFactory>().ToFactory(() => new TypeMatchingArgumentInheritanceInstanceProvider());
            //Kernel.Rebind<IDbFactory, IMultiDbDbFactory>().To<DbFactory>().InSingletonScope();
            //Kernel.Bind<IUnitOfWork>().To<UnitOfWork>()
            //    .WithConstructorArgument(typeof(IDbFactory))
            //    .WithConstructorArgument(typeof(ISession))
            //    .WithConstructorArgument(typeof(IsolationLevel));
            //Kernel.Bind<ISession>().To<MsSqlSession>();

            //Kernel.Bind<IUserRepository>().To<UserRepository>();
            //Kernel.Bind<IOperateLogRepository>().To<OperateLogRepository>();

            //Kernel.Bind<ICyxmRepository>().To<CyxmRepository>();
            //Kernel.Bind<IRestaurantRepository>().To<RestaurantRepository>();
            //Kernel.Bind<IAreaRepository>().To<AreaRepository>();
            //Kernel.Bind<IBoxRepository>().To<BoxRepository>();
            //Kernel.Bind<ITableRepository>().To<TableRepository>();
            //Kernel.Bind<IStallsRepository>().To<StallsRepository>();
            //Kernel.Bind<IMarketRepository>().To<MarketRepository>();
            //Kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            //Kernel.Bind<IExtendRepository>().To<ExtendRepository>();
            //Kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            //Kernel.Bind<IPackageRepository>().To<PackageRepository>();
            //Kernel.Bind<IDiscountRepository>().To<DiscountRepository>();
            //Kernel.Bind<IOrderRepository>().To<OrderRepository>();
            //Kernel.Bind<ICheckOutRepository>().To<CheckOutRepository>();
            //Kernel.Bind<IOrderRecordRepository>().To<OrderRecordRepository>();
            //Kernel.Bind<IPrinterRepository>().To<PrinterRepository>();
            //Kernel.Bind<IOrderPayRecordRepository>().To<OrderPayRecordRepository>();
            //Kernel.Bind<IStatisticsRepository>().To<StatisticsRepository>();
            //Kernel.Bind<IUserRepository_Old>().To<RestaurantUserRepository>();
            //Kernel.Bind<IPayMethodRepository>().To<PayMethodRepository>();

            //Kernel.Bind(x =>
            //    x.FromThisAssembly()
            //    .SelectAllClasses()
            //    .InheritedFrom(typeof(IRestaurantRepository))
            //    .EndingWith("Repository")
            //    .BindDefaultInterfaces());

            //Kernel.Bind<ICloudHotelRepository>().To<CloudHotelRepository>();

            //try
            //{
            //    var instance = Mapper.Instance;

            //    Mapper.Initialize(cfg =>
            //    {
            //        cfg.AddProfile(new RestaurantConvertModelProfile());
            //        cfg.AddProfiles(new string[] { "OPUPMS.Domain.Hotel.Model" });
            //    });
            //}
            //catch (Exception)
            //{
            //    Mapper.Initialize(cfg =>
            //    {
            //        cfg.AddProfile(new RestaurantConvertModelProfile());
            //    });
            //}
        }
    }

    public abstract class SqlSugarService
    {
        readonly static string _connectionString = ConfigurationManager
            .ConnectionStrings["OPUPMSConn"].ConnectionString;
        readonly static string _connectionGroupString = ConfigurationManager
            .ConnectionStrings["OPUPMSGroupConn"].ConnectionString;
        
        readonly static bool _enabledGroupFlag = ConfigurationManager
            .AppSettings["MemberGroup"].ObjToBool();
        readonly static int _round = ConfigurationManager
            .AppSettings["Round"].ObjToInt();
        readonly static int _printModel = ConfigurationManager.AppSettings["PrintModel"].ObjToInt();
        readonly static bool _nightTrial = ConfigurationManager.AppSettings["NightTrial"].ObjToBool();
        readonly static int _extractType = ConfigurationManager.AppSettings["ExtractType"].ObjToInt();
        readonly static string _checkOutRemovePayType= ConfigurationManager.AppSettings["CheckOutRemovePayType"].ObjToString();
        readonly static bool _orderDetailPrintTest= ConfigurationManager.AppSettings["OrderDetailPrintTest"].ObjToBool();
        readonly static bool _autoListPrint= ConfigurationManager.AppSettings["AutoListPrint"].ObjToBool();
        readonly static bool _defaultPromptly = ConfigurationManager.AppSettings["DefaultPromptly"].ObjToBool();

        public string Connection { get; } = _connectionString;

        /// <summary>
        /// 集团库连接
        /// </summary>
        public static string ConnentionGroup
        {
            get
            {
                return _connectionGroupString;
            }
        }

        /// <summary>
        /// 是否启用集团库标识
        /// </summary>
        public static bool EnabelGroupFlag
        {
            get { return _enabledGroupFlag; }
        }

        public static int Round
        {
            get { return _round; }
        }

        public static int PrintModel
        {
            get { return _printModel; }
        }

        public static bool NightTrial
        {
            get { return _nightTrial; }
        }

        public static int ExtractType
        {
            get { return _extractType; }
        }

        public static string CheckOutRemovePayType
        {
            get { return _checkOutRemovePayType; }
        }

        public static bool OrderDetailPrintTest
        {
            get { return _orderDetailPrintTest; }
        }

        public static bool AutoListPrint
        {
            get { return _autoListPrint; }
        }

        public static bool DefaultPromptly
        {
            get { return _defaultPromptly; }
        }

        protected SqlSugarService()
        {
        }

        public SqlSugarClient CreateClient()
        {
            return new SqlSugarClient(Connection);
        }
    }
}
