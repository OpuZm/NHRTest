using Dapper;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Infrastructure.Dapper;
using Smooth.IoC.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.FastCrud;

namespace OPUPMS.Domain.Repository
{
    public class OperateLogRepository : MultiDbRepository<CzjlModel, int>, IOperateLogRepository
    {
        public OperateLogRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        public async Task<int> SaveLog(string dbToken, OperateLogInfo log)
        {
            //return 0;
            CzjlModel model = ConvertToModel(log);

            var result = await SaveOrUpdateAsync<ISession>(dbToken, model);
            return result;
        }

        public int SaveLogN(string dbToken, OperateLogInfo log)
        {
            //return 0;
            CzjlModel model = ConvertToModel(log);

            var result =  SaveOrUpdate<ISession>(dbToken, model);
            return result;
        }

        /// <summary>
        /// 自动转换映射
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private CzjlModel ConvertToModel(OperateLogInfo log)
        {
            CzjlModel model = AutoMapper.Mapper.Map<OperateLogInfo, CzjlModel>(log);
            return model;
        }
    }
}
