using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 应收明细信息 对应于实体表 Ysmx
    /// </summary>
    public class ReceivableDetailInfo
    {
        /// <summary>
        /// 应收明细序号Id 标识列主键 Ysmxxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应收代码 Ysmxysdm
        /// 关联系统代码 YS
        /// </summary>
        public string AccReceivableCode { get; set; }

        /// <summary>
        /// 数量 Ysmxsl00
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 单价 Ysmxdj00
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 天数 Ysmxts00
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 金额 Ysmxje00
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 日期 Ysmxrq00
        /// </summary>
        public DateTime ReceivableDate { get; set; }

        /// <summary>
        /// 发票号码 Ysmxfph0
        /// </summary>
        public string InvoiceCode { get; set; }

        /// <summary>
        /// 备注 Ysmxbz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作User Ysmxczdm
        /// 关联 Czdm
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Ysmxczsj
        /// </summary>
        public DateTime? OperateTime { get; set; }

        /// <summary>
        /// 备注01 Ysmxbz01
        /// </summary>
        public string Remark01 { get; set; }

        /// <summary>
        /// 主单Id Ysmxzdxh
        /// 关联 Ysd0.ysd0xh00
        /// </summary>
        public int? MainId { get; set; }

        /// <summary>
        /// 结单性质，账单类型  Ysmxjdxz
        /// C-付款 D-消费
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 分帐号 Ysmxfzh0
        /// </summary>
        public int? SubId { get; set; }

        /// <summary>
        /// 类型 Ysmxlx00
        /// 默认值A，A-客房 B-餐饮
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 应收原因 Ysmxysyy
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 电话 Ysmxdh00
        /// </summary>
        public string Phone { get; set; }
    }
}
