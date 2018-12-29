using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Domain.Base;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common.Net;
using OPUPMS.Domain.Base.Services;
using OPUPMS.Domain.Base.Models;

namespace OPUPMS.Domain.Services
{
    public class UserDomainService : IUserDomainService
    {
        public UserDomainService(IGroupRepository groupRepository, IUserRepository userRepository, ISystemLogRepository logRepository)
        {
            GroupRepository = groupRepository;
            UserRepository = userRepository;
            SysLogRepository = logRepository;
        }
        
        public IUserRepository UserRepository { get; private set; }
        public IGroupRepository GroupRepository { get; private set; }

        public ISystemLogRepository SysLogRepository { get; private set; }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<UserDto> CheckLoginAsync(LoginInputDto info)
        {
            var group = await GroupRepository.GetByGroupAsync(info.GroupCode);
            UserDto user = new UserDto();
            //if (hotel == null)
            //{
            //    user.State = LoginState.InvalidHotelCode;
            //    return user;
            //}

            //var verifyUser = await UserRepository.GetByUserNameAsync(hotel.HotelId2.ToString(), info.UserName);
            var verifyUser = await UserRepository.GetByUserCodeAndGroupIdAsync(info.UserCode, info.GroupCode.ToInt());
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
            user.RoleId = "";//verifyUser.RoleId;
            user.GroupCode = null;//hotel.HotelId2.ToString();//指定数据库连接Token
            
            SystemLogModel model = new SystemLogModel();
            model.TypeId = 10;//系统登录
            model.OperateModuleType = 10; //登录
            model.OperatedTime = DateTime.Now;
            model.Content = "于" + DateTime.Now.ToString() + "登陆，电脑名称-" + Net.Host + "，登陆IP地址-" + Net.Ip; ;
            model.Title = "系统登录";
            model.OperateId = verifyUser.Id;
            model.Operator = verifyUser.UserName;
            model.OrganizationId = 0;
            model.OrganizationType = 1;

            //var logResult = UserLogRepository.SaveLog(hotel.HotelId2.ToString(), logInfo);//写入操作日志记录
            var logResult = SysLogRepository.SaveModel(model);//写入操作日志记录

            return user;
        }
    }
}
