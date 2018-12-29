using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Dapper.FastCrud;
using OPUPMS.Domain.Base.Dtos;
using Smooth.IoC.Repository.UnitOfWork;

namespace OPUPMS.Domain.Base.Models
{
    /// <summary>
    /// 系统代码 实体类
    /// </summary>
    [Table("Xtdm")]
    public class XtdmModel : Entity<string>
    {
        //static XtdmModel()
        //{
        //}

        /// <summary>
        /// 系统代码类型 主键列
        /// </summary>
        public string Xtdmlx00 { get; set; }

        /// <summary>
        /// 系统代码 主键列
        /// </summary>        
        public string Xtdmdm00 { get; set; }

        /// <summary>
        /// 系统代码名称 不为null
        /// </summary>
        public string Xtdmmc00 { get; set; }

        /// <summary>
        /// 系统代码标识 不为null
        /// </summary>
        public string Xtdmbzs0 { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int? Xtdmxsxh { get; set; }

        /// <summary>
        /// 日期01
        /// </summary>
        public string Xtdmrq01 { get; set; }

        /// <summary>
        /// 日期02
        /// </summary>
        public string Xtdmrq02 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Xtdmsslb { get; set; }
        
        /// <summary>
        /// 整数00
        /// </summary>
        public int? Xtdmzs00 { get; set; }
        
        /// <summary>
        /// 小数00
        /// </summary>
        public decimal Xtdmxs00 { get; set; }

        /// <summary>
        /// 备注01
        /// </summary>
        public string Xtdmbz00 { get; set; }
        
        /// <summary>
        /// 备注02
        /// </summary>
        public string Xtdmbz01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Xtdmtext { get; set; }

        /// <summary>
        /// 系统代码英文名称
        /// </summary>
        public string Xtdmywmc { get; set; }
    }    
}
