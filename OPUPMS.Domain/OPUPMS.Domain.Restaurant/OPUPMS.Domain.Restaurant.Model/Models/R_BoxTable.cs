/***********************************************************************
 * Module:  CybxTh.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CybxTh
 ***********************************************************************/

// <summary>
// 餐饮包厢台号
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮包厢台号
    public class R_BoxTable
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 包厢
        ///</summary>
        public int R_Box_Id { get; set; }


        ///<summary>
        /// 餐饮台号
        ///</summary>
        public int R_Table_Id { get; set; }

    }
}