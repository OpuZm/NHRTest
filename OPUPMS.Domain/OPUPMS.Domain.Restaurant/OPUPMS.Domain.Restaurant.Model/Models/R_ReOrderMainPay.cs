using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_ReOrderMainPay
    {
        ///<summary>
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 餐饮订单ID
        ///</summary>
        public int R_Order_Id { get; set; }

        /// <summary>
        /// 账务日期
        /// </summary>
        public DateTime BillDate { get; set; }

        /// <summary>
        /// 分市
        /// </summary>
        public int R_Market_Id { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateUser { get; set; }

        public int DiscountType { get; set; }

        public int R_Discount_Id { get; set; }
        public string Remark { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
