using OPUPMS.Domain.Hotel.Model.ConvertModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.IRepositories
{
    /// <summary>
    /// 客人事务信息接口 (Krsw)
    /// </summary>
    public interface IGuestRoutineRepository
    {
        /// <summary>
        /// 根据客人帐号Id查询关联客人事务信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="guestId"></param>
        /// <returns>异步返回客人事务信息信息</returns>
        Task<List<GuestRoutineInfo>> GetListByGuestIdAsync(string token, int guestId);

    }
}
