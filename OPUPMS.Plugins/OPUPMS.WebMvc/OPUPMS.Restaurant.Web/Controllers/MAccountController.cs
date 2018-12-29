using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Base;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using System.Threading.Tasks;
using OPUPMS.Restaurant.Web.Models;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using System.Web.Security;


namespace OPUPMS.Restaurant.Web.Controllers
{
    public class MAccountController : BaseController
    {
        readonly IUserService _userService;
        readonly IMarketRepository _marketRepository;
        readonly IExtendItemRepository _extendItemRepository;
        // GET: MAccount

        public MAccountController(IUserService userService,
            IMarketRepository marketRepository,
            IExtendItemRepository extendItemRepository
            )
        {
            _userService = userService;
            _marketRepository = marketRepository;
            _extendItemRepository = extendItemRepository;
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginIn(LoginInput req)
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

                            OperatorProvider.Provider.AddCurrent(mUser);
                            res.Data = list;
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

        private void ClearCache()
        {
            Session.Abandon();
            Session.Clear();
            OperatorProvider.Provider.RemoveCurrent();
            FormsAuthentication.SignOut();
        }
    }
}