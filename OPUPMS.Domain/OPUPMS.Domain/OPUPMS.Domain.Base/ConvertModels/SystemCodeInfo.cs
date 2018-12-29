using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.ConvertModels
{
    /// <summary>
    /// 对应于表实体 Xtdm
    /// </summary>
    public class SystemCodeInfo
    {
        /// <summary>
        /// 系统代码类型 Xtdmlx00
        /// </summary>
        public string SysCodeType { get; set; }

        /// <summary>
        /// 系统代码 Xtdmdm00
        /// </summary>
        public string SysCode { get; set; }

        /// <summary>
        /// 代码名称 Xtdmmc00
        /// </summary>
        public string SysCodeName { get; set; }

        /// <summary>
        /// 系统代码状态标识 Xtdmbzs0
        /// </summary>
        public string SysCodeState { get; set; }

        /// <summary>
        /// 所属类别 Xtdmsslb
        /// </summary>
        public string CodeBelongCategory { get; set; }

        /// <summary>
        /// 备注 Xtdmbz00
        /// </summary>
        public string Remark { get; set; }
    }
}
