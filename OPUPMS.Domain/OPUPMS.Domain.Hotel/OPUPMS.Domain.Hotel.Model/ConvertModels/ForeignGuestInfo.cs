using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于客人外事 表实体 Krws
    /// </summary>
    public class ForeignGuestInfo
    {
        /// <summary>
        /// 客人外事Id 标识列主键  Krwsxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客人帐号Id  Krwszh00
        /// 关联 Krzl.krzlzh00
        /// </summary>
        public int? GuestId { get; set; }

        /// <summary>
        /// 入境事由 Krwsrjsy
        /// 关联系统代码 SY
        /// </summary>
        public string EntryReason { get; set; }

        /// <summary>
        /// 入境日期 Krwsrjrq
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// 入境口岸 Krwsrjka
        /// 关联系统代码 KA
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// 签证类别 Krwsqzlb
        /// 关联系统代码 QZ
        /// </summary>
        public string VisaCategory { get; set; }

        /// <summary>
        /// 签证帐号 Krwsqzzh
        /// </summary>
        public string VisaNo { get; set; }

        /// <summary>
        /// 签证有效期 Krwsqzyx
        /// </summary>
        public DateTime? VisaEffectiveDate { get; set; }

        /// <summary>
        /// 签发单位 Krwsqfdw
        /// 关联系统代码 CS
        /// </summary>
        public string IssuedBy { get; set; }

        /// <summary>
        /// 备注 Krwsbz00
        /// </summary>
        public string Remark { get; set; }
    }
}
