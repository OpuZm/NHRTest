using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    public class RoomInfoDto
    {
        /// <summary>
        /// 房号代码 Fhdmdm00
        /// </summary>
        [JsonProperty("code")] //把属性名称序列化为指定名称
        public string RoomId { get; set; }

        /// <summary>
        /// 楼座代码 Fhdmlz00
        /// </summary>
        public string GalleryCode { get; set; }

        /// <summary>
        /// 楼座名称
        /// </summary>
        public string GalleryName { get; set; }

        /// <summary>
        /// 楼层代码 Fhdmlc00
        /// </summary>
        public string FloorCode { get; set; }

        /// <summary>
        /// 楼层名称
        /// </summary>
        public string FloorName { get; set; }

        /// <summary>
        /// 房型代码 Fhdmlx00
        /// </summary>
        [JsonProperty("roomtype")]
        public string RoomTypeCode { get; set; }

        /// <summary>
        /// 房型名称
        /// </summary>
        [JsonProperty("title")]
        public string RoomTypeName { get; set; }

        /// <summary>
        /// 单价 Fhdmdj00
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 状态 Fhdmzt00
        /// </summary>
        [JsonProperty("state")]
        public string Status { get; set; }

        /// <summary>
        /// 是否清洁 Fhdmcd00
        /// </summary>
        public string IsClean { get; set; }

        /// <summary>
        /// 房间状态 Fhdmft00
        /// </summary>
        public string RoomState { get; set; }


        /// <summary>
        /// 客人帐号 Krzlzh00
        /// </summary>
        public int? GuestId { get; set; }

        /// <summary>
        /// 帐号类型  Krzlzhlx
        /// </summary>
        public short? AccountType { get; set; }

        /// <summary>
        /// 同住序号Id  Krzltzxh
        /// </summary>
        public int? ChummageId { get; set; }

        /// <summary>
        /// 联房序号Id Krzltlxh
        /// </summary>
        public int? LinkRoomId { get; set; }
        
        /// <summary>
        /// 中文名 Krzlzwxm
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 英文名 Krzlywxm
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 性别 Krzlxb00
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 地址 Krzldz00
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 结账帐号 Krzljzzh
        /// </summary>
        public int? PayBillId { get; set; }

        /// <summary>
        /// 结账方式 Krzljzfs
        /// </summary>
        public string PayBillMethod { get; set; }

        /// <summary>
        /// VIP Krzlvip0
        /// </summary>
        public string IsVIP { get; set; }

        /// <summary>
        /// 客人备注 Krzlkrbz
        /// </summary>
        public string GuestRemark { get; set; }

        /// <summary>
        /// 入住日期 Krzlrzrq
        /// </summary>
        public DateTime? CheckInDate { get; set; }

        /// <summary>
        /// 入住时间 Krzlrzsj
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// 离店日期 Krzlldrq
        /// </summary>
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// 离店时间 Krzlldsj
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// 电话 Krzldh00
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 房价类别 Krzlfjlb
        /// </summary>
        public string RoomPriceCategory { get; set; }

        /// <summary>
        /// 房价类别名称
        /// </summary>
        public string RoomPriceCategoryName { get; set; }

        /// <summary>
        /// 客人类别 Krzlkrlb
        /// </summary>
        public string GuestCategory { get; set; }

        /// <summary>
        /// 客人类型 Krzlkrlx
        /// </summary>
        public string GuestType { get; set; }

        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 总付款金额
        /// </summary>
        public decimal TotalPayment { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 预订号 Krzlydh0
        /// </summary>
        public int? BookingNo { get; set; }

        /// <summary>
        /// 预订人姓名
        /// </summary>
        public string BookingGuestName { get; set; }
        
        /// <summary>
        /// 预订的入住日期
        /// </summary>
        public DateTime? BookingCheckInDate { get; set; }

        /// <summary>
        /// 预订的离店日期
        /// </summary>
        public DateTime? BookingCheckOutDate { get; set; }

        /// <summary>
        /// 预订人电话
        /// </summary>
        public string BookingPhone { get; set; }

        /// <summary>
        /// 预订备注
        /// </summary>
        public string BookingRemark { get; set; }

        /// <summary>
        /// 早餐
        /// </summary>
        public string Breakfast { get; set; }
    }
}
