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
    /// 通用字典项表 实体类
    /// </summary>
    [Table("DictionaryItems")]
    public class DictionaryItemModel : IEntity<int>
    {
        //static DictionaryItemModel()
        //{
        //    OrmConfiguration.GetDefaultEntityMapping<DictionaryItemModel>()
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
        /// ItemCode
        /// </summary>
        public virtual string ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// ItemName
        /// </summary>
        public virtual string ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// ItemValue
        /// </summary>
        public virtual string ItemValue
        {
            get;
            set;
        }

        /// <summary>
        /// DicTypeId
        /// </summary>
        public virtual int DicTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Enabled
        /// </summary>
        public virtual bool Enabled
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
        /// Sort
        /// </summary>
        public virtual int? Sort
        {
            get;
            set;
        }
    }
}
