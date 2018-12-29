using OPUPMS.Domain.Repository.IocManagerMoudles;
using Starts2000.DependencyInjection.DryIoc;

namespace OPUPMS.Domain.Hotel.Repository.IocManagerMoudles
{
    public class HotelRepositoryInjectModule : RepositoryIocManagerModule
    {
        public override void Load()
        {
            base.Load();

            IocManager.RegisterAssemblyTransient(
                typeof(HotelRepositoryInjectModule).Assembly,
                type => (type.IsClass && type.IsPublic && !type.IsAbstract) &&
                        type.FullName.EndsWith("Repository") &&
                        type.GetInterfaces().Length > 0);

            //Kernel.Bind<INinjectDbFactory>().ToFactory(() => new TypeMatchingArgumentInheritanceInstanceProvider());
            //Kernel.Rebind<IDbFactory>().To<DbFactory>().InSingletonScope();
            //Kernel.Bind<IDbFactory, IMultiDbDbFactory>().To<DbFactory>().InSingletonScope();
            //Kernel.Bind<IUnitOfWork>().To<UnitOfWork>()
            //    .WithConstructorArgument(typeof(IDbFactory))
            //    .WithConstructorArgument(typeof(ISession))
            //    .WithConstructorArgument(typeof(IsolationLevel));
            //Kernel.Bind<ISession>().To<MsSqlSession>();


            //Kernel.Bind<ICloudHotelRepository>().To<CloudHotelRepository>();
            //Kernel.Bind<IUserRepository>().To<HotelUserRepository>();
            //Kernel.Bind<IOperateLogRepository>().To<OperateLogRepository>();
            //Kernel.Bind<ISystemCodeRepository>().To<SystemCodeRepository>();
            //Kernel.Bind<IRoomRoutineRepository>().To<RoomRoutineRepository>();
            //Kernel.Bind<IRoomSymbolRepository>().To<RoomSymbolRepository>();
            //Kernel.Bind<IGuestAccountingRepository>().To<GuestAccountingRepository>();
            //Kernel.Bind<IGuestDataRepository>().To<GuestDataRepository>();
            //Kernel.Bind<IGuestRoutineRepository>().To<GuestRoutineRepository>();
            //Kernel.Bind<IBookingRepository>().To<BookingRepository>();
            //Kernel.Bind<IBookingDetailRepository>().To<BookingDetailRepository>();

            //try
            //{
            //    var instance = Mapper.Instance;
            //    //var list = instance.ConfigurationProvider.GetAllTypeMaps();

            //    Mapper.Initialize(cfg =>
            //    {
            //        cfg.AddProfile(new HotelConvertModelProfile());
            //        cfg.AddProfiles(new string[] { "OPUPMS.Domain.Restaurant.Model" });
            //    });
            //}
            //catch (Exception)
            //{
            //    Mapper.Initialize(cfg => cfg.AddProfile(new HotelConvertModelProfile()));
            //}            
        }
    }
}
