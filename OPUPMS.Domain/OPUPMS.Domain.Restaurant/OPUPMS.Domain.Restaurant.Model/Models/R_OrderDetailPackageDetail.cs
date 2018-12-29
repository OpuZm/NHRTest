using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    public  class R_OrderDetailPackageDetail
    {
        public int Id { get; set; }
        public int R_OrderDetail_Id { get; set; }
        public string Name { get; set; }
        public decimal Num { get; set; }
        public int R_ProjectDetail_Id { get; set; }
    }
}
