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
    /// 应收明细 实体类
    /// </summary>
    [Table("Ysmx")]
    public class YsmxModel : Entity<int>
    {
        static YsmxModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<YsmxModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Ysmxxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Ysmxxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Ysmxxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Ysmxysdm 应收代码，关联系统应收代码 YS
        /// </summary>
        public virtual string Ysmxysdm
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxsl00 数量
        /// </summary>
        public virtual decimal Ysmxsl00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxdj00 单价
        /// </summary>
        public virtual decimal Ysmxdj00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxts00 天数
        /// </summary>
        public virtual decimal Ysmxts00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxje00 金额
        /// </summary>
        public virtual decimal Ysmxje00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxrq00 日期
        /// </summary>
        public virtual DateTime Ysmxrq00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxfph0 发票号
        /// </summary>
        public virtual string Ysmxfph0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxbz00 备注
        /// </summary>
        public virtual string Ysmxbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxczdm 操作员，关联 Czdm.Czdmdm00
        /// </summary>
        public virtual string Ysmxczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxczsj 操作时间
        /// </summary>
        public virtual DateTime Ysmxczsj
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxbz01 备注01
        /// </summary>
        public virtual string Ysmxbz01
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxzdxh 主单序号 关联 Ysd0.Ysd0xh00
        /// </summary>
        public virtual int? Ysmxzdxh
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxjdxz 结单性质: C-付款 D-消费
        /// </summary>
        public virtual string Ysmxjdxz
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxfzh0 房账号
        /// </summary>
        public virtual int? Ysmxfzh0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxlx00 类型 默认值A，A-客房 B-餐饮
        /// </summary>
        public virtual string Ysmxlx00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxysyy 应收原因
        /// </summary>
        public virtual string Ysmxysyy
        {
            get;
            set;
        }

        /// <summary>
        /// Ysmxdh00 电话
        /// </summary>
        public virtual string Ysmxdh00
        {
            get;
            set;
        }
    }
}
