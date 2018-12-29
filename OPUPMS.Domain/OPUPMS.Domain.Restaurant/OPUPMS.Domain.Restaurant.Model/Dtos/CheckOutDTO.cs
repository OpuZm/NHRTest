using System;
using System.Collections.Generic;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    /// <summary>
    /// 结账相关DTO
    /// </summary>
    public class CheckOutOrderDTO
    {
        public CheckOutOrderDTO()
        {
            TableIds = new List<int>();
            OrderTableList = new List<CheckOutOrderTableDTO>();
            PayTypeList = new List<PayMethodListDTO>();
            PaidRecordList = new List<OrderPayRecordDTO>();
            MainPayList = new List<OrderMainPayDTO>();
            CheckOutStaticsList = new List<ProjectCheckOutStaticsDTO>();
            CheckOutRemovePayType = new List<int>();
        }

        /// <summary>
        /// 主键自增
        /// </summary>
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
        /// 订单来源(1:自来客,2:营销推广,3:协议客户,4:微信)
        ///</summary>
        public CyddOrderSource CyddOrderSource { get; set; }
        ///<summary>
        /// 人数
        ///</summary>
        public int PersonNum { get; set; }
        ///<summary>
        /// 业务员
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
        /// 餐台Id列表
        /// </summary>
        public List<int> TableIds { get; set; }

        public int CustomerId { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeName { get; set; }

        public List<CheckOutOrderTableDTO> OrderTableList { get; set; }


        /// <summary>
        /// 操作人ID
        /// </summary>
        public int OperateUser { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperateUserName { get; set; }

        /// <summary>
        /// 支付方式列表
        /// </summary>
        public List<PayMethodListDTO> PayTypeList { get; set; }

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
        /// 当前操作用户权限抹零值
        /// </summary>
        public decimal AuthClearValue { get; set; }

        //找零金额
        public decimal GiveChange { get; set; }

        /// <summary>
        /// 支付记录
        /// </summary>
        public List<OrderPayRecordDTO> PaidRecordList { get; set; }
        public List<OrderMainPayDTO> MainPayList { get; set; }
        public int CheckoutRound { get; set; }
        public string RestaurantName { get; set; }
        public string MarketName { get; set; }
        public bool IsPreCheckOut { get; set; }
        public int PreOrderMainPayId { get; set; }
        public int PrintModel { get; set; }
        public List<ProjectCheckOutStaticsDTO> CheckOutStaticsList { get; set; }

        public List<int> CheckOutRemovePayType { get; set; }
    }


    public class CheckOutOrderTableDTO : R_OrderTable
    {
        public CheckOutOrderTableDTO()
        {
            OrderDetailList = new List<CheckOutOrderDetailDTO>();
        }
        public List<CheckOutOrderDetailDTO> OrderDetailList { get; set; }

        #region  台号信息
        ///<summary>
        /// 餐饮餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 座位数
        ///</summary>
        public int SeatNum { get; set; }


        ///<summary>
        /// 描述信息
        ///</summary>
        public string Describe { get; set; }


        ///<summary>
        /// 状态(1:空置,2:在用,3:清理)
        ///</summary>
        public CythStatus CythStatus { get; set; }


        ///<summary>
        /// 服务费率
        ///</summary>
        public Nullable<decimal> ServerRate { get; set; }


        ///<summary>
        /// 餐饮区域
        ///</summary>
        public int R_Area_Id { get; set; }
        #endregion
    }

    public class CheckOutOrderDetailDTO
    {
        public CheckOutOrderDetailDTO()
        {
            OrderDetailAllExtends = new List<OrderDetailExtendDTO>();
            R_OrderDetailRecord_List = new List<R_OrderDetailRecord>();

            ProExtend = new List<OrderDetailExtendDTO>();
            ProExtendExtra = new List<OrderDetailExtendDTO>();
            ProExtendRequire = new List<OrderDetailExtendDTO>();
        }

        /// <summary>
        /// 主键自增id
        /// </summary>
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
        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUser { get; set; }
        /// <summary>
        /// 勾单数
        /// </summary>
        public int HookNum { get; set; }
        /// <summary>
        /// 手写菜名称
        /// </summary>
        public string ExtendName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string CyddMxName { get; set; }
        /// <summary>
        /// 起菜状态(1:即起 2:叫起)
        /// </summary>
        public int DishesStatus { get; set; }
        /// <summary>
        /// 拓展集合
        /// </summary>
        public List<OrderDetailExtendDTO> OrderDetailAllExtends { get; set; }

        #region 拓展分类列表

        /// <summary>
        /// 订单菜品做法列表
        /// </summary>
        public List<OrderDetailExtendDTO> ProExtend { get; set; }

        /// <summary>
        /// 订单菜品要求列表
        /// </summary>
        public List<OrderDetailExtendDTO> ProExtendRequire { get; set; }

        /// <summary>
        /// 订单菜品配菜列表
        /// </summary>
        public List<OrderDetailExtendDTO> ProExtendExtra { get; set; } 
        #endregion


        /// <summary>
        /// 操作记录集合
        /// </summary>
        public List<R_OrderDetailRecord> R_OrderDetailRecord_List { get; set; }

        public int CategoryId { get; set; }
        
        /// <summary>
        /// 是否可打折，由r-project.Property判断赋值
        /// </summary>
        public int IsDiscount { get; set; }
        /// <summary>
        /// 餐饮项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 强制打折
        /// </summary>
        public int IsForceDiscount { get; set; }

        /// <summary>
        /// 订单明细操作记录分组统计
        /// </summary>
        public List<OrderDetailRecordCountDTO> ODRecordCountList { get; set; }

        /// <summary>
        /// 是否收取服务费
        /// </summary>
        public int IsServiceCharge { get; set; } 
    }

    public class OrderDetailExtendDTO : R_OrderDetailExtend
    {
        /// <summary>
        /// 菜品下关联的扩展项的类型(做法、要求、配菜)
        /// </summary>
        public CyxmKzType ExtendType { get; set; }
    }

    public class CheckOutPrint
    {
        public string OrderType { get; set; }
        public string RestaurantName { get; set; }
        public string CheckOutUser { get; set; }
        public string OpenUser { get; set; }
        public string ReserveUser { get; set; }
        public string PrintDate { get; set; }
        public string BillDate { get; set; }
        public string Market { get; set; }
        public string PersonNum { get; set; }
        public string OrderNo { get; set; }
        public string TableName { get; set; }
        public List<OrderDetailDTO> ListOrderDetailDTO { get; set; }
        public string ConAmount { get; set; }
        public string GiveAmount { get; set; }
        public string DiscountRateNow { get; set; }
        public string DiscountAmount { get; set; }
        public string OriginalAmount { get; set; }
        public string ContactPerson { get; set; }
    }


    public class OrderPaymentDTO
    {
        public OrderPaymentDTO()
        {
            //PayRecordList = new List<OrderPayRecordDTO>();
            //OrderTableList = new List<R_OrderTable>();
            TabIdList = new List<int>();
        }
        public int OrderId { get; set; }
        public int OrderMainPayId { get; set; }

        public int MarketId { get; set; }
        public string MarketName { get; set; }

        /// <summary>
        /// 账务日期
        /// </summary>
        public string BillDate { get; set; }

        //public List<OrderPayRecordDTO> PayRecordList { get; set; }

        //public List<R_OrderTable> OrderTableList { get; set; }

        public List<int> TabIdList { get; set; }
    }

    public class OrderSearchDTO
    {
        public OrderSearchDTO(CheckOutOrderDTO obj)
        {
            OrderObj = obj;
            OrderRecordList = new List<OrderRecordDetailDTO>();
            InvoiceList = new List<InvoiceListDTO>();
        }

        public CheckOutOrderDTO OrderObj { get; set; }


        public List<OrderRecordDetailDTO> OrderRecordList { get; set; }

        public List<InvoiceListDTO> InvoiceList { get; set; }

        public string OrderStautsName
        {
            get
            {
                return OrderObj == null ? "" : Enum.GetName(typeof(CyddStatus), OrderObj.CyddStatus);
            }
        }

        /// <summary>
        /// 菜品数量统计
        /// </summary>
        public decimal DetailCounts
        {
            get
            {
                if (OrderObj == null)
                    return 0;
                else
                {
                    decimal count = 0;
                    foreach (var table in OrderObj.OrderTableList)
                    {
                        foreach (var detailItem in table.OrderDetailList)
                        {
                            count += detailItem.Num;
                        }
                    }
                    return count;
                }
            }
        }

        public string CreateUserName { get; set; }
    }

    public class VerifySourceInfoDTO
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public decimal OperateValue { get; set; }
        public int RestaruantId { get; set; }
        /// <summary>
        /// 支付操作方式
        /// </summary>
        public int PayMethod { get; set; }
    }

    public class SearchTableForCheckout
    {
        public SearchTableForCheckout()
        {
            DiscountDetailList = new List<SchemeDiscountDetailDTO>();
        }

        public int OrderTableId { get; set; }
        public int TableId { get; set; }
        public int AreaId { get; set; }
        public List<SchemeDiscountDetailDTO> DiscountDetailList { get; set; }
    }

    public class MemberInfoDTO
    {
        /// <summary>
        /// 客人历史序号Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string MemberCardNo { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberName { get; set; }
        public byte[] MemberPwbByte { get; set; }
        /// <summary>
        /// 会员密码
        /// </summary>
        public string MemberPwd { get { return MemberPwbByte.IsEmpty() ? string.Empty : MemberPwbByte.ToHexString(); } }
        /// <summary>
        /// 会员GPID
        /// </summary>
        public string MemberGPID { get; set; }
        /// <summary>
        /// 会员GUID
        /// </summary>
        public Guid MemberGUID { get; set; }
        /// <summary>
        /// 会员卡余额
        /// </summary>
        public decimal CardBalance { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string MemberIdentityNo { get; set; }
        /// <summary>
        /// 会员电话号码
        /// </summary>
        public string MemberPhoneNo { get; set; }
        /// <summary>
        /// 会员性别
        /// </summary>
        public string MemberGender { get; set; }
    }

    public class SearchKrzlInfo
    {
        public int CustomerId { get; set; }
        public decimal? LimitAmount { get; set; }   //余额
        public decimal? LastAmount { get; set; }    //限额
        public decimal? PreAmount { get; set; } //预售权
        public string CustomerName { get; set; }
        public string RoomNo { get; set; }  //房号
    }

    public class CheckOutResultDTO
    {
        public int OrderId { get; set; }
        public int OrderMainPayId { get; set; }
        public List<int> OrderTables { get; set; }
        public string ReverTableNames { get; set; }
    }

    public class ReverseOrderDTO
    {
        public int MainPayId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }

        public int CurrentMarketId { get; set; }
        public string UserCode { get; set; }
    }

    /// <summary>
    /// Lxdm 表信息
    /// </summary>
    public class SearchLxdmInfo
    {
        /// <summary>
        /// 类型代码主键 Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 挂账限制余额
        /// </summary>
        public decimal LimitAmount { get; set; }

        /// <summary>
        /// 挂账剩余余额
        /// </summary>
        public decimal RemainAmount { get; set; }
    }

    public class ProjectCheckOutStaticsDTO
    {
        public int ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Num { get; set; }
    }
}
