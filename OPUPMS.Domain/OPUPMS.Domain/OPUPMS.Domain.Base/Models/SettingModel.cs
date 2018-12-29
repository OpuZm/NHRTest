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
    /// 系统属性表 实体类
    /// </summary>
    [Table("Settings")]
    public class SettingModel : Entity<int>
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
        /// SettingId
        /// </summary>
        public virtual int SettingId
        {
            get;
            set;
        }

        /// <summary>
        /// SettingName
        /// </summary>
        public virtual string SettingName
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
        /// OrganizationType
        /// </summary>
        public virtual int OrganizationType
        {
            get;
            set;
        }

        /// <summary>
        /// SettingValue
        /// </summary>
        public virtual string SettingValue
        {
            get;
            set;
        }

        /// <summary>
        /// SetringGroup
        /// </summary>
        public virtual int SetringGroup
        {
            get;
            set;
        }

        /// <summary>
        /// UseInModule
        /// </summary>
        public virtual int? UseInModule
        {
            get;
            set;
        }

        /// <summary>
        /// Descriptrions
        /// </summary>
        public virtual string Descriptrions
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
