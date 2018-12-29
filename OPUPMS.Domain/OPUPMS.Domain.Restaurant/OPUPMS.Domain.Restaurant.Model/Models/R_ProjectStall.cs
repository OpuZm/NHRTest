/***********************************************************************
 * Module:  CyxmDk.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyxmDk
 ***********************************************************************/

// <summary>
// 餐饮项目档口
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目档口
    public class R_ProjectStall
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 餐饮项目
        ///</summary>
        public int R_Project_Id { get; set; }


        ///<summary>
        /// 餐饮档口
        ///</summary>
        public int R_Stall_Id { get; set; }

        /// <summary>
        /// 出单类型(1:餐饮详情单 2:餐饮总单)
        /// </summary>
        public int BillType { get; set; }
    }
}