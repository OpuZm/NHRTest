/***********************************************************************
 * Module:  CyxmMx.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyxmMx
 ***********************************************************************/

// <summary>
// 餐饮项目明细
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目明细
    public class R_ProjectDetail
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 餐饮项目ID
        ///</summary>
        public int R_Project_Id { get; set; }


        ///<summary>
        /// 单位
        ///</summary>
        public string Unit { get; set; }


        ///<summary>
        /// 价格
        ///</summary>
        public decimal Price { get; set; }


        ///<summary>
        /// 成本价
        ///</summary>
        public decimal CostPrice { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }


        ///<summary>
        /// 单位倍率 [1,1.5,2等]
        ///</summary>
        public decimal UnitRate { get; set; }

        public bool IsDelete { get; set; }

    }
}