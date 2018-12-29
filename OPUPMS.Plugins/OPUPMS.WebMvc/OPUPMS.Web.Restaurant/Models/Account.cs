using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPUPMS.Web.Restaurant.Models
{
    public class LoginInput
    {
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string Code { get; set; }
    }
}