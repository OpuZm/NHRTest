/********************************************************************************
 * Copyright © 2017 OPUPMS.Framework 版权所有
 * Author: OPUPMS - Justin
 * Description: OPUPMS快速开发平台
 * Website：http://www.opu.com.cn
*********************************************************************************/

using System.Configuration;
using OPUPMS.Infrastructure.Common.Web;
using OPUPMS.Infrastructure.Common.Security;

namespace OPUPMS.Infrastructure.Common.Operator
{
    /// <summary>
    /// 系统操作者提供处理类
    /// </summary>
    public class OperatorProvider
    {
        /// <summary>
        /// 提供操作的对象
        /// </summary>
        public static OperatorProvider Provider
        {
            get { return new OperatorProvider(); }
        }
        private string LoginUserKey = "opupms_loginuserkey_2017";
        private string LoginProvider = GetConfigValue("LoginProvider");

        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }

        /// <summary>
        /// 获取当前会话凭证
        /// </summary>
        /// <returns></returns>
        public OperatorModel GetCurrent()
        {
            OperatorModel operatorModel = new OperatorModel();
            if (LoginProvider == "Cookie")
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetAuthCookie(LoginUserKey).ToString()).ToObject<OperatorModel>();
            }
            else
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey).ToString()).ToObject<OperatorModel>();
            }
            return operatorModel;
        }

        /// <summary>
        /// 添加一个操作者会话凭证
        /// </summary>
        /// <param name="operatorModel">操作者Model</param>
        public void AddCurrent(OperatorModel operatorModel)
        {
            if (LoginProvider == "Cookie")
            {
                WebHelper.WriteAuthCookie(LoginUserKey, DESEncrypt.Encrypt(operatorModel.ToJson()), 12);
            }
            else
            {
                WebHelper.WriteSession(LoginUserKey, DESEncrypt.Encrypt(operatorModel.ToJson()));
            }
        }

        /// <summary>
        /// 移除当前会话凭证
        /// </summary>
        public void RemoveCurrent()
        {
            if (LoginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(LoginUserKey.Trim());
            }
            else
            {
                WebHelper.RemoveSession(LoginUserKey.Trim());
            }
        }
    }
}
