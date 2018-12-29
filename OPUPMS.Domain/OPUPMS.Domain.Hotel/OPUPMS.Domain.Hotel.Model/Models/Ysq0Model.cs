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
    /// 预授权 实体类
    /// </summary>
    [Table("Ysq0")]
    public class Ysq0Model : Entity<int>
    {
        static Ysq0Model()
        {
            OrmConfiguration.GetDefaultEntityMapping<Ysq0Model>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Ysq0xh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Ysq0xh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Ysq0xh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Ysq0zh00 帐号 关联Krzl.Krzlzh00, Krzw.Krzwzh00
        /// </summary>
        public virtual int Ysq0zh00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0kh00 卡号
        /// </summary>
        public virtual string Ysq0kh00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0je00 金额
        /// </summary>
        public virtual decimal Ysq0je00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0yxq0 有效期
        /// </summary>
        public virtual DateTime? Ysq0yxq0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0bz00 备注
        /// </summary>
        public virtual string Ysq0bz00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0sqh0 授权号
        /// </summary>
        public virtual string Ysq0sqh0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0zt00 状态 Y-已授权，X-未授权
        /// </summary>
        public virtual string Ysq0zt00
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0czdm 操作员 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Ysq0czdm
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0czsj 操作时间
        /// </summary>
        public virtual DateTime? Ysq0czsj
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0cxdm 撤销操作的人员 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Ysq0cxdm
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0cxsj 撤销时间
        /// </summary>
        public virtual DateTime? Ysq0cxsj
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0cxm0 查询码
        /// </summary>
        public virtual string Ysq0cxm0
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0cxm1
        /// </summary>
        public virtual string Ysq0cxm1
        {
            get;
            set;
        }

        /// <summary>
        /// Ysq0msg0 打印信息
        /// </summary>
        public virtual string Ysq0msg0
        {
            get;
            set;
        }
    }
}
