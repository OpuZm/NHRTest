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
    /// 区域代码表 实体类
    /// </summary>
    [Table("AreaCodes")]
    public class AreaCodeModel : Entity<int>
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
        /// Enabled
        /// </summary>
        public virtual bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// 如 GJ --> 国籍
        /// </summary>
        public virtual string TypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// ParentId
        /// </summary>
        public virtual int ParentId
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
