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
    /// 部门信息接口
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        /// 根据部门代码获取部门信息
        /// </summary>
        /// <param name="departmentCode">部门代码</param>
        /// <returns></returns>
        DepartmentModel GetByDepartmentCode(string departmentCode);

        /// <summary>
        /// 根据部门代码获取部门信息
        /// </summary>
        /// <param name="departmentCode">部门代码</param>
        /// <returns>异步返回部门信息</returns>
        Task<DepartmentModel> GetByDepartmentCodeAsync(string departmentCode);

        /// <summary>
        /// 根据部门ID获取部门信息
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <returns>异步返回部门信息</returns>
        Task<DepartmentModel> GetByDepartmentIdAsync(int departmentId);

        /// <summary>
        /// 添加一个新部门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddNewDepartment(DepartmentModel model);

        /// <summary>
        /// 异步更新一个部门信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateDepartment(DepartmentModel model, IUnitOfWork uow = null);
    }
}
