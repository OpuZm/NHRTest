using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface ICustomConfigRepository
    {
        bool Edit(List<CustomConfigDTO> req);
        List<CustomConfigDTO> GetList(CustomConfigDTO req);
    }
}
