using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starts2000.Domain.Repositories;
using Xunit;

namespace Starts2000.Tests
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
            var model = new Starts2000.Domain.Models.TestModel();
            var sql = model.GetSql();
            Assert.Contains("ii", sql);
        }
    }
}