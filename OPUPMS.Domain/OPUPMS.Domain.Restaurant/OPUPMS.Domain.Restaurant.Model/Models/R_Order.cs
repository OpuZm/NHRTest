/***********************************************************************
 * Module:  Cydd.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cydd
 ***********************************************************************/

// <summary>
// 餐饮订单// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮订单
    public class R_Order
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 订单编号
        ///</summary>
        public string OrderNo { get; set; }


        ///<summary>
        /// 餐饮餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 订单来源(1:自来客,2:营销推广,3:协议客户,4:微信)
        ///</summary>
        public int CyddOrderSource { get; set; }


        ///<summary>
        /// 人数
        ///</summary>
        public int PersonNum { get; set; }


        ///<summary>
        /// 开单人
        ///</summary>
        public int BillingUser { get; set; }


        ///<summary>
        /// 订单状态(1:预定,2:开台,3:送厨,4:用餐中,5:结账,6:取消,7:订单菜品修改,8:并台)
        ///</summary>
        public CyddStatus CyddStatus { get; set; }


        ///<summary>
        /// 备注信息
        ///</summary>
        public string Remark { get; set; }


        ///<summary>
        /// 预定用餐时间
        ///</summary>
        public Nullable<DateTime> ReserveDate { get; set; }


        ///<summary>
        /// 消费金额
        ///</summary>
        public decimal ConAmount { get; set; }


        ///<summary>
        /// 服务金额
        ///</summary>
        public decimal ServiceAmount { get; set; }


        ///<summary>
        /// 应收金额
        ///</summary>
        public decimal OriginalAmount { get; set; }


        ///<summary>
        /// 实收金额
        ///</summary>
        public decimal RealAmount { get; set; }


        ///<summary>
        /// 折扣率
        ///</summary>
        public decimal DiscountRate { get; set; }


        ///<summary>
        /// 折扣金额
        ///</summary>
        public decimal DiscountAmount { get; set; }


        ///<summary>
        /// 赠送金额
        ///</summary>
        public decimal GiveAmount { get; set; }


        ///<summary>
        /// 特殊要求
        ///</summary>
        public string SpecialPopc { get; set; }


        ///<summary>
        /// 起菜方式(1:即起,2:叫起)
        ///</summary>
        public CyddCallType CyddCallType { get; set; }


        ///<summary>
        /// 订金
        ///</summary>
        public decimal DepositAmount { get; set; }


        ///<summary>
        /// 创建时间
        ///</summary>
        public Nullable<DateTime> CreateDate { get; set; }


        ///<summary>
        /// 餐饮分市
        ///</summary>
        public int R_Market_Id { get; set; }
        /// <summary>
        /// 服务员
        /// </summary>
        public int CreateUser { get; set; }
        public string ContactPerson { get; set; }
        public string ContactTel { get; set; }
        public decimal ClearAmount { get; set; }
        public int TableNum { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public int CustomerId { get; set; }
        public bool IsDelete { get; set; }
        public string BillDepartMent { get; set; }
        public int MemberCardId { get; set; }
        public Nullable<DateTime> OpenDate { get; set; }
    }
}