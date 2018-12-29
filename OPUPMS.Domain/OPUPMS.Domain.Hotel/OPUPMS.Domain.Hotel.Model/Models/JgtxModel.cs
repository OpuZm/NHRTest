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
    /// 价格体系 实体类
    /// </summary>
    [Table("Jgtx")]
    public class JgtxModel : Entity<int>
    {
        static JgtxModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<JgtxModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Jgtxxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Jgtxxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Jgtxxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Jgtxsslb 所属类别 关联系统代码 JL
        /// </summary>
        public virtual string Jgtxsslb
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxmc00 名称
        /// </summary>
        public virtual string Jgtxmc00
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxksrq 开始日期
        /// </summary>
        public virtual string Jgtxksrq
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxjsrq 结束日期
        /// </summary>
        public virtual string Jgtxjsrq
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxxq00 星期 星期大写，逗号间隔
        /// </summary>
        public virtual string Jgtxxq00
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxtsrq 特殊日期 如：MM-DD
        /// </summary>
        public virtual string Jgtxtsrq
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxrs00 人数
        /// </summary>
        public virtual int Jgtxrs00
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxyxjb 优先级别
        /// </summary>
        public virtual int Jgtxyxjb
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxtext 内容 如：房型:房价/房型：房价
        /// </summary>
        public virtual string Jgtxtext
        {
            get;
            set;
        }

        /// <summary>
        /// Jgtxjcjg 加床价格
        /// </summary>
        public virtual string Jgtxjcjg
        {
            get;
            set;
        }
    }
}
