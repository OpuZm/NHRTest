/***********************************************************************
 * Module:  Cyddczjl.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cyddczjl
 ***********************************************************************/

// <summary>
// 餐饮订单操作纪录
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮订单操作纪录
    public class R_OrderRecord
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 操作人
        ///</summary>
        public int CreateUser { get; set; }


        ///<summary>
        /// 操作时间
        ///</summary>
        public Nullable<DateTime> CreateDate { get; set; }


        ///<summary>
        /// 操作类型(1:预定,2:开台,3:点餐,4:送厨,5:用餐中,6:结账,7:取消,8:订单菜品修改 9.并台 10.换桌/拆台)
        ///</summary>
        public CyddStatus CyddCzjlStatus { get; set; }


        ///<summary>
        /// 用户类型(1:餐饮员工,2:会员)
        ///</summary>
        public CyddCzjlUserType CyddCzjlUserType { get; set; }


        ///<summary>
        /// 备注信息
        ///</summary>
        public string Remark { get; set; }


        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int R_Order_Id { get; set; }
        /// <summary>
        /// 餐饮订单台号
        /// </summary>
        public int R_OrderTable_Id { get; set; }
    }
}