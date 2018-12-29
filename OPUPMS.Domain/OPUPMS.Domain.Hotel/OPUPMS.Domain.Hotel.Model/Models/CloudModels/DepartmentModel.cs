using Dapper.FastCrud;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Hotel.Model.CloudModels
{
    /// <summary>
    /// Department 实体类
    /// </summary>
    [Table("Department")]
    public class DepartmentModel : Entity<int>
    {
        static DepartmentModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<DepartmentModel>()
                .SetProperty(entity => entity.Id, 
                    prop =>
                    {
                        prop.SetDatabaseColumnName("Did");
                        prop.SetDatabaseGenerated(DatabaseGeneratedOption.Identity);
                    });
        }

        ///// <summary>
        ///// Did 序号 主键 标识列
        ///// </summary>
        //public virtual int Did
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Dname 酒店名称
        /// </summary>
        public virtual string Dname
        {
            get;
            set;
        }

        /// <summary>
        /// Dpid
        /// </summary>
        public virtual int Dpid
        {
            get;
            set;
        }

        /// <summary>
        /// Dppid
        /// </summary>
        public virtual int Dppid
        {
            get;
            set;
        }

        /// <summary>
        /// Dstatus
        /// </summary>
        public virtual string Dstatus
        {
            get;
            set;
        }

        /// <summary>
        /// Dorder
        /// </summary>
        public virtual int Dorder
        {
            get;
            set;
        }

        /// <summary>
        /// Ddata
        /// </summary>
        public virtual string Ddata
        {
            get;
            set;
        }

        /// <summary>
        /// DNO
        /// </summary>
        public virtual string DNO
        {
            get;
            set;
        }

        /// <summary>
        /// DEndDate
        /// </summary>
        public virtual DateTime? DEndDate
        {
            get;
            set;
        }


        /// <summary>
        /// Dxlh
        /// </summary>
        public virtual string Dxlh
        {
            get;
            set;
        }

        /// <summary>
        /// Dmk
        /// </summary>
        public virtual string Dmk
        {
            get;
            set;
        }

        /// <summary>
        /// Djdm
        /// </summary>
        public virtual string Djdm
        {
            get;
            set;
        }
    }
}
