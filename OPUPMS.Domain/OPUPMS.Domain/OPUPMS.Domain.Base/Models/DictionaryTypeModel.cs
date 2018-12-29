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
    /// 公共字典类型表 实体类
    /// </summary>
    [Table("DictionaryTypes")]
    public class DictionaryTypeModel : IEntity<int>
    {
        //static DictionaryTypeModel()
        //{
        //    OrmConfiguration.GetDefaultEntityMapping<DictionaryTypeModel>()
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
        /// TypeCode
        /// </summary>
        public virtual string TypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// TypeName
        /// </summary>
        public virtual string TypeName
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
    }
}
