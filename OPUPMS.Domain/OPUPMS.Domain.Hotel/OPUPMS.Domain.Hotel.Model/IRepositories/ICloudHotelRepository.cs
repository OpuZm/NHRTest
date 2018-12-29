using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Hotel.Model.CloudModels;
using OPUPMS.Domain.Hotel.Model.ConvertModels;

namespace OPUPMS.Domain.Hotel.Model.IRepositories
{
    /// <summary>
    /// 酒店云后台查询验证接口
    /// </summary>
    public interface ICloudHotelRepository : IMultiDbRepository<DepartmentModel, int>
    {
        /// <summary>
        /// 获取指定的客户信息
        /// </summary>
        /// <param name="hotelCode">酒店代码</param>
        /// <returns>返回云后台的酒店信息</returns>
        CloudHotelInfo GetHotelByCode(string hotelCode);

        /// <summary>
        /// 异步获取指定的客户信息
        /// </summary>
        /// <param name="hotelCode">酒店代码</param>
        /// <returns>异步返回云后台的酒店信息</returns>
        Task<CloudHotelInfo> GetHotelByCodeAsync(string hotelCode);
    }
}
