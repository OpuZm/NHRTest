using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
   public  class R_DiscountCategory
    {
        public int Id { get; set; }
        public int R_Discount_Id { get; set; }
        public int R_Category_Id { get; set; }
        ///<summary>
        /// 折扣率
        ///</summary>
        public decimal DiscountRate { get; set; }
    }
}
