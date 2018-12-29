using Starts2000.DependencyInjection;
using Starts2000.DependencyInjection.DryIoc;

namespace OPUPMS.Domain.Hotel.Services.IocManagerMoudles
{
    public class HotelDomainServiceIocManagerModule : IocManagerModule
    {
        public override void Load()
        {
            //Kernel.Bind<IHotelLoginService>().To<HotelLoginService>();
            //Kernel.Bind<IMenuManageService<HotelMenuDto>>().To<MenuManageService>();
            //Kernel.Bind<IRoomManageService>().To<RoomManageService>();

            IocManager.RegisterAssemblyTransient(
                typeof(HotelDomainServiceIocManagerModule).Assembly,
                type => (type.IsClass && type.IsPublic && !type.IsAbstract) &&
                        type.FullName.EndsWith("DomainService") &&
                        type.GetInterfaces().Length > 0);
        }
    }
}
