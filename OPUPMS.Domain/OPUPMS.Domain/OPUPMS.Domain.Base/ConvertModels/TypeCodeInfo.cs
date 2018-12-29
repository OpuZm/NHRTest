using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.ConvertModels
{
    /// <summary>
    /// 对应于类型代码表实体 Lxdm
    /// </summary>
    public class TypeCodeInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 代码 主键 Lxdmdm00
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称 Lxdmmc00
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址 Lxdmdz00
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮编 Lxdmzip0
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 联系人 Lxdmlxr0
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 负责人 Lxdmfzr0
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 电话 Lxdmdh00
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 喜好 Lxdmhp00
        /// </summary>
        public string PersonalPreference { get; set; }

        /// <summary>
        /// Email Lxdmmail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地区 Lxdmss00
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 佣金 Lxdmlxyj
        /// </summary>
        public decimal? Commission { get; set; }

        /// <summary>
        /// 消费总额 Lxdmxfze
        /// </summary>
        public decimal ConsumeAmount { get; set; }

        /// <summary>
        /// 余额 Lxdmye00
        /// </summary>
        public decimal? RemainAmount { get; set; }

        /// <summary>
        /// 挂账限额 Lxdmgzxe
        /// </summary>
        public decimal? ChargeLimitAmount { get; set; }

        /// <summary>
        /// 挂账标志 Lxdmgzbz
        /// </summary>
        public int? ChargeLimitFlag { get; set; }

        /// <summary>
        /// 结单性质，账单类型  Lxdmxz00
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 签单日期 Lxdmqdrq
        /// </summary>
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// 有效日期 Lxdmyxrq
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 传真FAX Lxdmfax0
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 协议备注 Lxdmxybz
        /// </summary>
        public string AgreementRemark { get; set; }

        /// <summary>
        /// 业务员 Lxdmywy0
        /// 关联CZDM
        /// </summary>
        public string SellerCode { get; set; }

        /// <summary>
        /// 挂账余额 Lxdmgzye
        /// </summary>
        public decimal? ChargeLimitRemainAmount { get; set; }

        /// <summary>
        /// 挂账总额 Lxdmgzze
        /// </summary>
        public decimal? ChargeLimitTotalAmount { get; set; }

        /// <summary>
        /// 相片 Lxdmxp00
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 签名图片 Lxdmqm00
        /// </summary>
        public byte[] SignImage { get; set; }

        /// <summary>
        /// 协议 Lxdmxy00
        /// </summary>
        public string Agreements { get; set; }

        /// <summary>
        /// 佣金代码 Lxdmyjdm
        /// 关联系统代码 YJDM
        /// </summary>
        public string CommisionCode { get; set; }

        /// <summary>
        /// 协议房价类别 Lxdmxyjl
        /// 关联系统代码 JL
        /// </summary>
        public string RoomPriceCategory { get; set; }

        /// <summary>
        /// 客户类别 Lxdmglkl
        /// 关联系统代码 KL
        /// </summary>
        public string ClientCategory { get; set; }

        /// <summary>
        /// 行业 Lxdmhy00
        /// </summary>
        public string ClientIndustry { get; set; }

        /// <summary>
        /// 包含 房间早餐数  Lxdmbh00
        /// 房价属性 含早 
        /// 关联系统代码 'JL2'
        /// </summary>
        public string RoomBreakfast { get; set; }
    }

    public class TypeCodeInfoSimple
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
