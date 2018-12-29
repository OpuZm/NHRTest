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
    /// 组织用户关系表 实体类
    /// </summary>
    [Table("OrganizationUsers")]
    public class OrganizationUserModel : Entity<int>
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
        /// GroupId
        /// </summary>
        public virtual int GroupId
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
        /// UserId
        /// </summary>
        public virtual int UserId
        {
            get;
            set;
        }
    }
}
