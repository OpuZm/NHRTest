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
    public interface IGroupRepository
    {
        /// <summary>
        /// 根据集团代码获取集团信息
        /// </summary>
        /// <param name="groupCode">集团代码</param>
        /// <returns></returns>
        GroupModel GetByGroupCode(string groupCode);

        /// <summary>
        /// 根据集团代码获取集团信息
        /// </summary>
        /// <param name="groupCode">集团代码</param>
        /// <returns>异步返回集团信息</returns>
        Task<GroupModel> GetByGroupAsync(string groupCode);

        /// <summary>
        /// 根据集团ID获取集团信息
        /// </summary>
        /// <param name="groupId">集团Id</param>
        /// <returns>异步返回集团信息</returns>
        Task<GroupModel> GetByGroupAsync(int groupId);

        /// <summary>
        /// 获取所有集团信息
        /// </summary>
        /// <returns>异步返回集团信息</returns>
        Task<List<GroupModel>> GetAllList();

        /// <summary>
        /// 异步添加一个集团信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> AddModel(GroupModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 异步更新一个集团信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateModel(GroupModel model, IUnitOfWork uow = null);
    }
}
