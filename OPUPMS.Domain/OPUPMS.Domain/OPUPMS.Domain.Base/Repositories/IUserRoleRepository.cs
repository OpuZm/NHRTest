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
    /// 用户角色信息接口
    /// </summary>
    public interface IUserRoleRepository
    {
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> SaveModel(UserRoleModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 移除用户角色关系
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> DelModel(int id, IUnitOfWork uow = null);

        /// <summary>
        /// 获取用户角色关系
        /// 角色ID 与 用户ID 两个参数，至少必须传其中一个或者同时传两个来查询数据
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<List<UserRoleModel>> GetList(int? roleId, int? userId);
    }
}
