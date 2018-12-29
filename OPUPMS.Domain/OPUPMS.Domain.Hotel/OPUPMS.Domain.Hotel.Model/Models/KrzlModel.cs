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
    /// 客人资料 实体类
    /// </summary>
    [Table("Krzl")]
    public class KrzlModel : Entity<int>
    {
        static KrzlModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<KrzlModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Krzlzh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
            //OrmConfiguration.GetDefaultEntityMapping<KrzlModel>()
            //    .SetProperty(entity => entity.Krzlaazd,
            //        prop =>
            //        {
            //            prop.IsExcludedFromUpdates = true;
            //            prop.IsExcludedFromInserts = true;
            //        });
        }

        ///// <summary>
        ///// 帐号 主键 标识列
        ///// </summary>
        //public int Krzlzh00 { get; set; }
        
        /// <summary>
        /// 帐号类型
        /// </summary>
        public short? Krzlzhlx { get; set; }

        /// <summary>
        /// 同住序号
        /// </summary>
        public int? Krzltzxh { get; set; }

        /// <summary>
        /// 同类序号
        /// </summary>
        public int? Krzltlxh { get; set; }

        /// <summary>
        /// 房号  关联房号代码 Fhdm.Fhdmdm00
        /// </summary>
        public string Krzlfh00 { get; set; }

        /// <summary>
        /// 状态 取值系统代码 = ZT
        /// I-在住;O-离店;N-预定;T-暂离;C-预定取消
        /// </summary>
        public string Krzlzt00 { get; set; }

        /// <summary>
        /// 中文姓名
        /// </summary>
        public string Krzlzwxm { get; set; }

        /// <summary>
        /// 英文姓名
        /// </summary>
        public string Krzlywxm { get; set; }

        /// <summary>
        /// 性别 xtdmlx00='XB'
        /// F-女;M -男
        /// </summary>
        public string Krzlxb00 { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Krzldz00 { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Krzlgs00 { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Krzlcsrq { get; set; }

        /// <summary>
        /// 证件类别  xtdmlx00='ZJ'
        /// </summary>
        public string Krzlzjlb { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string Krzlzjhm { get; set; }

        /// <summary>
        /// 籍贯 xtdmlx00='JG'
        /// </summary>
        public string Krzljg00 { get; set; }

        /// <summary>
        /// 国籍xtdmlx00='GJ'
        /// </summary>
        public string Krzlgj00 { get; set; }

        /// <summary>
        /// 国籍代码
        /// </summary>
        public string Krzlgjdm { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime? Krzlrzrq { get; set; }

        /// <summary>
        /// 离店日期
        /// </summary>
        public DateTime? Krzlldrq { get; set; }

        /// <summary>
        /// 原入住日期
        /// </summary>
        public DateTime? Krzlyrzr { get; set; }

        /// <summary>
        /// 原离店日期
        /// </summary>
        public DateTime? Krzlyldr { get; set; }

        /// <summary>
        /// 预定号 关联 Dfzl.Dfzlxh00
        /// </summary>
        public int Krzlydh0 { get; set; }

        /// <summary>
        /// 订房明细 关联 Dfmx.Dfmxxh00
        /// </summary>
        public int Krzldfmx { get; set; }

        /// <summary>
        /// 团号  不为null
        /// </summary>
        public string Krzlth00 { get; set; }

        /// <summary>
        /// 团名
        /// </summary>
        public string Krzltm00 { get; set; }

        /// <summary>
        /// 协议客户 关联 Lxdm.Lxdmdm00
        /// </summary>
        public string Krzllxdm { get; set; }

        /// <summary>
        /// VIP  不为null ; xtdmlx00='krvp'
        /// </summary>
        public string Krzlvip0 { get; set; }

        /// <summary>
        /// 客人类别  xtdmlx00='ttlx'
        /// </summary>
        public string Krzlkrlb { get; set; }

        /// <summary>
        /// 客人类型 xtdmlx00='kl'
        /// </summary>
        public string Krzlkrlx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Krzlkrty { get; set; }

        /// <summary>
        /// 房价类别 xtdmlx00='jl'
        /// </summary>
        public string Krzlfjlb { get; set; }

        /// <summary>
        /// 房价批准 关联 Czdm.Czdmdm00
        /// </summary>
        public string Krzlfjpz { get; set; }

        /// <summary>
        /// 分单代码xtdmlx00='fd'
        /// </summary>
        public string Krzlfddm { get; set; }

        /// <summary>
        /// 房型xtdmlx00='fl'
        /// </summary>
        public string Krzlfjlx { get; set; }

        /// <summary>
        /// 结账方式xtdmlx00='fk'
        /// </summary>
        public string Krzljzfs { get; set; }

        /// <summary>
        /// 结账货币 xtdmlx00='hb'
        /// </summary>
        public string Krzljzhb { get; set; }

        /// <summary>
        /// 房租 不为null
        /// </summary>
        public decimal Krzlfz00 { get; set; }

        /// <summary>
        /// 换房房号
        /// </summary>
        public string Krzlhffh { get; set; }

        /// <summary>
        /// 换房日期
        /// </summary>
        public DateTime? Krzlhfrq { get; set; }

        /// <summary>
        /// 续住日期
        /// </summary>
        public DateTime? Krzlxzrq { get; set; }

        /// <summary>
        /// 房全价
        /// </summary>
        public decimal Krzlfqj0 { get; set; }

        /// <summary>
        /// 优惠率 = Krzlfz00 / Krzlfqj0
        /// </summary>
        public decimal Krzlyhl0 { get; set; }

        /// <summary>
        /// 消费总额
        /// </summary>
        public decimal Krzlxfze { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Krzlye00 { get; set; }

        /// <summary>
        /// 上日余额
        /// </summary>
        public decimal Krzlsrye { get; set; }

        /// <summary>
        /// 客人历史序号  关联 Krls.Krlsxh00
        /// </summary>
        public int? Krzllsxh { get; set; }

        /// <summary>
        /// 客人历史明细  关联 Krlm.Krlmxh00
        /// </summary>
        public int? Krzllsmx { get; set; }

        /// <summary>
        /// 外事序号  关联 Krws.Krwsxh00
        /// </summary>
        public int? Krzlwsxh { get; set; }

        /// <summary>
        /// 结账帐号
        /// </summary>
        public int? Krzljzzh { get; set; }

        /// <summary>
        /// 客人备注
        /// </summary>
        public string Krzlkrbz { get; set; }

        /// <summary>
        /// 账务备注
        /// </summary>
        public string Krzlzwbz { get; set; }

        /// <summary>
        /// 房务备注
        /// </summary>
        public string Krzlfwbz { get; set; }

        /// <summary>
        /// 留言数量
        /// </summary>
        public short Krzllysl { get; set; }

        /// <summary>
        /// 挂账序号
        /// </summary>
        public int? Krzlgzxh { get; set; }

        /// <summary>
        /// 卡号  关联 Krls.Krlsvpkh
        /// </summary>
        public string Krzlkh00 { get; set; }

        /// <summary>
        /// 业务员 关联 Czdm.Czdmdm00
        /// </summary>
        public string Krzlywy0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Krzlfzyh { get; set; }

        /// <summary>
        /// 拼音简码
        /// </summary>
        public string Krzlpyjm { get; set; }

        /// <summary>
        /// 五笔简码
        /// </summary>
        public string Krzlwbjm { get; set; }

        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime? Krzlrzsj { get; set; }

        /// <summary>
        /// 离店时间
        /// </summary>
        public DateTime? Krzlldsj { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Krzlfjl2 { get; set; }

        /// <summary>
        /// 房价属性 含早 xtdmlx00='JL2';
        /// </summary>
        public int? Krzlfjsx { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string Krzlfphm { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal? Krzlfpje { get; set; }

        /// <summary>
        /// 发票数量
        /// </summary>
        public int? Krzlfpsl { get; set; }

        /// <summary>
        /// 序号01
        /// </summary>
        public int? Krzlxh01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Krzlzw00 { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Krzlmz00 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Krzlyktk { get; set; }

        /// <summary>
        /// 序号02
        /// </summary>
        public int? Krzlxh02 { get; set; }

        /// <summary>
        /// 序号03
        /// </summary>
        public int? Krzlxh03 { get; set; }

        /// <summary>
        /// 序号04
        /// </summary>
        public int? Krzlxh04 { get; set; }

        /// <summary>
        /// 序号05
        /// </summary>
        public int? Krzlxh05 { get; set; }

        /// <summary>
        /// 房间属性
        /// 资料保密2，房价保密16，钟点房4，成员自付32，VIP:8,不可转账64
        /// </summary>
        public int? Krzlattr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Krzlskxh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Krzlyyly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Krzlkmbh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Krzlaazd { get; set; }

        /// <summary>
        /// 授权金额
        /// </summary>
        public decimal? Krzlsqje { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Krzlcqsl { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Krzlsdcz { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Krzlsdrq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Krzlhscs { get; set; }

        /// <summary>
        /// 楼座代码 xtdmlx00='lz'
        /// </summary>
        public string Krzlfdd2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Krzlfdd3 { get; set; }

        /// <summary>
        /// Mail
        /// </summary>
        public string Krzlmail { get; set; }

        /// <summary>
        /// 英文名1
        /// </summary>
        public string Krzlywx1 { get; set; }

        /// <summary>
        /// 电话 00
        /// </summary>
        public string Krzldh00 { get; set; }

        /// <summary>
        /// 电话01
        /// </summary>
        public string Krzldh01 { get; set; }

        /// <summary>
        /// FAX
        /// </summary>
        public string Krzlfax0 { get; set; }

        /// <summary>
        /// 头像名称
        /// </summary>
        public string Krzltxmc { get; set; }

        /// <summary>
        /// 挂账标志
        /// </summary>
        public int? Krzlgzbz { get; set; }

        /// <summary>
        /// 同时操作
        /// </summary>
        public string Krzltscz { get; set; }
        
        /// <summary>
        /// 包价
        /// </summary>
        public string Krzlpkge { get; set; }

        /// <summary>
        /// 收取门卡  Y已收，N未收
        /// </summary>
        public string Krzlsqmk { get; set; }

        /// <summary>
        /// 市场xtdmlx00='SC'
        /// </summary>
        public string Krzlscdm { get; set; }

        /// <summary>
        /// 接待管家xtdmlx00='jdgj'
        /// </summary>
        public string Krzljdgj { get; set; }

        /// <summary>
        /// 管家电话
        /// </summary>
        public string Krzlgjdh { get; set; }

        /// <summary>
        /// 房价结构 xtdmlx00='fjjg'
        /// </summary>
        public string Krzlfjjg { get; set; }

        /// <summary>
        /// 特服 xtdmlx00='potf'
        /// </summary>
        public string Krzltf00 { get; set; }

        /// <summary>
        /// 挂账限额
        /// </summary>
        public decimal? Krzlgzxe { get; set; }

        /// <summary>
        /// 集团接口
        /// </summary>
        public string Krzljtjk { get; set; }

        /// <summary>
        /// 计算佣金
        /// </summary>
        public decimal? Krzljsyj { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal? Krzlyj00 { get; set; }

        /// <summary>
        /// 佣金审核
        /// Y已审，X未审
        /// </summary>
        public string Krzlyjsh { get; set; }

        /// <summary>
        /// 佣金支付
        /// Y已付，X未付
        /// </summary>
        public string Krzlyjzf { get; set; }

        /// <summary>
        /// 未到标志
        /// Noshow
        /// </summary>
        public string Krzlwdbz { get; set; }

        /// <summary>
        /// 自动转账
        /// 账务代码zwdm用逗号隔开
        /// </summary>
        public string Krzlzdzz { get; set; }

        /// <summary>
        /// 佣金代码
        /// </summary>
        public string Krzlyjdm { get; set; }

        /// <summary>
        /// 打印属性
        /// 0表示可操作打印
        /// </summary>
        public int? Krzldysx { get; set; }

        /// <summary>
        /// 升级序号 关联Dfmx.Dfmxxh00 
        /// 原订房明细序号
        /// </summary>
        public int? Krzlsjxh { get; set; }

        /// <summary>
        /// 锁定房间
        /// 排房锁定房间，不允许删除排房，Y已锁定
        /// </summary>
        public string Krzlsdfj { get; set; }

        /// <summary>
        /// 住店次数
        /// </summary>
        public int? Krzlzdcs { get; set; }

        /// <summary>
        /// 序号06
        /// </summary>
        public int? Krzlxh06 { get; set; }

    }
}
