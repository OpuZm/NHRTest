using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于订房资料表实体 Dfzl
    /// </summary>
    public class BookingInfo
    {
        /// <summary>
        /// 预订号 主键标识列 Dfzlydh0
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 状态  Dfzlzt00
        /// 关联系统代码 ZT
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 团号 Dfzlth00
        /// </summary>
        public string TeamNo { get; set; }

        /// <summary>
        /// 团名 Dfzltm00
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 预订类别，类型 Dfzllb00
        /// 关联系统代码 YD
        /// </summary>
        public string BookingCategory { get; set; }

        /// <summary>
        /// 人数 Dfzlrs00
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 房数 Dfzlfs00
        /// </summary>
        public int RoomNum { get; set; }

        /// <summary>
        /// 预订日期 Dfzlydrq
        /// </summary>
        public DateTime? BookingDate { get; set; }

        /// <summary>
        /// 失效日期 Dfzlsxrq
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// 入住日期 Dfzlrzrq
        /// </summary>
        public DateTime? CheckInDate { get; set; }

        /// <summary>
        /// 离店日期 Dfzlldrq
        /// </summary>
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// 客户类型（协议客户） Dfzllxdm
        /// 关联LXDM 
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        /// 联系人 Dfzllxr0
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系备注（联系方式）  Dfzllxbz
        /// </summary>
        public string ContactRemark { get; set; }

        /// <summary>
        /// 国籍代码 Dfzlgj00
        /// 关联系统代码 GJ
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 客人类别 Dfzlkrlb
        /// 关联系统代码 KL
        /// </summary>
        public string GuestCategory { get; set; }

        /// <summary>
        /// 房价类别  Dfzlfjlb
        /// 关联系统代码 JL
        /// </summary>
        public string RoomPriceCategory { get; set; }

        /// <summary>
        /// 结账方式 Dfzljzfs
        /// 关联系统代码 FK
        /// </summary>
        public string PayBillMethod { get; set; }

        /// <summary>
        /// 结账货币类型 Dfzljzhb
        /// 关联系统代码 HB
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 分单代码 Dfzlfddm
        /// 关联系统代码 FD
        /// </summary>
        public string SubReceiptCode { get; set; }

        /// <summary>
        /// 客人备注 Dfzlkrbz
        /// </summary>
        public string GuestRemark { get; set; }

        /// <summary>
        /// 账务备注  Dfzlzwbz
        /// </summary>
        public string AccRemark { get; set; }

        /// <summary>
        /// 业务员 Dfzlywy0
        /// 关联CZDM 
        /// </summary>
        public string SellerCode { get; set; }

        /// <summary>
        /// 入住时间 Dfzlrzsj
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// 离店时间 Dfzlldsj
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// 房间早餐数 Dfzlfjsx
        /// 房价属性 含早 
        /// 关联系统代码'JL2'
        /// </summary>
        public int? RoomBreakfast { get; set; }

        /// <summary>
        /// 订房备注/接待  Dfzldfbz
        /// </summary>
        public string BookingRemark { get; set; }

        /// <summary>
        /// 客户来源市场 Dfzlscdm
        /// 关联系统代码 SC
        /// </summary>
        public string ClientSourceCode { get; set; }

        /// <summary>
        /// 房价结构 Dfzlfjjg
        /// 关联系统代码 FJJG
        /// </summary>
        public string RoomPriceStructure { get; set; }

        /// <summary>
        /// 特服  Dfzltf00
        /// 关联系统代码 POTF
        /// </summary>
        public string SpecialServices { get; set; }

        /// <summary>
        /// VIP 类型  Dfzlvip0
        /// 关联系统代码 KRVP
        /// </summary>
        public string VipType { get; set; }

        /// <summary>
        /// 挂账限额 Dfzlgzxe
        /// </summary>
        public decimal? ChargeLimitAmount { get; set; }

        /// <summary>
        /// 佣金代码 Dfzlyjdm
        /// 关联系统代码 YJDM
        /// </summary>
        public string CommisionCode { get; set; }

        /// <summary>
        /// 预计人数 Dfzlyjrs
        /// </summary>
        public int? EstimateNum { get; set; }

        ///// <summary>
        ///// 合同号
        ///// </summary>
        //public string AgreementNo { get; set; }

        /// <summary>
        /// 特殊备注 Dfzlydbz
        /// </summary>
        public string SpecialRemark { get; set; }

        /// <summary>
        /// 审核标识 Dfzlshbz
        /// </summary>
        public string VerifyFlag { get; set; }
    }
}
