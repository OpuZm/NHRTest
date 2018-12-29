using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Services.Interfaces
{
    public interface IRestaurantService
    {
        /// <summary>
        /// 加载餐厅工作台信息
        /// </summary>
        /// <returns></returns>
        RestaurantPlatformDTO LoadPlatformInfo(int restaurantId);        
    }
}
