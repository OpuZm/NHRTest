using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 客人历史信息 对应于实体表 Krls
    /// </summary>
    public class GuestHistoryInfo
    {
        /// <summary>
        /// 序号 主键 Krlsxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 中文姓名 Krlszwxm
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 英文姓名 Krlsywxm
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 性别 Krlsxb00
        /// 关联系统代码 XB
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 客人类型 Krlskrlx
        /// 关联系统代码 TTLX
        /// </summary>
        public string GuestType { get; set; }

        /// <summary>
        /// 出生日期 Krlscsrq
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 证件类别 Krlszjlb
        /// 关联系统代码 ZJ
        /// </summary>
        public string CredentialCategory { get; set; }

        /// <summary>
        /// 证件号码 Krlszjhm
        /// </summary>
        public string CredentialNo { get; set; }

        /// <summary>
        /// VIP 
        /// 关联系统代码 KRVP
        /// </summary>
        public string VipType { get; set; }

        /// <summary>
        /// 会员卡号 Krlsvpkh
        /// </summary>
        public string VipCardNo { get; set; }

        /// <summary>
        /// 工作单位名称（公司名称） Krlsdw00
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 地址 Krlsdz00
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话 Krlsdh00
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 房租，房费金额 Krlsfzze
        /// </summary>
        public decimal? RoomRentAmount { get; set; }

        /// <summary>
        /// 餐饮金额 Krlscyze
        /// </summary>
        public decimal? RestaurantAmount { get; set; }

        /// <summary>
        /// 其它金额 Krlsqtze
        /// </summary>
        public decimal? OtherAmount { get; set; }


        /// <summary>
        /// 其它付款 Krlsqtfk
        /// </summary>
        public decimal? OtherPayment { get; set; }


        /// <summary>
        /// 住店天数 Krlszdts
        /// </summary>
        public int RoomDays { get; set; }

        /// <summary>
        /// 住店次数 Krlszdcs
        /// </summary>
        public int RoomCheckIns { get; set; }

        /// <summary>
        /// 首次房号 Krlsscfh
        /// </summary>
        public string FirstRoomNo { get; set; }

        /// <summary>
        /// 首次房型 Krlsscfl
        /// </summary>
        public string FirstRoomType { get; set; }

        /// <summary>
        /// 首次价类 Krlsscjl
        /// </summary>
        public string FirstPriceCategory { get; set; }

        /// <summary>
        /// 首次房价 Krlsscfj
        /// </summary>
        public decimal? FirstRoomPrice { get; set; }

        /// <summary>
        /// 首次入住日期 Krlsscrz
        /// </summary>
        public DateTime? FirstCheckInDate { get; set; }

        /// <summary>
        /// 首次离店日期 Krlsscld
        /// </summary>
        public DateTime? FirstCheckOutDate { get; set; }

        /// <summary>
        /// 首次优惠 Krlsscyh
        /// </summary>
        public decimal? FirstDiscount { get; set; }

        /// <summary>
        /// 首次入住时间 Krlsdyrz
        /// </summary>
        public DateTime? FirstCheckInTime { get; set; }

        /// <summary>
        /// 优惠标志 Krlsyhbz
        /// </summary>
        public string DiscountFlag { get; set; }

        /// <summary>
        /// 籍贯 Krlsjg00
        /// </summary>
        public string NativePlace { get; set; }

        /// <summary>
        /// 有效日期 Krlsyxrq
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 发卡日期 Krlsfkrq
        /// </summary>
        public DateTime? ICDate { get; set; }

        /// <summary>
        /// 发卡人 Krlsfkr0
        /// </summary>
        public string CardIssuer { get; set; }

        /// <summary>
        /// 电子邮箱 Krlsmail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 卡号01 Krlskh01
        /// </summary>
        public string CardNo01 { get; set; }

        /// <summary>
        /// 挂账限额 Krlsgzxe
        /// </summary>
        public decimal? ChargeLimitAmount { get; set; }

        /// <summary>
        /// 挂账总额 Krlsgzze
        /// </summary>
        public decimal? ChargeLimitTotalAmount { get; set; }

        /// <summary>
        /// 优惠帐号 Krlsyhzh
        /// </summary>
        public string DiscountNo { get; set; }

        /// <summary>
        /// 备注 Krlsbz00
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备注 Krlsbz01
        /// </summary>
        public string Remark01 { get; set; }

        /// <summary>
        /// 备注 Krlsbz02
        /// </summary>
        public string Remark02 { get; set; }

        /// <summary>
        /// 备注 Krlsbz03
        /// </summary>
        public string Remark03 { get; set; }

        /// <summary>
        /// 消费总额 Krlsxfze
        /// </summary>
        public decimal ConsumeAmount { get; set; }

        /// <summary>
        /// 相片 Krlsxp00
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 签名图片 Krlsqm00
        /// </summary>
        public byte[] SignImage { get; set; }

        /// <summary>
        /// 国籍代码 Krlsgj00
        /// 关联系统代码 GJ
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 卡号Id Krlskhid
        /// </summary>
        public int? CardId { get; set; }

        /// <summary>
        /// 登录Id Krlsdlid
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// 传真FAX Krlsfax0
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 余额 Krlsye00
        /// </summary>
        public decimal? RemainAmount { get; set; }


        /// <summary>
        /// 积分余额 Krlsje01
        /// </summary>
        public decimal? PointRemainAmount { get; set; }

        /// <summary>
        /// 积分总额 Krlsjfze
        /// </summary>
        public decimal? PointTotalAmount { get; set; }

        /// <summary>
        /// 客人密码 Krlsmm00
        /// </summary>
        public byte[] GuestPwd { get; set; }

        /// <summary>
        /// 状态 Krlszt00
        /// 关联系统代码 ZT
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 客户类型（协议客户） Krlslxdm
        /// 关联Lxdm 
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        /// 客房标识 krlxkfbz
        /// </summary>
        public string RoomFlag { get; set; }

        /// <summary>
        /// 餐饮标识 krlxcybz
        /// </summary>
        public string RestaurantFlag { get; set; }
    }
}
