using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Repositories;
using Xunit;

namespace OPUPMS.Tests
{
    public class TestModelRepositroyTests : TestBase<ITestModelRepository>
    {
        [Fact]
        public void GetListTest()
        {
            var rep = Get();
            var datas = rep.GetList();
            Assert.NotNull(datas);
            Assert.True(datas.Count() == 4);
            Assert.True(datas.ToList()[0].Name == "Test-1");
        }

        [Fact]
        public void TestModelMappingTest()
        {
            var model = new OPUPMS.Domain.Base.Models.TestModel();
            var sql = model.GetSql();
            Assert.Contains("ii", sql);
        }
    }
}