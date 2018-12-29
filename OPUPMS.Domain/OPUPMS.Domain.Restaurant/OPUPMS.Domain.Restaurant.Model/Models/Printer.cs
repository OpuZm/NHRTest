/***********************************************************************
 * Module:  Printer.cs
 * Author:  Justin-Administrator
 * Purpose: Definition of the Class Printer
 ***********************************************************************/

// <summary>
// 打印机
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// <summary>
    /// 打印机Model
    /// </summary>
    public class Printer
    {
        ///<summary>
        ///
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 备注信息
        ///</summary>
        public string Remark { get; set; }

        ///<summary>
        /// 电脑名称 
        ///</summary>
        public string PcName { get; set; }

        ///<summary>
        /// IP地址
        ///</summary>
        public string IpAddress { get; set; }

        ///<summary>
        /// 打印机端口
        ///</summary>
        public string PrintPort { get; set; }

        /// <summary>
        /// 自定义接口
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        public PrintStatus RealStatus { get; set; }
        public string RealStatusRemark { get; set; }
    }
}