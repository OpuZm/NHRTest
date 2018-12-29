using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于价格体系 表实体 Jgtx
    /// </summary>
    public class PriceSystemInfo
    {
        /// <summary>
        /// 价格体系Id 标识列主键  Jgtxxh00
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类别 Jgtxsslb
        /// 关联系统代码 JL
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 名称 Jgtxmc00
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开始日期 Jgtxksrq
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期 Jgtxjsrq
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 星期 Jgtxxq00
        /// </summary>
        public string Weeks { get; set; }

        /// <summary>
        /// 特殊日期 Jgtxtsrq
        /// </summary>
        public string SpecialDate { get; set; }

        /// <summary>
        /// 人数 Jgtxrs00
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 优先级别 Jgtxyxjb
        /// </summary>
        public int UseLevel { get; set; }

        /// <summary>
        /// 内容 Jgtxtext
        /// </summary>
        public string Contents { get; set; }
        
    }
}
