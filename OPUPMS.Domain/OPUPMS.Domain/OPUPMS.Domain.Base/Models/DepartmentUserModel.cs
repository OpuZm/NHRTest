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
    /// 部门用户关系表 实体类
    /// </summary>
    [Table("DepartmentUsers")]
    public class DepartmentUserModel : Entity<int>
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
        /// DepartmentId
        /// </summary>
        public virtual int DepartmentId
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
