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
    /// 客人住店 实体类
    /// </summary>
    [Table("Krzd")]
    public class KrzdModel : Entity<int>
    {
        static KrzdModel()
        {
            //组合主键无法自动映射
            //OrmConfiguration.GetDefaultEntityMapping<FhdmModel>().SetProperty(entity => entity.Id, prop => prop.SetDatabaseColumnName("Krzdzh00"));
        }


        /// <summary>
        /// Krzdrq00 日期 组合主键列
        /// </summary>
        public virtual DateTime Krzdrq00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdzh00 帐号 组合主键列
        /// </summary>
        public virtual int Krzdzh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdjzzh 结账帐号
        /// </summary>
        public virtual int? Krzdjzzh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdydh0 预定号
        /// </summary>
        public virtual int? Krzdydh0
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdzt00 状态
        /// </summary>
        public virtual string Krzdzt00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfh00 房号
        /// </summary>
        public virtual string Krzdfh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdzwxm 中文姓名
        /// </summary>
        public virtual string Krzdzwxm
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdywxm 英文姓名
        /// </summary>
        public virtual string Krzdywxm
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdxb00 性别
        /// </summary>
        public virtual string Krzdxb00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdrzrq 入住日期
        /// </summary>
        public virtual DateTime? Krzdrzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdldrq 离店日期
        /// </summary>
        public virtual DateTime? Krzdldrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdlxdm 类型代码
        /// </summary>
        public virtual string Krzdlxdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdvip0 VIP
        /// </summary>
        public virtual string Krzdvip0
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfjlb 房价类别
        /// </summary>
        public virtual string Krzdfjlb
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfjl2
        /// </summary>
        public virtual string Krzdfjl2
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdkrbz 客人备注
        /// </summary>
        public virtual string Krzdkrbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdzwbz 账务备注
        /// </summary>
        public virtual string Krzdzwbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfz00 房价
        /// </summary>
        public virtual decimal Krzdfz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdxfze 消费总额
        /// </summary>
        public virtual decimal Krzdxfze
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdsrye 上日余额
        /// </summary>
        public virtual decimal Krzdsrye
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdye00 余额
        /// </summary>
        public virtual decimal Krzdye00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfkje 付款金额
        /// </summary>
        public virtual decimal Krzdfkje
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfkzz
        /// </summary>
        public virtual decimal Krzdfkzz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdxfje 消费金额
        /// </summary>
        public virtual decimal Krzdxfje
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdxfzz
        /// </summary>
        public virtual decimal Krzdxfzz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdkrlb 客人类别
        /// </summary>
        public virtual string Krzdkrlb
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdkrlx 客人类型
        /// </summary>
        public virtual string Krzdkrlx
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdth00 团号
        /// </summary>
        public virtual string Krzdth00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdtzxh 同住序号
        /// </summary>
        public virtual int? Krzdtzxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdzhlx 帐号类型
        /// </summary>
        public virtual int? Krzdzhlx
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdywy0 业务员
        /// </summary>
        public virtual string Krzdywy0
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfjlx 房价类型
        /// </summary>
        public virtual string Krzdfjlx
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdkh00 卡号
        /// </summary>
        public virtual string Krzdkh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfjjg 房价结构
        /// </summary>
        public virtual string Krzdfjjg
        {
            get;
            set;
        }

        /// <summary>
        /// KrzdDfmx 订房明细
        /// </summary>
        public virtual int? KrzdDfmx
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdfjsx 房价属性
        /// </summary>
        public virtual int? Krzdfjsx
        {
            get;
            set;
        }

        /// <summary>
        /// Krzdydlb 预定类别
        /// </summary>
        public virtual string Krzdydlb
        {
            get;
            set;
        }
    }
}
