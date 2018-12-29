using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Hotel.Model.IRepositories
{
    /// <summary>
    /// 订房明细信息接口 Dfmx
    /// </summary>
    public interface IBookingDetailRepository
    {
        /// <summary>
        /// 新增或更新修改预订详情（订房明细）信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="info"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        Task<bool> AddNewOrUpdateBookingDetailAsync(string token, BookingDetailInfo info, IUnitOfWork uow = null);

        /// <summary>
        /// 根据预订号Id 获取预订明细（排房）信息列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<BookingDetailInfo>> GetListByBookingIdAsync(string token, int bookingId);
    }
}
