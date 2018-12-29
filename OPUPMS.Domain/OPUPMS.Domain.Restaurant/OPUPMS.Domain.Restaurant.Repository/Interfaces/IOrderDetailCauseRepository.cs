using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IOrderDetailCauseRepository
    {
        bool Edit(OrderDetailCauseDTO req);
        OrderDetailCauseDTO GetModel(int id);
        List<OrderDetailCauseListDTO> GetList(out int total, OrderDetailCauseSearch req);
        bool Delete(List<int> ids);
        List<OrderDetailCauseListDTO> GetAllList();
    }
}
