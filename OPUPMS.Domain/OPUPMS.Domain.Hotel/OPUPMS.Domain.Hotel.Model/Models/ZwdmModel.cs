using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Model
{
    /// <summary>
    /// 账务代码 实体类
    /// </summary>
    [Table("Zwdm")]
    public class ZwdmModel : Entity<string>
    {
        //static ZwdmModel()
        //{
        //    OrmConfiguration.GetDefaultEntityMapping<ZwdmModel>()
        //        .SetProperty(entity => entity.Id, 
        //            prop =>
        //            {
        //                prop.SetDatabaseColumnName("Zwdmdm00");
        //            });
        //}

        /// <summary>
        /// Zwdmdm00 账务代码 主键列
        /// </summary>
        [Key]
        [Column("Zwdmdm00")]
        public string Id { get; set; }

        ///// <summary>
        ///// Zwdmdm00 账务代码 主键列
        ///// </summary>
        //public virtual string Zwdmdm00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Zwdmckhm 参考号码 关联系统代码 ck
        /// </summary>
        public virtual string Zwdmckhm
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmmc00 账务名称
        /// </summary>
        public virtual string Zwdmmc00
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmjdxz 结单性质  C-付款项，D-消费项
        /// </summary>
        public virtual string Zwdmjdxz
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmdw00 单位
        /// </summary>
        public virtual string Zwdmdw00
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmdj00 单价
        /// </summary>
        public virtual decimal? Zwdmdj00
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmbz00 备注  FW-房务中心，SW-商务中心
        /// </summary>
        public virtual string Zwdmbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmbzs0 标志  Y-启用,X-可删除，N-禁用
        /// </summary>
        public virtual string Zwdmbzs0
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmlb00 类别  Z-总类，X-小类
        /// </summary>
        public virtual string Zwdmlb00
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmattr 属性
        /// </summary>
        public virtual int? Zwdmattr
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmfzh0 分账号
        /// </summary>
        public virtual int? Zwdmfzh0
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmzwlb 账务类别  A-可冲账，B-不可冲账
        /// </summary>
        public virtual string Zwdmzwlb
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmsslb 所属类别  填写总类代码zwdmdm00
        /// </summary>
        public virtual string Zwdmsslb
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmysdm 应收代码 关联系统代码 ys
        /// </summary>
        public virtual string Zwdmysdm
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmywmc 英文名称
        /// </summary>
        public virtual string Zwdmywmc
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmfpfl
        /// </summary>
        public virtual string Zwdmfpfl
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmckdm 仓库代码 与HT仓库对应的仓库hwdm
        /// </summary>
        public virtual string Zwdmckdm
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmkcsl 仓库库存数量
        /// </summary>
        public virtual decimal? Zwdmkcsl
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmckbl 仓库比率
        /// </summary>
        public virtual decimal? Zwdmckbl
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmdhdm
        /// </summary>
        public virtual string Zwdmdhdm
        {
            get;
            set;
        }

        /// <summary>
        /// Zwdmdhdy
        /// </summary>
        public virtual string Zwdmdhdy
        {
            get;
            set;
        }
    }    
}
