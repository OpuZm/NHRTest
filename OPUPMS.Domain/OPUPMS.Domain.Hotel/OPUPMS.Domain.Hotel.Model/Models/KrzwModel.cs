using Dapper.FastCrud;
using OPUPMS.Domain.Hotel.Model.Dtos;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Model
{
    /// <summary>
    /// 客人账务 实体类
    /// </summary>
    [Table("Krzw")]
    public class KrzwModel : Entity<int>
    {
        static KrzwModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrzwModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krzwxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });

        }

        ///// <summary>
        ///// Krzwxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krzwxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krzwfzh0 分账号 关联系统代码 fz  
        /// </summary>
        public virtual int Krzwfzh0
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwzh00 账号 关联Krzl.Krzlzh00
        /// </summary>
        public virtual int Krzwzh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwgrzh 关联账号
        /// </summary>
        public virtual int Krzwgrzh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwzwrq 账务日期
        /// </summary>
        public virtual DateTime Krzwzwrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwzwdm 账务代码 关联Zwdm.Zwdmdm00
        /// </summary>
        public virtual string Krzwzwdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwckhm 参考号码（票据号）
        /// </summary>
        public virtual string Krzwckhm
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwxfje 消费金额
        /// </summary>
        public virtual decimal Krzwxfje
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwyfje 应付金额
        /// </summary>
        public virtual decimal Krzwyfje
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwhsje 核算金额
        /// </summary>
        public virtual decimal Krzwhsje
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwfkhb 付款货币 关联系统代码 hb
        /// </summary>
        public virtual string Krzwfkhb
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwfkfs 付款方式 关联系统代码 fk
        /// </summary>
        public virtual string Krzwfkfs
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwjdxz 结单性质  C-表示付款，D-表示消费
        /// </summary>
        public virtual string Krzwjdxz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwczdm 操作员  关联Czdm.Czdmdm00
        /// </summary>
        public virtual string Krzwczdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwczsj 操作时间
        /// </summary>
        public virtual DateTime Krzwczsj
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwzzbz 转账备注
        /// </summary>
        public virtual string Krzwzzbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwlx00 账务类型       
        /// A:正常（若是迷你吧等，则与krzwztbz中的H对应）;
        /// X:大项的冲账（迷你吧，商务中心等有汇总的小项的冲账是汇总不改变的，还是A）;
        /// Y:转出账（原账号的原账和新增的账）;
        /// Z:转入账;
        /// H:迷你吧，商务中心等有汇总的大项（与krzwztbz的A对应）;
        /// </summary>
        public virtual string Krzwlx00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwztbz 状态标志
        /// </summary>
        public virtual string Krzwztbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwxh01 序号01
        /// </summary>
        public virtual int Krzwxh01
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwbz00 备注
        /// </summary>
        public virtual string Krzwbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwxh02 序号02
        /// </summary>
        public virtual int? Krzwxh02
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwzwck 账务仓库
        /// </summary>
        public virtual string Krzwzwck
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwczyy 操作原因
        /// </summary>
        public virtual string Krzwczyy
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwbc00 班次
        /// </summary>
        public virtual string Krzwbc00
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwxh03 序号03
        /// </summary>
        public virtual int? Krzwxh03
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwysxh 应收序号
        /// </summary>
        public virtual int? Krzwysxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwxh04 序号04
        /// </summary>
        public virtual int? Krzwxh04
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwxh05 序号05
        /// </summary>
        public virtual int? Krzwxh05
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwfpxh 发票序号
        /// </summary>
        public virtual int? Krzwfpxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwczcz
        /// </summary>
        public virtual string Krzwczcz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwzyxh
        /// </summary>
        public virtual int? Krzwzyxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwyzrq
        /// </summary>
        public virtual DateTime? Krzwyzrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwrzzq
        /// </summary>
        public virtual DateTime? Krzwrzzq
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwpody
        /// </summary>
        public virtual string Krzwpody
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwjzbz
        /// </summary>
        public virtual string Krzwjzbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwjzxh
        /// </summary>
        public virtual int? Krzwjzxh
        {
            get;
            set;
        }

        /// <summary>
        /// Krzwjzpj
        /// </summary>
        public virtual string Krzwjzpj
        {
            get;
            set;
        }
    }
}
