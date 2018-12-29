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
    /// 客人外事 实体类
    /// </summary>
    [Table("Krws")]
    public class KrwsModel : Entity<int>
    {
        static KrwsModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrwsModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krwsxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Krwsxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krwsxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krwslxxh
        /// </summary>
        public virtual int Krwslxxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krwszh00 帐号  关联Krzl.Krzlzh00
        /// </summary>
        public virtual int? Krwszh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsrjsy 入境事由  关联系统代码 SY
        /// </summary>
        public virtual string Krwsrjsy
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsrjrq 入境日期
        /// </summary>
        public virtual DateTime? Krwsrjrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsrjka 入境口岸  关联系统代码 KA
        /// </summary>
        public virtual string Krwsrjka
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsqzlb 签证类别  关联系统代码 QZ
        /// </summary>
        public virtual string Krwsqzlb
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsqzzh 签证帐号
        /// </summary>
        public virtual string Krwsqzzh
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsqzyx 签证有效期
        /// </summary>
        public virtual DateTime? Krwsqzyx
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsqfdw 签发单位  关联系统代码 CS
        /// </summary>
        public virtual string Krwsqfdw
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsbz00 备注
        /// </summary>
        public virtual string Krwsbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krwsdqcz
        /// </summary>
        public virtual string Krwsdqcz
        {
            get;
            set;
        }
    }    
}
