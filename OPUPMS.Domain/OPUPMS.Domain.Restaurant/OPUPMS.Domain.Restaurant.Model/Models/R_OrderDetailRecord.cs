using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_OrderDetailRecord
    {
        public int Id { get; set; }
        public int R_OrderDetail_Id { get; set; }
        public CyddMxCzType CyddMxCzType { get; set; }
        public decimal Num { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUser { get; set; }
        public string Remark { get; set; }
        public bool IsCalculation { get; set; }
        public int R_OrderDetailCause_Id { get; set; }
        public string DetailCauseRemark { get; set; }
    }
}
