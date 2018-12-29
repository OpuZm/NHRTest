using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Base.Repositories
{
    /// <summary>
    /// 区域记录信息接口
    /// </summary>
    public interface IAreaLogRepository
    {
        /// <summary>
        /// 根据区域记录ID获取区域记录信息
        /// </summary>
        /// <param name="areaLogId">区域记录Id</param>
        /// <returns>异步返回区域记录信息</returns>
        Task<AreaLogModel> GetByAreaLogIdAsync(int areaLogId);

        /// <summary>
        /// 添加一个新区域记录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddNewAreaLog(AreaLogModel model);

        /// <summary>
        /// 异步更新一个区域记录信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateAreaLog(AreaLogModel model, IUnitOfWork uow = null);
    }
}
