using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 客人消费信息 对应于实体表 Krxf
    /// </summary>
    public class GuestConsumeInfo
    {
        /// <summary>
        /// 客人消费序号Id  Krxfxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 卡号Id Krxfkhid
        /// 关联Krls.Krlsxh00
        /// </summary>
        public int? CardId { get; set; }

        /// <summary>
        /// 账务日期 Krxfzwrq
        /// </summary>
        public DateTime? AccDate { get; set; }

        /// <summary>
        /// 类别  Krxflb00
        /// C-充值，Y-预授权，A-消费，S-转账
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 消费点，地方 Krxfxfd0
        /// A-全部，Z-总台，1 - 一号餐厅，2 - 二号餐厅
        /// </summary>
        public string ConsumePlace { get; set; }

        /// <summary>
        /// 金额 Krxfje00
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 操作日期 Krxfrq00
        /// </summary>
        public DateTime? OperateDate { get; set; }

        /// <summary>
        /// 操作User Krxfczdm
        /// 关联 Czdm
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 备注 Krxfbz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 余额 Krxfye00
        /// </summary>
        public decimal? RemainAmount { get; set; }

        /// <summary>
        /// 分账号 Krxfzh00
        /// </summary>
        public int? SubId { get; set; }

        /// <summary>
        /// 属性 Krxfattr
        /// </summary>
        public int? PropertyValue { get; set; }

        /// <summary>
        /// 次数 Krxfje01
        /// </summary>
        public decimal? Times { get; set; }

        /// <summary>
        /// 付款方式 Krxffkfs
        /// 关联系统代码 CZFK
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 主卡序号 Krxfzkxh
        /// 主卡的krlsvpkh
        /// </summary>
        public int MainCardId { get; set; }

        /// <summary>
        /// 赠送标识 Krxfzsbz
        /// </summary>
        public string FreeFlag { get; set; }
    }
}
