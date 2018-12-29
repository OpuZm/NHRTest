/***********************************************************************
 * Module:  CyddMx.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyddMx
 ***********************************************************************/

// <summary>
// 餐饮订单明细
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮订单明细
    public class R_OrderDetail
    {


        ///<summary>
        ///</summary>
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


        ///<summary>
        /// 折扣率
        ///</summary>
        public decimal DiscountRate { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUser { get; set; }
        public int HookNum { get; set; }
        public string ExtendName { get; set; }
        public string CyddMxName { get; set; }
        public DishesStatus DishesStatus { get; set; }
        public string Unit { get; set; }
        /// <summary>
        /// 当前菜品消费金额
        /// </summary>
        public decimal OriginalTotalPrice { get; set; }
        /// <summary>
        /// 当前菜品实付金额（折后金额）
        /// </summary>
        public decimal PayableTotalPrice { get; set; }
        /// <summary>
        /// 当前菜品赠送金额
        /// </summary>
        public decimal GiveTotalPrice { get; set; }
        public bool IsListPrint { get; set; }
        public decimal ExtractPrice { get; set; }
    }
}