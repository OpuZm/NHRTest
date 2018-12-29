using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Repositories.OldRepositories
{
    public interface IOperateLogRepository : IMultiDbRepository<CzjlModel, int>
    {
        Task<int> SaveLog(string dbToken, OperateLogInfo log);
        int SaveLogN(string dbToken, OperateLogInfo log);
    }
}
