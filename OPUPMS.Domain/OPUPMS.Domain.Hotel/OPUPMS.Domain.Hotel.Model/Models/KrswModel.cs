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
    /// 客人事务 实体类
    /// </summary>
    [Table("Krsw")]
    public class KrswModel : Entity<int>
    {
        static KrswModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrswModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krswxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }
        
        ///// <summary>
        ///// Krswxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krswxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krswzh00 帐号  关联Krzl.Krzlzh00
        /// </summary>
        public virtual int Krswzh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krswbzs0 标志数
        /// </summary>
        public virtual string Krswbzs0
        {
            get;
            set;
        }

        /// <summary>
        /// Krswswdm 事务代码
        /// </summary>
        public virtual string Krswswdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krswyydm 原因代码  xtdmlx00=事务代码
        /// </summary>
        public virtual string Krswyydm
        {
            get;
            set;
        }

        /// <summary>
        /// Krswczdm 操作员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krswczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krswzwrq 账务日期
        /// </summary>
        public virtual DateTime? Krswzwrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krswczsj 操作时间
        /// </summary>
        public virtual DateTime Krswczsj
        {
            get;
            set;
        }

        /// <summary>
        /// Krswysz0 原始值
        /// </summary>
        public virtual string Krswysz0
        {
            get;
            set;
        }

        /// <summary>
        /// Krswgxz0 更新值
        /// </summary>
        public virtual string Krswgxz0
        {
            get;
            set;
        }

        /// <summary>
        /// Krswfpxh
        /// </summary>
        public virtual int? Krswfpxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krswtext
        /// </summary>
        public virtual string Krswtext
        {
            get;
            set;
        }

        /// <summary>
        /// Krswydh0
        /// </summary>
        public virtual int? Krswydh0
        {
            get;
            set;
        }

        /// <summary>
        /// Krswlx00
        /// </summary>
        public virtual string Krswlx00
        {
            get;
            set;
        }

        /// <summary>
        /// Krswxh01
        /// </summary>
        public virtual int? Krswxh01
        {
            get;
            set;
        }

        /// <summary>
        /// Krswxh02
        /// </summary>
        public virtual int? Krswxh02
        {
            get;
            set;
        }
    }    
}
