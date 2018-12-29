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
    /// 订房资料信息接口 Dfzl
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// 新增或更新修改预订（订房资料）信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="info"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        Task<bool> AddNewOrUpdateBookingAsync(string token, BookingInfo info, IUnitOfWork uow = null);

        /// <summary>
        /// 获取预订信息列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<BookingInfo>> GetListByStatusAsync(string token, string status);
    }
}
