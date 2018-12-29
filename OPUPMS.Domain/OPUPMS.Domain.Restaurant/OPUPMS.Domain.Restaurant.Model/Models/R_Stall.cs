/***********************************************************************
 * Module:  Cydk.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cydk
 ***********************************************************************/

// <summary>
// 餐饮档口
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮档口
    public class R_Stall
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 档口名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }
        public int R_Company_Id { get; set; }
        public int Print_Id { get; set; }

        public bool IsDelete { get; set; }
    }
}