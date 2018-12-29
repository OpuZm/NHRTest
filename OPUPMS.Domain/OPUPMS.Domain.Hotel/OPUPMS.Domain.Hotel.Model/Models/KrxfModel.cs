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
    /// 客人消费 实体类
    /// </summary>
    [Table("Krxf")]
    public class KrxfModel : Entity<int>
    {
        static KrxfModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrxfModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krxfxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Krxfxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krxfxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krxfkhid 卡号ID 关联Krls.Krlsxh00
        /// </summary>
        public virtual int? Krxfkhid
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfzwrq 账务日期
        /// </summary>
        public virtual DateTime? Krxfzwrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krxflb00 类别  C-充值，Y-预授权，A-消费，S-转账
        /// </summary>
        public virtual string Krxflb00
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfxfd0 消费点  A-全部，Z-总台，1 - 一号餐厅，2 - 二号餐厅
        /// </summary>
        public virtual string Krxfxfd0
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfje00 金额
        /// </summary>
        public virtual decimal? Krxfje00
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfrq00 操作时间
        /// </summary>
        public virtual DateTime? Krxfrq00
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfczdm 操作员 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krxfczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfbz00 备注
        /// </summary>
        public virtual string Krxfbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfye00 余额
        /// </summary>
        public virtual decimal? Krxfye00
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfzh00 分帐号
        /// </summary>
        public virtual int? Krxfzh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfattr 属性
        /// </summary>
        public virtual int? Krxfattr
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfje01 次数
        /// </summary>
        public virtual decimal? Krxfje01
        {
            get;
            set;
        }

        /// <summary>
        /// Krxffkfs 付款方式 关联系统代码: czfk
        /// </summary>
        public virtual string Krxffkfs
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfzkxh 主卡序号 主卡的krlsvpkh
        /// </summary>
        public virtual int? Krxfzkxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krxfzsbz 赠送标志
        /// </summary>
        public virtual string Krxfzsbz
        {
            get;
            set;
        }

        /// <summary>
        /// KrxfGPID 集团专用
        /// </summary>
        public virtual string KrxfGPID
        {
            get;
            set;
        }

        /// <summary>
        /// KrxfGUID 集团专用
        /// </summary>
        public virtual Guid KrxfGUID
        {
            get;
            set;
        }
    }
}
