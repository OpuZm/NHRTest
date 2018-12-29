using System.Collections.Generic;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class CyxmRepository : SqlSugarService, ICyxmRepository
    {
        public CyxmRepository()
        {
        }

        public bool Create(R_Project req)
        {
            using (var db = CreateClient())
            {
                bool result = true;
                var model = db.Insert(req);

                if (model != null)
                {
                    result = false;
                }

                return result;
            }
        }

        public R_Project GetModel(int id)
        {
            using (var db = CreateClient())
            {
                R_Project res = db.Queryable<R_Project>()
                    .Where(p => p.Id == id).FirstOrDefault();
                return res;
            }
        }

        public List<R_Project> GetList()
        {
            using (var db = CreateClient())
            {
                return db.Queryable<R_Project>().ToList();
            }
        }
    }
}