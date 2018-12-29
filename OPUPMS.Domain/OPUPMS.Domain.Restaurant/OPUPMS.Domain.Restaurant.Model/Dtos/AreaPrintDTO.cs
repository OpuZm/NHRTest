using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class WeixinPrintDTO : R_WeixinPrint
    {
        public List<R_WeixinPrintArea> PrintAreas { get; set; }
    }

    public class WeixinPrintSearchDTO : BaseSearch
    {
        public int Restaurant { get; set; }
        public PrintType PrintType { get; set; }
    }

    public class WeixinPrintListDTO : R_WeixinPrint
    {
        public string RestaurantName { get; set; }
        public List<string> AreaNames { get; set; }
        public string PrintName { get; set; }
    }
}
