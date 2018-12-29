using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_WeixinPrint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int R_Restaurant_Id { get; set; }
        public int Print_Id { get; set; }
        public PrintType PrintType { get; set; }
    }
}
