using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;

namespace OPUPMS.Domain.Base.Models
{
    /// <summary>
    /// 用户可自定义子项与系统属性关联关系表 实体类
    /// </summary>
    [Table("ExtendItemSettings")]
    public class ExtendItemSettingModel : Entity<int>
    {
        ///// <summary>
        ///// Id
        ///// </summary>
        //public virtual int Id
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// ExtendItemId
        /// </summary>
        public virtual int ExtendItemId
        {
            get;
            set;
        }

        /// <summary>
        /// SettingId
        /// </summary>
        public virtual int SettingId
        {
            get;
            set;
        }

        /// <summary>
        /// SettingName
        /// </summary>
        public virtual string SettingName
        {
            get;
            set;
        }
    }
}
