/********************************************************************************
 * Copyright © 2017 OPUPMS.Framework 版权所有
 * Author: OPUPMS - Justin
 * Description: OPUPMS快速开发平台
 * Website：http://www.opu.com.cn
*********************************************************************************/

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
    /// 操作历史 实体类
    /// </summary>
    [Table("Czls")]
    public class CzlsModel : Entity<int>
    {
        static CzlsModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<CzlsModel>()
                .SetProperty(entity => entity.Id,
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Czlsxh00");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// 操作历史序号 主键 标识列 
        ///// </summary>        
        //public int Czlsxh00 { get; set; }

        /// <summary>
        /// 操作代码 关联 Czdm.Czdmdm00 
        /// </summary>
        public string Czlsczdm { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime? Czlsczrq { get; set; }

        /// <summary>
        /// 电脑名称
        /// </summary>
        public string Czlspc00 { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Czlstext { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Czlstype { get; set; }
        
    }
}
