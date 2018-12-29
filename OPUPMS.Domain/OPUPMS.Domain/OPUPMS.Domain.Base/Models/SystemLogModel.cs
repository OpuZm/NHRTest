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
    /// 系统日志记录表 实体类
    /// </summary>
    [Table("SystemLogs")]
    public class SystemLogModel : Entity<int>
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
        /// TypeId
        /// </summary>
        public virtual int TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Title
        /// </summary>
        public virtual string Title
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
        /// OperateId
        /// </summary>
        public virtual int OperateId
        {
            get;
            set;
        }

        /// <summary>
        /// Operator
        /// </summary>
        public virtual string Operator
        {
            get;
            set;
        }

        /// <summary>
        /// OperatedTime
        /// </summary>
        public virtual DateTime OperatedTime
        {
            get;
            set;
        }

        /// <summary>
        /// OperateUrl
        /// </summary>
        public virtual string OperateUrl
        {
            get;
            set;
        }

        /// <summary>
        /// OperateParas
        /// </summary>
        public virtual string OperateParas
        {
            get;
            set;
        }

        /// <summary>
        /// OperateModuleType
        /// </summary>
        public virtual int OperateModuleType
        {
            get;
            set;
        }
    }
}
