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
    /// 用户角色关系表 实体类
    /// </summary>
    [Table("UserRoles")]
    public class UserRoleModel : Entity<int>
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
        /// UserId
        /// </summary>
        public virtual int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// RoleId
        /// </summary>
        public virtual int RoleId
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
        /// Sort
        /// </summary>
        public virtual int? Sort
        {
            get;
            set;
        }
    }
}
