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
    /// 角色权限
    /// </summary>
    public interface IRolePermissionRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddRolePermission(RolePermissionModel model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateRolePermission(RolePermissionModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRolePermissionById(int id, IUnitOfWork uow = null);

        /// <summary>
        /// 根据主键标识Id查找角色权限实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RolePermissionModel GetRolePermissionByID(int id);

        /// <summary>
        /// 根据角色Id查找角色权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<RolePermissionModel>> GetRolePermissionByRoleId(int roleId);

        /// <summary>
        /// 根据权限Id查找角色权限列表
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<List<RolePermissionModel>> GetRolePermissionByPermissionId(int permissionId);

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <returns></returns>
        Task<List<RolePermissionModel>> GetRolePermissionAll();

    }
}
