using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUPMS.Domain.Base;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Net;
using OPUPMS.Infrastructure.Common.Security;
using OPUPMS.Infrastructure.Dapper;

namespace OPUPMS.Domain.Restaurant.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository_Old _userRepository;
        readonly IOperateLogRepository _userLogRepository;
        readonly IRestaurantRepository _resRepository;
        public UserService(
            IUserRepository_Old repository,
            IOperateLogRepository logRepository,
            IRestaurantRepository restaurantRepository)
        {
            _userRepository = repository;
            _userLogRepository = logRepository;
            _resRepository = restaurantRepository;
        }

        public async Task<UserDto> CheckLogin(string userName, string password,int companyId=0)
        {
            UserDto user = GetUserInfo(new VerifyUserDTO() { UserName = userName, UserPwd = password,CompanyId=companyId });
            if (user == null || user.UserId <= 0)
                return user;

            //OperateLogInfo logInfo = new OperateLogInfo
            //{
            //    OperateType = "Z_2",
            //    OperateTime = DateTime.Now,
            //    UserCode = user.UserCode,
            //    Remark = "于" + DateTime.Now.ToString() +
            //        "登陆，电脑名称-" + Net.Host + "，登陆IP地址-" + Net.Ip
            //};

            //logInfo.OperateRemark = "登录";
            //logInfo.ActionName = "系统登录-" + user.UserName;

            //var logResult = await _userLogRepository.SaveLog("", logInfo);//写入操作日志记录

            return user;
        }

        public List<UserDto> GetSales()
        {
            var res = _userRepository.GetByUsersSql("ZZ").Select(p=> new UserDto()
            {
                UserId=p.UserId,
                UserName=p.UserName,
                UserCode=p.UserCode
            }).ToList();
            return res;
        }

        public UserDto GetUserInfo(VerifyUserDTO verifyUserDTO)
        {
            UserDto user = new UserDto();
            
            UserInfo verifyUser = null;

            if (verifyUserDTO.UserId > 0)
                verifyUser = _userRepository.GetByUserId(verifyUserDTO.UserId);
            else
                verifyUser= _userRepository.GetByUserName("", verifyUserDTO.UserName,verifyUserDTO.CompanyId);

            if (verifyUser == null)
            {
                user.State = LoginState.InvalidAccount;
                return user;
            }

            if (verifyUserDTO.UserPwd != null && DESEncrypt.GetMD5(verifyUserDTO.UserPwd) != verifyUser.UserPwd)
            {
                user.State = LoginState.InvalidPassword;
                return user;
            }

            var verifyStr = verifyUser.ManagerRestaurant.Replace(",", "");
            if (verifyUser.ManagerRestaurant.IsEmpty() || verifyStr.IsEmpty() || !ValidateExtend.IsNumber(verifyStr))
            {
                user.State = LoginState.NoPermission;
                return user;
            }

            if (!string.IsNullOrEmpty(verifyUser.RoleId) && verifyUser.RoleId.Contains("ZZ"))
            {
                user.State = LoginState.NotActivated;
                return user;
            }

            string[] ids = verifyUser.ManagerRestaurant.Split(',');

            var resList = _resRepository.GetList(ids);

            if (resList == null || resList.Count == 0)
            {
                user.State = LoginState.NoPermission;
                return user;
            }

            //验证当前用户操作餐厅权限是否包含指定的餐厅
            if(verifyUserDTO.RestaurantId > 0 && !ids.Contains(verifyUserDTO.RestaurantId.ToString()))
            {
                user.State = LoginState.NoPermission;
                return user;
            }

            var list = resList.Select(x => x.Id + "-" + x.Name).ToList();

            user.UserId = verifyUser.UserId;
            user.State = LoginState.Successed;
            user.UserCode = verifyUser.UserCode.Trim();
            user.UserName = verifyUser.UserName.Trim();
            //user.RoleId = verifyUser.RoleId.Trim();
            user.GroupCode = verifyUserDTO.CompanyId.ToString();//餐饮登录暂存公司Id
            user.Permission = verifyUser.Permission;
            user.ManagerRestaurant = list.Join(";"); //verifyUser.ManagerRestaurant;
            user.MinDiscountValue = verifyUser.Discount / 100; //折扣值需要除以100变成折扣率
            user.MaxClearValue = verifyUser.MaxClearValue;

            return user;
        }

        public bool UpdateUserPassWord(int userId,string oldPassword,string newPassword)
        {
            bool result = true;
            try
            {
                UserInfo verifyUserDTO = _userRepository.GetByUserId(userId);
                if (DESEncrypt.Rc4PassHex(oldPassword) != verifyUserDTO.UserPwd)
                {
                    throw new Exception("原密码不一致，请重新输入");
                }
                string passWord = DESEncrypt.Rc4PassHex(newPassword);
                result = _userRepository.UpdatePassWord(userId, passWord);
            }
            catch (Exception e)
            {
                result = false;
                throw e;
            }
            return result;
        }
    }
}