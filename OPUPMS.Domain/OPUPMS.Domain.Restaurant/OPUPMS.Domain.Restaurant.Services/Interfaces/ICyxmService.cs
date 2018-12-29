using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Domain.Restaurant.Services
{
    public interface ICyxmService
    {
        bool Create(R_Project req);

        R_Project GetModel(int id);

        List<R_Project> GetList();
    }
}
