using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于客人事务表实体 Krsw
    /// </summary>
    public class GuestRoutineInfo
    {
        /// <summary>
        /// 客人事务序号 标识列主键  Krswxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客人帐号Id Krswzh00
        /// 关联Krzl.Krzlzh00
        /// </summary>
        public int GuestId { get; set; }

        /// <summary>
        /// 事务标志（状态）  Krswbzs0
        /// </summary>
        public string RoutineState { get; set; }

        /// <summary>
        /// 事务代码 Krswswdm
        /// 关联系统代码 SW
        /// </summary>
        public string RoutineCode { get; set; }

        /// <summary>
        /// 原因代码 Krswyydm
        /// 关联系统代码
        /// </summary>
        public string ReasonCode { get; set; }

        /// <summary>
        /// 操作员 Krswczdm
        /// 关联CZDM.Czdmdm00
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 账务日期  Krswzwrq
        /// </summary>
        public DateTime? AccDate { get; set; }

        /// <summary>
        /// 操作时间 Krswczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 原始值 Krswysz0
        /// </summary>
        public string OriginalContent { get; set; }

        /// <summary>
        /// 更新值 Krswgxz0
        /// </summary>
        public string UpdateContent { get; set; }
    }
}
