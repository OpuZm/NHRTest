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
    /// 角色表 实体类
    /// </summary>
    [Table("Roles")]
    public class RoleModel : Entity<int>
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
        /// Name
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationId
        /// </summary>
        public virtual int OrganizationId
        {
            get;
            set;
        }

        /// <summary>
        /// 1.集团 / 2.公司 
        /// </summary>
        public virtual int OrganizationType
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
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// Creator
        /// </summary>
        public virtual int Creator
        {
            get;
            set;
        }

        /// <summary>
        /// CreatedTime
        /// </summary>
        public virtual DateTime CreatedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Editor
        /// </summary>
        public virtual int? Editor
        {
            get;
            set;
        }

        /// <summary>
        /// EditedTime
        /// </summary>
        public virtual DateTime? EditedTime
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
