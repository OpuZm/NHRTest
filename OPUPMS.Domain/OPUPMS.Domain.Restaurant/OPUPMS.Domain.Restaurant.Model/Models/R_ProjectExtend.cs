/***********************************************************************
 * Module:  CyxmKz.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyxmKz
 ***********************************************************************/

// <summary>
// 餐饮项目扩展
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目扩展
    public class R_ProjectExtend
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
        /// 费用
        ///</summary>
        public decimal Price { get; set; }


        ///<summary>
        /// 类型(1:做法,2:要求,3.配菜)
        ///</summary>
        public CyxmKzType CyxmKzType { get; set; }


        ///<summary>
        /// 单位
        ///</summary>
        public string Unit { get; set; }
        public int R_Company_Id { get; set; }
        public int R_ProjectExtendType_Id { get; set; }
        public bool IsDelete { get; set; }
    }
}