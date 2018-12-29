using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Base.Services;

namespace OPUPMS.Domain.Services
{
    public class TestModelDomainService : ITestModelDomainService
    {
        readonly IDbFactory _dbFactory;
        readonly ITestModelRepository _testModelRepository;

        public TestModelDomainService(
            IDbFactory dbFactory,
            ITestModelRepository testModelRepository)
        {
            _dbFactory = dbFactory;
            _testModelRepository = testModelRepository;
        }

        public IEnumerable<TestModel> ComplexBll()
        {
            var data = _testModelRepository.GetList();
            foreach(var item in data)
            {
                item.Name = item.Name + "-Service";
            }

            return data;
        }

        //public IEnumerable<TestModel> ComplexBll()
        //{
        //    using (var uow = _dbFactory.Create<IUnitOfWork, ISession>(IsolationLevel.RepeatableRead))
        //    {
        //        var data = _testModelRepository.GetList(uow);
        //        foreach (var item in data)
        //        {
        //            item.Name = item.Name + "-Service";
        //        }

        //        return data;
        //    }
        //}
    }
}
