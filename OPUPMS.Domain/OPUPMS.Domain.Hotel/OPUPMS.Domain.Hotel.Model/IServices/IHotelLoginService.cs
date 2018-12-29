using System.Threading.Tasks;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Services;

namespace OPUPMS.Domain.Hotel.Model.IServices
{
    /// <summary>
    /// 云酒店登录验证处理接口
    /// </summary>
    public interface IHotelLoginDomainService : IDomainService
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<UserDto> CheckLoginAsync(LoginInputDto info);
    }
}
