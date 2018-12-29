using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Infrastructure.Dapper;
using Smooth.IoC.UnitOfWork;
using Dapper;

namespace OPUPMS.Domain.Repository
{
    public class SystemCodeRepository : MultiDbRepository<XtdmModel, string>, ISystemCodeRepository
    {
        public SystemCodeRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        static readonly string GetByMutliTypesSql = @"SELECT Xtdmlx00, Xtdmdm00, Xtdmmc00, Xtdmbzs0 FROM Xtdm WHERE xtdmlx00 IN @Types";

        public virtual List<SystemCodeInfo> GetCodeListByMutliTypes(string token, string[] paras)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = session.Query<XtdmModel>(GetByMutliTypesSql, new { Types = paras });

                return ConvertModelList(result.ToList());
            }
        }

        public virtual async Task<List<SystemCodeInfo>> GetCodeListByMutliTypesAsync(string token, string[] paras)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<XtdmModel>(GetByMutliTypesSql, new { Types = paras });

                return ConvertModelList(result.ToList());
            }
        }

        protected List<SystemCodeInfo> ConvertModelList(List<XtdmModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<SystemCodeInfo> list = new List<SystemCodeInfo>();
            foreach (XtdmModel model in sourceList)
            {
                //调用AutoMapper映射
                SystemCodeInfo sysCodeInfo = AutoMapper.Mapper.Map<XtdmModel, SystemCodeInfo>(model);
                
                list.Add(sysCodeInfo);
            }

            return list;
        }
    }
}
