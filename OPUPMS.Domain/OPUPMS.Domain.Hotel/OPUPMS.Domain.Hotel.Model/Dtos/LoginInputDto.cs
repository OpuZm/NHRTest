using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    public class LoginInputDto
    {
        public string HotelCode { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }
    }
}
