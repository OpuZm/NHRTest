using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 账务代码信息 对应于实体表 Zwdm
    /// </summary>
    public class AccountingCodeInfo
    {
        /// <summary>
        /// 账务代码 主键 Zwdmdm00
        /// </summary>
        public string AccCode { get; set; }

        /// <summary>
        /// 参考号码 Zwdmckhm
        /// 关联系统代码 CK
        /// </summary>
        public string ReferenceNo { get; set; }

        /// <summary>
        /// 账务名称 Zwdmmc00
        /// </summary>
        public string AccName { get; set; }

        /// <summary>
        /// 结单性质，账单类型  Zwdmjdxz
        /// C-付款项，D-消费项
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 单位 Zwdmdw00
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 单价 Zwdmdj00
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 备注 Zwdmbz00
        /// FW-房务中心，SW-商务中心
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 标志，状态 Zwdmbzs0
        /// Y-启用,X-可删除，N-禁用
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类别 Zwdmlb00
        /// Z-总类，X-小类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 属性 Zwdmattr
        /// </summary>
        public int AccProperty { get; set; }

        /// <summary>
        /// 分账号 Zwdmfzh0
        /// </summary>
        public int? SubId { get; set; }

        /// <summary>
        /// 账务类别 Zwdmzwlb
        /// </summary>
        public string AccCategory { get; set; }

        /// <summary>
        /// 所属类别，父类代码 Zwdmsslb
        /// 填写总类代码zwdmdm00
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 应收代码 Zwdmysdm
        /// 关联系统代码 YS
        /// </summary>
        public string AccReceivableCode { get; set; }

        /// <summary>
        /// 英文名 Zwdmywmc
        /// </summary>
        public string EnglishName { get; set; }
        
    }
}
