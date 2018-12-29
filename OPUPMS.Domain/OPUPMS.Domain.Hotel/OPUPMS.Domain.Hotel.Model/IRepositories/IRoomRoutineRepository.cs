using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model.ConvertModels;

namespace OPUPMS.Domain.Hotel.Model.IRepositories
{
    /// <summary>
    /// 房间事务信息接口 (Fhsw)
    /// </summary>
    public interface IRoomRoutineRepository
    {
        /// <summary>
        /// 异步根据日期及状态，查询关联房务的房号信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="status"></param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>异步返回房务的指定列(房号)信息</returns>
        Task<List<RoomRoutineInfo>> GetRoomNoListByStatusAndDateAsync(string token, string[] status, DateTime beginDate, DateTime endDate);

    }
}
