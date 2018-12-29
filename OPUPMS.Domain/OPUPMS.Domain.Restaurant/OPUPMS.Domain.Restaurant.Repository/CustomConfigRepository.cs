using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class CustomConfigRepository : SqlSugarService, ICustomConfigRepository
    {
        public bool Edit(List<CustomConfigDTO> req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var res = true;
                try
                {
                    if (req.Any())
                    {
                        foreach (var item in req)
                        {
                            db.Update<R_CustomConfig>(new { Sorted = item.Sorted, ModuleName = item.ModuleName, FunctionName=item.FunctionName,Colour=item.Colour }, p => p.Id == item.Id);
                        }
                    }
                }
                catch (Exception e)
                {
                    res = false;
                    throw e;
                }
                return res;
            }
        }

        public List<CustomConfigDTO> GetList(CustomConfigDTO req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var query = db.Queryable<R_CustomConfig>().Where(p=>p.PageModule==req.PageModule);
                if (!string.IsNullOrEmpty(req.ModuleName))
                {
                    query.Where(p => p.ModuleName == req.ModuleName);
                }
                var res = query.Select(p => new CustomConfigDTO()
                {
                    Id= p.Id,
                    FunctionName= p.FunctionName,
                    ModuleName= p.ModuleName,
                    Sorted= p.Sorted,
                    PageModule=p.PageModule,
                    Colour=p.Colour
                }).OrderBy(p=>p.ModuleName).OrderBy(p=>p.Sorted).ToList();
                return res;
            }
        }
    }
}
