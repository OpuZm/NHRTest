using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Model
{
    /// <summary>
    /// 清洁分配 实体类
    /// </summary>
    [Table("Qjfp")]
    public class QjfpModel : Entity<int>
    {
        static QjfpModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<QjfpModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Qjfpxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// 序号 主键 标识列
        ///// </summary>        
        //public int Qjfpxh00 { get; set; }

        /// <summary>
        /// 账务日期  不为null
        /// </summary>
        public DateTime Qjfpzwrq { get; set; }

        /// <summary>
        /// 服务人员
        /// </summary>
        public string Qjfpfwry { get; set; }

        /// <summary>
        /// 房号代码  关联房号代码 Fhdm.Fhdmdm00  不为null
        /// </summary>
        public string Qjfpfhdm { get; set; }

        /// <summary>
        /// 分配操作员 关联操作代码 Czdm.Czdmdm00  不为null
        /// </summary>
        public string Qjfpfpcz { get; set; }

        /// <summary>
        /// 分配时间  不为null
        /// </summary>
        public DateTime Qjfpfpsj { get; set; }

        /// <summary>
        /// 状态  Y 分配，X 撤销  不为null
        /// </summary>
        public string Qjfpzt00 { get; set; }

        /// <summary>
        /// 清洁时间
        /// </summary>
        public DateTime? Qjfpqjsj { get; set; }

        /// <summary>
        /// 撤销操作代码  关联Czdm.Czdmdm00 
        /// </summary>
        public string Qjfpcxcz { get; set; }

        /// <summary>
        /// 撤销操作时间
        /// </summary>
        public DateTime? Qjfpcxsj { get; set; }
        
    }
}
