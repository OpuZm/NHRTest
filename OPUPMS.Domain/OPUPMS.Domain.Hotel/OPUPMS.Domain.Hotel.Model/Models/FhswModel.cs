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
    /// 房号事务 实体类
    /// </summary>
    [Table("Fhsw")]
    public class FhswModel : Entity<int>
    {
        static FhswModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<FhswModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Fhswxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// 房号事务序号 主键 标识列
        ///// </summary>        
        //public int Fhswxh00 { get; set; }

        /// <summary>
        /// 房号 不为null
        /// </summary>
        public string Fhswfh00 { get; set; }

        /// <summary>
        /// 标志
        /// </summary>
        public string Fhswbzs0 { get; set; }

        /// <summary>
        /// 事务代码 关联系统代码 SW
        /// </summary>
        public string Fhswswdm { get; set; }

        /// <summary>
        /// 原因代码 取值 Xtdm.Xtdmlx00=事务代码
        /// </summary>
        public string Fhswyydm { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? Fhswksrq { get; set; }

        /// <summary>
        /// 终止日期
        /// </summary>
        public DateTime? Fhswzzrq { get; set; }

        /// <summary>
        /// 操作代码  外键  关联Czdm.Czdmdm00 
        /// </summary>
        public string Fhswczdm { get; set; }

        /// <summary>
        /// 账务日期
        /// </summary>
        public DateTime? Fhswzwrq { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Fhswczsj { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Fhswbz00 { get; set; }
        
        /// <summary>
        /// 提交操作  关联Czdm.Czdmdm00 
        /// </summary>
        public string Fhswtjcz { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime? Fhswtjrq { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? Fhswtjsj { get; set; }
        
    }
}
