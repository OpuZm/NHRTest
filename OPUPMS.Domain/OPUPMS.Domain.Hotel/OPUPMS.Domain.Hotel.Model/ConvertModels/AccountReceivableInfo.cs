using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 应收账（AR）信息 对应于实体表 Arz0
    /// </summary>
    public class AccountReceivableInfo
    {
        /// <summary>
        /// 应收账（AR）信息Id 主键标识列 Arz0xh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客人账务Id  Arz0zwxh
        /// 关联 Krzw.krzwxh00
        /// </summary>
        public int GuestAccId { get; set; }

        /// <summary>
        /// 操作员 Arz0czdm
        /// 关联 CZDM
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Arz0czsj
        /// </summary>
        public DateTime? OperateTime { get; set; }

        /// <summary>
        /// 类型 Arz0lx00
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 撤销人 Arz0cxcz
        /// 关联 CZDM
        /// </summary>
        public string CancelUser { get; set; }

        /// <summary>
        /// 撤销时间 Arz0cxsj
        /// </summary>
        public DateTime? CancelTime { get; set; }
    }
}
