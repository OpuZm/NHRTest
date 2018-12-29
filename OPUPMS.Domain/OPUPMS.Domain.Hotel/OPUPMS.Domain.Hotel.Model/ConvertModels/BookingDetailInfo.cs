using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于订房详细 表实体 Dfmx
    /// </summary>
    public class BookingDetailInfo
    {
        /// <summary>
        /// 序号主键 Dfmxxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 预订号Id Dfmxydh0
        /// 关联 Dfzl.dfzlydh0
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// 状态  Dfmxzt00
        /// 关联系统代码 ZT
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 预订日期 Dfmxydrq
        /// </summary>
        public DateTime? BookingDate { get; set; }

        /// <summary>
        /// 入住日期 Dfmxrzrq
        /// </summary>
        public DateTime? CheckInDate { get; set; }

        /// <summary>
        /// 离店日期 Dfmxldrq
        /// </summary>
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// 取消日期 Dfmxqxrq
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 失效日期 Dfmxsxrq
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// 房价类别  Dfmxfjlb
        /// 关联系统代码 JL
        /// </summary>
        public string RoomPriceCategory { get; set; }

        /// <summary>
        /// 人数 Dfmxrs00
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 房型 Dfmxfl00
        /// 关联系统代码 FL
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// 房价 Dfmxfj00
        /// </summary>
        public decimal RoomPrice { get; set; }

        /// <summary>
        /// （预订）房数 Dfmxfs00
        /// </summary>
        public int RoomNum { get; set; }

        /// <summary>
        /// 确入房间数(排房房数)  Dfmxqr00
        /// </summary>
        public int ConfirmRoomNum { get; set; }

        /// <summary>
        /// 实际入住房数 Dfmxrz00
        /// </summary>
        public int ActualStayRoomNum { get; set; }

        /// <summary>
        /// 取消房间数 Dfmxqx00
        /// </summary>
        public int CancelRoomNum { get; set; }

        /// <summary>
        /// 操作员 Dfmxczdm
        /// 关联CZDM
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 操作时间 Dfmxczsj
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 房间属性 Dfmxfjsx
        /// 2-资料保密，4-钟点房，8-VIP，16-房价保密，32-成员自付，64-不可转账
        /// </summary>
        public int? RoomProperty { get; set; }

        /// <summary>
        /// 升级序号  Dfmxsjxh
        /// 关联原Dfmxxh00
        /// </summary>
        public int? UpGradeId { get; set; }
    }
}
