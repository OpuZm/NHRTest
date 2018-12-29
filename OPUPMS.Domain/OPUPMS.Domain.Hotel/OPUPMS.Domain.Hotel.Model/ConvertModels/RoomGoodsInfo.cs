using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 房间物品信息 对应于表实体 Fjwp
    /// </summary>
    public class RoomGoodsInfo
    {
        /// <summary>
        /// 房间物品序号Id 标识列主键 Fjwpxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 房号 Fjwpfhdm
        /// 关联 Fhdm.fhdmdm00
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 物品代码 Fjwpdm00
        /// 关联系统代码 FJWP
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 数量 Fjwpsl00
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 操作员 Fjwpczdm
        /// 关联 CZDM
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Fjwpczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 客人帐号 Fjwpzh00
        /// 关联 Krzl.krzlzh00
        /// </summary>
        public int? GuestId { get; set; }

        /// <summary>
        /// 类型 Fjwplx00
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 账务日期 Fjwpzwrq
        /// </summary>
        public DateTime AccDate { get; set; }

    }
}
