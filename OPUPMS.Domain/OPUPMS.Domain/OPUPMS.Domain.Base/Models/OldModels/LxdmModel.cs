using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Base.Models
{
    /// <summary>
    /// 类型代码 实体类
    /// </summary>
    [Table("Lxdm")]
    public class LxdmModel : Entity<int>
    {
        //static LxdmModel()
        //{
        //    OrmConfiguration.GetDefaultEntityMapping<LxdmModel>()
        //        .SetProperty(entity => entity.Id,
        //            prop =>
        //            {
        //                prop.SetDatabaseColumnName("Lxdmdm00");
        //            });
        //}

        /// <summary>
        /// lxdmdm00 代码 主键列
        /// </summary>
        public virtual string Lxdmdm00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmmc00 名称
        /// </summary>
        public virtual string Lxdmmc00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmdz00 地址
        /// </summary>
        public virtual string Lxdmdz00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmzip0 邮编
        /// </summary>
        public virtual string Lxdmzip0
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmlxr0 联系人
        /// </summary>
        public virtual string Lxdmlxr0
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmfzr0 负责人
        /// </summary>
        public virtual string Lxdmfzr0
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmdh00 电话
        /// </summary>
        public virtual string Lxdmdh00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmcz00
        /// </summary>
        public virtual string Lxdmcz00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmcall
        /// </summary>
        public virtual string Lxdmcall
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmhp00 喜欢
        /// </summary>
        public virtual string Lxdmhp00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmmail Email
        /// </summary>
        public virtual string Lxdmmail
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmss00 地区
        /// </summary>
        public virtual string Lxdmss00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmlxyj 佣金
        /// </summary>
        public virtual decimal Lxdmlxyj
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxfze 消费总额
        /// </summary>
        public virtual decimal Lxdmxfze
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmye00 余额
        /// </summary>
        public virtual decimal Lxdmye00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmgzxe 挂账限额
        /// </summary>
        public virtual decimal Lxdmgzxe
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmgzbz 挂账标志
        /// </summary>
        public virtual string Lxdmgzbz
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmmdxe
        /// </summary>
        public virtual decimal? Lxdmmdxe
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxz00 性质
        /// </summary>
        public virtual string Lxdmxz00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmqdrq 签单日期
        /// </summary>
        public virtual DateTime? Lxdmqdrq
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmyxrq 有效日期
        /// </summary>
        public virtual DateTime? Lxdmyxrq
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmfax0 FAX
        /// </summary>
        public virtual string Lxdmfax0
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxybz 协议备注
        /// </summary>
        public virtual string Lxdmxybz
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmywy0 业务员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Lxdmywy0
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmpyjm 拼音简码
        /// </summary>
        public virtual string Lxdmpyjm
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmwbjm 五笔简码
        /// </summary>
        public virtual string Lxdmwbjm
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxyfa
        /// </summary>
        public virtual string Lxdmxyfa
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmysye
        /// </summary>
        public virtual decimal? Lxdmysye
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmgzye 挂账余额
        /// </summary>
        public virtual decimal? Lxdmgzye
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmgzze 挂账总额
        /// </summary>
        public virtual decimal? Lxdmgzze
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmgzsl
        /// </summary>
        public virtual int? Lxdmgzsl
        {
            get;
            set;
        }
        /// <summary>
        /// Lxdmgzsx
        /// </summary>
        public virtual string Lxdmgzsx
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxp00 相片
        /// </summary>
        public virtual byte[] Lxdmxp00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmqm00 签名
        /// </summary>
        public virtual byte[] Lxdmqm00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmvpxf
        /// </summary>
        public virtual decimal? Lxdmvpxf
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmvpjf
        /// </summary>
        public virtual decimal? Lxdmvpjf
        {
            get;
            set;
        }
        
        /// <summary>
        /// Lxdmggcz
        /// </summary>
        public virtual string Lxdmggcz
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmggsj
        /// </summary>
        public virtual DateTime? Lxdmggsj
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxy00 协议  如：房型、房价
        /// </summary>
        public virtual string Lxdmxy00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmkfxh
        /// </summary>
        public virtual string Lxdmkfxh
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmcyxh
        /// </summary>
        public virtual string Lxdmcyxh
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmyjdm 佣金代码  关联系统代码 YJDM
        /// </summary>
        public virtual string Lxdmyjdm
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmxyjl 协议价类  关联系统代码 JL
        /// </summary>
        public virtual string Lxdmxyjl
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmjtqx 集团权限
        /// </summary>
        public virtual string Lxdmjtqx
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmglkl 关联客类  关联系统代码 KL
        /// </summary>
        public virtual string Lxdmglkl
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmkzjl 控制价类  关联系统代码 JL
        /// </summary>
        public virtual string Lxdmkzjl
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmupdatemark
        /// </summary>
        public virtual string Lxdmupdatemark
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmhy00 行业
        /// </summary>
        public virtual string Lxdmhy00
        {
            get;
            set;
        }

        /// <summary>
        /// Lxdmbh00 包含  关联系统代码 JL2
        /// </summary>
        public virtual string Lxdmbh00
        {
            get;
            set;
        }
    }
}
