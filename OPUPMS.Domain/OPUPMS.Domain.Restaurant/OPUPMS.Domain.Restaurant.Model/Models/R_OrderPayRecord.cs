/***********************************************************************
 * Module:  CyddJzjl.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyddJzjl
 ***********************************************************************/

// <summary>
// 餐饮订单结账纪录
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮订单结账纪录
    public class R_OrderPayRecord
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int R_Order_Id { get; set; }


        ///<summary>
        /// 付款方式(1:现金,2:银行卡,3:会员卡,4:挂账,5:转客房,6:代金券,7:免单)
        ///</summary>
        public int CyddPayType { get; set; }


        ///<summary>
        /// 付款金额
        ///</summary>
        public decimal PayAmount { get; set; }


        ///<summary>
        /// 付款方式(1:未付,2:已付,3:已退)
        ///</summary>
        public CyddJzStatus CyddJzStatus { get; set; }
        /// <summary>
        /// 付款类型
        /// </summary>
        public CyddJzType CyddJzType { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUser { get; set; }

        /// <summary>
        /// 资源ID，跟据根据支付方式判断 类型(1:挂账 2:转客房) 
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 资源名称 
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// 账务日期
        /// </summary>
        public Nullable<DateTime> BillDate { get; set; }

        /// <summary>
        /// 当次主结账Id
        /// </summary>
        public int R_OrderMainPay_Id { get; set; }

        /// <summary>
        /// 分市
        /// </summary>
        public int R_Market_Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public int PId { get; set; }

        public int PrintNum { get; set; }
        public int R_Restaurant_Id { get; set; }
    }
}