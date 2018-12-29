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
    /// 客人发卡 实体类
    /// </summary>
    [Table("Krfk")]
    public class KrfkModel : Entity<int>
    {
        static KrfkModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrfkModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krfkxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Krfkxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krfkxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krfkkh00 卡号房号 关联Fhdm.Fhdmdm00
        /// </summary>
        public virtual string Krfkkh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkbzs0 标志
        /// </summary>
        public virtual string Krfkbzs0
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkrq00 日期
        /// </summary>
        public virtual DateTime? Krfkrq00
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkczdm 操作员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krfkczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkksrq 开始日期
        /// </summary>
        public virtual DateTime? Krfkksrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krfksxrq 失效日期
        /// </summary>
        public virtual DateTime? Krfksxrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krfksxcz 失效操作人员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krfksxcz
        {
            get;
            set;
        }
        
        /// <summary>
        /// Krfkkhid 卡号ID
        /// </summary>
        public virtual long? Krfkkhid
        {
            get;
            set;
        }

        /// <summary>
        /// Krfklx00 类型
        /// 2-资料保密，4-钟点房，8-VIP，16-房价保密，32-成员自付，64-不可转账
        /// </summary>
        public virtual int Krfklx00
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkfkxh
        /// </summary>
        public virtual int? Krfkfkxh
        {
            get;
            set;
        }

        /// <summary>
        /// KrfkmsSi
        /// </summary>
        public virtual int? KrfkmsSi
        {
            get;
            set;
        }

        /// <summary>
        /// KrfkmsLo
        /// </summary>
        public virtual int? KrfkmsLo
        {
            get;
            set;
        }

        /// <summary>
        /// KrfkmsHi
        /// </summary>
        public virtual int? KrfkmsHi
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkxm00 姓名
        /// </summary>
        public virtual string Krfkxm00
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkzh00 账号
        /// </summary>
        public virtual string Krfkzh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkbz00
        /// </summary>
        public virtual string Krfkbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkjzrq 记账日期
        /// </summary>
        public virtual DateTime? Krfkjzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkmsbz
        /// </summary>
        public virtual string Krfkmsbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krfkkrzh 客人帐号  关联Krzl.Krzlzh00
        /// </summary>
        public virtual int? Krfkkrzh
        {
            get;
            set;
        }
    }
}
