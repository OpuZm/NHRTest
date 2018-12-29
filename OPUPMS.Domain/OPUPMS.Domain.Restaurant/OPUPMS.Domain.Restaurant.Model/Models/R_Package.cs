/***********************************************************************
 * Module:  Cytc.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cytc
 ***********************************************************************/

// <summary>
// 餐饮套餐
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮套餐
    public class R_Package
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 套餐名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 价格
        ///</summary>
        public decimal Price { get; set; }


        ///<summary>
        /// 成本价格
        ///</summary>
        public decimal CostPrice { get; set; }


        ///<summary>
        /// 是否上架
        ///</summary>
        public bool IsOnSale { get; set; }


        ///<summary>
        /// 套餐描述
        ///</summary>
        public string Describe { get; set; }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool IsCustomer { get; set; }
        public int R_Company_Id { get; set; }
        public int Property { get; set; }
        public bool IsDelete { get; set; }
        public int R_Category_Id { get; set; }

    }
}