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
    /// 区域代码信息接口
    /// </summary>
    public interface IAreaCodeRepository
    {
        /// <summary>
        /// 根据区域代码获取区域代码信息
        /// </summary>
        /// <param name="areaCode">区域代码</param>
        /// <returns></returns>
        AreaCodeModel GetByAreaCode(string areaCode);

        /// <summary>
        /// 根据区域代码获取区域代码信息
        /// </summary>
        /// <param name="areaCode">区域代码</param>
        /// <returns>异步返回区域信息</returns>
        Task<AreaCodeModel> GetByAreaCodeAsync(string areaCode);

        /// <summary>
        /// 根据区域代码ID获取区域代码信息
        /// </summary>
        /// <param name="areaCodeId">区域代码Id</param>
        /// <returns>异步返回区域代码信息</returns>
        Task<AreaCodeModel> GetByAreaCodeIdAsync(int areaCodeId);

        /// <summary>
        /// 添加一个新区域代码信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddNewAreaCode(AreaCodeModel model);

        /// <summary>
        /// 异步更新一个区域代码信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateAreaCode(AreaCodeModel model, IUnitOfWork uow = null);
    }
}
