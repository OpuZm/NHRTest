using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 清洁分配信息 对应于表实体 Qjfp
    /// </summary>
    public class CleanDistributionInfo
    {
        /// <summary>
        /// 清洁分配序号Id 标识列主键  Qjfpxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账务日期 Qjfpzwrq
        /// </summary>
        public DateTime AccDate { get; set; }

        /// <summary>
        /// 服务人员 Qjfpfwry
        /// 关联系统代码 FWCZ
        /// </summary>
        public string ServerCode { get; set; }

        /// <summary>
        /// 房号 Qjfpfhdm
        /// 关联 Fhdm.fhdmdm00
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 操作员 Qjfpfpcz
        /// 关联 CZDM
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 分配时间 Qjfpfpsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 状态 Qjfpzt00
        /// Y 分配，X 撤销
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 清洁时间 Qjfpqjsj
        /// </summary>
        public DateTime? CleanTime { get; set; }

        /// <summary>
        /// 撤销人 Qjfpcxcz
        /// 关联 CZDM
        /// </summary>
        public string CancelUser { get; set; }

        /// <summary>
        /// 撤销时间 Qjfpcxsj
        /// </summary>
        public DateTime? CancelTime { get; set; }

    }
}
