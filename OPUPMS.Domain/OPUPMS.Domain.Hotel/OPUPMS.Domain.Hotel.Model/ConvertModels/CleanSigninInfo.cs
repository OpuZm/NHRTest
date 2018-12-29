using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 清洁签到信息 对应于表实体 Qjbd
    /// </summary>
    public class CleanSigninInfo
    {
        /// <summary>
        /// 清洁签到序号Id 标识列主键  Qjbdxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账务日期 Qjbdzwrq
        /// </summary>
        public DateTime AccDate { get; set; }

        /// <summary>
        /// 签到（报到）方式  Qjbdbdfs
        /// </summary>
        public int? SigninMethod { get; set; }

        /// <summary>
        /// 服务人员 Qjbdfwry
        /// 关联系统代码 FWCZ
        /// </summary>
        public string ServerCode { get; set; }

        /// <summary>
        /// 操作员 Qjbdczdm
        /// 关联 CZDM
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Qjbdczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 状态 Qjbdzt00
        /// Y 分配，X 撤销
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 撤销人 Qjbdcxcz
        /// 关联 CZDM
        /// </summary>
        public string CancelUser { get; set; }

        /// <summary>
        /// 撤销时间 Qjbdcxsj
        /// </summary>
        public DateTime? CancelTime { get; set; }
    }
}
