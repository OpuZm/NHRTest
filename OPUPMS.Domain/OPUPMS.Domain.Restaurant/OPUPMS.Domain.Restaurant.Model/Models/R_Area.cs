/***********************************************************************
 * Module:  CyctQy.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyctQy
 ***********************************************************************/

// <summary>
// 餐饮餐厅区域
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮餐厅区域
    public class R_Area
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }


        ///<summary>
        /// 所属餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 服务费率
        ///</summary>
        public Nullable<decimal> ServerRate { get; set; }
        public bool IsDelete { get; set; }
    }
}