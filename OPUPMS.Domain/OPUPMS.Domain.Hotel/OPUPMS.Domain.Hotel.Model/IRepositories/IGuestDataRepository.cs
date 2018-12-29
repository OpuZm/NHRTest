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
    /// 客人资料信息接口 (Krzl)
    /// </summary>
    public interface IGuestDataRepository : IMultiDbRepository<KrzlModel, int>
    {
        /// <summary>
        /// 根据状态获取客人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回客人信息</returns>
        List<GuestDataInfo> GetListByStatus(string token, string status);

        /// <summary>
        /// 异步根据状态获取客人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>异步返回客人所有信息</returns>
        Task<List<GuestDataInfo>> GetListByStatusAsync(string token, string status);

        /// <summary>
        /// 异步根据日期及状态（已入住或预订），查询客人关联房号信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="status"></param>
        /// <param name="fromDate">入住日期</param>
        /// <param name="toDate">离店日期</param>
        /// <returns>异步返回客人的指定列(房号)信息</returns>
        Task<List<GuestDataInfo>> GetRoomNoListByStatusAndDateAsync(string token, string[] status, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// 增加客人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="guest"></param>
        /// <returns></returns>
        bool AddNewGuest(string token, GuestDataInfo guest, IUnitOfWork uow = null);

        /// <summary>
        /// 异步增加客人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="guest"></param>
        /// <returns></returns>
        Task<bool> AddNewGuestAsync(string token, GuestDataInfo guest, IUnitOfWork uow = null);
        
        /// <summary>
        /// 更新客人资料信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="guest"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        Task<bool> UpdateGuestAsync(string token, GuestDataInfo guest, IUnitOfWork uow = null);

        /// <summary>
        /// 根据客人Id 获取其关联的联房的客人信息列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="guestId">客人帐号</param>
        /// <returns></returns>
        Task<List<GuestDataInfo>> GetLinkRoomListByGuestIdAsync(string token, int guestId);
    }
}
