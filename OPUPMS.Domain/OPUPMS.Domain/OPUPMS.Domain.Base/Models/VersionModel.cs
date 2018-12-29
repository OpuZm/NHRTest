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
    /// 版本记录表 实体类
    /// </summary>
    [Table("Versions")]
    public class VersionModel : Entity<int>
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
        /// CodeVersion
        /// </summary>
        public virtual string CodeVersion
        {
            get;
            set;
        }

        /// <summary>
        /// ScriptId
        /// </summary>
        public virtual int ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// ScriptStatus
        /// </summary>
        public virtual string ScriptStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Product
        /// </summary>
        public virtual string Product
        {
            get;
            set;
        }

        /// <summary>
        /// LastUpdate
        /// </summary>
        public virtual DateTime LastUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Revision
        /// </summary>
        public virtual string Revision
        {
            get;
            set;
        }
    }
}
