using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Base.Models
{
    /// <summary>
    /// 系统参数 实体类
    /// </summary>
    [Table("Xtcs")]
    public class XtcsModel : Entity<string>
    {
        static XtcsModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<XtcsModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Xtcsdm00");
                    });
        }

        ///// <summary>
        ///// 系统参数代码 主键列
        ///// </summary>        
        //public string Xtcsdm00 { get; set; }

        /// <summary>
        /// 系统参数字符串
        /// </summary>
        public string Xtcszc00 { get; set; }

        /// <summary>
        /// 系统参数名称  不为null
        /// </summary>
        public string Xtcsmc00 { get; set; }

        /// <summary>
        /// 二进制列
        /// </summary>
        public byte[] Xtcsvar0 { get; set; }

        /// <summary>
        /// 参数日期
        /// </summary>
        public DateTime? Xtcsrq00 { get; set; }

        /// <summary>
        /// 整数00
        /// </summary>
        public int Xtcszs00 { get; set; }

        /// <summary>
        /// 小数01
        /// </summary>
        public decimal Xtcsxs01 { get; set; }
        
        /// <summary>
        /// 小数02
        /// </summary>
        public decimal Xtcsxs02 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Xtcsbz00 { get; set; }

        /// <summary>
        /// 整数01
        /// </summary>
        public int? Xtcszs01 { get; set; }

        /// <summary>
        /// 整数02
        /// </summary>
        public int? Xtcszs02 { get; set; }
        
    }    
}
