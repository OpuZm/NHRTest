using System;
using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class OrderCreateDTO
    {
    }

    public class OrderTableDTO
    {
        public int Id { get; set; }

        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int OrderId { get; set; }

        ///<summary>
        /// 餐饮台号
        ///</summary>
        public int TableId { get; set; }

        ///<summary>
        /// 开台时间
        ///</summary>
        public Nullable<DateTime> CreateDate { get; set; }
        ///<summary>
        /// 订单台号人数
        ///</summary>
        public int PersonNum { get; set; }
        /// <summary>
        /// 是否已结账
        /// </summary>
        public bool IsCheckOut { get; set; }
        /// <summary>
        /// 是否已开台
        /// </summary>
        public bool IsOpen { get; set; }
        public bool IsLock { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Nullable<decimal> TotalAmount { get; set; }
        public string OrderNo { get; set; }

        public string OrderStatusDesc { get; set; }
        public bool IsControl { get; set; }
        public bool IsWeiXin { get; set; }
        public string ContactPerson { get; set; }
        public string ContactTel { get; set; }
        #region 台号基本信息

        public string Name { get; set; }
        public string Description { get; set; }
        public int RestaurantId { get; set; }
        public int SeatNum { get; set; }
        public CythStatus CythStatus { get; set; }
        public Nullable<decimal> ServerRate { get; set; }
        public int RestaurantArea { get; set; }
        public int Box { get; set; }

        #endregion

    }

    /// <summary>
    /// 订单台号 订单信息和台号信息
    /// </summary>
    public class OrderAndTablesDTO
    {
        /// <summary>
        /// 台号id
        /// </summary>
        public int R_Table_Id { get; set; }
        /// <summary>
        /// 台号名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 餐厅名称
        /// </summary>
        public string Restaurant { get; set; }

        /// <summary>
        /// 餐厅id
        /// </summary>
        public int R_Restaurant_Id { get; set; }
        
        /// <summary>
        /// 订单Id
        /// </summary> 
        public int OrderId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public CyddStatus CyddStatus { get; set; }

        /// <summary>
        /// 订单号
        /// </summary> 
        public string OrderNo { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PersonNum { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客源类型
        /// </summary>
        public string OrderSourceType { get; set; }

        /// <summary>
        /// 开单时间
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        public bool IsControl { get; set; }
        public int MarketId { get; set; }
    }
    public class OrderTableProjectDTO
    {
        public int MxId { get; set; }
        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int OrderId { get; set; }
        ///<summary>
        /// 餐饮台号
        ///</summary>
        public int OrderTableId { get; set; }
        /// <summary>
        /// 台号
        /// </summary>
        public int TableId { get; set; }
        ///<summary>
        /// 价格
        ///</summary>
        public Decimal Price { get; set; }
        ///<summary>
        /// 数量
        ///</summary>
        public decimal Num { get; set; }
        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderNo { get; set; }
    }

    public class ReserveCreateDTO
    {
        public ReserveCreateDTO()
        {
            OrderTableIds = new List<int>();
        }

        public int Id { get; set; }
        ///<summary>
        /// 订单编号
        ///</summary>
        public string OrderNo { get; set; }
        ///<summary>
        /// 餐饮餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 餐饮餐厅名称
        ///</summary>
        public string RestaurantName { get; set; }

        ///<summary>
        /// 订单来源（用户自定义）(1:自来客,2:营销推广,3:协议客户,4:微信) TypeId =10002
        ///</summary>
        public int CyddOrderSource { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public int OrderType { get; set; }

        ///<summary>
        /// 人数
        ///</summary>
        public int PersonNum { get; set; }
        ///<summary>
        /// 开单人
        ///</summary>
        public int BillingUser { get; set; }
        ///<summary>
        /// 订单状态(1:预定,2:开台,3:送厨,4:用餐中,5:结账,6:取消,7:订单菜品修改,8:并台)
        ///</summary>
        public CyddStatus CyddStatus { get; set; }
        ///<summary>
        /// 备注信息
        ///</summary>
        public string Remark { get; set; }
        ///<summary>
        /// 预定用餐时间
        ///</summary>
        public Nullable<DateTime> ReserveDate { get; set; }
        ///<summary>
        /// 消费金额
        ///</summary>
        public decimal ConAmount { get; set; }
        ///<summary>
        /// 服务金额
        ///</summary>
        public decimal ServiceAmount { get; set; }
        ///<summary>
        /// 应收金额
        ///</summary>
        public decimal OriginalAmount { get; set; }
        ///<summary>
        /// 实收金额
        ///</summary>
        public decimal RealAmount { get; set; }
        ///<summary>
        /// 折扣率
        ///</summary>
        public decimal DiscountRate { get; set; }
        ///<summary>
        /// 折扣金额
        ///</summary>
        public decimal DiscountAmount { get; set; }
        ///<summary>
        /// 赠送金额
        ///</summary>
        public decimal GiveAmount { get; set; }
        ///<summary>
        /// 特殊要求
        ///</summary>
        public string SpecialPopc { get; set; }
        ///<summary>
        /// 起菜方式(1:即起,2:叫起)
        ///</summary>
        public CyddCallType CyddCallType { get; set; }
        ///<summary>
        /// 订金
        ///</summary>
        public decimal DepositAmount { get; set; }
        ///<summary>
        /// 创建时间
        ///</summary>
        public Nullable<DateTime> CreateDate { get; set; }
        ///<summary>
        /// 餐饮分市
        ///</summary>
        public int R_Market_Id { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUser { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal ClearAmount { get; set; }
        /// <summary>
        /// 台数
        /// </summary>
        public int TableNum { get; set; }
        public List<TableListDTO> Tables { get; set; }

        /// <summary>
        /// 订单状态(1:预定,2:开台,3.点餐,4:送厨,5:用餐中,6:结账,7:取消,8:订单菜品修改 9.并台) 
        /// 粟新增2017-9-5
        /// </summary>
        public string orderStatus
        {
            get
            {
                string str = "";
                str = CyddStatus > 0 ? Enum.GetName(CyddStatus.GetType(), CyddStatus) : "";
                return str;
            }
        }


        public string SourceTypeName
        {
            get; set;
        }

        public string OrderTypeName { get; set; }

        public string MarketName { get; set; }

        /// <summary>
        /// 账务日期
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// 取支付记录表里面的预订金额
        /// </summary>
        public decimal BookingAmount { get; set; }

        /// <summary>
        /// 预订订单的订单台号Id List
        /// </summary>
        public List<int> OrderTableIds { get; set; }

        public int CompanyId { get; set; }
        public int CurrentMarketId { get; set; }
        public string BillDepartMent { get; set; }
        public CyddCzjlUserType UserType { get; set; }
        public int MemberCardId { get; set; }
        public string TablesName { get; set; }
        public int CurrentRestaurantId { get; set; }
    }

    public class OrderDetailDTO
    {
        public int Id { get; set; }
        ///<summary>
        /// 餐饮订单台号
        ///</summary>
        public int R_OrderTable_Id { get; set; }

        ///<summary>
        /// 餐饮明细项目类型(1:餐饮项目,2:餐饮套餐)
        ///</summary>
        public CyddMxType CyddMxType { get; set; }

        ///<summary>
        /// 餐饮项目ID(根据类型)
        ///</summary>
        public int CyddMxId { get; set; }

        ///<summary>
        /// 销售成本价
        ///</summary>
        public decimal CostPrice { get; set; }

        ///<summary>
        /// 销售价
        ///</summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 折后金额
        /// </summary>
        public decimal DiscountedAmount { get; set; }

        ///<summary>
        /// 数量
        ///</summary>
        public decimal Num { get; set; }

        ///<summary>
        /// 制作人(厨师)
        ///</summary>
        public string MakeUser { get; set; }

        ///<summary>
        /// 状态(1:未出,2:已出)
        ///</summary>
        public CyddMxStatus CyddMxStatus { get; set; }

        ///<summary>
        /// 顺序
        ///</summary>
        public int SortNum { get; set; }

        ///<summary>
        /// 催促次数
        ///</summary>
        public int RemindNum { get; set; }

        ///<summary>
        /// 备注信息
        ///</summary>
        public string Remark { get; set; }

        public int CreateUser { get; set; }
        public int HookNum { get; set; }
        public string ExtendName { get; set; }
        public string CyddMxName { get; set; }
        ///<summary>
        /// 折扣率
        ///</summary>
        public decimal DiscountRate { get; set; }
        public string ProjectName { get; set; }
        public string Unit { get; set; }
        public int R_Project_Id { get; set; }
        public List<int> ExtendIds { get; set; }
        public string StrTime { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public List<ProjectExtendDTO> Extend { get; set; }
        public List<ProjectExtendDTO> ExtendRequire { get; set; }
        public List<ProjectExtendDTO> ExtendExtra { get; set; }
        public int IsChangeNum { get; set; }//送厨房后是否可更改数量
        public int IsGive { get; set; }//是否可赠送
        public int IsDiscount { get; set; } //是否可打折
        public int IsCustomer { get; set; }// 是否自定义
        public int IsQzdz { get; set; }// 是否可强制打折y>
        public int IsChangePrice { get; set; }// 是否可改价
        public int IsRecommend { get; set; }// 是否推荐
        public int Property { get; set; }//菜品权限总值
        public bool IsUpdateNum { get; set; }//默认为false，用于送厨更改数量时为true
        public bool IsUpdatePrice { get; set; } //用于下单后改价(暂时只开放套餐)
        public List<OrderDetailRecordCountDTO> OrderDetailRecordCount { get; set; }
        public List<OrderDetailRecordDTO> OrderDetailRecord { get; set; }
        public List<R_OrderDetailPackageDetail> PackageDetailList { get; set; }//套餐菜品明细
        public DishesStatus DishesStatus { get; set; }
        public bool IsListPrint { get; set; }
        public int R_OrderDetailCause_Id { get; set; }
    }

    /// <summary>
    /// 订单明细操作记录
    /// </summary>
    public class OrderDetailRecordDTO
    {
        public int Id { get; set; }
        public int R_OrderDetail_Id { get; set; }
        public CyddMxCzType CyddMxCzType { get; set; }
        public string CyddMxCzTypeName { get; set; }
        public decimal Num { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUser { get; set; }
        public string CreateUserName { get; set; }
        public string Remark { get; set; }
        public bool IsCalculation { get; set; }
    }

    /// <summary>
    /// 统计订单明细记录赠，退，入，出数量
    /// </summary>
    public class OrderDetailRecordCountDTO
    {
        /// <summary>
        /// 类型 1赠送 2退菜 3转入 4转出
        /// </summary>
        public CyddMxCzType CyddMxCzType { get; set; }
        /// <summary>
        /// 统计数量
        /// </summary>
        public decimal Num { get; set; }
    }

    /// <summary>
    /// 订单明细记录
    /// </summary>
    public class R_OrderDetailRecordDTO
    {
        public int Id { get; set; }
        public int R_OrderDetail_Id { get; set; }
        public CyddMxCzType CyddMxCzType { get; set; }
        public decimal Num { get; set; }
        public string CyddMxName { get; set; }
        public int OrderId { get; set; }
        public string TableName { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUser { get; set; }
        public string Remark { get; set; }
        public int R_OrderDetailCause_Id { get; set; }
        public string DetailCauseRemark { get; set; }
    }

    public class OpenTableCreateResultDTO
    {
        public int OrderId { get; set; }
        public List<int> OrderTableIds { get; set; }
        public List<string> TablesName { get; set; }
    }

    public class ChangeTableDTO
    {
        public int RestaurantId { get; set; }
        public int TableId { get; set; }
        public int OrderTableId { get; set; }
        public CythStatus CythStatus { get; set; }
    }

    public class ChangeTableSubmitDTO
    {
        public int OrderTableId { get; set; }
        public int NewTableId { get; set; }
        public int OldTableId { get; set; }

        public int CreateUser { get; set; }
    }

    public class AddTableSubmitDTO
    {
        public int OrderTableId { get; set; }
        public List<int> NewTableIds { get; set; }
        public int CreateUser { get; set; }
        public int CurrentMarketId { get; set; }
        public int CompanyId { get; set; }
    }

    public class CancelOrderTableSubmitDTO
    {
        public int OrderTableId { get; set; }
        public int CreateUser { get; set; }
    }

    /// <summary>
    /// 并台DTO 对象
    /// </summary>
    public class JoinTableDTO : ChangeTableDTO
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
    }

    /// <summary>
    /// 并台提交对象
    /// </summary>
    public class JoinTableSubmitDTO
    {
        /// <summary>
        /// 需要并台的台号订单对象
        /// </summary>
        public JoinTableDTO FromTableObj { get; set; }

        /// <summary>
        /// 并台后的台号订单对象
        /// </summary>
        public JoinTableDTO ToTableObj { get; set; }

        /// <summary>
        /// 操作用户Id
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// 拆台DTO对象
    /// </summary>
    public class SeparateTableDTO : ChangeTableDTO
    {
    }

    /// <summary>
    /// 拆台提交对象
    /// </summary>
    public class SeparateTableSubmitDTO : ChangeTableSubmitDTO
    {
        /// <summary>
        /// 选中的订单明细信息
        /// </summary>
        public List<OrderDetailDTO> SelectedList { get; set; }
    }


    public class OrderPayHistoryDTO
    {
        public int Id { get; set; }
        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int OrderId { get; set; }
        ///<summary>
        /// 付款方式(1:现金,2:信用卡,3:会员卡,4:转账,5:定期结算,6:支付宝,7:微信)
        ///</summary>
        public int CyddPayType { get; set; }
        ///<summary>
        /// 付款金额
        ///</summary>
        public decimal PayAmount { get; set; }
        ///<summary>
        /// 付款状态(1:未付,2:已付,3:已退)
        ///</summary>
        public CyddJzStatus CyddJzStatus { get; set; }
        /// <summary>
        /// 付款类型
        /// </summary>
        public CyddJzType CyddJzType { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUser { get; set; }

        public int MarketId { get; set; }

        public DateTime BillDate { get; set; }
        public int R_Restaurant_Id { get; set; }
    }

    public class OrderPayHistoryListDTO
    {
        public int Id { get; set; }
        ///<summary>
        /// 餐饮订单
        ///</summary>
        public int OrderId { get; set; }
        ///<summary>
        /// 付款方式(1:现金,2:信用卡,3:会员卡,4:转账,5:定期结算,6:支付宝,7:微信)
        ///</summary>
        public string CyddPayType { get; set; }
        ///<summary>
        /// 付款金额
        ///</summary>
        public decimal PayAmount { get; set; }
        ///<summary>
        /// 付款状态(1:未付,2:已付,3:已退)
        ///</summary>
        public string CyddJzStatus { get; set; }
        /// <summary>
        /// 付款类型
        /// </summary>
        public CyddJzType CyddJzType { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    /// <summary>
    /// 订单列表查询实体类
    /// </summary>
    public class OrderListSearchDTO : BaseSearch
    {
        //餐厅，区域，订单来源，订单状态，开始日期，结束日期
        public int Restaurant { get; set; }
        public int Area { get; set; }
        public int CyddOrderSource { get; set; }
        public int CyddStatus { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单查询
        /// 预定查询 yd
        /// </summary>
        public bool IsReserveList { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        /// 管理餐厅集合
        /// </summary>
        public List<int> ManagerRestaurant { get; set; }

        /// <summary>
        /// 分市Id
        /// </summary>
        public int MarketId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 是否包括已删除订单
        /// </summary>
        public bool IsIncludeDelete { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public OrderSearchListType OrderSearchListType { get; set; }
    }

    #region 结账画面相关Dto

    /// <summary>
    /// 结账DTO
    /// </summary>
    public class CheckoutReqDTO
    {
        /// <summary>
        /// 待结账订单Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 待结账订单下的Table的Id集合
        /// </summary>
        public List<int> TableIds { get; set; }
        /// <summary>
        /// 是否反结
        /// </summary>
        public OrderTableStatus OrderTableStatus { get; set; }
    }

    /// <summary>
    /// 单品折设置请求参数DTO
    /// </summary>
    public class SingleProductDiscountSetRequestDto
    {
        public int OrderDetailID { get; set; } //订单详情ID

        public decimal DiscountRate { get; set; }//折扣率
    }



    /// <summary>
    /// 整单全部结账或按台号结账请求参数
    /// </summary>
    public class WholeOrPartialCheckoutDto
    {
        public WholeOrPartialCheckoutDto()
        {
            ListOrderPayRecordDTO = new List<OrderPayRecordDTO>();
            TableIds = new List<int>();
            ListOrderDetailDTO = new List<OrderDetailDTO>();
            AuthUserList = new List<AuthUserDTO>();
        }

        /// <summary>
        /// 折扣比率
        /// </summary>
        public decimal DiscountRateNow { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 本次结账金额
        /// </summary>
        public decimal Money { get; set; }


        /// <summary>
        /// 本次结账服务费金额
        /// </summary>
        public decimal ServiceAmount { get; set; }


        /// <summary>
        /// 本次结账赠送金额
        /// </summary>
        public decimal GiveAmount { get; set; }

        /// <summary>
        /// 本次结账折扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 本次结账抹零金额
        /// </summary>
        public decimal ClearAmount { get; set; }

        /// <summary>
        /// 本次结账应收金额
        /// </summary>
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// 本次结账消费金额
        /// </summary>
        public decimal ConAmount { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public int OperateUser { get; set; }

        /// <summary>
        /// 操作人Code
        /// </summary>
        public string OperateUserCode { get; set; }

        /// <summary>
        /// 当前操作用户权限折扣
        /// </summary>
        public decimal AuthPermissionDiscount { get; set; }

        /// <summary>
        /// 待结账订单下的Table的Id集合
        /// </summary>
        public List<int> TableIds { get; set; }

        /// <summary>
        /// 餐厅所属Company
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 订单付款记录
        /// </summary>
        public List<OrderPayRecordDTO> ListOrderPayRecordDTO { get; set; }

        /// <summary>
        /// 菜品列表
        /// </summary>
        public List<OrderDetailDTO> ListOrderDetailDTO { get; set; }

        /// <summary>
        /// 结账授权用户List
        /// </summary>
        public List<AuthUserDTO> AuthUserList { get; set; }

        /// <summary>
        /// 折扣方式
        /// </summary>
        public DiscountMethods DiscountMethod { get; set; }

        /// <summary>
        /// 方案折Id
        /// </summary>
        public int SchemeDiscountId { get; set; }

        /// <summary>
        /// 四舍五入 值
        /// </summary>
        public decimal Fraction { get; set; }

        /// <summary>
        /// 当前分市Id
        /// </summary>
        public int CurrentMarketId { get; set; }

        /// <summary>
        /// 餐厅名称
        /// </summary>
        public string RestaurantName { get; set; }
        /// <summary>
        /// 订单联系人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 订单创建人名称
        /// </summary>
        public string OperUserName { get; set; }

        /// <summary>
        /// 是否反结操作 false = 正常结账；true = 反结操作
        /// </summary>
        public bool IsReCheckout { get; set; }
        public OrderTableStatus OrderTableStatus { get; set; }
        public string Remark { get; set; }
        public int PreOrderMainPayId { get; set; }
    }

    #endregion

    /// <summary>
    /// 预定预测查询条件实体
    /// </summary>
    public class ForecastSearchDTO
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 餐厅
        /// </summary>
        public int Restaurant { get; set; }
    }

    /// <summary>
    /// 预定预测查询返回列表
    /// </summary>
    public class ForecastReserveInfoDTO
    {
        /// <summary>
        /// 餐厅ID
        /// </summary>
        public int RestaurantId { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 台号ID
        /// </summary>
        public int TableId { get; set; }
        /// <summary>
        /// 台号名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 分市Id
        /// </summary>
        public int MarketId { get; set; }
        /// <summary>
        /// 分市名称
        /// </summary>
        public string MarketName { get; set; }
        /// <summary>
        /// 预定日期
        /// </summary>
        public DateTime? ReserveDate { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 预定人数
        /// </summary>
        public string PersonNum { get; set; }

        public DateTime? BookingDate { get { return ReserveDate.HasValue ? ReserveDate.Value.Date : (DateTime?)null; } }

        public decimal BookingAmount { get; set; }
    }

    public class ForecastInfoDTO
    {
        public ForecastInfoDTO()
        {
            DateList = new List<ForecastDateDTO>();
            TableList = new List<ForecastTableDTO>();
            MarketList = new List<MarketListDTO>();
        }

        public List<ForecastDateDTO> DateList { get; set; }

        public List<ForecastTableDTO> TableList { get; set; }

        public List<MarketListDTO> MarketList { get; set; }
    }

    public class ForecastDateDTO
    {
        public DateTime DayOfDate { get; set; }

        public string TitleDate { get; set; }

        /// <summary>
        /// 星期几
        /// </summary>
        public string DayOfWeekName { get; set; }
    }

    public class ForecastTableDTO
    {
        public ForecastTableDTO()
        {
            BookingList = new List<ForecastReserveInfoDTO>();
        }

        public int RestaurantId { get; set; }

        public int TableId { get; set; }

        public string TableName { get; set; }
        
        public List<ForecastReserveInfoDTO> BookingList { get; set; }
    }



    public class TableLinkOrderDTO : R_OrderTable
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public CyddStatus OrderStatus { get; set; }

        /// <summary>
        /// 订单状态描述
        /// </summary>
        public string OrderStatusDesc
        {
            get
            {
                return Enum.GetName(typeof(CyddStatus), OrderStatus);
            }
        }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime? OrderCreatedTime { get; set; }

        /// <summary>
        /// 订单总人数
        /// </summary>
        public int OrderPersonNum { get; set; }

        /// <summary>
        /// 订单起菜方式
        /// </summary>
        public CyddCallType OrderCallType { get; set; }
        public int MemberCardId { get; set; }
        public string ContactPerson { get; set; }
        public string ContactTel { get; set; }
    }

    /// <summary>
    /// 取消订单的操作对象
    /// </summary>
    public class CancelOrderOperateDTO
    {
        public int CompanyId { get; set; }
        public int OrderId { get; set; }
        public int OperateUserId { get; set; }
    }

    /// <summary>
    /// 结账单/预结单
    /// </summary>
    public class CheckOutBillDTO
    {
        public int OrderId { get; set; }
        public List<int> OrderTableIds { get; set; }
        public bool IsLocked { get; set; }
    }

    public class AuthUserDTO
    {
        /// <summary>
        /// 1-->折扣操作，2-->抹零操作, 3-->反结操作
        /// </summary>
        public AuthOperateTypes OperateType { get; set; }

        public int AuthUserId { get; set; }
    }

    public class SearchOrderStatisticalDTO
    {
        public SearchOrderStatisticalDTO()
        {
            OrderList = new List<OrderStatisticalDTO>();
            ListSummaryObj = new OrderListSummaryDTO();
        }

        public List<OrderStatisticalDTO> OrderList { get; set; }

        public OrderListSummaryDTO ListSummaryObj { get; set; }
    }

    /// <summary>
    /// 查单列表汇总信息对象
    /// </summary>
    public class OrderListSummaryDTO
    {
        /// <summary>
        /// 总单数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 总人数
        /// </summary>
        public int TotalPeopleNum { get; set; }

        /// <summary>
        /// 抹零金额汇总
        /// </summary>
        public decimal TotalClearAmount { get; set; }

        /// <summary>
        /// 四舍五入金额汇总
        /// </summary>
        public decimal TotalFractionAmount { get; set; }

        /// <summary>
        /// 折扣金额汇总
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }
        
        /// <summary>
        /// 服务费汇总
        /// </summary>
        public decimal TotalServiceAmount { get; set; }

        /// <summary>
        /// 实收金额汇总
        /// </summary>
        public decimal TotalRealAmount { get; set; }

        /// <summary>
        /// 消费金额汇总
        /// </summary>
        public decimal TotalConAmount { get; set; }

        /// <summary>
        /// 找零
        /// </summary>
        public decimal TotalGiveChangeAmount { get; set; }
    }

    public class OrderStatisticalDTO
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }


        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
        ///<summary>
        /// 订单状态(1:预定,2:开台,3:送厨,4:用餐中,5:结账,6:取消,7:订单菜品修改,8:并台)
        ///</summary>
        public CyddStatus CyddStatus { get; set; }
        ///<summary>
        /// 预定用餐时间
        ///</summary>
        public DateTime? ReserveDate { get; set; }
        ///<summary>
        /// 消费金额
        ///</summary>
        public decimal ConAmount { get; set; }
        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreateDate { get; set; }
        ///<summary>
        /// 餐饮分市
        ///</summary>
        public int MarketId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUser { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 订单状态(1:预定,2:开台,3.点餐,4:送厨,5:用餐中,6:结账,7:取消,8:订单菜品修改 9.并台) 
        /// </summary>
        public string OrderStatus
        {
            get
            {
                return CyddStatus > 0 ? Enum.GetName(CyddStatus.GetType(), CyddStatus) : "";
            }
        }

        ///<summary>
        /// 订单来源（用户自定义）(1:自来客,2:营销推广,3:协议客户,4:微信) TypeId =10002
        ///</summary>
        public int CyddOrderSource { get; set; }
        public string SourceTypeName
        {
            get; set;
        }

        public string OrderTypeName { get; set; }

        public string MarketName { get; set; }
        public int SumPeopleNum { get; set; }
        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal SumConAmount { get; set; }
        public decimal SumBookingAmount { get; set; }
        public decimal SumRealAmount { get; set; }
        public decimal SumFractionAmount { get; set; }
        public decimal SumClearAmount { get; set; }
        public decimal SumServiceAmount { get; set; }
        public decimal SumDiscountAmount { get; set; }
        public decimal SumGiveAmount { get; set; }
        public string OrderTableNames { get; set; }
    }

    /// <summary>
    /// 删除(恢复)订单业务实体
    /// </summary>
    public class OrderDeleteDTO
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// 创建发票实体
    /// </summary>
    public class InvoiceCreateDTO : R_Invoice
    {
        #region ExtendModel
        public int RestaurantId { get; set; }
        #endregion
    }

    /// <summary>
    /// 发票列表
    /// </summary>
    public class InvoiceListDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
        public string OrderMainPayMarket { get; set; }
        public Nullable<DateTime> OrderMainPayDate { get; set; }
        public string CreateUserName { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> BillDate { get; set; }
    }

    public class OrderDetailCauseDTO : R_OrderDetailCause
    {

    }

    public class OrderDetailCauseSearch : BaseSearch
    {
        public int CauseType { get; set; }
    }

    public class OrderDetailCauseListDTO : R_OrderDetailCause
    {
        public string CauseTypeName { get; set; }
    }
}
