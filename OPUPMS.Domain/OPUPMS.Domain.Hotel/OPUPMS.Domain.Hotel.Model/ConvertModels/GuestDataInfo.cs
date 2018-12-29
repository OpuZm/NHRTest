using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 客人资料信息 对应于实体表 Krzl
    /// </summary>
    public class GuestDataInfo
    {
        /// <summary>
        /// 客人帐号 Krzlzh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 帐号类型  Krzlzhlx
        /// 是否当前房主结人 1-->是， 0-->否
        /// </summary>
        public short AccountType { get; set; }

        /// <summary>
        /// 同住序号Id  Krzltzxh
        /// </summary>
        public int? ChummageId { get; set; }

        /// <summary>
        /// 联房序号Id Krzltlxh
        /// </summary>
        public int? LinkRoomId { get; set; }

        /// <summary>
        /// 房号 Krzlfh00
        /// 关联FHDM
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 状态 Krzlzt00
        /// 关联系统代码 ZT
        /// </summary>
        public string Status { get; set; }

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
        /// 关联系统代码 XB
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 地址 Krzldz00
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 公司名称 Krzlgs00
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 结账帐号 Krzljzzh
        /// </summary>
        public int? PayBillId { get; set; }

        /// <summary>
        /// 结账方式 Krzljzfs
        /// 关联系统代码 FK
        /// </summary>
        public string PayBillMethod { get; set; }

        /// <summary>
        /// 结账货币类型 Krzljzhb
        /// 关联系统代码 HB
        /// </summary>
        public string CurrencyType { get; set; }

        /// <summary>
        /// VIP Krzlvip0
        /// 关联系统代码 KRVP
        /// </summary>
        public string VipType { get; set; }

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
        /// 原入住日期 Krzlyrzr
        /// </summary>
        public DateTime? OriginCheckInDate { get; set; }

        /// <summary>
        /// 原离店日期 Krzlyldr
        /// </summary>
        public DateTime? OriginCheckOutDate { get; set; }

        /// <summary>
        /// 换房日期 Krzlhfrq
        /// </summary>
        public DateTime? RoomChuangeDate { get; set; }

        /// <summary>
        /// 续住日期 Krzlxzrq
        /// </summary>
        public DateTime? ExtendStayDate { get; set; }

        /// <summary>
        /// 电话 Krzldh00
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 房价类别 Krzlfjlb
        /// 关联系统代码 JL
        /// </summary>
        public string RoomPriceCategory { get; set; }

        /// <summary>
        /// 房间类型 Krzlfjlx
        /// 关联系统代码 FL
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// 客人类别 Krzlkrlb
        /// 关联系统代码 KL
        /// </summary>
        public string GuestCategory { get; set; }

        /// <summary>
        /// 客人类型 Krzlkrlx
        /// 关联系统代码 TTLX
        /// </summary>
        public string GuestType { get; set; }

        /// <summary>
        /// 预订号 Krzlydh0
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// 订房明细序号 Krzldfmx
        /// 关联Dfmx
        /// </summary>
        public int BookingDetailId { get; set; }

        /// <summary>
        /// 国籍代码 Krzlgj00
        /// 关联系统代码 GJ
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 籍贯代码 Krzljg00
        /// 关联系统代码 JG
        /// </summary>
        public string NativePlaceCode { get; set; }

        /// <summary>
        /// 电子邮箱 Krzlmail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 证件类别 Krzlzjlb
        /// 关联系统代码 ZJ
        /// </summary>
        public string CredentialCategory { get; set; }

        /// <summary>
        /// 证件号码 Krzlzjhm
        /// </summary>
        public string CredentialNo { get; set; }

        /// <summary>
        /// 民族 Krzlmz00
        /// </summary>
        public string PeopleCategory { get; set; }

        /// <summary>
        /// 团号 Krzlth00
        /// </summary>
        public string TeamNo { get; set; }

        /// <summary>
        /// 团名 Krzltm00
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 客户类型（协议客户） Krzllxdm
        /// 关联 Lxdm 
        /// </summary>
        public string CustomerType { get; set; }

        /// <summary>
        /// 房租 Krzlfz00
        /// </summary>
        public decimal RoomRent { get; set; }

        /// <summary>
        /// 房全价 Krzlfqj0
        /// </summary>
        public decimal RoomFullPrice { get; set; }

        /// <summary>
        /// 优惠率，折扣率
        /// </summary>
        public decimal DiscountRate { get { return RoomRent / RoomFullPrice; } }

        /// <summary>
        /// 消费总额 Krzlxfze
        /// </summary>
        public decimal ConsumeAmount { get; set; }

        /// <summary>
        /// 余额 Krzlye00
        /// </summary>
        public decimal RemainAmount { get; set; }

        /// <summary>
        /// 上日余额 Krzlsrye
        /// </summary>
        public decimal LastTimeAmount { get; set; }

        /// <summary>
        /// 账务备注 Krzlzwbz
        /// </summary>
        public string AccRemark { get; set; }

        /// <summary>
        /// 房务备注 Krzlfwbz
        /// </summary>
        public string RoomRemark { get; set; }

        /// <summary>
        /// 卡号 Krzlkh00
        /// 关联Krls.Krlsvpkh
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 客户来源市场 Krzlscdm
        /// 关联系统代码 SC
        /// </summary>
        public string ClientSourceCode { get; set; }

        /// <summary>
        /// 特服 Krzltf00
        /// 关联系统代码 POTF
        /// </summary>
        public string SpecialServices { get; set; }

        /// <summary>
        /// 业务员 Krzlywy0
        /// 关联CZDM
        /// </summary>
        public string SellerCode { get; set; }

        /// <summary>
        /// 房价批准人 Krzlfjpz
        /// </summary>
        public string ApprovePriceUser { get; set; }

        /// <summary>
        /// 授权金额 Krzlsqje
        /// </summary>
        public decimal? AuthorizeAmount { get; set; }

        /// <summary>
        /// 分单代码 Krzlfddm
        /// 关联系统代码 FD
        /// </summary>
        public string SubReceiptCode { get; set; }

        /// <summary>
        /// 出生日期 Krzlcsrq
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 外事序号Id  Krzlwsxh
        /// 关联Krws
        /// </summary>
        public int? ForeignerDetailId { get; set; }

        /// <summary>
        /// 客人历史Id  Krzllsxh
        /// 关联Krls
        /// </summary>
        public int? GuestHistoryId { get; set; }

        /// <summary>
        /// 房间早餐数 Krzlfjsx
        /// 房价属性 含早 xtdmlx00='JL2'
        /// </summary>
        public int? RoomBreakfast { get; set; }

        /// <summary>
        /// 发票号码 Krzlfphm
        /// </summary>
        public string InvoiceCode { get; set; }

        /// <summary>
        /// 发票金额 Krzlfpje
        /// </summary>
        public decimal? InvoiceAmount { get; set; }

        /// <summary>
        /// 发票数量 Krzlfpsl
        /// </summary>
        public int? InvoiceCount { get; set; }

        /// <summary>
        /// 传真FAX   Krzlfax0
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 电话01 Krzldh01
        /// </summary>
        public string Phone01 { get; set; }

        /// <summary>
        /// 英文名01 Krzlywx1
        /// </summary>
        public string EnglishName01 { get; set; }

        /// <summary>
        /// 换房房号 Krzlhffh
        /// </summary>
        public string RoomChuangeNo { get; set; }

        /// <summary>
        /// 挂账序号Id  Krzlgzxh
        /// </summary>
        public int? ChargeLimitId { get; set; }

        /// <summary>
        /// 挂账标志 Krzlgzbz
        /// </summary>
        public int? ChargeLimitFlag { get; set; }

        /// <summary>
        /// 挂账限额 Krzlgzxe
        /// </summary>
        public decimal? ChargeLimitAmount { get; set; }

        /// <summary>
        /// 房价结构 Krzlfjjg
        /// 关联系统代码 FJJG
        /// </summary>
        public string RoomPriceStructure { get; set; }
    }
}
