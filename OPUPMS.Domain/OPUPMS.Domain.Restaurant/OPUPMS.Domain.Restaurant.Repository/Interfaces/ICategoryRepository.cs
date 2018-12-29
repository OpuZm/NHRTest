using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface ICategoryRepository
    {
        bool Create(CategoryCreateDTO req);

        bool Update(CategoryCreateDTO req);

        CategoryCreateDTO GetModel(int id);

        List<CategoryListDTO> GetList(out int total, CategorySearchDTO req);

        List<CategoryListDTO> GetList(int pid);

        List<AllCategoryListDTO> GetAllCategoryList(bool includeDelete= false);

        List<CategoryListDTO> GetChildList(bool includeDelete = false);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
