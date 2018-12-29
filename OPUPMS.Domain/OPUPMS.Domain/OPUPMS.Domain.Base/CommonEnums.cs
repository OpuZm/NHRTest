using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base
{
    /// <summary>
    /// 用户登录状态。
    /// </summary>
    public enum LoginState
    {
        /// <summary>
        /// 登陆成功。
        /// </summary>
        Successed,
        /// <summary>
        /// 登录失败。
        /// </summary>
        Failed,
        /// <summary>
        /// 用户名错误。
        /// </summary>
        InvalidAccount,
        /// <summary>
        /// 密码错误。
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// 无效的酒店码
        /// </summary>
        InvalidHotelCode,
        /// <summary>
        /// 验证码错误或失效。
        /// </summary>
        InvalidVerifyCode,
        /// <summary>
        /// 验证码过期失效。
        /// </summary>
        ExpiredVerifyCode,
        /// <summary>
        /// 帐号没有激活。
        /// </summary>
        NotActivated,
        /// <summary>
        /// 无权限。
        /// </summary>
        NoPermission,
    }

    public enum CharsetSource
    {
        餐饮项目=1
    }

    public enum CharsetType
    {
        拼音=1,
        五笔=2
    }
}
