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
    /// AR账 实体类
    /// </summary>
    [Table("")]
    public class Arz0Model : Entity<int>
    {
        static Arz0Model()
        {
            OrmConfiguration.GetDefaultEntityMapping<Arz0Model>()
                .SetProperty(entity => entity.Id,
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Arz0xh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Arz0xh00 序号 主键列
        ///// </summary>
        //public virtual int Arz0xh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Arz0zwxh 账务序号 关联Krzw.Krzwxh00
        /// </summary>
        public virtual int Arz0zwxh
        {
            get;
            set;
        }

        /// <summary>
        /// Arz0czdm 操作代码 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Arz0czdm
        {
            get;
            set;
        }

        /// <summary>
        /// Arz0czsj 操作时间
        /// </summary>
        public virtual DateTime? Arz0czsj
        {
            get;
            set;
        }

        /// <summary>
        /// Arz0lx00 类型
        /// </summary>
        public virtual string Arz0lx00
        {
            get;
            set;
        }

        /// <summary>
        /// Arz0cxcz 撤销操作的人员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Arz0cxcz
        {
            get;
            set;
        }

        /// <summary>
        /// Arz0cxsj 撤销时间
        /// </summary>
        public virtual DateTime? Arz0cxsj
        {
            get;
            set;
        }

    }
}
