using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IOrderRecordRepository
    {
        bool Create(OrderRecordDTO req);

        bool Update(OrderRecordDTO req);

        OrderRecordDetailDTO GetModel(int id);

        List<OrderRecordDetailDTO> GetList(OrderRecordSearchDTO req, out int total);
        List<OrderRecordDetailDTO> GetList(int orderId, int tableId = 0);
    }
}
