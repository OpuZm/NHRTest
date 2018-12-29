/***********************************************************************
 * Module:  CyddTh.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyddTh
 ***********************************************************************/

// <summary>
// 餐饮订单台号
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮订单台号
    public class R_OrderTable
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int R_Order_Id { get; set; }


        ///<summary>
        /// 餐饮台号
        ///</summary>
        public int R_Table_Id { get; set; }


        ///<summary>
        /// 开台时间
        ///</summary>
        public Nullable<DateTime> CreateDate { get; set; }


        ///<summary>
        /// 订单台号人数
        ///</summary>
        public int PersonNum { get; set; }
        public bool IsCheckOut { get; set; }
        public bool IsOpen { get; set; }
        public bool IsLock { get; set; }

        /// <summary>
        /// 账务日期
        /// </summary>
        public DateTime? BillDate { get; set; }

        public int R_Market_Id { get; set; }

        public int R_OrderMainPay_Id { get; set; }
        public bool IsControl { get; set; }
    }
}