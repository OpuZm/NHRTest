using System;
using System.Collections;
using System.Collections.Generic;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class OrderPayRecordDTO
    {
        ///<summary>
        ///
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int R_Order_Id { get; set; }

        ///<summary>
        /// 付款方式(0:系统,1:现金,2:信用卡,3:会员卡,4:挂账,5:转房账,6:代金券,7:免单,8:支付宝,9:微信)
        ///</summary>
        public int CyddPayType { get; set; }

        /// <summary>
        /// 付款方式文本表示
        /// </summary>
        public string PayTypeName
        {
            get;set;
            //get
            //{
            //    return CyddPayType >= 0 ? Enum.GetName(typeof(CyddPayType), CyddPayType) : "";
            //}
        }

        ///<summary>
        /// 付款金额
        ///</summary>
        public decimal PayAmount { get; set; }

        ///<summary>
        /// 付款状态(1:未付,2:已付,3:已退)
        ///</summary>
        public CyddJzStatus CyddJzStatus { get; set; }

        /// <summary>
        /// 付款状态文本表示
        /// </summary>
        public string JzStatusName
        {
            get
            {
                return CyddJzStatus > 0 ? Enum.GetName(typeof(CyddJzStatus), CyddJzStatus) : "";
            }
        }

        /// <summary>
        /// 付款类型
        /// </summary>
        public CyddJzType CyddJzType { get; set; }
        public string JzTypeName
        {
            get
            {
                return CyddJzType > 0 ? Enum.GetName(typeof(CyddJzType), CyddJzType) : "";
            }
        }

        public Nullable<DateTime> CreateDate { get; set; }

        public int CreateUser { get; set; }

        public string CreateUserName { get; set; }

        public int SourceId { get; set; }
        
        public string SourceName { get; set; }

        public decimal Fraction { get; set; }

        public string Remark { get; set; }
        public int MarketId { get; set; }
        /// <summary>
        /// 账务日期
        /// </summary>
        public Nullable<DateTime> BillDate { get; set; }
        public int OrderMainPayId { get; set; }
        public string Pwd { get; set; }
        public int PId { get; set; }
        public int PrintNum { get; set; }
        public int R_Restaurant_Id { get; set; }
    }

    /// <summary>
    /// 订单订金实体
    /// </summary>
    public class OrderPayDeposit
    {
        public int OrderId { get; set; }

        public decimal PayAmount { get; set; }
    }

    /// <summary>
    /// 分市收银统计
    /// </summary>
    public class DayMarketStatistics
    {
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public string Date { get; set; }
        public decimal Total { get; set; }
        public List<UserDayMarketStatistics> UserList { get; set; }
    }

    public class UserDayMarketStatistics
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderPayRecordStatistics:R_OrderPayRecord
    {
        public int MarketId { get; set; }
    }

    public class RefundDepositDTO
    {
        /// <summary>
        /// 原定金记录Id
        /// </summary>
        public int OrigianlDepositId { get; set; }

        /// <summary>
        /// 当前操作用户Id
        /// </summary>
        public int CurrentUserId { get; set; }

        /// <summary>
        /// 当前分市Id
        /// </summary>
        public int CurrentMarketId { get; set; }

        public int CompanyId { get; set; }
        public int RestaurantId { get; set; }
    }

    public class OrderMainPayDTO
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime BillDate { get; set; }

        public int CreateUser { get; set; }
        public string CreateUserName { get; set; }

        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public int DiscountId { get; set; }

        public DiscountMethods DiscountType { get; set; }
        public List<int> OrderTableIds { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
