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
    ///权限表 接口 
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddPermission(PermissionModel model);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdatePermission(PermissionModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据权限代码获取权限
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        PermissionModel GetPermissionByCode(string code);

    


        /// <summary>
        /// 根据权限父级Id  获取权限列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<PermissionModel>> GetPermissionByParentId(int parentId);

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionModel>> GetPermissionAll();

    }
}
