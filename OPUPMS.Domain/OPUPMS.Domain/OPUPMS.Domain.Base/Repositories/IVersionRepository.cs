using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Base.Repositories
{
    /// <summary>
    /// 版本记录表 
    /// </summary>
    public interface IVersionRepository
    {

        /// <summary>
        /// 新增版本记录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddVersionsAsync(VersionModel model);

        /// <summary>
        /// 修改版本信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateVersionsAsync(VersionModel model, IUnitOfWork uow = null);

 

        /// <summary>
        /// 根据代码版本标识，获取版本信息实体
        /// </summary>
        /// <param name="codeVersion">代码版本标识 </param>
        /// <returns></returns>
        VersionModel GetVersionByCodeVersion(string codeVersion);

        /// <summary>
        /// 获取所有版本信息
        /// </summary>
        /// <returns></returns>
        Task<List<VersionModel>> GetVersionAllAsync();


    }
}
