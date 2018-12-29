using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IBoxRepository
    {
        bool Create(BoxCreateDTO req);

        bool Update(BoxCreateDTO req);

        BoxCreateDTO GetModel(int id);

        List<BoxListDTO> GetList(out int total, BoxSearchDTO req);

        List<BoxListDTO> GetList();
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
