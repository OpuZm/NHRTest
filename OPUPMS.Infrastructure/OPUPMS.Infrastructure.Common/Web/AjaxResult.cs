/********************************************************************************
 * Copyright © 2017 OPUPMS.Framework 版权所有
 * Author: OPUPMS - Justin
 * Description: OPUPMS快速开发平台
 * Website：http://www.opu.com.cn
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPUPMS.Infrastructure.Common
{
    /// <summary>
    /// Ajax 结果对象类
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 操作结果类型
        /// </summary>
        public object Status { get; set; }
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        Info,
        /// <summary>
        /// 成功结果类型
        /// </summary>
        Success,
        /// <summary>
        /// 警告结果类型
        /// </summary>
        Warning,
        /// <summary>
        /// 异常结果类型
        /// </summary>
        Error
    }
}
