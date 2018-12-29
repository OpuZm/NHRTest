using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_OrderDetailCause
    {
        public int Id { get; set; }
        public CauseType CauseType { get; set; }
        public string Remark { get; set; }
        public bool IsDelete { get; set; }
        public int R_Company_Id { get; set; }
    }
}
