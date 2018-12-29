using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IMarketRepository
    {
        bool Create(MarketCreateDTO req);

        bool Update(MarketCreateDTO req);

        MarketCreateDTO GetModel(int id);

        List<MarketListDTO> GetList(out int total, MarketSearchDTO req);
        List<MarketListDTO> GetList(int restaurantId=0);

        /// <summary>
        /// 根据餐厅Id 列表获取相关餐厅分市信息
        /// </summary>
        /// <param name="resIdList"></param>
        /// <returns></returns>
        List<MarketListDTO> GetList(List<int> resIdList);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
