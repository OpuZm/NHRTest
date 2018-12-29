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
    /// 公司表 实体类
    /// </summary>
    [Table("Company")]
    public class CompanyModel : Entity<int>
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
        /// FullName
        /// </summary>
        public virtual string FullName
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
        /// Address
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }

        /// <summary>
        /// ContactTel
        /// </summary>
        public virtual string ContactTel
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
        /// Manager
        /// </summary>
        public virtual string Manager
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
        /// Content
        /// </summary>
        public virtual string Content
        {
            get;
            set;
        }

        /// <summary>
        /// AreaId
        /// </summary>
        public virtual int? AreaId
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
        /// Fax
        /// </summary>
        public virtual string Fax
        {
            get;
            set;
        }
    }
}
