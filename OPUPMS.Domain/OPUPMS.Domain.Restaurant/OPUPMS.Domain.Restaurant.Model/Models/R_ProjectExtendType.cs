using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// <summary>
    /// 餐饮项目扩展类型
    /// </summary>
    public class R_ProjectExtendType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int R_Company_Id { get; set; }
        public bool IsDelete { get; set; }
    }
}
