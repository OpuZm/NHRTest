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
    /// 清洁报道 实体类
    /// </summary>
    [Table("Qjbd")]
    public class QjbdModel : Entity<int>
    {
        static QjbdModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<QjbdModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Qjbdxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// 序号 主键 标识列
        ///// </summary>        
        //public int Qjbdxh00 { get; set; }

        /// <summary>
        /// 账务日期
        /// </summary>
        public DateTime Qjbdzwrq { get; set; }

        /// <summary>
        /// 报道方式
        /// </summary>
        public int? Qjbdbdfs { get; set; }

        /// <summary>
        /// 服务人员
        /// </summary>
        public string Qjbdfwry { get; set; }

        /// <summary>
        /// 操作员 关联操作代码 Czdm.Czdmdm00  不为null
        /// </summary>
        public string Qjbdczdm { get; set; }

        /// <summary>
        /// 操作日期  不为null
        /// </summary>
        public DateTime Qjbdczsj { get; set; }

        /// <summary>
        /// 状态  Y 分配，X 撤销
        /// </summary>
        public string Qjbdzt00 { get; set; }
        
        /// <summary>
        /// 撤销操作代码  关联Czdm.Czdmdm00 
        /// </summary>
        public string Qjbdcxcz { get; set; }

        /// <summary>
        /// 撤销操作时间
        /// </summary>
        public DateTime? Qjbdcxsj { get; set; }
        
    }
}
