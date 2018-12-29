using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.ConvertModels
{
    /// <summary>
    /// 对应于操作记录 表实体 Czjl
    /// </summary>
    public class OperateLogInfo
    {
        /// <summary>
        /// 操作时间 Czjlczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作类型 Czjllx00
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// 操作备注 Czjlczbz
        /// </summary>
        public string OperateRemark { get; set; }

        /// <summary>
        /// 用户代码，UserID  Czjlczdm
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 动作描述（名称） Czjlmc00
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 备注 Czjlbz00
        /// </summary>
        public string Remark { get; set; }
    }
}
