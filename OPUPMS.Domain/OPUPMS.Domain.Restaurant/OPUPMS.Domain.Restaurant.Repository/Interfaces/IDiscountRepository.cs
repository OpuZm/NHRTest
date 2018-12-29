using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IDiscountRepository
    {
        bool Create(DiscountCreateDTO req);

        bool Update(DiscountCreateDTO req);

        DiscountCreateDTO GetModel(int id);

        List<DiscountDTO> GetList(out int total, DiscountSearchDTO req);

        List<DiscountDTO> GetList(out int total, PayDiscountSearchDTO req);

        List<DiscountDTO> GetList();

        /// <summary>
        /// 获取方案折信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        List<SchemeDiscountDTO> GetSchemeDiscountList(SchemeDiscountSearchDTO req);
        List<SchemeDiscountDetailDTO> GetSchemeDetailListById(int discountId);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
    }
}
