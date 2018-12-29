using System;
using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IRestaurantCategoryRepository
    {
        bool Create(RestaurantCategoryCreateDTO req);

        bool Update(RestaurantCategoryCreateDTO req);

        RestaurantCategoryCreateDTO GetModel(int id);

        List<RestaurantCategoryListDTO> GetList(out int total, RestaurantCategorySearchDTO req);
        /// <summary>
        /// 过滤餐厅下设置关联类别的菜品
        /// </summary>
        /// <param name="req"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        List<ProjectAndDetailListDTO> FilterRestaurantCategorys(List<ProjectAndDetailListDTO> req, int restaurantId);
        /// <summary>
        /// 过滤餐厅下设置关联类别
        /// </summary>
        /// <param name="req"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        List<AllCategoryListDTO> FilterRestaurantCategorys(List<AllCategoryListDTO> req, int restaurantId);
    }
}
