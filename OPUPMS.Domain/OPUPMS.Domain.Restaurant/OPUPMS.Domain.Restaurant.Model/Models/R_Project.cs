/***********************************************************************
 * Module:  Cyxm.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cyxm
 ***********************************************************************/

// <summary>
// 餐饮项目
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目
    public class R_Project
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 餐饮项目类别
        ///</summary>
        public int R_Category_Id { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }


        ///<summary>
        /// 创建时间
        ///</summary>
        public Nullable<DateTime> CreateDate { get; set; }



        ///<summary>
        /// 是否删除
        ///</summary>
        public bool IsDelete { get; set; }



        ///<summary>
        /// 是否使用库存
        ///</summary>
        public bool IsStock { get; set; }


        ///<summary>
        /// 库存数
        ///</summary>
        public decimal Stock { get; set; }

        /// <summary>
        /// 餐饮项目属性
        /// </summary>
        public int Property { get; set; }


        ///<summary>
        /// 成本价
        ///</summary>
        public decimal CostPrice { get; set; }


        ///<summary>
        /// 销售价
        ///</summary>
        public decimal Price { get; set; }
        public int R_Company_Id { get; set; }
        public string Code { get; set; }
        public int Sorted { get; set; }
        public bool IsEnable { get; set; }
        public int ExtractType { get; set; }
        public decimal ExtractPrice { get; set; }
    }
}