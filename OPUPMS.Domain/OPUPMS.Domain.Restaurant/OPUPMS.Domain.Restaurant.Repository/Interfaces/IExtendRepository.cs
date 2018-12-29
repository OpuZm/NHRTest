using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IExtendRepository
    {
        bool Create(ExtendCreateDTO req);

        bool Update(ExtendCreateDTO req);

        ExtendCreateDTO GetModel(int id);

        List<ExtendListDTO> GetList(out int total, ExtendSearchDTO req);
        List<ExtendListDTO> GetList(int type = 0);
        Dictionary<int, string> GetCategory();
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);

        #region [扩展类别]
        bool CreateExtendType(ExtendTypeCreateDTO req);

        bool UpdateExtendType(ExtendTypeCreateDTO req);

        ExtendTypeCreateDTO GetExtendTypeModel(int id);

        List<ExtendTypeListDTO> GetExtendTypeList(out int total, ExtendTypeSearchDTO req);

        List<ExtendTypeListDTO> GetExtendTypeList(int type = 0);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDeleteExtendType(int id);
        #endregion
    }
}
