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
    /// 部门表 实体类
    /// </summary>
    [Table("Departments")]
    public class DepartmentModel : Entity<int>
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
        /// OrganizationId
        /// </summary>
        public virtual int OrganizationId
        {
            get;
            set;
        }

        /// <summary>
        /// 1.集团  /  2.公司
        /// </summary>
        public virtual int OrganizationType
        {
            get;
            set;
        }

        /// <summary>
        /// 用户Id，关联User.Id
        /// </summary>
        public virtual int? LeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// 关联上级部门Id
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
