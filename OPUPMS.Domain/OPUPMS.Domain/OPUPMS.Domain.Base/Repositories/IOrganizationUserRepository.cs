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
    public interface IOrganizationUserRepository
    {
        /// <summary>
        /// 保存组织用户关系
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> SaveModel(OrganizationUserModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 移除组织用户关系
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> DelModel(OrganizationUserModel model, IUnitOfWork uow = null);
        
        /// <summary>
        /// 获取组织用户关系
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<List<OrganizationUserModel>> GetList(int groupId, int userId);
    }
}
