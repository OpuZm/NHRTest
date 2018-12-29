using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class AutoAdditionRepository : SqlSugarService, IAutoAdditionRepository
    {
        public bool Create(AutoAdditionCreateDTO req)
        {
            return true;
        }

        public List<AutoAdditionListDTO> GetList(out int total, AutoAdditionSearchDTO req)
        {
            total = 1;
            return null;
        }

        public AutoAdditionCreateDTO GetModel(int id)
        {
            return null;
        }

        public bool IsDelete(int id)
        {
            return true;
        }

        public bool Update(AutoAdditionCreateDTO req)
        {
            return true;
        }
    }
}
