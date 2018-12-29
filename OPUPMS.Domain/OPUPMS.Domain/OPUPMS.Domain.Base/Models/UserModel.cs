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
    /// 用户表 实体类
    /// </summary>
    [Table("Users")]
    public class UserModel : Entity<int>
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
        /// UserCode
        /// </summary>
        public virtual string UserCode
        {
            get;
            set;
        }

        /// <summary>
        /// UserName
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// UserPwd
        /// </summary>
        public virtual string UserPwd
        {
            get;
            set;
        }

        /// <summary>
        /// GroupId
        /// </summary>
        public virtual int GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// CardId
        /// </summary>
        public virtual string CardId
        {
            get;
            set;
        }

        /// <summary>
        /// BirthDate
        /// </summary>
        public virtual DateTime? BirthDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gender
        /// </summary>
        public virtual int Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Phone
        /// </summary>
        public virtual string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// Status
        /// </summary>
        public virtual int Status
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
    }
}
