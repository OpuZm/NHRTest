/***********************************************************************
 * Module:  Cyth.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cyth
 ***********************************************************************/

// <summary>
// 餐饮台号
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮台号
    public class R_Table
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 餐饮餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 座位数
        ///</summary>
        public int SeatNum { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Describe { get; set; }


        ///<summary>
        /// 状态(1:空置,2:在用,3:清理)
        ///</summary>
        public CythStatus CythStatus { get; set; }


        ///<summary>
        /// 服务费率
        ///</summary>
        public Nullable<decimal> ServerRate { get; set; }


        ///<summary>
        /// 餐饮区域
        ///</summary>
        public int R_Area_Id { get; set; }
        public bool IsDelete { get; set; }
        public bool IsVirtual { get; set; }
        public int Sorted { get; set; }
    }
}