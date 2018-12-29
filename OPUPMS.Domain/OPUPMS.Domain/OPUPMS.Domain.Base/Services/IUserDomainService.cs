using System.Threading.Tasks;
using OPUPMS.Domain.Base.Dtos;

namespace OPUPMS.Domain.Base.Services
{
    public interface IUserDomainService : IDomainService
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<UserDto> CheckLoginAsync(LoginInputDto info);
    }
}
