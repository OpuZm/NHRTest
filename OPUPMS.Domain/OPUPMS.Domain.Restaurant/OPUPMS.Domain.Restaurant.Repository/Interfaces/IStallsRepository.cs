using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IStallsRepository
    {
        bool Create(StallsCreateDTO req);

        bool Update(StallsCreateDTO req);

        StallsCreateDTO GetModel(int id, int? billType = null);

        List<StallsListDTO> GetList(out int total, StallsSearchDTO req);
        List<StallsListDTO> GetList();
        bool ProjectStallSubmit(int id, int billType, List<R_ProjectStall> req);
        bool ProjectStallSubmitNew(int id, List<R_ProjectStall> req);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
