using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IRestaurantRepository
    {
        bool Create(RestaurantCreateDTO req);
        bool Update(RestaurantCreateDTO req);

        RestaurantCreateDTO GetModel(int id);

        List<RestaurantListDTO> GetList(out int total, RestaurantSearchDTO req);
        List<RestaurantListDTO> GetList();

        /// <summary>
        /// 根据公司Id 获取关联餐厅信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        List<RestaurantListDTO> GetList(int companyId);

        /// <summary>
        /// 根据Id 列表查询餐厅信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<R_Restaurant> GetList(string[] ids);
        List<RestaurantListDTO> FilterCompanyRestaurant(List<RestaurantListDTO> req, int companyId);
    }
}
