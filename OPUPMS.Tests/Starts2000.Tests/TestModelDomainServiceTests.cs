using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starts2000.Domain.Repositories;
using Starts2000.Domain.Services;
using Xunit;

namespace Starts2000.Tests
{
    public class TestModelDomainServiceTests : TestBase<ITestModelDomainService>
    {
        [Fact]
        public void GetListTest()
        {
            var service = Get();
            var datas = service.ComplexBll();

            Assert.NotNull(datas);
            Assert.True(datas.Count() == 4);
            Assert.All(datas, data => data.Name.Contains("Service"));
        }
    }
}