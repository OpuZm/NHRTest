using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IAreaRepository
    {
        bool Create(AreaCreateDTO req);

        bool Update(AreaCreateDTO req);

        AreaCreateDTO GetModel(int id);

        List<AreaListDTO> GetList(out int total, AreaSearchDTO req);

        List<AreaListDTO> GetList(int restaurantId);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
        bool UpdateWeixinPrint(WeixinPrintDTO req);
        WeixinPrintDTO GetWeixinPrint(int id);
        List<WeixinPrintListDTO> GetWeixinPrints(WeixinPrintSearchDTO req, out int total);
        bool IsDeleteWeixinPrint(int id);
    }
}
