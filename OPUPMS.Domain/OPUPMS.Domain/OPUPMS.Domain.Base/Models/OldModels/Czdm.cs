using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Models
{
    public class Czdm
    {
        /// <summary>
        /// 操作代码 主键列
        /// </summary>        
        public string Czdmdm00 { get; set; }

        /// <summary>
        /// 操作代码名称 不为null
        /// </summary>
        public string Czdmmc00 { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Czdmbm00 { get; set; }

        /// <summary>
        /// 密码 二进制
        /// </summary>
        public byte[] Czdmmm00 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Czdmcddm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmyh00 { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string Czdmzb00 { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string Czdmip00 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmnyh0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmnzb0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Czdmmmrq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Czdmmmts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Czdmbdcs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Czdmcgcs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Czdmffcs { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public int Czdmattr { get; set; }

        /// <summary>
        /// 权限00
        /// </summary>
        public int Czdmqx00 { get; set; }

        /// <summary>
        /// 权限01
        /// </summary>
        public int Czdmqx01 { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Czdmcsrq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmaddr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmtype { get; set; }

        /// <summary>
        /// 权限组别 不为null
        /// </summary>
        public string Czdmqxzb { get; set; }

        /// <summary>
        /// 其它密码
        /// </summary>
        public string Czdmpwd2 { get; set; }

        /// <summary>
        /// 属性设置2
        /// </summary>
        public int? Czdmatr2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmfzxh { get; set; }

        /// <summary>
        /// 属性设置1
        /// </summary>
        public int? Czdmatr1 { get; set; }

        /// <summary>
        /// 登录卡号
        /// </summary>
        public string Czdmcard { get; set; }

        /// <summary>
        /// 赠送余额
        /// </summary>
        public decimal? Czdmzsye { get; set; }

        /// <summary>
        /// 赠送类型
        /// </summary>
        public string Czdmzslx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmlslx { get; set; }

        /// <summary>
        /// 折扣类型
        /// </summary>
        public decimal? Czdmzkl0 { get; set; }

        /// <summary>
        /// 餐厅属性
        /// </summary>
        public int? Czdmposx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Czdmsx02 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmbbqx { get; set; }

        /// <summary>
        /// 权限03
        /// </summary>
        public string Czdmqx03 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Czdmsx03 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Czdmsx04 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmxfq0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Czdmslqx { get; set; }

        /// <summary>
        /// 取消金额
        /// </summary>
        public decimal? Czdmqxje { get; set; }
        public int Id { get; set; }
        public int Czdmcyqx { get; set; }
        public decimal Czdmmlje { get; set; }
    }
}
