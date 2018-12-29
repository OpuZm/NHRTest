using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IPackageRepository
    {
        int Create(PackageCreateDTO req);

        int Update(PackageCreateDTO req);

        PackageCreateDTO GetModel(int id);

        List<PackageListDTO> GetList(out int total, PackageSearchDTO req);
        List<PackageListDTO> GetList();
        bool DetailCreate(PackageCreateDTO model,List<PackageDetailCreateDTO> req);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
