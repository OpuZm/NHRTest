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
    /// 角色表 
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddRole(RoleModel model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        Task<bool> UpdateRole(RoleModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleByIdAsync(int Id,IUnitOfWork uow=null);

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoleModel GetRoleByID(int id);

        /// <summary>
        /// 查找所有列表
        /// </summary>
        /// <returns></returns>
        Task<List<RoleModel>> GetRoleAll();

    }
}
