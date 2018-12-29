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
    /// 订房明细 实体类
    /// </summary>
    [Table("Dfmx")]
    public class DfmxModel : Entity<int>
    {
        static DfmxModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<DfmxModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Dfmxxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Dfmxxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Dfmxxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Dfmxydh0
        /// </summary>
        public virtual int Dfmxydh0
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxzt00 状态
        /// </summary>
        public virtual string Dfmxzt00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxydrq 预定日期
        /// </summary>
        public virtual DateTime? Dfmxydrq
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
        /// Dfmxqxrq 取消日期
        /// </summary>
        public virtual DateTime? Dfmxqxrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxsxrq 失效日期
        /// </summary>
        public virtual DateTime? Dfmxsxrq
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
        /// Dfmxrs00 人数
        /// </summary>
        public virtual decimal? Dfmxrs00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfl00 房型
        /// </summary>
        public virtual string Dfmxfl00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfj00 房价
        /// </summary>
        public virtual decimal Dfmxfj00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxfs00 房数
        /// </summary>
        public virtual decimal Dfmxfs00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxqr00 确认房数
        /// </summary>
        public virtual decimal Dfmxqr00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxrz00 入住房数
        /// </summary>
        public virtual decimal Dfmxrz00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfmxqx00 取消房数
        /// </summary>
        public virtual decimal Dfmxqx00
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
        public virtual DateTime Dfmxczsj
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
        /// Dfmxsjxh 升级序号
        /// </summary>
        public virtual int? Dfmxsjxh
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
        /// Dfmxyfl0 原房类
        /// </summary>
        public virtual string Dfmxyfl0
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
    }
}
