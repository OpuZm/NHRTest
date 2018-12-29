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
    /// 用户信息接口
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 根据用户代码，集团id获取用户信息
        /// </summary>
        /// <param name="userCode">用户代码</param>
        /// <param name="groupId">集团Id</param>
        /// <returns></returns>
        UserModel GetByUserCodeAndGroupId(string userCode, int groupId);

        /// <summary>
        /// 根据用户代码，集团id获取用户信息
        /// </summary>
        /// <param name="userCode">用户代码</param>
        /// <param name="groupId">集团Id</param>
        /// <returns>异步返回用户信息</returns>
        Task<UserModel> GetByUserCodeAndGroupIdAsync(string userCode, int groupId);

        /// <summary>
        /// 添加一个新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> AddModel(UserModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 异步更新一个用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateModel(UserModel model, IUnitOfWork uow = null);
    }
}
