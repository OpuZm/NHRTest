using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Repository;
using OPUPMS.Domain.Services;
using Xunit;
using OPUPMS.Domain.Base.Services;

namespace OPUPMS.Tests
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
            //Assert.All(datas, data => data.Name.Contains("Service"));
        }
    }
}