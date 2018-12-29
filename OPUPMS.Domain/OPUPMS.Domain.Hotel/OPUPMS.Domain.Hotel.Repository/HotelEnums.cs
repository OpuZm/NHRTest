using OPUPMS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Repository
{
    /// <summary>
    /// 系统代码
    /// </summary>
    public sealed class SystemCodeTypes
    {
        #region 财务相关代码
        /// <summary>
        /// 账务类型码 -- "CK"
        /// </summary>
        public static readonly string ACC_TYPE = "CK";

        /// <summary>
        /// 财务日报类别码 -- "CWRB"
        /// </summary>
        public static readonly string ACC_DAILY = "CWRB";

        /// <summary>
        /// 账务班次代码 -- "ZWBC"
        /// </summary>
        public static readonly string ACC_WORKING_SHIFT = "ZWBC";

        /// <summary>
        /// 账类代码 -- "ZL"
        /// </summary>
        public static readonly string BILL_CATEGORY = "ZL";

        #endregion


        #region 地区相关代码
        /// <summary>
        /// 地区代码 -- "DQ"
        /// </summary>
        public static readonly string AREA_CATEGORY = "DQ";

        /// <summary>
        /// 城市代码 -- "CS"
        /// </summary>
        public static readonly string CITY_CODE = "CS";

        /// <summary>
        /// 城市县份代码 -- "CSXF"
        /// </summary>
        public static readonly string COUNTY_CODE = "CSXF";

        /// <summary>
        /// 省份代码 -- "SF"
        /// </summary>
        public static readonly string PROVINCE_CODE = "SF";

        /// <summary>
        /// 国家代码 -- "GJ"
        /// </summary>
        public static readonly string COUNTRY_CODE = "GJ";

        #endregion


        #region 房间相关代码
        /// <summary>
        /// 房类代码 -- "FL"
        /// </summary>
        public static readonly string ROOM_CATEGORY = "FL";

        /// <summary>
        /// 房间房态事务代码 - "FTSW"
        /// </summary>
        public static readonly string ROOM_AFFAIR = "FTSW";

        /// <summary>
        /// 房态类别 -- "FT"
        /// </summary>
        public static readonly string ROOM_USING_STATE = "FT";

        /// <summary>
        /// 房间状态类别码 -- "FJZT"
        /// </summary>
        public static readonly string ROOM_STATE_TYPE = "FJZT";

        /// <summary>
        /// 房价结构代码 -- "FJJG"
        /// </summary>
        public static readonly string ROOM_PRICE_STRUCT = "FJJG";

        /// <summary>
        /// 房价属性代码 -- "FJSX"
        /// </summary>
        public static readonly string ROOM_PRICE_ATTR = "FJSX";

        /// <summary>
        /// 房价折扣代码 -- "FJZK"
        /// </summary>
        public static readonly string ROOM_DISCOUNT_TYPE = "FJZK";

        /// <summary>
        /// 房间楼层代码 -- "LC"
        /// </summary>
        public static readonly string ROOM_FLOOR_CODE = "LC";

        /// <summary>
        /// 房间楼座代码 -- "LZ"
        /// </summary>
        public static readonly string ROOM_GALLERY_CODE = "LZ";

        #endregion


        #region 客户相关代码

        /// <summary>
        /// 电话等级代码 -- "DHLX"
        /// </summary>
        public static readonly string TEL_LEVEL_CODE = "DHLX";

        /// <summary>
        /// 电话入账状态代码 -- "DHZT"
        /// </summary>
        public static readonly string TEL_ENTRY_STATE_CODE = "DHZT";

        /// <summary>
        /// 充值方式代码 -- "CZFK"
        /// </summary>
        public static readonly string RECHARGE_METHOD = "CZFK";

        /// <summary>
        /// 充值赠送类别代码 -- "CZZS"
        /// </summary>
        public static readonly string RECHARGE_GIVE_AWAY = "CZZS";

        /// <summary>
        /// 付款方式代码 -- "FK"
        /// </summary>
        public static readonly string PAYMENT_METHOD = "FK";

        /// <summary>
        /// 货币代码 -- "HB"
        /// </summary>
        public static readonly string CURRENCY_CODE = "HB";

        /// <summary>
        /// 会议场次 -- "HYCC"
        /// </summary>
        public static readonly string MEETING_SHIFT = "HYCC";

        /// <summary>
        /// 协议客户类型代码 -- "KHLX"
        /// </summary>
        public static readonly string CUSTOMER_TYPE = "KHLX";

        /// <summary>
        /// 客人类型代码 -- "KL"
        /// </summary>
        public static readonly string GUEST_TYPE = "KL";

        /// <summary>
        /// 留言类型代码 -- "LYLX"
        /// </summary>
        public static readonly string LEAVE_MESSAGE_TYPE = "LYLX";

        /// <summary>
        /// 口岸代码 -- "KA"
        /// </summary>
        public static readonly string PORT_CODE = "KA";

        /// <summary>
        /// 签证类型代码 -- "QZ"
        /// </summary>
        public static readonly string VISA_TYPE = "QZ";

        /// <summary>
        /// 签证事由代码 -- "SY"
        /// </summary>
        public static readonly string VISA_REASON = "SY";

        /// <summary>
        /// 客户市场来源代码 -- "SC"
        /// </summary>
        public static readonly string MARKET_TYPE = "SC";

        /// <summary>
        /// 团队类型代码 -- "TTLX"
        /// </summary>
        public static readonly string TEAM_TYPE = "TTLX";

        /// <summary>
        /// 性别代码 -- "XB"
        /// </summary>
        public static readonly string GENDER = "XB";

        /// <summary>
        /// 证件类别代码 -- "ZJ"
        /// </summary>
        public static readonly string CREDENTIAL_CATEGORY = "ZJ";
        #endregion


        #region 事务相关代码

        /// <summary>
        /// 事务代码 -- "SW"
        /// </summary>
        public static readonly string AFFAIR_TYPE = "SW";

        /// <summary>
        /// 事务-维修原因 -- "Y00"
        /// </summary>
        public static readonly string REASON_REPAIR = "Y00";

        /// <summary>
        /// 事务-入住原因 -- "Y01"
        /// </summary>
        public static readonly string REASON_CHECKIN = "Y01";

        /// <summary>
        /// 事务-离店原因 -- "Y02"
        /// </summary>
        public static readonly string REASON_CHECKOUT = "Y02";

        /// <summary>
        /// 事务-修改房价原因 -- "Y03"
        /// </summary>
        public static readonly string REASON_CHANGE_PRICE = "Y03";

        /// <summary>
        /// 事务-换房原因 -- "Y04"
        /// </summary>
        public static readonly string REASON_CHANGE_ROOM = "Y04";

        /// <summary>
        /// 事务-续住原因 -- "Y05"
        /// </summary>
        public static readonly string REASON_EXTEND_STAY = "Y05";

        /// <summary>
        /// 事务-转散原因 -- "Y06"
        /// </summary>
        public static readonly string REASON_TO_WALKIN = "Y06";

        /// <summary>
        /// 事务-转团原因 -- "Y07"
        /// </summary>
        public static readonly string REASON_TO_TEAM = "Y07";

        /// <summary>
        /// 事务-重入原因 -- "Y08"
        /// </summary>
        public static readonly string REASON_REENTRY = "Y08";

        /// <summary>
        /// 事务-暂离原因 -- "Y09"
        /// </summary>
        public static readonly string REASON_AFK = "Y09";

        /// <summary>
        /// 事务-追回原因 -- "Y10"
        /// </summary>
        public static readonly string REASON_CLAWBACK = "Y10";

        /// <summary>
        /// 事务-预订原因 -- "Y11"
        /// </summary>
        public static readonly string REASON_BOOKING = "Y11";

        /// <summary>
        /// 事务-取消原因 -- "Y12"
        /// </summary>
        public static readonly string REASON_CANCEL = "Y12";

        /// <summary>
        /// 事务-修改预订原因 -- "Y15"
        /// </summary>
        public static readonly string REASON_BOOKING_CHANGE = "Y15";

        /// <summary>
        /// 事务-冲账原因 -- "Y16"
        /// </summary>
        public static readonly string REASON_CHARGEOFF = "Y16";

        /// <summary>
        /// 事务-锁房原因 -- "Y17"
        /// </summary>
        public static readonly string REASON_LOCK_ROOM = "Y17";


        /// <summary>
        /// 事务-佣金冲账 -- "Y20"
        /// </summary>
        public static readonly string COMMISSION_CHARGEOFF = "Y20";

        #endregion


        #region 其它代码

        /// <summary>
        /// 价类代码 -- "JL"
        /// </summary>
        public static readonly string PRICE_CATEGORY = "JL";

        /// <summary>
        /// 统计类别代码 -- "TJ"
        /// </summary>
        public static readonly string SUMMARY_CATEGORY = "TJ";
        #endregion
    }


    /// <summary>
    /// 房间使用状态类型
    /// </summary>
    public sealed class RoomUsingState
    {
        /// <summary>
        /// 空房状态码 -- "V"
        /// </summary>
        public static readonly string EMPTY = "V";

        /// <summary>
        /// 在住房状态码 -- "O"
        /// </summary>
        public static readonly string HOUSING = "O";

        /// <summary>
        /// 维修状态码 -- "B"
        /// </summary>
        public static readonly string REPAIR = "B";

        /// <summary>
        /// 锁房状态码 -- "I"
        /// </summary>
        public static readonly string LOCK = "I";
    }



    /// <summary>
    /// 房态描述类型
    /// </summary>
    public sealed class RoomStateTypes
    {
        /// <summary>
        /// 空干净房状态码 -- "VC"
        /// </summary>
        public static readonly string EMPTY_CLEND = "VC";

        /// <summary>
        /// 空脏房状态码 -- "VD"
        /// </summary>
        public static readonly string EMPTY_DIRTY = "VD";

        /// <summary>
        /// 预抵房状态码 -- "EA"
        /// </summary>
        public static readonly string EXPECT_ARRIVAL = "EA";

        /// <summary>
        /// 预离房状态码 -- "ED"
        /// </summary>
        public static readonly string EXPECT_DEPARTURE = "ED";

        /// <summary>
        /// 免费房状态码 -- "CP"
        /// </summary>
        public static readonly string FREE = "CP";

        /// <summary>
        /// 在住干净房 -- "OC"
        /// </summary>
        public static readonly string HOUSING_CLENG = "OC";

        /// <summary>
        /// 在住脏房状态码 -- "OD"
        /// </summary>
        public static readonly string HOUSING_DIRTY = "OD";

        /// <summary>
        /// 维修房状态码 -- "BD"
        /// </summary>
        public static readonly string REPAIR = "BD";

        /// <summary>
        /// 锁房、封房状态码 -- "ID"
        /// </summary>
        public static readonly string LOCK = "ID";
    }


    /// <summary>
    /// 性别描述
    /// </summary>
    public sealed class GenderDescTypes
    {
        /// <summary>
        /// 女性
        /// </summary>
        [Description("女", "blue")]
        public const string Female = "F";

        /// <summary>
        /// 男性
        /// </summary>
        [Description("男", "blue")]
        public const string Male = "M";

    }


    /// <summary>
    /// 客人资料信息状态类型 --> Krzlzt00
    /// </summary>
    public sealed class GuestInfoState
    {
        /// <summary>
        /// 在住状态 -- "I"
        /// </summary>
        public const string I = "I";

        /// <summary>
        /// 预定取消状态 -- "C"
        /// </summary>
        public const string C = "C";

        /// <summary>
        /// 预定状态 -- "N"
        /// </summary>
        public const string N = "N";

        /// <summary>
        /// 离店状态 -- "O"
        /// </summary>
        public const string O = "O";

        /// <summary>
        /// 暂离状态 -- "T"
        /// </summary>
        public const string T = "T";
    }


    /// <summary>
    /// 客人账务类型 --> Krzwlx00
    /// </summary>
    public sealed class GuestAccType
    {
        /// <summary>
        /// 正常（若是迷你吧等，则与krzwztbz中的H对应）
        /// </summary>
        public const string A = "A";

        /// <summary>
        /// 迷你吧，商务中心等有汇总的大项（与krzwztbz的A对应）
        /// </summary>
        public const string H = "H";

        /// <summary>
        /// 大项的冲账（迷你吧，商务中心等有汇总的小项的冲账是汇总不改变的，还是A）
        /// </summary>
        public const string X = "X";

        /// <summary>
        /// 转出账（原账号的原账和新增的账）
        /// </summary>
        public const string Y = "Y";

        /// <summary>
        /// 转入账
        /// </summary>
        public const string Z = "Z";
    }


    /// <summary>
    /// 账务状态标志 --> Krzwztbz
    /// </summary>
    public sealed class AccStateSign
    {
        /// <summary>
        /// 迷你吧，商务中心等有汇总的大项
        /// </summary>
        public const string A = "A";

        /// <summary>
        /// 迷你吧，商务中心等有汇总的小项
        /// </summary>
        public const string H = "H";
    }

    /// <summary>
    /// 结账结单性质 --> Krzwjdxz
    /// </summary>
    public sealed class PayBillTypes
    {
        /// <summary>
        /// 付款 -- "C"
        /// </summary>
        public const string C = "C";

        /// <summary>
        /// 消费 -- "D"
        /// </summary>
        public const string D = "D";
    }

    /// <summary>
    /// 房间事务代码类型 --> Fhswswdm
    /// </summary>
    public sealed class RoomRoutineTypes
    {
        /// <summary>
        /// 查房 -- "A"
        /// </summary>
        public const string A = "A";

        /// <summary>
        /// 坏房 -- "B"
        /// </summary>
        public const string B = "B";

        /// <summary>
        /// 清洁 -- "C"
        /// </summary>
        public const string C = "C";

        /// <summary>
        /// 脏房 -- "D"
        /// </summary>
        public const string D = "D";

        /// <summary>
        /// 换房 -- "H"
        /// </summary>
        public const string H = "H";

        /// <summary>
        /// 封房 -- "I"
        /// </summary>
        public const string I = "I";

        /// <summary>
        /// ED+EA -- "J"
        /// </summary>
        public const string J = "J";

        /// <summary>
        /// 锁房 -- "L"
        /// </summary>
        public const string L = "L";

        /// <summary>
        /// 取消矛盾房 -- "N"
        /// </summary>
        public const string N = "N";

        /// <summary>
        /// 开房 -- "O"
        /// </summary>
        public const string O = "O";

        /// <summary>
        /// 预订 -- "R"
        /// </summary>
        public const string R = "R";

        /// <summary>
        /// 变空 -- "V"
        /// </summary>
        public const string V = "V";

        /// <summary>
        /// 设置矛盾房 -- "Y"
        /// </summary>
        public const string Y = "Y";

        /// <summary>
        /// 备注 -- "Z"
        /// </summary>
        public const string Z = "Z";
    }
}
