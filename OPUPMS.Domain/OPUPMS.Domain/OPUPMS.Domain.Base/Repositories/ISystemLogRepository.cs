using OPUPMS.Domain.Base.Models;
using OPUPMS.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Base.Repositories
{
    /// <summary>
    /// 系统操作记录接口
    /// </summary>
    public interface ISystemLogRepository
    {
        /// <summary>
        /// 保存系统操作记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> SaveModel(SystemLogModel model, IUnitOfWork uow = null);
        
    }
}
