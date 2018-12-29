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
    /// 客人历史 实体类
    /// </summary>
    [Table("Krls")]
    public class KrlsModel : Entity<int>
    {
        static KrlsModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrlsModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krlsxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Krlsxh00 序号 主键 标识列
        ///// </summary>
        //public virtual int Krlsxh00
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Krlszwxm 中文姓名
        /// </summary>
        public virtual string Krlszwxm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsywxm 英文姓名
        /// </summary>
        public virtual string Krlsywxm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsxb00 性别
        /// </summary>
        public virtual string Krlsxb00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlskrlx 客人类型
        /// </summary>
        public virtual string Krlskrlx
        {
            get;
            set;
        }

        /// <summary>
        /// Krlscsrq 出生日期
        /// </summary>
        public virtual DateTime? Krlscsrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszjlb 证件类别
        /// </summary>
        public virtual string Krlszjlb
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszjhm 证件号码
        /// </summary>
        public virtual string Krlszjhm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsvip0 VIP
        /// </summary>
        public virtual string Krlsvip0
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsvpkh 会员卡号
        /// </summary>
        public virtual string Krlsvpkh
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsdw00 单位
        /// </summary>
        public virtual string Krlsdw00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsdz00 地址
        /// </summary>
        public virtual string Krlsdz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsdh00 电话
        /// </summary>
        public virtual string Krlsdh00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsfzze 房费金额
        /// </summary>
        public virtual decimal? Krlsfzze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlscyze 餐饮金额
        /// </summary>
        public virtual decimal? Krlscyze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsqtze 其它金额
        /// </summary>
        public virtual decimal? Krlsqtze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsxjze
        /// </summary>
        public virtual decimal? Krlsxjze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsqtfk 其它付款
        /// </summary>
        public virtual decimal? Krlsqtfk
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszdts 住店天数
        /// </summary>
        public virtual decimal? Krlszdts
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszdcs 住店次数
        /// </summary>
        public virtual int? Krlszdcs
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscfh 首次房号
        /// </summary>
        public virtual string Krlsscfh
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscfl 首次房型
        /// </summary>
        public virtual string Krlsscfl
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscjl 首次价类
        /// </summary>
        public virtual string Krlsscjl
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscfj 首次房价
        /// </summary>
        public virtual decimal? Krlsscfj
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscrz 首次入住时间
        /// </summary>
        public virtual DateTime? Krlsscrz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscld 首次离店时间
        /// </summary>
        public virtual DateTime? Krlsscld
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsscyh 首次优惠
        /// </summary>
        public virtual decimal? Krlsscyh
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsdyrz
        /// </summary>
        public virtual DateTime? Krlsdyrz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsyhbz 优惠标志
        /// </summary>
        public virtual string Krlsyhbz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsjg00 籍贯
        /// </summary>
        public virtual string Krlsjg00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsyxrq 有效日期
        /// </summary>
        public virtual DateTime? Krlsyxrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krlspzr0
        /// </summary>
        public virtual string Krlspzr0
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsfkrq 发卡日期
        /// </summary>
        public virtual DateTime? Krlsfkrq
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsfkr0 发卡人
        /// </summary>
        public virtual string Krlsfkr0
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsmail Email
        /// </summary>
        public virtual string Krlsmail
        {
            get;
            set;
        }

        /// <summary>
        /// Krlskh01 卡号1
        /// </summary>
        public virtual string Krlskh01
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsgzxe 挂账限额
        /// </summary>
        public virtual decimal? Krlsgzxe
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsmcxe
        /// </summary>
        public virtual decimal? Krlsmcxe
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsgzze 挂账总额
        /// </summary>
        public virtual decimal? Krlsgzze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlskhyh
        /// </summary>
        public virtual string Krlskhyh
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsyhzh 优惠帐号
        /// </summary>
        public virtual string Krlsyhzh
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsbz00 备注
        /// </summary>
        public virtual string Krlsbz00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsxfze 消费总额
        /// </summary>
        public virtual decimal? Krlsxfze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsxp00 相片
        /// </summary>
        public virtual byte[] Krlsxp00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsqm00 签名图片
        /// </summary>
        public virtual byte[] Krlsqm00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsgj00 国籍
        /// </summary>
        public virtual string Krlsgj00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsgjm0 简码
        /// </summary>
        public virtual string Krlsgjm0
        {
            get;
            set;
        }

        /// <summary>
        /// Krlskhid 卡号ID
        /// </summary>
        public virtual int? Krlskhid
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsdlid 登录ID
        /// </summary>
        public virtual string Krlsdlid
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsfax0 FAX
        /// </summary>
        public virtual string Krlsfax0
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsye00 余额
        /// </summary>
        public virtual decimal? Krlsye00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlspyjm 拼音简码
        /// </summary>
        public virtual string Krlspyjm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlswbjm 五笔简码
        /// </summary>
        public virtual string Krlswbjm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsjfze 积分总额
        /// </summary>
        public virtual decimal? Krlsjfze
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsje01 
        /// </summary>
        public virtual decimal? Krlsje01
        {
            get;
            set;
        }

        /// <summary>
        /// Krlscs01
        /// </summary>
        public virtual decimal? Krlscs01
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsmm00 密码
        /// </summary>
        public virtual byte[] Krlsmm00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszt00 状态
        /// </summary>
        public virtual string Krlszt00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlslxdm 类型代码
        /// </summary>
        public virtual string Krlslxdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsyktk
        /// </summary>
        public virtual int? Krlsyktk
        {
            get;
            set;
        }

        /// <summary>
        /// krlxkfbz 客房标志
        /// </summary>
        public virtual string krlxkfbz
        {
            get;
            set;
        }

        /// <summary>
        /// krlxcybz 餐饮标志
        /// </summary>
        public virtual string krlxcybz
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsywx1
        /// </summary>
        public virtual string Krlsywx1
        {
            get;
            set;
        }

        /// <summary>
        /// Krlstel1
        /// </summary>
        public virtual string Krlstel1
        {
            get;
            set;
        }

        /// <summary>
        /// Krlstel2
        /// </summary>
        public virtual string Krlstel2
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszw00 职务
        /// </summary>
        public virtual string Krlszw00
        {
            get;
            set;
        }

        /// <summary>
        /// Krlshttp 网址
        /// </summary>
        public virtual string Krlshttp
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsbz01 备注01
        /// </summary>
        public virtual string Krlsbz01
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsbz02 备注02
        /// </summary>
        public virtual string Krlsbz02
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsbz03 备注03
        /// </summary>
        public virtual string Krlsbz03
        {
            get;
            set;
        }

        /// <summary>
        /// KrlsPeNo 
        /// </summary>
        public virtual string KrlsPeNo
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsywdm 
        /// </summary>
        public virtual string Krlsywdm
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsqxjd 权限酒店
        /// </summary>
        public virtual string Krlsqxjd
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsysje 应收金额
        /// </summary>
        public virtual decimal? Krlsysje
        {
            get;
            set;
        }

        /// <summary>
        /// KrlsGPID 集团酒店
        /// </summary>
        public virtual string KrlsGPID
        {
            get;
            set;
        }

        /// <summary>
        /// KrlsGUID 集团标志
        /// </summary>
        public virtual Guid KrlsGUID
        {
            get;
            set;
        }

        /// <summary>
        /// Krlsksdj
        /// </summary>
        public virtual string Krlsksdj
        {
            get;
            set;
        }

        /// <summary>
        /// Krlszsye 赠送余额
        /// </summary>
        public virtual decimal? Krlszsye
        {
            get;
            set;
        }

        /// <summary>
        /// Krlskzcs
        /// </summary>
        public virtual decimal? Krlskzcs
        {
            get;
            set;
        }
    }
}
