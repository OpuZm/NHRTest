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
    /// 区域记录表 实体类
    /// </summary>
    [Table("AreaLogs")]
    public class AreaLogModel : Entity<int>
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
        /// CountryId
        /// </summary>
        public virtual int? CountryId
        {
            get;
            set;
        }

        /// <summary>
        /// ProvinceId
        /// </summary>
        public virtual int? ProvinceId
        {
            get;
            set;
        }

        /// <summary>
        /// CityId
        /// </summary>
        public virtual int? CityId
        {
            get;
            set;
        }

        /// <summary>
        /// CountyId
        /// </summary>
        public virtual int? CountyId
        {
            get;
            set;
        }
    }
}
