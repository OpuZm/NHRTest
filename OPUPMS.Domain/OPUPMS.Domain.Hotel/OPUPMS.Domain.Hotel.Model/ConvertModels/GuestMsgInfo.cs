using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于客人留言表实体 Krly
    /// </summary>
    public class GuestMsgInfo
    {
        /// <summary>
        /// 客人留言Id 标识列主键  Krlyxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客人帐号Id Krlyzh00
        /// 关联Krzl.Krzlzh00
        /// </summary>
        public int GuestId { get; set; }

        /// <summary>
        /// 留言类型 Krlylx00
        /// 关联系统代码 LYLX
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 留言状态 Krlyzt00
        /// X-未读，D-取消
        /// </summary>
        public string MsgState { get; set; }

        /// <summary>
        /// 留言主题 Krlybt00
        /// </summary>
        public string MsgTopic { get; set; }

        /// <summary>
        /// 操作员 Krlyczdm
        /// 关联CZDM.Czdmdm00
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Krlyczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 内容 Krlytext
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 取消时间 Krlyqxsj
        /// </summary>
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 取消操作人 Krlyqxcz
        /// 关联Czdm
        /// </summary>
        public string CancelByUser { get; set; }

        /// <summary>
        /// 取消日期 Krlysxrq
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 权限备注 Krlyqxbz
        /// 里面czdm或者系统代码qxzb的数据用逗号，隔开
        /// </summary>
        public string PermissionRemark { get; set; }

        /// <summary>
        /// 读取过留言的用户列表  Krlyyd00
        /// 操作已读，填入czdmdm00,逗号隔断
        /// </summary>
        public string ReadUsers { get; set; }

        /// <summary>
        /// 开始日期 Krlyksrq
        /// </summary>
        public DateTime? StartDate { get; set; }
    }
}
