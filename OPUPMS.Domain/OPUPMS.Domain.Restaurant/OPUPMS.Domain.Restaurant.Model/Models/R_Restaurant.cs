/***********************************************************************
 * Module:  Cyct.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cyct
 ***********************************************************************/

// <summary>
// 餐饮餐厅
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮餐厅
    public class R_Restaurant
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 座位数
        ///</summary>
        public int SeatNum { get; set; }


        ///<summary>
        /// 台数
        ///</summary>
        public int TableNum { get; set; }


        ///<summary>
        /// 服务费率
        ///</summary>
        public decimal ServiceRate { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }
        public int R_Company_Id { get; set; }
        public bool IsDelete { get; set; }

    }
}