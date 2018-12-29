using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 预授权信息 对应于实体表 Ysq0
    /// </summary>
    public class PreAuthorizationInfo
    {
        /// <summary>
        /// 预授权序号Id 标识列主键 Ysq0xh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 帐号 Ysq0zh00
        /// 关联 Krzl.Krzlzh00/Krzw.Krzwzh00
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 卡号 Ysq0kh00
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 金额 Ysq0je00
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 有效期 Ysq0yxq0
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 备注 Ysq0bz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 授权号码 Ysq0sqh0
        /// </summary>
        public string AuthNo { get; set; }

        /// <summary>
        /// 状态 Ysq0zt00
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 操作员 Ysq0czdm
        /// 关联 CZDM
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Ysq0czsj
        /// </summary>
        public DateTime? OperateTime { get; set; }

        /// <summary>
        /// 撤销人 Ysq0cxdm
        /// 关联 CZDM
        /// </summary>
        public string CancelUser { get; set; }

        /// <summary>
        /// 撤销时间 Ysq0cxsj
        /// </summary>
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 查询码 Ysq0cxm0
        /// </summary>
        public string SearchCode { get; set; }

        /// <summary>
        /// 时间 Ysq0cxm1
        /// </summary>
        public string TimeRemark { get; set; }

        /// <summary>
        /// 打印信息 Ysq0msg0
        /// </summary>
        public string PrintMsg { get; set; }
    }
}
