using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OPUPMS.Domain.Hotel.Model.IServices;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Security;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Hotel.Model.Dtos;
using OPUPMS.Domain.Base;
using OPUPMS.Infrastructure.Common.Net;

namespace OPUPMS.Hotel.Web.Controllers
{
    public class AccountController : BaseController
    {
        readonly IHotelLoginService _userDomainService;

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public AccountController(IHotelLoginService userDomainService)
        {
            _userDomainService = userDomainService;
        }

        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        
        [HttpPost]
        //[HandlerAjaxOnly]
        public async Task<ActionResult> CheckLogin(string hotelCode, string userName, string password, string code)
        {
            try
            {
                if (hotelCode.IsEmpty())
                    throw new Exception("请输入酒店码！");

                var verifyCode = Session["opupms_session_verifycode"];
                if (verifyCode.IsEmpty() || DESEncrypt.MD5(code.ToLower(), 16) != verifyCode.ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }

                var result = await _userDomainService.CheckLoginAsync(
                    new LoginInputDto
                    {
                        HotelCode = hotelCode,
                        UserName = userName,
                        UserPwd = password,
                    });

                if (result.State == LoginState.Successed)
                {
                    OperatorModel operatorModel = new OperatorModel();
                    operatorModel.UserId = result.UserCode;
                    operatorModel.UserCode = result.UserCode;
                    operatorModel.UserName = result.UserName;
                    //operatorModel.UserPwd = userEntity.UserPwd;
                    string pwd = DESEncrypt.Rc4PassHex(DESEncrypt.DecryptFromBase64(password));
                    operatorModel.CompanyId = result.HotelCode;
                    //operatorModel.DepartmentId = userEntity.F_DepartmentId;
                    operatorModel.ConnectToken = result.HotelCode;
                    operatorModel.RoleId = result.RoleId;
                    operatorModel.LoginIPAddress = Net.Ip;
                    operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
                    operatorModel.LoginTime = DateTime.Now;
                    operatorModel.LoginToken = DESEncrypt.Encrypt(Common.GuId());
                    //if (userEntity.Czdmmc00 == "admin")
                    //{
                    //    operatorModel.IsSystem = true;
                    //}
                    //else
                    //{
                    //    operatorModel.IsSystem = false;
                    //}
                    ClearCache();

                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, operatorModel.UserId, DateTime.Now, DateTime.Now.AddDays(2), false, string.Concat(operatorModel.UserId, "|", operatorModel.UserName));
                    //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                    ////if (loginInput.AutoLogin)
                    ////{
                    ////    cookie.Expires = ticket.Expiration;
                    ////}
                    //cookie.Domain = FormsAuthentication.CookieDomain;
                    //cookie.Path = FormsAuthentication.FormsCookiePath;

                    OperatorProvider.Provider.AddCurrent(operatorModel);

                    //return Content(new AjaxResult { Status = ResultType.Success.ToString(), Message = "登录成功。" }.ToJson());
                    return NewtonSoftJson(new JsonMessage<int, object> { Status = 1 }, "text/html" /*解决IE直接返回json提示下载文件问题*/, true);

                }
                else
                {
                    //return Content(new AjaxResult { Status = ResultType.Error.ToString(), Message = "登录失败，用户名密码错误或被禁用！" }.ToJson());
                    return NewtonSoftJson(new JsonMessage<int, object> { Status = result.State.ToInt(), Message = "登录失败，用户名密码错误或被禁用！" }, "text/html" /*解决IE直接返回json提示下载文件问题*/, true);
                }
            }
            catch (Exception ex)
            {
                //return Content(new AjaxResult { Status = ResultType.Error.ToString(), Message = ex.Message }.ToJson());
                return NewtonSoftJson(new JsonMessage<int, object> { Status = 0, Message = ex.Message }, "text/html" /*解决IE直接返回json提示下载文件问题*/, true);
            }
        }

        //public ActionResult Logout(string returnUrl)
        //{
        //    ClearCache();
        //    FormsAuthentication.SignOut();
        //    return NewtonSoftJson(new JsonMessage<int, object> { Status = 1 }, "text/html" /*解决IE直接返回json提示下载文件问题*/, true);
        //}

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
            return RedirectToAction("Login", "Account");//重定向到登录页面
        }
    }
}