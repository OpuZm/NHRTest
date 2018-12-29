using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 客人住店信息 对应于实体表 Krzd
    /// </summary>
    public class GuestRoomDailyInfo
    {
        /// <summary>
        /// 日期 组合主键 Krzdrq00
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 客人帐号 组合主键 Krzdzh00
        /// 取值Krzl.Krzlzh00
        /// </summary>
        public int GuestId { get; set; }

        /// <summary>
        /// 结账帐号 Krzdjzzh
        /// 取值Krzl.Krzljzzh
        /// </summary>
        public int? PayBillId { get; set; }

        /// <summary>
        /// 预订号  Krzdydh0
        /// Krzl.Krzlydh0
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// 状态 Krzdzt00
        /// 取值 Krzl.Krzlzt00
        /// 关联系统代码 ZT
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 房号 Krzdfh00
        /// 取值 Krzl.Krzlfh00
        /// 关联FHDM
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 中文名  Krzdzwxm
        /// 取值 Krzl.Krzlzwxm
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 英文名  Krzdywxm
        /// 取值 Krzl.Krzlywxm
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 性别  Krzdxb00
        /// 取值 Krzl.Krzlxb00
        /// 关联系统代码 XB
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 入住日期  Krzdrzrq
        /// 取值 Krzl.Krzlrzrq
        /// </summary>
        public DateTime? CheckInDate { get; set; }

        /// <summary>
        /// 离店日期  Krzdldrq
        /// 取值 Krzl.Krzlldrq
        /// </summary>
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// 客户类型（协议客户） Krzdlxdm
        /// 取值 Krzl.Krzllxdm
        /// 关联系统代码 
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        /// VIP  Krzdvip0
        /// 取值 Krzl.Krzlvip0
        /// 关联系统代码 KRVP
        /// </summary>
        public string VipType { get; set; }

        /// <summary>
        /// 房价类别  Krzdfjlb
        /// 取值 Krzl.Krzlfjlb
        /// 关联系统代码 JL
        /// </summary>
        public string RoomPriceCategory { get; set; }

        /// <summary>
        /// 客人备注  Krzdkrbz
        /// 取值 Krzl.Krzlkrbz
        /// </summary>
        public string GuestRemark { get; set; }

        /// <summary>
        /// 账务备注  Krzdzwbz
        /// 取值 Krzl.Krzlzwbz
        /// </summary>
        public string AccRemark { get; set; }

        /// <summary>
        /// 房价，房租 Krzdfz00
        /// 取值 Krzl.Krzlfz00
        /// </summary>
        public decimal RoomRent { get; set; }

        /// <summary>
        /// 消费总额  Krzdxfze
        /// 取值 Krzl.Krzlxfze
        /// </summary>
        public decimal ConsumeAmount { get; set; }

        /// <summary>
        /// 上日余额  Krzdsrye
        /// 取值 Krzl.Krzlsrye
        /// </summary>
        public decimal LastTimeAmount { get; set; }

        /// <summary>
        /// 余额 Krzdye00
        /// 取值 Krzl.Krzlye00
        /// </summary>
        public decimal RemainAmount { get; set; }


        /// <summary>
        /// 客人类别  Krzdkrlb
        /// 取值 Krzl.Krzlkrlb
        /// 关联系统代码 KL
        /// </summary>
        public string GuestCategory { get; set; }

        /// <summary>
        /// 客人类型  Krzdkrlx
        /// 取值 Krzl.Krzlkrlx
        /// 关联系统代码 TTLX
        /// </summary>
        public string GuestType { get; set; }

        /// <summary>
        /// 团号 Krzdth00
        /// 取值 Krzl.Krzlth00
        /// </summary>
        public string TeamNo { get; set; }

        /// <summary>
        /// 同住序号Id   Krzdtzxh
        /// 取值 Krzl.Krzltzxh
        /// </summary>
        public int? ChummageId { get; set; }

        /// <summary>
        /// 帐号类型  Krzdzhlx
        /// 取值 Krzl.Krzlzhlx
        /// </summary>
        public short? AccountType { get; set; }

        /// <summary>
        /// 业务员 Krzdywy0
        /// 取值 Krzl.Krzlywy0
        /// 关联CZDM
        /// </summary>
        public string SellerCode { get; set; }

        /// <summary>
        /// 房间类型  Krzdfjlx
        /// 取值 Krzl.Krzlfjlx
        /// 关联系统代码 FL
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// 卡号 Krzdkh00
        /// 取值 Krzl.Krzlkh00
        /// 关联Krls.Krlsvpkh
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 房价结构  Krzdfjjg
        /// 取值 Krzl.Krzlfjjg
        /// 关联系统代码 FJJG
        /// </summary>
        public string RoomPriceStructure { get; set; }

        /// <summary>
        /// 订房明细序号  KrzdDfmx
        /// 取值 Krzl.Krzldfmx
        /// 关联Dfmx
        /// </summary>
        public int BookingDetailId { get; set; }

        /// <summary>
        /// 房间早餐数  Krzdfjsx
        /// 取值 Krzl.Krzlfjsx
        /// 房价属性 含早 xtdmlx00='JL2'
        /// </summary>
        public int? RoomBreakfast { get; set; }

        /// <summary>
        /// 预订类型，类别 Krzdydlb
        /// 关联系统代码 YD
        /// </summary>
        public string BookingTpye { get; set; }
    }
}
