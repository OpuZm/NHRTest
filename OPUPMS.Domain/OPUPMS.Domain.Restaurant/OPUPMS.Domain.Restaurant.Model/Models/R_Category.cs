/***********************************************************************
 * Module:  CyxmLb.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyxmLb
 ***********************************************************************/

// <summary>
// 餐饮项目类别
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目类别
    public class R_Category
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
        /// 父ID
        ///</summary>
        public int PId { get; set; }


        ///<summary>
        /// 打折比率
        ///</summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 是否可打折
        /// </summary>
        public bool IsDiscount { get; set; }
        public int R_Company_Id { get; set; }

        public bool IsDelete { get; set; }
        public int Sorted { get; set; }

    }
}