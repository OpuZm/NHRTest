using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 客人账务信息 对应于实体表 Krzw
    /// </summary>
    public class GuestAccountingInfo
    {
        /// <summary>
        /// 客人账务序号Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分帐号，子帐号 Krzwfzh0
        /// </summary>
        public int ChildId { get; set; }

        /// <summary>
        /// 客人帐号Id Krzwzh00
        /// 关联Krzl.Krzlzh00 
        /// </summary>
        public int GuestId { get; set; }

        /// <summary>
        /// 关联帐号Id Krzwgrzh
        /// </summary>
        public int RelationId { get; set; }

        /// <summary>
        /// 账务日期 Krzwzwrq
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// 账务代码 Krzwzwdm
        /// 关联Zwdm.zwdmdm00 
        /// </summary>
        public string AccountingCode { get; set; }

        /// <summary>
        /// 账务类型 Krzwlx00
        /// </summary>
        public string AccountingType { get; set; }

        /// <summary>
        /// 参考号码 Krzwckhm
        /// </summary>
        public string ReferenceNo { get; set; }

        /// <summary>
        /// 消费金额 Krzwxfje
        /// </summary>
        public decimal ConsumeAmount { get; set; }

        /// <summary>
        /// 应付金额 Krzwyfje
        /// </summary>
        public decimal PayableAmount { get; set; }

        /// <summary>
        /// 核算金额 Krzwhsje
        /// </summary>
        public decimal CalculateAmount { get; set; }

        /// <summary>
        /// 付款货币类型 Krzwfkhb
        /// 关联系统代码 HB
        /// </summary>
        public string CurrencyType { get; set; }

        /// <summary>
        /// 付款方式 Krzwfkfs
        /// 关联系统代码 FK
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 结单性质，账单类型  Krzwjdxz
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 操作代码，用户Code Krzwczdm 
        /// 关联CZDM 
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Krzwczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作原因
        /// </summary>
        public string OperateReason { get; set; }

        /// <summary>
        /// 状态标识 Krzwztbz
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 备注 Krzwbz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 班次 Krzwbc00
        /// 关联系统代码 ZWBC
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// Krzwczcz
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// 客人账务序号01 Krzwxh01
        /// </summary>
        public int AccId01 { get; set; }

        /// <summary>
        /// 客人账务序号02 Krzwxh02
        /// </summary>
        public int AccId02 { get; set; }

        /// <summary>
        /// 客人账务序号03 Krzwxh03
        /// </summary>
        public int AccId03 { get; set; }
    }
}
