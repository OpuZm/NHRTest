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
    /// 对指定对象的描述
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
    {
        /// <summary>
        /// 获取Html文本描述
        /// </summary>
        public string HtmlDescription
        {
            get
            {
                if (Color.IsEmpty())
                    return DescriptionValue;
                return string.Format("<font color=\"{0}\">{1}</font>", Color, DescriptionValue);
            }
        }

        /// <summary>
        /// 获取指定的值
        /// </summary>
        public string AssignValue
        {
            get
            {
                if (Value.IsEmpty())
                    return DescriptionValue;
                return Value;
            }
        }

        /// <summary>
        /// 获取或设置特定字符串值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 获取或设置呈现为Html时的文本颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 初始化 OPUPMS.DescriptionAttribute
        /// </summary>
        public DescriptionAttribute()
            : base()
        {
        }

        /// <summary>
        /// 初始化 DescriptionAttribute
        /// </summary>
        /// <param name="description">文本描述的值</param>
        public DescriptionAttribute(string description)
            : base(description)
        {
        }

        /// <summary>
        /// 初始化 OPUPMS.DescriptionAttribute
        /// </summary>
        /// <param name="description">文本描述的值</param>
        /// <param name="color">呈现为Html时的文本颜色</param>
        public DescriptionAttribute(string description, string color)
            : base(description)
        {
            Color = color;
        }

        /// <summary>
        /// 初始化 OPUPMS.DescriptionAttribute
        /// </summary>
        /// <param name="value">指定值</param>
        /// <param name="description">描述的文本</param>
        /// <param name="color">呈现为Html时的文本颜色</param>
        public DescriptionAttribute(string value, string description, string color)
            : base(description)
        {
            Color = color;
            Value = value;
        }
    }
}
