using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPUPMS.Restaurant.Web.Models
{
    public class LoginInput
    {
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string Code { get; set; }
        public int CompanyId { get; set; }
    }
}