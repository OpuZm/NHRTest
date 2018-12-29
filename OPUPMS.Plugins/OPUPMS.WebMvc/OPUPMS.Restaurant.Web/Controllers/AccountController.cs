using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using OPUPMS.Domain.Base;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common.Security;
using OPUPMS.Restaurant.Web.Models;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Base.Repositories;

namespace OPUPMS.Restaurant.Web.Controllers
{
    public class AccountController : BaseController
    {
        readonly IUserService _userService;
        readonly IMarketRepository _marketRepository;
        readonly IExtendItemRepository _extendItemRepository;
        readonly IRestaurantRepository _restaurantRepository;

        public AccountController(IUserService userService,
            IMarketRepository marketRepository,
            IExtendItemRepository extendItemRepository,
            IRestaurantRepository restaurantRepository
            )
        {
            _userService = userService;
            _marketRepository = marketRepository;
            _extendItemRepository = extendItemRepository;
            _restaurantRepository = restaurantRepository;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult NewLogin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }

        [HttpPost]
        public async Task<ActionResult> LoginIn(LoginInput req)
        {
            Response res = new Response();
            res.Successed = false;
            if (req.CompanyId <= 0)
            {
                req.CompanyId = 1;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    req.PassWord = string.IsNullOrEmpty(req.PassWord) ? "" : req.PassWord;
                    //var verifyCode = Session["opupms_session_verifycode"];
                    //if (verifyCode.IsEmpty() || DESEncrypt.MD5(req.Code.ToLower(), 16) != verifyCode.ToString())
                    //{
                    //    res.Data = false;
                    //    res.Message = "验证码错误，请重新输入";
                    //}
                    if (req.Account.IsEmpty())
                    {
                        res.Data = false;
                        res.Message = "请输入用户名！";
                    }
                    else
                    {
                        var user = await _userService.CheckLogin(req.Account, req.PassWord,req.CompanyId);
                        if (user.State == LoginState.Successed)
                        {
                            OperatorModel mUser = new OperatorModel();
                            mUser.UserId = user.UserId;
                            mUser.UserCode = user.UserCode;
                            mUser.UserName = user.UserName;
                            mUser.UserPwd = user.UserPwd;
                            mUser.RoleId = user.RoleId;
                            mUser.CompanyId = user.GroupCode;
                            //mUser.MinDiscountValue = user.Discount;
                            mUser.Permission = user.Permission;
                            mUser.LoginTime = DateTime.Now;
                            //mUser.MaxClearValue = user.MaxClearValue;

                            List<RestaurantListDTO> list = new List<RestaurantListDTO>();
                            if (!string.IsNullOrEmpty(user.ManagerRestaurant))
                            {
                                mUser.ManagerRestaurant = new List<int>();
                                var sourceList = user.ManagerRestaurant.Split(';').ToList();
                                foreach (var str in sourceList)
                                {
                                    string id = str.Substring(0, str.IndexOf('-'));
                                    string name = str.Substring(str.IndexOf('-') + 1);
                                    list.Add(new RestaurantListDTO()
                                    {
                                        Id = id.ToInt(),
                                        Name = name
                                    });
                                    //mUser.ManagerRestaurant.Add(id.ToInt());
                                }

                                list = _restaurantRepository.FilterCompanyRestaurant(list, req.CompanyId);
                                mUser.ManagerRestaurant = list.Select(p => p.Id).ToList();
                                var ids = list.Select(x => x.Id).ToList();
                                var allMarkets = _marketRepository.GetList(ids);
                                var selectMarkets = allMarkets.Where(x => ids.Contains(x.RestaurantId)).ToList();
                                foreach (var item in list)
                                {
                                    item.MarketList = selectMarkets.Where(x => x.RestaurantId == item.Id).ToList();
                                }
                            }

                            OperatorProvider.Provider.AddCurrent(mUser);
                            res.Data = list;
                            res.Successed = true;

                            #region 账务日期
                            var businessDate = _extendItemRepository.GetModelList(Convert.ToInt32(mUser.CompanyId), 10003).FirstOrDefault();
                            if (businessDate == null)
                                throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

                            DateTime accDate = DateTime.Today;

                            if (!DateTime.TryParse(businessDate.ItemValue, out accDate))
                                throw new Exception("餐饮账务日期设置错误，请联系管理员");

                            if (!DateTime.Now.Date.Equals(accDate.Date))
                            {
                                res.Message = "系统账务日期和当前日期不一致,确认要继续操作吗?";
                            }
                            #endregion
                        }
                        else
                        {
                            res.Data = false;
                            switch (user.State)
                            {
                                case LoginState.Failed:
                                    break;
                                case LoginState.InvalidAccount:
                                case LoginState.InvalidPassword:
                                    res.Message = "账号或密码错误，请重新输入";
                                    break;
                                case LoginState.InvalidHotelCode:
                                    break;
                                case LoginState.InvalidVerifyCode:
                                    break;
                                case LoginState.ExpiredVerifyCode:
                                    break;
                                case LoginState.NotActivated:
                                    res.Message = "账号已经被禁用,请联系管理员";
                                    break;
                                case LoginState.NoPermission:
                                    res.Message = "抱歉您无权限登录系统";
                                    break;
                                default:
                                    res.Message = "网络异常，请重新登录";
                                    break;
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    res.Data = false;
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Data = false;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private void ClearCache()
        {
            Session.Abandon();
            Session.Clear();
            OperatorProvider.Provider.RemoveCurrent();
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            ClearCache();
            return Redirect("/");//重定向到登录页面
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult NewLogout()
        {
            ClearCache();
            return Redirect("/Res/Account/NewLogin");//重定向到登录页面
        }

        public ActionResult SelectRestaurant(int id, int marketId)
        {
            Response res = new Response();
            var operatorUser = OperatorProvider.Provider.GetCurrent();
            OperatorProvider.Provider.RemoveCurrent();
            OperatorModel mUser = new OperatorModel();
            mUser.UserId = operatorUser.UserId;
            mUser.UserCode = operatorUser.UserCode;
            mUser.UserName = operatorUser.UserName;
            mUser.UserPwd = operatorUser.UserPwd;
            mUser.RoleId = operatorUser.RoleId;
            mUser.CompanyId = operatorUser.CompanyId;
            //mUser.MinDiscountValue = operatorUser.MinDiscountValue;
            mUser.Permission = operatorUser.Permission;
            mUser.ManagerRestaurant = operatorUser.ManagerRestaurant;
            mUser.DepartmentId = id.ToString(); //设置选择进入的餐厅
            mUser.LoginTime = DateTime.Now;
            mUser.LoginMarketId = marketId;
            //mUser.MaxClearValue = operatorUser.MaxClearValue;

            OperatorProvider.Provider.AddCurrent(mUser);
            res.Data = true;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取登陆用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfo()
        {
            Response res = new Response();
            var userInfo = OperatorProvider.Provider.GetCurrent();
            if (userInfo != null)
            {
                var businessDate = _extendItemRepository.GetModelList(Convert.ToInt32(userInfo.CompanyId), 10003).FirstOrDefault();
                userInfo.BusinessDate = businessDate.ItemValue;
                res.Data = userInfo;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> FlatLoginIn(LoginInput req)
        {
            Response res = new Response();
            res.Successed = false;

            if (ModelState.IsValid)
            {
                try
                {
                    req.PassWord = string.IsNullOrEmpty(req.PassWord) ? "" : req.PassWord;
                    //var verifyCode = Session["opupms_session_verifycode"];
                    //if (verifyCode.IsEmpty() || DESEncrypt.MD5(req.Code.ToLower(), 16) != verifyCode.ToString())
                    //{
                    //    res.Data = false;
                    //    res.Message = "验证码错误，请重新输入";
                    //}
                    if (req.Account.IsEmpty())
                    {
                        res.Data = false;
                        res.Message = "请输入用户名！";
                    }
                    else
                    {
                        var user = await _userService.CheckLogin(req.Account, req.PassWord);
                        if (user.State == LoginState.Successed)
                        {
                            OperatorModel mUser = new OperatorModel();
                            mUser.UserId = user.UserId;
                            mUser.UserCode = user.UserCode;
                            mUser.UserName = user.UserName;
                            mUser.UserPwd = user.UserPwd;
                            mUser.RoleId = user.RoleId;
                            mUser.CompanyId = user.GroupCode;
                            //mUser.MinDiscountValue = user.Discount;
                            mUser.Permission = user.Permission;
                            mUser.LoginTime = DateTime.Now;
                            //mUser.MaxClearValue = user.MaxClearValue;

                            List<RestaurantListDTO> list = new List<RestaurantListDTO>();
                            if (!string.IsNullOrEmpty(user.ManagerRestaurant))
                            {
                                mUser.ManagerRestaurant = new List<int>();
                                var sourceList = user.ManagerRestaurant.Split(';').ToList();
                                foreach (var str in sourceList)
                                {
                                    string id = str.Substring(0, str.IndexOf('-'));
                                    string name = str.Substring(str.IndexOf('-') + 1);
                                    list.Add(new RestaurantListDTO()
                                    {
                                        Id = id.ToInt(),
                                        Name = name
                                    });
                                    mUser.ManagerRestaurant.Add(id.ToInt());
                                }

                                var ids = list.Select(x => x.Id).ToList();
                                var allMarkets = _marketRepository.GetList(ids);
                                var selectMarkets = allMarkets.Where(x => ids.Contains(x.RestaurantId)).ToList();
                                foreach (var item in list)
                                {
                                    item.MarketList = selectMarkets.Where(x => x.RestaurantId == item.Id).ToList();
                                }
                            }

                            //OperatorProvider.Provider.AddCurrent(mUser);
                            res.Data = mUser;
                            res.Successed = true;
                        }
                        else
                        {
                            res.Data = false;
                            switch (user.State)
                            {
                                case LoginState.Failed:
                                    break;
                                case LoginState.InvalidAccount:
                                case LoginState.InvalidPassword:
                                    res.Message = "账号或密码错误，请重新输入";
                                    break;
                                case LoginState.InvalidHotelCode:
                                    break;
                                case LoginState.InvalidVerifyCode:
                                    break;
                                case LoginState.ExpiredVerifyCode:
                                    break;
                                case LoginState.NotActivated:
                                    break;
                                case LoginState.NoPermission:
                                    res.Message = "抱歉您无权限登录系统";
                                    break;
                                default:
                                    res.Message = "网络异常，请重新登录";
                                    break;
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    res.Data = null;
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Data = null;
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }
            return Json(res);
        }

        //public JsonResult FlatLogout()
        //{
        //    Response res = new Response() { Data = true, Successed = true };
        //    try
        //    {
        //        Session.Abandon();
        //        Session.Clear();
        //        OperatorProvider.Provider.RemoveCurrent();
        //        FormsAuthentication.SignOut();
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Data = false;
        //        res.Message = ex.Message;
        //    }
        //    return Json(res);
        //}

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword,string newPassWord)
        {
            Response res = new Response();
            try
            {
                var userInfo = OperatorProvider.Provider.GetCurrent();
                res.Data = _userService.UpdateUserPassWord(userInfo.UserId, oldPassword, newPassWord);
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.Message = ex.Message;
            }
            return Json(res);
        } 
    }
}