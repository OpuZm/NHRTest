using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_Invoice
    {
        #region Model
        public int Id { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public int Company { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }
        /// <summary>
        /// 订单主结表ID
        /// </summary>
        public int R_OrderMainPay_Id { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUser { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 开单账务日期
        /// </summary>
        public Nullable<DateTime>BillDate { get; set; }
        #endregion Model
    }
}
