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
    /// 用户可自定义编辑子项表 实体类
    /// </summary>
    [Table("ExtendItems")]
    public class ExtendItemModel : Entity<int>
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
        /// Code
        /// </summary>
        public virtual string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// TypeId
        /// </summary>
        public virtual int TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// ItemValue
        /// </summary>
        public virtual string ItemValue
        {
            get;
            set;
        }

        /// <summary>
        /// CompanyId
        /// </summary>
        public virtual int CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// Sort
        /// </summary>
        public virtual int? Sort
        {
            get;
            set;
        }
    }
}
