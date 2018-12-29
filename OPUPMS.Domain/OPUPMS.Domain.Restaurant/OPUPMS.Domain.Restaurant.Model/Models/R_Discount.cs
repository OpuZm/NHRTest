/***********************************************************************
 * Module:  CyxmZkSz.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyxmZkSz
 ***********************************************************************/

// <summary>
// 餐饮项目折扣设置
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目折扣设置
    public class R_Discount
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 折扣名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 餐饮分市
        ///</summary>
        public int R_Market_Id { get; set; }


        ///<summary>
        /// 餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 餐厅区域
        ///</summary>
        public int R_Area_Id { get; set; }
        ///<summary>
        /// 是否启用
        ///</summary>
        public bool IsEnable { get; set; }
        public int R_Company_Id { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public bool IsDelete { get; set; }

    }
}