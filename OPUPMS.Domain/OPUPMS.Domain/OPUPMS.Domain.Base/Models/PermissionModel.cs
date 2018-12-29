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
    /// 权限表 实体类
    /// </summary>
    [Table("Permissions")]
    public class PermissionModel : Entity<int>
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
        /// PermissionValue
        /// </summary>
        public virtual string PermissionValue
        {
            get;
            set;
        }

        /// <summary>
        /// PermissionType
        /// </summary>
        public virtual int PermissionType
        {
            get;
            set;
        }

        /// <summary>
        /// UseInModule
        /// </summary>
        public virtual int UseInModule
        {
            get;
            set;
        }

        /// <summary>
        /// Descriptions
        /// </summary>
        public virtual string Descriptions
        {
            get;
            set;
        }

        /// <summary>
        /// 关联上级权限Id
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
