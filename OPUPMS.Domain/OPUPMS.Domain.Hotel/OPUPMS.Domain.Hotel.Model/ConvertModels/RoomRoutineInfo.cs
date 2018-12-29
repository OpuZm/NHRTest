using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于房间事务表实体 Fhsw
    /// </summary>
    public class RoomRoutineInfo
    {
        /// <summary>
        /// 房间事务序号 标识列主键  
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 房号 Fhswfh00
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 事务标志（状态） Fhswbzs0
        /// </summary>
        public string RoutineState { get; set; }

        /// <summary>
        /// 事务代码 Fhswswdm
        /// </summary>
        public string RoutineCode { get; set; }

        /// <summary>
        /// 原因代码 Fhswyydm
        /// </summary>
        public string ReasonCode { get; set; }

        /// <summary>
        /// 开始日期 Fhswksrq
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 终止日期 Fhswzzrq
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 操作员 Fhswczdm
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 账务日期 Fhswzwrq
        /// </summary>
        public DateTime? AccountingDate { get; set; }

        /// <summary>
        /// 操作时间 Fhswczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 备注 Fhswbz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary> 
        /// 提交人User  Fhswtjcz
        /// </summary>
        public string SubmitUser { get; set; }

        /// <summary>
        /// 提交日期 Fhswtjrq
        /// </summary>
        public DateTime? SubmitDate { get; set; }

        /// <summary>
        /// 提交时间 Fhswtjsj
        /// </summary>
        public DateTime? SubmitTime { get; set; }
    }
}
