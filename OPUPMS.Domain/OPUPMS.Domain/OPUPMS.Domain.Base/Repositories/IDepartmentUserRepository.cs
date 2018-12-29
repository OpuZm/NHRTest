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
    /// 部门用户关系信息接口
    /// </summary>
    public interface IDepartmentUserRepository
    {
        /// <summary>
        /// 根据部门ID获取部门用户关系信息
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        DepartmentUserModel GetByDepartmentId(int departmentId);

        /// <summary>
        /// 根据部门ID获取部门用户关系信息
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>异步返回部门用户关系信息</returns>
        Task<DepartmentUserModel> GetByDepartmentIdAsync(int departmentId);

        /// <summary>
        /// 根据用户ID获取部门用户关系信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        DepartmentUserModel GetByUserId(int userId);

        /// <summary>
        /// 根据用户ID获取部门用户关系信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>异步返回部门用户关系信息</returns>
        Task<DepartmentUserModel> GetByUserIdAsync(int userId);

        /// <summary>
        /// 根据部门用户关系ID获取部门用户关系信息
        /// </summary>
        /// <param name="departmentUserId">部门用户关系Id</param>
        /// <returns>异步返回部门用户关系信息</returns>
        Task<DepartmentUserModel> GetByDepartmentUserIdAsync(int departmentUserId);

        /// <summary>
        /// 添加一个新部门用户关系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddNewDepartmentUser(DepartmentUserModel model);

        /// <summary>
        /// 异步更新一个部门用户关系信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateDepartmentUser(DepartmentUserModel model, IUnitOfWork uow = null);
    }
}
