using OPUPMS.Domain.Restaurant.Services.Interfaces;
using Starts2000.DependencyInjection;
using Starts2000.DependencyInjection.DryIoc;

namespace OPUPMS.Domain.Restaurant.Services.IocManagerMoudles
{
    public class RestaurantDomainServiceIocManagerModule : IocManagerModule
    {
        public override void Load()
        {
            IocManager.RegisterAssemblyTransient(
                typeof(RestaurantDomainServiceIocManagerModule).Assembly,
                type => (type.IsClass && type.IsPublic && !type.IsAbstract) &&
                        type.FullName.EndsWith("DomainService") &&
                        type.GetInterfaces().Length > 0);

            IocManager
                .RegisterTransient<ICyxmService, CyxmService>()
                .RegisterTransient<IUserService, UserService>()
                .RegisterTransient<ITableService, TableService>()
                .RegisterTransient<IOrderService, OrderService>()
                .RegisterTransient<IRestaurantService, RestaurantService>()
                .RegisterTransient<ICheckOutService, CheckOutService>()
                .RegisterTransient<IPrintService, PrintService>();
        }
    }
}
