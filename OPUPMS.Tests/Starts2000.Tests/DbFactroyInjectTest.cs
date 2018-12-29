using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smooth.IoC.UnitOfWork;
using Ninject;
using Xunit;
using Ninject.Parameters;
using Starts2000.Domain.Repository.InjectMoudles;
using Starts2000.Infrastructure.Dapper;

namespace Starts2000.Tests
{
    public class DbFactroyInjectTest : TestBase<IDbFactory>
    {
        public DbFactroyInjectTest()
        {
            Kernel.Bind<IConfigSettings>().To<ConfigSettings>();
        }


        [Fact]
        public void Test()
        {
            var factory = Get();
            var session = factory.Create<ISession>();
            var session1 = (factory as IMultiDbDbFactory).Create<ISession>("aaaa");
            //Assert.True(session.ConnectionString);

            var setting = Kernel.Get<IConfigSettings>(new ConstructorArgument("key", "key1"));
            Assert.Equal(setting.GetConnectionString(), "value1");
        }

        public interface IConfigSettings
        {
            string GetConnectionString();
        }

        public class ConfigSettings : IConfigSettings
        {
            readonly IDictionary<string, string> _dict = new Dictionary<string, string>
            {
                {"key1", "value1" },
                {"key2", "value2" },
                {"key3", "value3" }
            };

            readonly string _key;

            public ConfigSettings(string key)
            {
                _key = key;
            }

            public string GetConnectionString()
            {
                return _dict[_key];
            }
        }
    }
}
