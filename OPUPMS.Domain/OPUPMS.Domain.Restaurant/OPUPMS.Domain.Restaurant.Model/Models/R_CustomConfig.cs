using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_CustomConfig
    {
        public int Id { get; set; }
        public string FunctionName { get; set; }
        public string ModuleName { get; set; }
        public int Sorted { get; set; }
        public int PageModule { get; set; }
        public string Colour { get; set; }
        public int R_Company_Id { get; set; }

    }
}
