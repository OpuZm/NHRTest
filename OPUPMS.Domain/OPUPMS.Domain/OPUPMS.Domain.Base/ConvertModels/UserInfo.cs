using OPUPMS.Domain.Base.Services;
using OPUPMS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.ConvertModels
{
    /// <summary>
    /// 对应于操作代码表实体 Czdm 
    /// </summary>
    public class UserInfo
    {
        public int UserId { get; set; }
        /// <summary>
        /// 主键 Czdmdm00
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名 Czdmmc00
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码 Czdmmm00
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 部门 Czdmbm00
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 出生日期 Czdmcsrq
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 角色 Czdmqxzb
        /// </summary>
        public string RoleId { get; set; }
        public int Permission { get; set; }
        public string ManagerRestaurant { get; set; }
        public decimal Discount { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 最大抹零金额
        /// </summary>
        public decimal MaxClearValue { get; set; }
        /// <summary>
        /// 是否技术支持
        /// </summary>
        public bool IsTechnicalAssistance { get; set; }
    }
}
