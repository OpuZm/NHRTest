using System;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class OrderRecordDTO
    {
    }

    public class OrderRecordDetailDTO
    {
        //Id, CreateUser, CreateDate, CyddCzjlStatus, CyddCzjlUserType, Remark, R_Order_Id, R_OrderTable_Id
        public int Id { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 操作类型(1:预定,2:开台,3:点餐,4:送厨,5:用餐中,6:结账,7:取消,8:订单菜品修改 9.并台) 
        /// </summary>
        public CyddStatus CyddCzjlStatus { get; set; }
        /// <summary>
        /// 用户类型(1:餐饮员工,2:会员) 
        /// </summary>
        public CyddCzjlUserType CyddCzjlUserType { get; set; }
        public string Remark { get; set; }
        public int R_Order_Id { get; set; }
        public int R_OrderTable_Id { get; set; }

        public string UserName { get; set; }

        public int TableId { get; set; }
        public string TableName { get; set; }
        public string OperatorTypeName
        {
            get
            {
                return (int)CyddCzjlStatus > 0 ?
                    Enum.GetName(typeof(CyddStatus), CyddCzjlStatus) : "";
            }
        }
        public string UserTypeName
        {
            get
            {
                return (int)CyddCzjlUserType > 0 ?
                    Enum.GetName(typeof(CyddCzjlUserType), CyddCzjlUserType) : "";
            }
        }
    }

    /// <summary>
    /// 订单列表查询实体类
    /// </summary>
    public class OrderRecordSearchDTO : BaseSearch
    {
        //餐厅，区域，订单来源，订单状态，开始日期，结束日期
        public int RestaurantId { get; set; }
        public int TableId { get; set; }
        public int OrderId { get; set; }
    }
}
