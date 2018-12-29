using OPUPMS.Domain.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Dtos
{
    /// <summary>
    /// 对应于Czdm 
    /// </summary>
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string RoleId { get; set; }

        public string ConnectionString { get; set; }

        public string GroupCode { get; set; }

        public LoginState State { get; set; }
        public int Permission { get; set; }
        public string ManagerRestaurant { get; set; }
        public decimal MinDiscountValue { get; set; }
        public decimal MaxClearValue { get; set; }
    }
}
