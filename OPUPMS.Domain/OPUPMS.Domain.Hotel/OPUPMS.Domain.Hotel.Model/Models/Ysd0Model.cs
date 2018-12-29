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
    /// 应收单 实体类
    /// </summary>
    [Table("Ysd0")]
    public class Ysd0Model : Entity<int>
    {
        static Ysd0Model()
        {
            OrmConfiguration.GetDefaultEntityMapping<Ysd0Model>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Ysd0xh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Ysd0xh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Ysd0xh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Ysd0th00 团号
        /// </summary>
        public virtual string Ysd0th00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0tm00 团名
        /// </summary>
        public virtual string Ysd0tm00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0lxdm 协议客户 关联Lxdm.lxdmdm00
        /// </summary>
        public virtual string Ysd0lxdm
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0rzrq 入住日期
        /// </summary>
        public virtual DateTime Ysd0rzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0ldrq 离店日期
        /// </summary>
        public virtual DateTime Ysd0ldrq
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0rs00 人数
        /// </summary>
        public virtual decimal Ysd0rs00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0fph0 发票号
        /// </summary>
        public virtual string Ysd0fph0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0rq00 日期
        /// </summary>
        public virtual DateTime Ysd0rq00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0ysje 应收金额
        /// </summary>
        public virtual decimal Ysd0ysje
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0yfje 已付金额
        /// </summary>
        public virtual decimal Ysd0yfje
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0qtje 其他金额
        /// </summary>
        public virtual decimal Ysd0qtje
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0czdm 操作代码 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Ysd0czdm
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0bzs0 标注
        /// </summary>
        public virtual string Ysd0bzs0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0bz00 备注
        /// </summary>
        public virtual string Ysd0bz00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0krzh 客人账号 关联Krzl.Kzlzh00
        /// </summary>
        public virtual int? Ysd0krzh
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0ye00 余额
        /// </summary>
        public virtual decimal? Ysd0ye00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0jzrq 记账日期
        /// </summary>
        public virtual DateTime? Ysd0jzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Ysd0jzcz 记账操作
        /// </summary>
        public virtual string Ysd0jzcz
        {
            get;
            set;
        }
    }
}
