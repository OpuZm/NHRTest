using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IPayMethodRepository
    {
        bool Create(PayMethodCreateDTO req);
        List<PayMethodListDTO> GetList(out int total, PayMethodSearchDTO req);
        bool Update(PayMethodCreateDTO req);
        List<PayMethodListDTO> GetParents();
        PayMethodCreateDTO GetModel(int id);
        bool Delete(int id);
        List<PayMethodListDTO> GetList();
        List<int> GetCheckOutRemovePayType();
    }
}
