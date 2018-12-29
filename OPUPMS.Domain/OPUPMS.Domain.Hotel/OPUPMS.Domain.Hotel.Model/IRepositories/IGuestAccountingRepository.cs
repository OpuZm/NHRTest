using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using OPUPMS.Infrastructure.Dapper;

namespace OPUPMS.Domain.Hotel.Model.IRepositories
{
    /// <summary>
    /// 客人账务信息接口 (Krzw)
    /// </summary>
    public interface IGuestAccountingRepository : IMultiDbRepository<KrzwModel, int>
    {
        /// <summary>
        /// 根据客人Id数组获取客人账务信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回客人账务信息列表</returns>
        List<GuestAccountingInfo> GetListByGuestIds(string token, int[] idArray);

        /// <summary>
        /// 异步根据客人Id数组获取客人账务信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>异步返回客人账务信息列表</returns>
        Task<List<GuestAccountingInfo>> GetListByGuestIdsAsync(string token, int[] idArray);

        /// <summary>
        /// 添加或更新修改客人账务信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<bool> AddNewOrUpdateAccountingInfo(string token, GuestAccountingInfo info);
    }
}
