using System;
using System.Threading.Tasks;
using OPUPMS.Domain.Base;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Domain.Hotel.Model.IServices;
using OPUPMS.Infrastructure.Common.Net;

namespace OPUPMS.Domain.Hotel.Services
{
    public class HotelLoginDomainService : IHotelLoginDomainService
    {
        public HotelLoginDomainService(ICloudHotelRepository cloudRepository, IUserRepository_Old repository, IOperateLogRepository logRepository)
        {
            CloudHotelRepository = cloudRepository;
            UserRepository = repository;
            UserLogRepository = logRepository;
        }
        
        public IUserRepository_Old UserRepository { get; private set; }
        public ICloudHotelRepository CloudHotelRepository { get; private set; }

        public IOperateLogRepository UserLogRepository { get; private set; }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<UserDto> CheckLoginAsync(LoginInputDto info)
        {
            //var hotel = await CloudHotelRepository.GetHotelByCodeAsync(info.HotelCode);
            UserDto user = new UserDto();
            //if (hotel == null)
            //{
            //    user.State = LoginState.InvalidHotelCode;
            //    return user;
            //}

            //var verifyUser = await UserRepository.GetByUserNameAsync(hotel.HotelId2.ToString(), info.UserName);
            var verifyUser = await UserRepository.GetByUserNameAsync(null, info.UserName);
            if(verifyUser == null)
            {
                user.State = LoginState.InvalidAccount;
                return user;
            }

            //if (user.UserPwd != info.UserPwd)
            //{
            //    user.State = LoginState.InvalidPassword;
            //    return user;
            //}

            user.State = LoginState.Successed;
            user.UserCode = verifyUser.UserCode;
            user.UserName = verifyUser.UserName;
            user.ConnectionString = null;//hotel.ConnectionString;
            user.RoleId = verifyUser.RoleId;
            user.GroupCode = null;//hotel.HotelId2.ToString();//指定数据库连接Token
            
            OperateLogInfo logInfo = new OperateLogInfo();
            logInfo.OperateType = "Z_2";
            logInfo.OperateTime = DateTime.Now;
            logInfo.UserCode = user.UserCode;
            logInfo.Remark = "于" + DateTime.Now.ToString() + "登陆，电脑名称-" + Net.Host + "，登陆IP地址-" + Net.Ip; ;
            logInfo.OperateRemark = "登录";
            logInfo.ActionName = "系统登录-" + user.UserName;

            //var logResult = UserLogRepository.SaveLog(hotel.HotelId2.ToString(), logInfo);//写入操作日志记录
            var logResult = UserLogRepository.SaveLog(null, logInfo);//写入操作日志记录

            return user;
        }
    }
}
