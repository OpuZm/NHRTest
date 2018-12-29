using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Repository
{
    /// <summary>
    /// 系统属性值描述说明类
    /// </summary>
    public class SysSetting
    {
        /// <summary>
        /// 接待提示入单
        /// </summary>
        public static readonly int TipInputOrderForReception = 1001;

        /// <summary>
        /// 预订可入预付金
        /// </summary>
        public static readonly int AllowBookingToPayAdvance = 1002;

        /// <summary>
        /// 可打入住登记单
        /// </summary>
        public static readonly int AllowPrintCheck_inForm = 1003;

        /// <summary>
        /// 提示入住登记单
        /// </summary>
        public static readonly int TipCheck_inForm = 1004;

        /// <summary>
        /// 启用新协议价
        /// </summary>
        //public static readonly int  = 1005;

        /// <summary>
        /// 可排其他房类
        /// </summary>
        public static readonly int AllowSettingToOtherRoomTypes = 1006;

        /// <summary>
        /// 启用package
        /// </summary>
        //public static readonly int  = 1007;

        /// <summary>
        /// 启用会议餐饮预订
        /// </summary>
        public static readonly int EnableMeetingAndRepastForBooking = 1008;

        /// <summary>
        /// 入住后发卡
        /// </summary>
        public static readonly int EnableAfterCheck_inNeedTheRoomCard = 1009;

        /// <summary>
        /// 换房禁止换在住房
        /// </summary>
        public static readonly int DisableChangeRoomToInHousing = 1010;

        /// <summary>
        /// 开房提示PMS
        /// </summary>
        public static readonly int TipPMS_ForOpenRoom = 1011;

        /// <summary>
        /// 接待提示账注
        /// </summary>
        public static readonly int TipAccountNoteForReception = 1012;

        /// <summary>
        /// 锁房可开房
        /// </summary>
        public static readonly int AllowOpenTheLockingRoom = 1013;

        /// <summary>
        /// 门锁发卡后读卡
        /// </summary>
        public static readonly int EnableRoomCardToRead = 1014;

        /// <summary>
        /// 预订后提示排房
        /// </summary>
        public static readonly int TipAfterBookingToArrangeRoom = 1015;

        /// <summary>
        /// 快住后提示修改
        /// </summary>
        public static readonly int TipFastCheck_inToUpdate = 1016;

        /// <summary>
        /// 快住后提示打登记单
        /// </summary>
        public static readonly int TipFastCheck_inToPrintForm = 1017;

        /// <summary>
        /// 当前房态可订房
        /// </summary>
        public static readonly int AllowBookingInCurrentRoomState = 1018;

        /// <summary>
        /// 刷卡提示在用会员卡
        /// </summary>
        public static readonly int TipSwipeCardToUsingMemberCard = 1019;

        /// <summary>
        /// 钟点房到钟提醒
        /// </summary>
        public static readonly int TipHourRoomForTimed = 1020;

        /// <summary>
        /// 会员卡入住不自动勾VIP
        /// </summary>
        public static readonly int DisableAutoSeletedVIPForTheMemberCarkCheck_in = 1021;

        /// <summary>
        /// 启用房价收益控制
        /// </summary>
        public static readonly int EnableHotelPricesRevenueControl = 1022;

        /// <summary>
        /// 协议单位不关联联系人和电话
        /// </summary>
        public static readonly int DisablePartnerToAutoLinkContactAndPhone = 1023;

        /// <summary>
        /// 房态图显示叫醒信息
        /// </summary>
        public static readonly int TipRoomMapAwakenMsg = 1024;

        /// <summary>
        /// 不可排预抵房
        /// </summary>
        public static readonly int DisableArrangeForExpectedArrivalRoom = 1025;

        /// <summary>
        /// 宾客列表团队列表显示有效房
        /// </summary>
        public static readonly int TipEffectiveRoomInGuest_TeamList = 1026;

        /// <summary>
        /// 续住提示入押金
        /// </summary>
        public static readonly int TipInputDepositForExtendStay = 1027;

        /// <summary>
        /// 不自动提示留言
        /// </summary>
        public static readonly int TipLeavingMessage = 1028;

        /// <summary>
        /// 房态图显示已发门锁
        /// </summary>
        public static readonly int ShowIssuedRoomCardIconInRoomMap = 1029;

        /// <summary>
        /// 预订发送短信
        /// </summary>
        public static readonly int EnableSendSmsForBooking = 1030;

        /// <summary>
        /// 预订不关联客户备注
        /// </summary>
        public static readonly int DisableLinkRemarkForBooking = 1031;

        /// <summary>
        /// 主资料可选楼座
        /// </summary>
        public static readonly int AllowMainDataToSelectGallery = 1032;

        /// <summary>
        /// 续住不提示发门锁
        /// </summary>
        public static readonly int TipPassRoomCardForExtendStay = 1033;

        /// <summary>
        /// 修改客人资料可改离店时间
        /// </summary>
        public static readonly int AllowUpdatingCustomerInfoCanChangeCheck_outTime = 1034;

        /// <summary>
        /// 合住不提示修改为自己结账
        /// </summary>
        public static readonly int TipUpdatingToSelfPayBillForChummage = 1035;

        /// <summary>
        /// 脏房不允许入住
        /// </summary>
        public static readonly int DisableCheck_inToDirtyRoom = 1036;

        /// <summary>
        /// 启用价格体系
        /// </summary>
        public static readonly int EnablePriceSystem = 1037;

        /// <summary>
        /// 团队发卡适配人数
        /// </summary>
        public static readonly int EnableTeamRoomCardAdaptionPeopleNumber = 1038;

        /// <summary>
        /// 房态图显示房间人数
        /// </summary>
        public static readonly int ShowNumOfRoomInRoomMap = 1039;

        /// <summary>
        /// 房态图显示留言内容
        /// </summary>
        public static readonly int ShowLeavingMessageInRoomMap = 1040;

        /// <summary>
        /// 夜审自动适配价格体系
        /// </summary>
        public static readonly int EnableNightAuditAutoAdaptionPriceSystem = 1041;

        /// <summary>
        /// 房态清洁置脏矛盾需提示
        /// </summary>
        public static readonly int EnablePromptMsgWhenRoomStatusChangeToDirty = 1042;

        /// <summary>
        /// 在住脏房不变色
        /// </summary>
        public static readonly int EnableInHousingOfTheDirtyRoomNoChangeColor = 1043;

        /// <summary>
        /// 判断是否存在遗留物品
        /// </summary>
        public static readonly int EnableCheckLegacyItem = 1044;

        /// <summary>
        /// 不可手工修改团号
        /// </summary>
        public static readonly int DisableChangeTeamNumManually = 1045;

        /// <summary>
        /// 开房判断同名客人
        /// </summary>
        public static readonly int EnableCheckSameGuestNameWhenCheck_in = 1046;

        /// <summary>
        /// 合住提示修改同住人房价
        /// </summary>
        public static readonly int TipChangePriceForChummage = 1047;

        /// <summary>
        /// 房态图显示矛盾房信息
        /// </summary>
        public static readonly int ShowConflictRoomInfoInRoomMap = 1048;

        /// <summary>
        /// 房态图默认显示客人姓名
        /// </summary>
        public static readonly int ShowCustomerNameInRoomMapByDefault = 1049;

        /// <summary>
        /// 转换后提示批量修改
        /// </summary>
        public static readonly int TipBatchUpdateWhenAfterTransform = 1050;

        /// <summary>
        /// 预订状态不能打入住单
        /// </summary>
        public static readonly int DisablePrintCheck_inFormInBookingState = 1051;

        /// <summary>
        /// 房态图不使用笑脸图标
        /// </summary>
        public static readonly int ShowSmilingFaceIconInRoomMap = 1052;

        /// <summary>
        /// 房态图显示团队会议标志
        /// </summary>
        public static readonly int ShowTeamMeetingMarkInRoomMap = 1053;

        /// <summary>
        /// 显示实际入日离日
        /// </summary>
        public static readonly int ShowActualFromDateToDate = 1054;

        /// <summary>
        /// 预订自动显示房类预测
        /// </summary>
        public static readonly int EnableWhenBookingAutoShowRoomTypeForecast = 1055;

        /// <summary>
        /// 预订及开房自动更新客户
        /// </summary>
        public static readonly int EnableAutoUpdateCustomerForBookingAndCheck_in = 1056;

        /// <summary>
        /// 房态开房判断同时操作
        /// </summary>
        public static readonly int EnableCheckConcurrentOperationForCheck_in = 1057;

        /// <summary>
        /// 取消入住后房间置脏
        /// </summary>
        public static readonly int EnableChangeToDirtyWhenAfterCancelCheck_in = 1058;

        /// <summary>
        /// 换房自动提示发门锁
        /// </summary>
        public static readonly int TipAutoSendCardWhenChangeRoom = 1059;

        /// <summary>
        /// 启用查找历史客人列表
        /// </summary>
        public static readonly int EnableSearchHistoryGuestList = 1060;

        /// <summary>
        /// 启用保存开房和预订操作功能
        /// </summary>
        public static readonly int EnableSaveForCheck_inAndBookingFunction = 1061;

        /// <summary>
        /// 宾客列表显示房类统计信息
        /// </summary>
        public static readonly int ShowRoomTypeStatisticalInfoInGuestList = 1062;

        /// <summary>
        /// 预订英文名不默认
        /// </summary>
        public static readonly int DisableEnglishNameByDefaultForBooking = 1063;

        /// <summary>
        /// 启用外宾入住登记单
        /// </summary>
        public static readonly int EnableForeignGuestCheck_inForm = 1064;

        /// <summary>
        /// 入住发送短信
        /// </summary>
        public static readonly int EnableSendSmsForCheck_in = 1065;

        /// <summary>
        /// 启用新会议预订
        /// </summary>
        public static readonly int EnableNewMeetingBooking = 1066;

        /// <summary>
        /// 启用排房删房
        /// </summary>
        public static readonly int EnableArrangeRoomAndDelRoom = 1067;

        /// <summary>
        /// 预订开房不取历史协议单位
        /// </summary>
        public static readonly int DisableHistoryAgreementUnitForBookingAndCheck_in = 1068;

        /// <summary>
        /// 排房界面默认显示列表
        /// </summary>
        public static readonly int ShowListByDefaultInArrangeRoomGUI = 1069;

        /// <summary>
        /// 点击团队列表判断留言
        /// </summary>
        public static readonly int EnableCheckLeavingMsgWhenClickTeamList = 1070;

        /// <summary>
        /// 可开虚拟账号
        /// </summary>
        public static readonly int AllowCreateVirtualAccount = 1071;

        /// <summary>
        /// 可打押金单
        /// </summary>
        public static readonly int AllowPrintDepositForm = 1072;

        /// <summary>
        /// 提示押金单
        /// </summary>
        public static readonly int TipDepositForm = 1073;

        /// <summary>
        /// 入账判断过日租
        /// </summary>
        public static readonly int EnableCheckDailyRentalForEntry = 1074;

        /// <summary>
        /// 不预览账单
        /// </summary>
        public static readonly int EnablePreviewBill = 1075;

        /// <summary>
        /// 提示同时操作
        /// </summary>
        public static readonly int TipConcurrentOperation = 1076;

        /// <summary>
        /// 启用积分卡
        /// </summary>
        public static readonly int EnablePointCard = 1077;

        /// <summary>
        /// 打印一卡通账单
        /// </summary>
        public static readonly int EnablePrintOneCardPassBill = 1078;

        /// <summary>
        /// 账务明细建账提首
        /// </summary>
        public static readonly int EnableAccountingEstablishAccountsMoveToFirst = 1079;

        /// <summary>
        /// 可打汇总账单
        /// </summary>
        public static readonly int AllowPrintSummaryBill = 1080;

        /// <summary>
        /// 标识主结账房
        /// </summary>
        public static readonly int EnableMarkMainPayBillRoom = 1081;

        /// <summary>
        /// 收银提示账注
        /// </summary>
        public static readonly int TipAccountNoteForCashiering = 1082;

        /// <summary>
        /// 转账自动入废单
        /// </summary>
        public static readonly int EnableAccountTransferAutoCreateWasteForm = 1083;

        /// <summary>
        /// 启用新迷你吧入账
        /// </summary>
        public static readonly int EnableMiniBarEntry = 1084;

        /// <summary>
        /// 账项票据不默认
        /// </summary>
        public static readonly int DisableBillItemAndTicket = 1085;

        /// <summary>
        /// 判断团队主资料最后离店
        /// </summary>
        public static readonly int EnableCheckTeamMainDataForLastCheck_out = 1086;

        /// <summary>
        /// 按当前时间判断过日租
        /// </summary>
        public static readonly int EnableCheckDailyRentalForCurrentDateTime = 1087;

        /// <summary>
        /// 结账离店判断预授权
        /// </summary>
        public static readonly int EnableCheckAUTHForCheck_out = 1088;

        /// <summary>
        /// 宾客列表显示会员卡发票
        /// </summary>
        public static readonly int ShowMemberCardInvoiceInGuestList = 1089;

        /// <summary>
        /// 迷你吧入账判断客人余额
        /// </summary>
        public static readonly int EnableCheckGuestRemainAmountForMiniBarEntry = 1090;

        /// <summary>
        /// 启用结账离店提示未注销门卡
        /// </summary>
        public static readonly int EnableNotCanceledCardPromptMsgWhenCheck_out = 1091;

        /// <summary>
        /// 暂离主账房提示是否转账
        /// </summary>
        public static readonly int TipAccountTransferInMainRoomWhenAFK = 1092;

        /// <summary>
        /// 结账判断联房是否过日租
        /// </summary>
        public static readonly int EnablePayBillToCheckLinkRoomAlreadyDailyRental = 1093;

        /// <summary>
        /// 离店发送短信
        /// </summary>
        public static readonly int EnableSendSmsForCheck_out = 1094;

        /// <summary>
        /// 离店不默认选择同住客人
        /// </summary>
        public static readonly int DisableDefaultSelectChummageForCheck_out = 1095;

        /// <summary>
        /// 离店判断过日租
        /// </summary>
        public static readonly int EnableCheckDailyRentalForCheck_out = 1096;

        /// <summary>
        /// 会员卡消费积分发送短信
        /// </summary>
        public static readonly int EnableSendSmsForMemberCardPointPay = 1097;

        /// <summary>
        /// 不提示打印结账单
        /// </summary>
        public static readonly int TipPrintPayBill = 1098;

        /// <summary>
        /// 相同身份证号不允许开房
        /// </summary>
        public static readonly int DisableCheck_inWhenSameIdentityCary = 1099;

        /// <summary>
        /// 开发票默认计算所有消费
        /// </summary>
        public static readonly int EnableCalcAllBillForWriteReceipt = 1100;

        /// <summary>
        /// 开多房自动获取房价
        /// </summary>
        public static readonly int EnableOpenMultiRoomAutoGetPrice = 1101;

        /// <summary>
        /// 打单提示电子签名账单
        /// </summary>
        public static readonly int TipElectronicSignatureBillWhenPrintForm = 1102;

        /// <summary>
        /// 启用银行POS机接口
        /// </summary>
        public static readonly int EnableBankPOS = 1103;

        /// <summary>
        /// 宾客列表共享房态留言图标
        /// </summary>
        public static readonly int ShowLeavingMessageIconInGuestList = 1104;

        /// <summary>
        /// 宾客列表启用分页
        /// </summary>
        public static readonly int EnableGuestListPaging = 1105;

        /// <summary>
        /// 离店自动取消矛盾房
        /// </summary>
        public static readonly int EnableCancelConflictRoomWhenAutoCheck_out = 1106;

        /// <summary>
        /// 启用账项红冲
        /// </summary>
        //public static readonly int  = 1107;

        /// <summary>
        /// 二次押金不提示打单
        /// </summary>
        public static readonly int TipPrintFormWhenSecondDeposit = 1108;

        /// <summary>
        /// 打单默认迷你吧明细
        /// </summary>
        public static readonly int EnablePrintWithMiniBarDetail = 1109;

        /// <summary>
        /// 特定时间开房自动收房费
        /// </summary>
        public static readonly int EnableAutoCollectRoomFeeForSpecialTime = 1110;

        /// <summary>
        /// 主结房离店后默认主房
        /// </summary>
        public static readonly int EnableDefaultMainRoomWhenMainRoomCheck_out = 1111;

        /// <summary>
        /// 操作自动转账项目需提示
        /// </summary>
        public static readonly int TipAutoAccountTransferItemOperate = 1112;

        /// <summary>
        /// 启用报表设计器账单
        /// </summary>
        public static readonly int EnableReportDesignerBill = 1113;

        /// <summary>
        /// 转账账项显示原操作员
        /// </summary>
        public static readonly int ShowOriginalUserNameInAccountTransferItem = 1114;

        /// <summary>
        /// 离店状态可打入住单
        /// </summary>
        public static readonly int EnablePrintCheck_inFormOnCheck_outState = 1115;

        /// <summary>
        /// 夜审过租可过0房租
        /// </summary>
        public static readonly int EnableNightAuditDailyRentalAllowZeroRoomRent = 1116;

        /// <summary>
        /// 入押金审核票据
        /// </summary>
        public static readonly int EnableInputDepositAuditTicket = 1117;

        /// <summary>
        /// 启用账务入账进位判断
        /// </summary>
        public static readonly int EnableAccountingCheckEntryCarryBit = 1118;

        /// <summary>
        /// 标识欠费客人
        /// </summary>
        public static readonly int EnableMarkOwingBillGuest = 1119;

        /// <summary>
        /// 结账后余额为0直接离店
        /// </summary>
        public static readonly int EnableDirectCheck_outWhenRemain0AndPayBill = 1120;

        /// <summary>
        /// 启用会员卡独立赠送
        /// </summary>
        public static readonly int EnableMemberCardIndividualGiving = 1121;

        /// <summary>
        /// 转账可转总账
        /// </summary>
        public static readonly int AllowTransferGeneralLedger = 1122;

        /// <summary>
        /// 房态图显示欠费标志
        /// </summary>
        public static readonly int ShowOwingBillIconInRoomMap = 1123;

        /// <summary>
        /// 追回至暂离判断当前日期
        /// </summary>
        public static readonly int EnableCheckCurrentDateWhenClawbackToAFK = 1124;

        /// <summary>
        /// 结账提示账注
        /// </summary>
        public static readonly int TipAccountNoteForPayBill = 1125;

        /// <summary>
        /// 离店提示更改结账账号
        /// </summary>
        public static readonly int TipChangePayBillAccountNoForCheck_out = 1126;

        /// <summary>
        /// 离店可查看预授权
        /// </summary>
        public static readonly int AllowViewAUTHForCheck_out = 1127;

        /// <summary>
        /// 入账可入0单价
        /// </summary>
        public static readonly int EnableEntryContainZeroPrice = 1128;

        /// <summary>
        /// 启用外宾账单
        /// </summary>
        public static readonly int EnableForeignGuestBill = 1129;

        /// <summary>
        /// 房务中心入账适用挂房账限额
        /// </summary>
        public static readonly int AllowRoomServiceCenterEntryAdaptionTheCreditLimit = 1130;

        /// <summary>
        /// 账务界面不显示备注
        /// </summary>
        public static readonly int ShowRemarkInAccountingGUI = 1131;

        /// <summary>
        /// 启用独立虚拟账单
        /// </summary>
        public static readonly int EnableIndividualVirtualBill = 1132;

        /// <summary>
        /// 入账界面显示快捷入账按钮
        /// </summary>
        public static readonly int ShowShortcutRecordButtonInEntryGUI = 1133;

        /// <summary>
        /// 修改订房明细房数强制提示删房
        /// </summary>
        public static readonly int TipDelRoomWhenChangingRoomNumberOfDetails = 1134;

        /// <summary>
        /// 未关联业务员的客户可修改业务员
        /// </summary>
        public static readonly int AllowChangeSalesWhenCustomerNoLinkSales = 1135;

        /// <summary>
        /// 批量修改客户自动获取关联信息
        /// </summary>
        public static readonly int EnableBatchChangeCustomerAutoLoadLinkInfo = 1136;

        /// <summary>
        /// 客户协议价关联包含
        /// </summary>
        public static readonly int EnableCustomerAgreementLinkContains = 1137;

        /// <summary>
        /// 宾客界面团队列表自定义宽度
        /// </summary>
        public static readonly int EnableCustomWidthInGuest_TeamList = 1138;

        /// <summary>
        /// 账务列表启用分页
        /// </summary>
        public static readonly int EnableAccountingListPaging = 1139;

        /// <summary>
        /// 离店只显示所选客人
        /// </summary>
        public static readonly int ShowTheSelectedCustomerForCheck_out = 1140;

        /// <summary>
        /// 启用门锁接口
        /// </summary>
        public static readonly int EnableDoorLockPort = 1141;

        /// <summary>
        /// 提示直接打印
        /// </summary>
        public static readonly int TipDirectPrint = 1142;

        /// <summary>
        /// 客人生日自动提醒
        /// </summary>
        public static readonly int TipBirthdayOfCustomer = 1143;

        /// <summary>
        /// 客人欠款自动提醒
        /// </summary>
        public static readonly int TipOwingBillOfCustomer = 1144;

        /// <summary>
        /// 开启PMS
        /// </summary>
        public static readonly int EnablePMS = 1145;

        /// <summary>
        /// 启用读取公安数据1
        /// </summary>
        public static readonly int EnableReadPublicSecurityData1 = 1146;

        /// <summary>
        /// 启用读取公安数据2
        /// </summary>
        public static readonly int EnableReadPublicSecurityData2 = 1147;

        /// <summary>
        /// 可标识收取门卡
        /// </summary>
        public static readonly int AllowMarkCollectDoorCard = 1148;

        /// <summary>
        /// 房号仅向后检索
        /// </summary>
        public static readonly int EnableRoomNoCanOnlyBackwardSearch = 1149;

        /// <summary>
        /// 选择清洁服务员
        /// </summary>
        public static readonly int EnableSelectCleanWaiter = 1150;

        /// <summary>
        /// 不能自动续住
        /// </summary>
        public static readonly int DisableAutoExtendStay = 1151;

        /// <summary>
        /// 不能自动下转未结单
        /// </summary>
        public static readonly int DisableAutoTransferOfUnpaidBill = 1152;

        /// <summary>
        /// 启用新报表控件
        /// </summary>
        //public static readonly int  = 1153;

        /// <summary>
        /// 宾客列表报表显示分类
        /// </summary>
        public static readonly int ShowCategoryInGuestListReport = 1154;

        /// <summary>
        /// 启用分配清洁人员功能
        /// </summary>
        public static readonly int EnableDistributionCleanerFunction = 1155;

        /// <summary>
        /// 启用第三方会员卡
        /// </summary>
        public static readonly int EnableThirdMemberCard = 1156;

        /// <summary>
        /// 启用自定义分组报表
        /// </summary>
        public static readonly int EnableCustomGroupingReport = 1157;

        /// <summary>
        /// 启用二级仓
        /// </summary>
        public static readonly int EnableSecondWareroom = 1158;

        /// <summary>
        /// 启用内存优化
        /// </summary>
        //public static readonly int  = 1159;

        /// <summary>
        /// 房态信息显示房间属性
        /// </summary>
        public static readonly int ShowRoomPropertyInRoomMap = 1160;

        /// <summary>
        /// 房态楼座楼层显示房数信息
        /// </summary>
        public static readonly int ShowRoomNumberInRoomMap_GalleryFloor = 1161;

        /// <summary>
        /// 维修房适应超卖设置
        /// </summary>
        public static readonly int EnableOutOfOrderRoomAdaptionOversoldSetting = 1162;

        /// <summary>
        /// 门锁发卡强制注销
        /// </summary>
        public static readonly int EnableRoomCardForceLogout = 1163;

        /// <summary>
        /// 点击即提示留言
        /// </summary>
        public static readonly int EnableClickToPropmtLeavingMessage = 1164;

        /// <summary>
        /// 续住提示打印入住登记单
        /// </summary>
        public static readonly int TipPrintCheck_inFormForExtendStay = 1165;

        /// <summary>
        /// 团队列表显示团名和单位
        /// </summary>
        public static readonly int ShowTeamNameAndCompanyInTeamList = 1166;

        /// <summary>
        /// 门锁模式2
        /// </summary>
        public static readonly int EnableDoorLockMode2 = 1167;

        /// <summary>
        /// 团队列表标注集团预订单
        /// </summary>
        public static readonly int ShowGroupBookingFormInTeamList = 1168;

        /// <summary>
        /// 开多间房不默认连续打登记单
        /// </summary>
        public static readonly int EnableContinuousPrintCheck_inFormWhenOpenMulti_Room = 1169;

        /// <summary>
        /// 宾客列表读卡查房只查房号
        /// </summary>
        public static readonly int EnableOnlySearchRoomNoForCheckRoomInGuestList = 1170;

        /// <summary>
        /// 执行集团系统日处理
        /// </summary>
        public static readonly int EnableExecuteGroupSystemDailyProcessing = 1171;

        /// <summary>
        /// 开房读取会员卡不改结账方式
        /// </summary>
        public static readonly int DisableChangePayBillModeWhenReadMemberCardForCheck_in = 1172;

        /// <summary>
        /// 预订开房不取历史客类价类
        /// </summary>
        public static readonly int DisableLinkCustomerAndPriceTypeForBookingCheck_in = 1173;

        /// <summary>
        /// 房态图界面显示房间状态统计
        /// </summary>
        public static readonly int ShowRoomStateStatisticalInfoInRoomMap = 1174;

        /// <summary>
        /// 报查房判断是否重复
        /// </summary>
        public static readonly int EnableCheckingRoomNeedCheckRepeat = 1175;

        /// <summary>
        /// 星程接口
        /// </summary>
        //public static readonly int  = 1176;

        /// <summary>
        /// 回车后检索
        /// </summary>
        public static readonly int EnableSearchOnAfterEnter = 1177;

        /// <summary>
        /// 精确检索
        /// </summary>
        public static readonly int EnableExactSearch = 1178;

        /// <summary>
        /// 自动消息弹出
        /// </summary>
        public static readonly int EnableMsgAutoPropmt = 1179;

        /// <summary>
        /// 接待账务列表不显示房数人数
        /// </summary>
        public static readonly int ShowNumberAndNumberOfRoomsInReception_AccountingList = 1180;

        /// <summary>
        /// 自动启动集团接收程序
        /// </summary>
        public static readonly int EnableAutoGroupReceiveProgram = 1181;

        /// <summary>
        /// 不可手工输入会员卡号
        /// </summary>
        public static readonly int DisableInputMemberCardNoManually = 1182;

        /// <summary>
        /// 双击在住房态进入客人列表
        /// </summary>
        public static readonly int EnableDoubleClickInHousingIntoGuestList = 1183;

        /// <summary>
        /// 客人列表显示入离时间
        /// </summary>
        public static readonly int ShowCheck_inAndOutTimeInGuestList = 1184;

        /// <summary>
        /// 启用新客人列表
        /// </summary>
        //public static readonly int  = 1185;

        /// <summary>
        /// 公司客户资料自动检索类似名
        /// </summary>
        public static readonly int EnableAutoSearchSimilarNameInCompanyInfo = 1186;

        /// <summary>
        /// 登录提示相同操作员
        /// </summary>
        public static readonly int TipSameUserForLogin = 1187;

        /// <summary>
        /// 登录控制相同操作员
        /// </summary>
        public static readonly int EnableCheckSameUserAfterLogin = 1188;

        /// <summary>
        /// 启用检查VC房
        /// </summary>
        public static readonly int EnableCheckVCRoom = 1189;

        /// <summary>
        /// 启用在线式会员卡
        /// </summary>
        public static readonly int EnableOnlineMemberCard = 1190;

        /// <summary>
        /// 程序登录自动更新
        /// </summary>
        public static readonly int EnableAutoUpdatingProgramForLogin = 1191;

        /// <summary>
        /// 启用自定义单据单号
        /// </summary>
        public static readonly int EnableCustomBillNo = 1192;

        /// <summary>
        /// 启用留言自动弹出
        /// </summary>
        public static readonly int EnableAutoPropmtLeavingMessage = 1193;

        /// <summary>
        /// 自动消息查询后不停止自动刷新
        /// </summary>
        public static readonly int DisableAutoRefreshAfterSearchAutoMessage = 1194;

        /// <summary>
        /// 发卡不记录客人事务
        /// </summary>
        public static readonly int DisableKeepingGuestAffairForDistributionRoomCard = 1195;

        /// <summary>
        /// 离店时间超出默认时间自动提醒
        /// </summary>
        public static readonly int EnableAutoDingWhenCheck_outTimeout = 1196;

        /// <summary>
        /// 断网自动检测连接
        /// </summary>
        public static readonly int EnableAutoCheckingAndLinkingForOffline = 1197;

        /// <summary>
        /// 门锁发卡强制发副卡
        /// </summary>
        public static readonly int EnableForceDistributionViceCardForCheck_in = 1198;

        /// <summary>
        /// 宾客列表显示中文状态
        /// </summary>
        public static readonly int ShowChineseStateInGuestList = 1199;

        /// <summary>
        /// 修改会员卡号提示是否修改资料
        /// </summary>
        public static readonly int TipUpdatingInfoMsgWhenChangingMemberCardNo = 1200;

        /// <summary>
        /// 房态图空方格不显示边框
        /// </summary>
        public static readonly int ShowBoxBoderInRoomMap = 1201;

        /// <summary>
        /// 预订后直接产生客人资料
        /// </summary>
        public static readonly int EnableDirectCreateGuestInfoAfterBooking = 1202;

        /// <summary>
        /// 取消房间自动删除房号
        /// </summary>
        public static readonly int EnableAutoDelRoomNoForCancelRoom = 1203;

        /// <summary>
        /// 排房界面客人列表按房号排序
        /// </summary>
        public static readonly int EnableSortByRoomNoInArrangeRoomGUI_GuestList = 1204;

        /// <summary>
        /// 上传客人历史
        /// </summary>
        public static readonly int EnableUploadGuestHistory = 1205;

        /// <summary>
        /// 多人开房入住登记单最后打印
        /// </summary>
        public static readonly int EnablePrintFormOnLastWhenMultiToCheck_in = 1206;

        /// <summary>
        /// 开房不提示打押金单
        /// </summary>
        public static readonly int TipPrintDepositForCheck_in = 1207;

        /// <summary>
        /// 房类预测界面已买房不区分预订在住
        /// </summary>
        public static readonly int EnableDistinguishBookingAndInHousingStateInForecastGUI = 1208;

        /// <summary>
        /// 排房界面显示检索房号
        /// </summary>
        public static readonly int ShowSearchRoomNoInArrangeRoomGUI = 1209;

        /// <summary>
        /// 团队列表默认按预订号排序
        /// </summary>
        public static readonly int EnableSortByBookingNoInTeamList = 1210;

        /// <summary>
        /// 启用打印银行POS机账单
        /// </summary>
        public static readonly int EnablePrintBankPOS_Bill = 1211;

        /// <summary>
        /// 预览银行POS机账单
        /// </summary>
        public static readonly int EnablePreviewBankPOS_Bill = 1212;

        /// <summary>
        /// 预订查询按日期分类
        /// </summary>
        public static readonly int EnableSearchGroupByDateInBooking = 1213;

        /// <summary>
        /// 房态图客人姓名和房类同时显示
        /// </summary>
        public static readonly int ShowCustomerNameAndRoomTypeTegetherInRoomMap = 1214;

        /// <summary>
        /// 房态图标识联房客人
        /// </summary>
        public static readonly int ShowMarkLinkCustomerInRoomMap = 1215;

        /// <summary>
        /// 启用新模块
        /// </summary>
        public static readonly int EnableNewModule = 1216;

        /// <summary>
        /// 宾客列表在住客人可关联查询其他状态
        /// </summary>
        public static readonly int AllowSearchOtherStateForInHousingGuestInGuestList = 1217;

        /// <summary>
        /// 开房判断第二天是否被预订
        /// </summary>
        public static readonly int EnableCheckNextDayBookingWhenCheck_in = 1218;

        /// <summary>
        /// 重入只判断房间状态
        /// </summary>
        public static readonly int EnableReentryOnlyCheckRoomState = 1219;

        /// <summary>
        /// 脏房不能追回或重入
        /// </summary>
        public static readonly int DisableDirtyRoomClawbackOrReentry = 1220;

        /// <summary>
        /// 修改客户或价类时自动更新房价
        /// </summary>
        public static readonly int EnableAutoUpdatePriceWhenChangingCustomerOrPriceType = 1221;

        /// <summary>
        /// 排房升级房间不提示输入原因
        /// </summary>
        public static readonly int TipInputReasonWhenArrangeRoomMoveUp = 1222;

        /// <summary>
        /// 开房强制提示客史
        /// </summary>
        public static readonly int TipCustomerHistoryForCheck_in = 1223;

        /// <summary>
        /// 新宾客模块宾客列表不分组
        /// </summary>
        //public static readonly int  = 1224;

        /// <summary>
        /// 新宾客模块宾客列表格子颜色自定义
        /// </summary>
        public static readonly int EnableCustomBoxColorInGuestList = 1225;

        /// <summary>
        /// 强制过日租
        /// </summary>
        public static readonly int EnableForceProcessingDailyRental = 1226;

        /// <summary>
        /// 换房启用换房单
        /// </summary>
        public static readonly int EnableFormForChangeRoom = 1227;

        /// <summary>
        /// 续房启用续房单
        /// </summary>
        public static readonly int EnableFormForExtenStay = 1228;

        /// <summary>
        /// 开房不默认设置主房
        /// </summary>
        public static readonly int DisableSettingMainRoomForOpenRoom = 1229;

        /// <summary>
        /// 预订默认团员自付
        /// </summary>
        public static readonly int EnableTeamMemberSelfPayForBooking = 1230;

        /// <summary>
        /// 启用自定义价格
        /// </summary>
        public static readonly int EnableCustomPrice = 1231;

        /// <summary>
        /// 清洁房间自动设为房间已检查
        /// </summary>
        public static readonly int EnableAutoSetRoomStateCheckedAfterClean = 1232;

        /// <summary>
        /// 读取客史不判断身份证号
        /// </summary>
        public static readonly int EnableCheckIdentityIdToReadHistory = 1233;

        /// <summary>
        /// 过租判断是否已过房租
        /// </summary>
        public static readonly int EnableCheckRoomRentForProcessingRent = 1234;

        /// <summary>
        /// 启用记录房间入住间夜数功能
        /// </summary>
        public static readonly int EnableKeepingCheck_inRoomNightCountFunction = 1235;

        /// <summary>
        /// 排房界面房间按入住间夜数排序
        /// </summary>
        public static readonly int EnableRoomSortByNightCountInArrangeRoomGUI = 1236;

        /// <summary>
        /// 主结人离店自动修改同住客人房价
        /// </summary>
        public static readonly int EnableAutoChangeTheRoomPriceAfterMainGuestCheck_out = 1237;

        /// <summary>
        /// 迷你吧入账界面显示当日账项
        /// </summary>
        public static readonly int ShowCurrentDateBillItemInMiniBar_EntryGUI = 1238;

        /// <summary>
        /// 合住不允许修改房价
        /// </summary>
        public static readonly int DisableChangePriceForChummage = 1239;

        /// <summary>
        /// 启用订房明细预计房数
        /// </summary>
        public static readonly int EnableBookingDetailEstimateCount = 1240;

        /// <summary>
        /// 启用学院一卡通
        /// </summary>
        //public static readonly int  = 1241;

        /// <summary>
        /// 开房界面回车直接确认
        /// </summary>
        public static readonly int EnableDirectEntryApplyInOpenRoomGUI = 1242;

        /// <summary>
        /// 不启用选择性结账
        /// </summary>
        public static readonly int DisableSelectivityPayBill = 1243;

        /// <summary>
        /// 清洁分配界面显示维修房封房
        /// </summary>
        public static readonly int ShowOutOfOrderRoomInCleanAssignGUI = 1244;

        /// <summary>
        /// 余额为0不提示离店
        /// </summary>
        public static readonly int TipCheck_outWhenRemain0 = 1245;

        /// <summary>
        /// 多人房间快住默认只选一人
        /// </summary>
        public static readonly int EnableOnlySelectedOneForFastCheck_inWhenMulti_ShareRoom = 1246;
    }

    /// <summary>
    /// 系统属性组别类型
    /// </summary>
    public sealed class SettingGroupTypes
    {
        /// <summary>
        /// 房务管理属性组
        /// </summary>
        public static readonly int RoomGroup = 10;

        /// <summary>
        /// 账务管理属性组
        /// </summary>
        public static readonly int AccountingGroup = 20;

        /// <summary>
        /// 短信、信息提示属性组
        /// </summary>
        public static readonly int SmsMsgGroup = 30;

        /// <summary>
        /// 第三方接口属性设置组，如：门锁接口设置，身份证接口，公安接口
        /// </summary>
        public static readonly int ThridPartyInterfaceGroup = 40;
    }

    /// <summary>
    /// 包含的属性关联的控件类型
    /// </summary>
    public sealed class SettingControlTypes
    {
        /// <summary>
        /// 对应于复选框控件
        /// </summary>
        public static readonly string CHECKBOX = "CheckBox";

        /// <summary>
        /// 对应于下拉框控件
        /// </summary>
        public static readonly string COMBO = "Combo";

        /// <summary>
        /// 对应于文本框控件
        /// </summary>
        public static readonly string TEXT = "Text";
    }
}
