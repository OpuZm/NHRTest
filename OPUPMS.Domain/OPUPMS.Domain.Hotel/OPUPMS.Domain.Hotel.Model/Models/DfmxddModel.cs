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
    /// 预定明细等待 实体类
    /// </summary>
    [Table("Dfmxdd")]
    public class DfmxddModel : Entity<int>
    {
        static DfmxddModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<DfmxddModel>()
                .SetProperty(entity => entity.Id,
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Dfmxddxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Dfmxddxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Dfmxddxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Dfmxdfzlxh 等待主单序号
        /// </summary>
        public virtual int Dfmxdfzlxh
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfjlb 房价类别（价类）
        /// 关联系统代码 JL
        /// </summary>
        public virtual string Dfmxfjlb
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxrzrq 入住日期
        /// </summary>
        public virtual DateTime? Dfmxrzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxldrq 离店日期
        /// </summary>
        public virtual DateTime? Dfmxldrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfl00 房型
        /// 关联系统代码 JL
        /// </summary>
        public virtual string Dfmxfl00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxrs00 人数
        /// </summary>
        public virtual decimal? Dfmxrs00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfj00 房价
        /// </summary>
        public virtual decimal? Dfmxfj00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfs00 房数
        /// </summary>
        public virtual decimal? Dfmxfs00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfjsx 属性
        /// 2-资料保密，4-钟点房，8-VIP，16-房价保密，32-成员自付，64-不可转账
        /// </summary>
        public virtual int? Dfmxfjsx
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxpkge 包价
        /// </summary>
        public virtual string Dfmxpkge
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxyjfs 预计房数
        /// </summary>
        public virtual int? Dfmxyjfs
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxczdm
        /// 操作员 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Dfmxczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxczsj 操作时间
        /// </summary>
        public virtual DateTime? Dfmxczsj
        {
            get;
            set;
        }
    }
}
