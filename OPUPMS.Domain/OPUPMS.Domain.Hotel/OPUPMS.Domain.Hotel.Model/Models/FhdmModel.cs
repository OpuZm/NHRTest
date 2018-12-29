using Dapper.FastCrud;
using OPUPMS.Domain.Hotel.Model.Dtos;
using Smooth.IoC.Repository.UnitOfWork;
using Smooth.IoC.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Model
{
    /// <summary>
    /// 房号代码 实体类
    /// </summary>
    [Table("Fhdm")]
    public class FhdmModel : IEntity<string>
    {
        //static FhdmModel()
        //{
        //}

        /// <summary>
        /// 房号代码 主键列 Fhdmdm00 
        /// </summary>
        [Key]
        [Column("Fhdmdm00")]
        public string Id { get; set; }

        ///// <summary>
        ///// 房号代码 主键列
        ///// </summary>        
        //public string Fhdmdm00 { get; set; }

        /// <summary>
        /// 房号代码楼座 不为null 关联系统代码 LZ
        /// </summary>
        public string Fhdmlz00 { get; set; }

        /// <summary>
        /// 房号代码楼层 不为null 关联系统代码 LC
        /// </summary>
        public string Fhdmlc00 { get; set; }

        /// <summary>
        /// 类型00 不为null   关联系统代码 FL
        /// </summary>
        public string Fhdmlx00 { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Fhdmdj00 { get; set; }

        /// <summary>
        /// 状态 不为null  
        /// 关联系统代码 FT   V表示空房、O表示住房
        /// </summary>
        public string Fhdmzt00 { get; set; }

        /// <summary>
        /// 操作 不为null  
        /// 关联系统代码 FTSW   C表示净房、D表示脏房
        /// </summary>
        public string Fhdmcd00 { get; set; }

        /// <summary>
        /// 房态 不为null
        /// </summary>
        public string Fhdmft00 { get; set; }

        /// <summary>
        /// 其它状态
        /// </summary>
        public string Fhdmqtzt { get; set; }

        /// <summary>
        /// 可住人数
        /// </summary>
        public short Fhdmkzrs { get; set; }

        /// <summary>
        /// 实住人数
        /// </summary>
        public short Fhdmszrs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Fhdmjcrs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Fhdmggrq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmggcz { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public int Fhdmattr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmpms0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Fhdmmsid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Fhdmmsxh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmlock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmzt01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Fhdmfksl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Fhdmbz00 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Fhdmlkxh { get; set; }

        /// <summary>
        /// 二进制列
        /// </summary>
        public byte[] Fhdmtjlx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmzt02 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Fhdmdj01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Fhdmxs01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Fhdmwzr0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Fhdmwzc0 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmczbz { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmchek { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Fhdmrzts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fhdmyz00 { get; set; }
    }
}
