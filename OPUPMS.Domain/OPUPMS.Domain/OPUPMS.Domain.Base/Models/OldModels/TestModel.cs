using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smooth.IoC.Repository.UnitOfWork;
using Dapper.FastCrud;

namespace OPUPMS.Domain.Base.Models
{
    public class TestModel : Entity<int>
    {
        public string Name
        {
            get;
            set;
        }

        static TestModel()
        {
            OrmConfiguration.GetDefaultEntityMapping<TestModel>().SetProperty(entity => entity.Id, prop => prop.SetDatabaseColumnName("ii"));
        }

        public string GetSql()
        {
            var rawSqlQuery = OrmConfiguration.GetSqlBuilder<TestModel>().Format(
                $@"SELECT {nameof(Id):C},{nameof(Name):C} FROM {nameof(TestModel):T}");
            return rawSqlQuery;
        }
    }
}
