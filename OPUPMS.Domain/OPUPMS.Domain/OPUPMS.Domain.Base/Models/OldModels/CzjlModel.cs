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
    /// 操作记录实体类
    /// </summary>
    [Table("Czjl")] 
    public class CzjlModel : Entity<int>
    {
        static CzjlModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<CzjlModel>()
                .SetProperty(entity => entity.Id,
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Czjlxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// 序号自增 主键列
        ///// </summary>
        //public int Czjlxh00 { get; set; }

        /// <summary>
        /// 类型 不为null
        /// Z_0	修改密码
        /// Z_1 新增操作员
        /// Z_2 登录
        /// </summary>
        public string Czjllx00 { get; set; }

        /// <summary>
        /// 操作代码 不为null
        /// </summary>
        public string Czjlczdm { get; set; }

        /// <summary>
        /// 操作时间 不为null
        /// </summary>
        public DateTime? Czjlczsj { get; set; }

        /// <summary>
        /// 操作备注 
        /// </summary>
        public string Czjlczbz { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czjlysz0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czjlgxz0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czjlgl00 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czjlglh0 { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Czjlmc00 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Czjlbz00 { get; set; }

        /// <summary>
        /// 备注01
        /// </summary>
        public string Czjlbz01 { get; set; }

       
    }
}
