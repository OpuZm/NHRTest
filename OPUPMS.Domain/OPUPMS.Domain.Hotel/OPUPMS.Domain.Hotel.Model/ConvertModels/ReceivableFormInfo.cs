using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 应收单信息 对应于实体表 Ysd0
    /// </summary>
    public class ReceivableFormInfo
    {
        /// <summary>
        /// 应收单序号Id 标识列主键 Ysd0xh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 团号 Ysd0th00
        /// </summary>
        public string TeamNo { get; set; }

        /// <summary>
        /// 团名 Ysd0tm00
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 客户类型（协议客户）  Ysd0lxdm
        /// 关联 Lxdm 
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        /// 入住日期 Ysd0rzrq
        /// </summary>
        public DateTime? CheckInDate { get; set; }

        /// <summary>
        /// 离店日期 Ysd0ldrq
        /// </summary>
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// 人数 Ysd0rs00
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 发票号码 Ysd0fph0
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 日期 Ysd0rq00
        /// </summary>
        public DateTime ReceivabelDate { get; set; }

        /// <summary>
        /// 应收金额 Ysd0ysje
        /// </summary>
        public decimal ReceivableAmount { get; set; }

        /// <summary>
        /// 已付金额 Ysd0yfje
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 其它金额 Ysd0qtje
        /// </summary>
        public decimal OtherAmount { get; set; }

        /// <summary>
        /// 操作User Ysd0czdm
        /// 关联 Czdm
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 标识，状态 Ysd0bzs0
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 备注 Ysd0bz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 客人帐号 Ysd0krzh
        /// 关联 Krzl.Krzlzh00
        /// </summary>
        public int? GuestId { get; set; }

        /// <summary>
        /// 余额 Ysd0ye00
        /// </summary>
        public decimal? RemainAmount { get; set; }

        /// <summary>
        /// 记账日期 Ysd0jzrq
        /// </summary>
        public DateTime? PostDate { get; set; }

        /// <summary>
        /// 记账操作，备注  Ysd0jzcz
        /// </summary>
        public string PostRemark { get; set; }
    }
}
