using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Base.Models
{
    /// <summary>
    /// 可扩展类型表 实体类
    /// </summary>
    [Table("ExtendTypes")]
    public class ExtendTypeModel : IEntity<int>
    {
        //static ExtendTypeModel()
        //{
        //    OrmConfiguration.GetDefaultEntityMapping<ExtendTypeModel>()
        //        .SetProperty(entity => entity.Id,
        //            prop =>
        //            {
        //                prop.SetDatabaseColumnName("Id");
        //                //prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
        //            });
        //}

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public virtual int Id
        {
            get;
            set;
        }

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
        /// Status
        /// </summary>
        public virtual bool Status
        {
            get;
            set;
        }
    }
}
