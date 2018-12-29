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
    /// 房间物品 实体对象
    /// </summary>
    [Table("Fjwp")]
    public class FjwpModel : Entity<int>
    {
        static FjwpModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<FjwpModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Fjwpxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// 序号 主键 标识列
        ///// </summary>        
        //public int Fjwpxh00 { get; set; }

        /// <summary>
        /// 房号代码  关联房号代码 Fhdm.Fhdmdm00  不为null
        /// </summary>
        public string Fjwpfhdm { get; set; }

        /// <summary>
        /// 代码 不为null
        /// </summary>
        public string Fjwpdm00 { get; set; }

        /// <summary>
        /// 数量 不为null
        /// </summary>
        public int Fjwpsl00 { get; set; }

        /// <summary>
        /// 操作员 关联操作代码 Czdm.Czdmdm00  不为null
        /// </summary>
        public string Fjwpczdm { get; set; }

        /// <summary>
        /// 操作时间  不为null
        /// </summary>
        public DateTime Fjwpczsj { get; set; }
        
        /// <summary>
        /// 帐号  关联Krzl.Krzlzh00 
        /// </summary>
        public int? Fjwpzh00 { get; set; }

        /// <summary>
        /// 类型  不为null
        /// </summary>
        public string Fjwplx00 { get; set; }

        /// <summary>
        /// 账务日期  不为null
        /// </summary>
        public DateTime Fjwpzwrq { get; set; }
        
    }
}
