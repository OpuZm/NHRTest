using System;
using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IAutoAdditionRepository
    {
        bool Create(AutoAdditionCreateDTO req);

        bool Update(AutoAdditionCreateDTO req);

        AutoAdditionCreateDTO GetModel(int id);

        List<AutoAdditionListDTO> GetList(out int total, AutoAdditionSearchDTO req);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
