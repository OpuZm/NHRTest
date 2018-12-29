using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于客人发卡 表实体 Krfk
    /// </summary>
    public class GuestCardInfo
    {
        /// <summary>
        /// 客人发卡Id 标识列主键  Krfkxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 卡号 Krfkkh00
        /// 关联 Fhdm.Fhdmdm00
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 卡状态标识 Krfkbzs0
        /// </summary>
        public string CardState { get; set; }

        /// <summary>
        /// 日期 Krfkrq00
        /// </summary>
        public DateTime? CardDate { get; set; }

        /// <summary>
        /// 操作员 Krfkczdm
        /// 关联Czdm
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 开始日期 Krfkksrq
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 失效日期 Krfksxrq
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// 撤销人 Krfksxcz
        /// 关联Czdm
        /// </summary>
        public string CancelByUser { get; set; }

        /// <summary>
        /// 卡号Id Krfkkhid
        /// </summary>
        public long? CardId { get; set; }

        /// <summary>
        /// 类型 Krfklx00
        /// 2-资料保密，4-钟点房，8-VIP，16-房价保密，32-成员自付，64-不可转账
        /// </summary>
        public short Types { get; set; }

        /// <summary>
        /// 姓名 Krfkxm00
        /// </summary>
        public string GuestName { get; set; }

        /// <summary>
        /// 备注 Krfkbz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 记账日期 Krfkjzrq
        /// </summary>
        public DateTime? PostDate { get; set; }

        /// <summary>
        /// 客人帐号 Krfkkrzh
        /// 关联 Krzl.krzlzh00
        /// </summary>
        public int? GuestId { get; set; }
    }
}
