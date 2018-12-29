using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Dtos
{
    public class MenuInfo
    {
        public int MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单的URL
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 对应于权限值
        /// </summary>
        public int MenuValue { get; set; }
        /// <summary>
        /// 菜单类型值
        /// </summary>
        public int MenuType { get; set; }
        public int SortId { get; set; }
        public string MenuHtmlContent { get; set; }
        public bool HasPermission { get; set; }

        public string IconName { get; set; }

        public int ParentMenuId { get; set; }
    }
}
