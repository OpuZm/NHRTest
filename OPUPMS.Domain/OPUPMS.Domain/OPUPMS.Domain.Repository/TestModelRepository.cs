using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Infrastructure.Dapper;
using Dapper;
using OPUPMS.Domain.Base.Repositories;

namespace OPUPMS.Domain.Repository
{
    public class TestModelRepository : RepositoryEx<TestModel, int>, ITestModelRepository
    {
        public TestModelRepository(IDbFactory factory) : base(factory)
        {
        }

        public IEnumerable<TestModel> GetList()
        {
            return new List<TestModel>
            {
                new TestModel
                {
                    Name = "Test-1"
                },
                new TestModel
                {
                },
                new TestModel
                {
                    Name = "Test-3"
                },
                new TestModel
                {
                    Name = "Test-4"
                }
            };
        }

        //public IEnumerable<TestModel> GetList(IUnitOfWork uow)
        //{
        //    if(uow != null)
        //    {
        //        return uow.Connection.Query<TestModel>(sql, new TestModel
        //        {
        //            Id = 1
        //        }, uow.Transaction);
        //    }

        //    using (var session = Factory.Create<ISession>())
        //    {
        //        return session.Query<TestModel>(sql, new TestModel
        //        {
        //            Id = 1
        //        });
        //    }
        //}
    }
}
