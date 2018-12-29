using OPUPMS.Domain.Hotel.Model.ConvertModels;
using OPUPMS.Domain.Hotel.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.IServices
{
    /// <summary>
    /// 房态，房间操作管理事务接口
    /// </summary>
    public interface IRoomManageDomainService
    {
        /// <summary>
        /// 获取房态界面信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回房态信息列表</returns>
        RoomListDto LoadRoomList(string token);

        /// <summary>
        /// 异步获取房态信息列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns>异步返回房态信息列表</returns>
        Task<RoomListDto> LoadRoomListAsync(string token);

        /// <summary>
        /// 初始化房间信息-开房操作
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        InitRoomDto InitRoomInfo(string token);

        /// <summary>
        /// 开房操作
        /// </summary>
        /// <param name="token"></param>
        /// <param name="checkinObj">开房信息对象</param>
        /// <returns></returns>
        Task<bool> ApplyCheckinAsync(string token, SumbitCheckInDto checkinObj);

        /// <summary>
        /// 根据房号及客人帐号获取当前房间客人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="roomNo">房号</param>
        /// <param name="guestId">客人帐号</param>
        /// <returns></returns>
        Task<GuestDataInfo> GetGuestDetailsByRoomNoAsync(string token, string roomNo, int guestId);

        /// <summary>
        /// 根据客人帐号Id 获取关联的联房信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="guestId">客人帐号</param>
        /// <returns>异步返回联房信息列表</returns>
        Task<List<LinkRoomDto>> GetLinkRoomListAsync(string token, int guestId);
    }
}
