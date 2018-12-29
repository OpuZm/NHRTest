/***********************************************************************
 * Module:  Cyfs.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cyfs
 ***********************************************************************/

// <summary>
// 餐饮分市
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮分市
    public class R_Market
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 分市名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 餐饮餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 开始时间
        ///</summary>
        public string StartTime { get; set; }


        ///<summary>
        /// 结束时间
        ///</summary>
        public string EndTime { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }
        public bool IsDelete { get; set; }
    }
}