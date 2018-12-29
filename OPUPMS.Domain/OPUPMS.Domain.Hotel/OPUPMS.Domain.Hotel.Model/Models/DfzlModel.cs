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
    /// 订房资料 实体类
    /// </summary>
    [Table("Dfzl")]
    public class DfzlModel : Entity<int>
    {
        static DfzlModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<DfzlModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Dfzlydh0");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Dfzlydh0 序号 主键 标识列
        ///// </summary>
        //public virtual int Dfzlydh0
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Dfzlzt00 状态
        /// 关联系统代码 ZT
        /// </summary>
        public virtual string Dfzlzt00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlth00 团号
        /// </summary>
        public virtual string Dfzlth00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzltm00 团名
        /// </summary>
        public virtual string Dfzltm00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzllb00 预定类别
        /// 关联系统代码 YD
        /// </summary>
        public virtual string Dfzllb00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlrs00 人数
        /// </summary>
        public virtual decimal Dfzlrs00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlfs00 房数
        /// </summary>
        public virtual decimal Dfzlfs00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlydrq 预定日期
        /// </summary>
        public virtual DateTime? Dfzlydrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlsxrq 失效日期
        /// </summary>
        public virtual DateTime? Dfzlsxrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlrzrq 入住日期
        /// </summary>
        public virtual DateTime? Dfzlrzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlldrq 离店日期
        /// </summary>
        public virtual DateTime? Dfzlldrq
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzllxdm 类型代码
        /// </summary>
        public virtual string Dfzllxdm
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzllxr0 联系人
        /// </summary>
        public virtual string Dfzllxr0
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzllxbz 联系备注，联系方式
        /// </summary>
        public virtual string Dfzllxbz
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzllxyj
        /// </summary>
        public virtual decimal Dfzllxyj
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlgj00 国籍
        /// 关联系统代码 GJ
        /// </summary>
        public virtual string Dfzlgj00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlkrlb 客人类别
        /// 关联系统代码 KL
        /// </summary>
        public virtual string Dfzlkrlb
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlfjlb 房价类别（价类）
        /// 关联系统代码 JL
        /// </summary>
        public virtual string Dfzlfjlb
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzljzfs 结账方式
        /// 关联系统代码 FK
        /// </summary>
        public virtual string Dfzljzfs
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzljzhb 结账货币
        /// 关联系统代码 HB
        /// </summary>
        public virtual string Dfzljzhb
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlfddm 分单代码
        /// 关联系统代码 FD
        /// </summary>
        public virtual string Dfzlfddm
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlkrbz 客人备注
        /// </summary>
        public virtual string Dfzlkrbz
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlzwbz 账务备注
        /// </summary>
        public virtual string Dfzlzwbz
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlywy0 业务员 关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Dfzlywy0
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlrzsj 入住时间
        /// </summary>
        public virtual DateTime? Dfzlrzsj
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlldsj 离店时间
        /// </summary>
        public virtual DateTime? Dfzlldsj
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlfjsx 房价属性（含早）
        /// 关联系统代码 JL2
        /// </summary>
        public virtual int? Dfzlfjsx
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlzcsf
        /// </summary>
        public virtual string Dfzlzcsf
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzltext
        /// </summary>
        public virtual string Dfzltext
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlhyxx
        /// </summary>
        public virtual string Dfzlhyxx
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzldfbz 订房备注/接待
        /// </summary>
        public virtual string Dfzldfbz
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzldd22
        /// </summary>
        public virtual string Dfzldd22
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlpkge 包价
        /// </summary>
        public virtual string Dfzlpkge
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlscdm 市场代码
        /// 关联系统代码 SC
        /// </summary>
        public virtual string Dfzlscdm
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzljdgj 接待管家/关联
        /// </summary>
        public virtual string Dfzljdgj
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlgjdh 管家电话/关联备注
        /// </summary>
        public virtual string Dfzlgjdh
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlfjjg 房价结构
        /// 关联系统代码 FJJG
        /// </summary>
        public virtual string Dfzlfjjg
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzltf00 特服
        /// 关联系统代码 POTF
        /// </summary>
        public virtual string Dfzltf00
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlvip0 VIP 类别
        /// 关联系统代码 KRVP
        /// </summary>
        public virtual string Dfzlvip0
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlgzxe 挂账限额
        /// </summary>
        public virtual decimal? Dfzlgzxe
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlattr 属性
        /// 2-资料保密，4-钟点房，8-VIP，16-房价保密，32-成员自付，64-不可转账
        /// </summary>
        public virtual int? Dfzlattr
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlzdzz
        /// </summary>
        public virtual string Dfzlzdzz
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlyjdm 佣金代码
        /// 关联系统代码 YJDM
        /// </summary>
        public virtual string Dfzlyjdm
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlyjrs 佣金人数
        /// </summary>
        public virtual int? Dfzlyjrs
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlhth0 合同号
        /// </summary>
        public virtual string Dfzlhth0
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlydbz 特殊备注
        /// </summary>
        public virtual string Dfzlydbz
        {
            get;
            set;
        }

        /// <summary>
        /// Dfzlshbz 审核备注
        /// </summary>
        public virtual string Dfzlshbz
        {
            get;
            set;
        }
    }
}
