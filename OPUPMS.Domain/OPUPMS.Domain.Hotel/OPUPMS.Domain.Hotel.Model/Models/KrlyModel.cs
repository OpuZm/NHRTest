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
    /// 客人留言 实体类
    /// </summary>
    [Table("Krly")]
    public class KrlyModel : Entity<int>
    {
        static KrlyModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrlyModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krlyxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Krlyxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krlyxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krlyzh00 帐号 关联Krzl.Krzlzh00
        /// </summary>
        public virtual int? Krlyzh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlylx00 类型 关联系统代码 LYLX
        /// </summary>
        public virtual string Krlylx00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyjm00
        /// </summary>
        public virtual string Krlyjm00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyzt00 状态
        /// X-未读，D-取消
        /// </summary>
        public virtual string Krlyzt00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlybt00 主题
        /// </summary>
        public virtual string Krlybt00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyczdm 操作员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krlyczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyczsj 操作时间
        /// </summary>
        public virtual DateTime? Krlyczsj
        {
            get;
            set;
        }

        /// <summary>
        /// Krlytext 内容
        /// </summary>
        public virtual string Krlytext
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyqxsj 取消时间
        /// </summary>
        public virtual DateTime? Krlyqxsj
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyqxcz 取消操作人员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krlyqxcz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlysxrq
        /// </summary>
        public virtual DateTime? Krlysxrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyqxbz 备注
        /// 里面czdm或者系统代码qxzb的数据用逗号，隔开
        /// </summary>
        public virtual string Krlyqxbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyfjsl
        /// </summary>
        public virtual int? Krlyfjsl
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyydh0 预定号
        /// </summary>
        public virtual int? Krlyydh0
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyyd00 已读  关联Czdm.Czdmdm00
        /// 操作已读，填入czdmdm00,逗号隔断
        /// </summary>
        public virtual string Krlyyd00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyzybz
        /// </summary>
        public virtual string Krlyzybz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyexxh
        /// </summary>
        public virtual int? Krlyexxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krlyksrq 开始日期
        /// </summary>
        public virtual DateTime? Krlyksrq
        {
            get;
            set;
        }
    }
}
