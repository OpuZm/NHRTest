using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;

namespace OPUPMS.Domain.Base.Repositories.OldRepositories
{
    public interface IUserRepository_Old : IMultiDbRepository<CzdmModel, int>
    {
        UserInfo GetByUserName(string token, string userName,int companyId);

        Task<UserInfo> GetByUserNameAsync(string token, string userName);

        /// <summary>
        /// 根据UserId 获取user 信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserInfo GetByUserId(int userId);

        /// <summary>
        /// 根据Id 列表取相关用户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        List<UserInfo> GetByUserIds(List<int> userIds);
        List<UserInfo> GetByUsersSql(string czdmqxzb);
        bool UpdatePassWord(int id, string passWord);
    }
}
