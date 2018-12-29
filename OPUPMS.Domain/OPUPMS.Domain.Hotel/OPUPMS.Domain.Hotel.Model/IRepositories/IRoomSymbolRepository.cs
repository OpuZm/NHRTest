using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Hotel.Model.IRepositories
{
    /// <summary>
    /// 房号代码信息接口 (Fhdm)
    /// </summary>
    public interface IRoomSymbolRepository : IMultiDbRepository<FhdmModel, string>
    {
        /// <summary>
        /// 获取所有房间房号代码信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回房号代码列表</returns>
        List<RoomSymbolInfo> LoadRoomSymbolList(string token);

        /// <summary>
        /// 异步获取所有房间房号代码信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>异步返回房号代码列表</returns>
        Task<List<RoomSymbolInfo>> LoadRoomSymbolListAsync(string token);

        /// <summary>
        /// 根据房号获取房间信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="roomNo">房号</param>
        /// <returns>异步返回指定房间信息</returns>
        Task<RoomSymbolInfo> GetRoomInfoByNoAsync(string token, string roomNo);

        /// <summary>
        /// 根据相关房号获取房间信息列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="roomNoArray">房号数组</param>
        /// <returns>异步返回指定房间信息列表</returns>
        Task<List<RoomSymbolInfo>> GetRoomInfoListByNoAsync(string token, string[] roomNoArray);


        /// <summary>
        /// 更新指定房间信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="room"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        Task<bool> UpdateRoomInfoByNoAsync(string token, RoomSymbolInfo room, IUnitOfWork uow = null);
    }
}
