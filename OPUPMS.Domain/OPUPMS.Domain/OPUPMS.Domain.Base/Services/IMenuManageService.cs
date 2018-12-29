using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Services
{
    public interface IMenuManageDomainService<T> : IDomainService
        where T : class
    {
        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="token">指定数据库连接Key，用于多数据库，如若单客户请传Null</param>
        /// <param name="userId">操作用户Id</param>
        /// <returns>返回菜单列表</returns>
        List<T> GetMenuList(string token, string userId);

        /// <summary>
        /// 异步获取系统菜单
        /// </summary>
        /// <param name="token">指定数据库连接Key，用于多数据库，如若单客户请传Null</param>
        /// <param name="userId">操作用户Id</param>
        /// <returns>异步返回菜单列表</returns>
        Task<List<T>> GetMenuListAsync(string token, string userId);
    }
}
