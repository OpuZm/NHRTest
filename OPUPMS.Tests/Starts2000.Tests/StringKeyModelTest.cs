using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper.FastCrud;
using Shouldly;
using Smooth.IoC.UnitOfWork;
using Starts2000.Infrastructure.Dapper;
using Xunit;

namespace Starts2000.Tests
{
    [Table("StringIdTable")]
    public partial class StringIdTable : IEntity<string>
    {
        [Key]
        [Column("StrId")]
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface IStringIdTableRespository : IMultiDbRepository<StringIdTable, string>
    {

    }

    public class StringIdTableRespository : MultiDbRepository<StringIdTable, string>, IStringIdTableRespository
    {
        public StringIdTableRespository(IMultiDbDbFactory factory) : base(factory)
        {
        }
    }

    public class StringKeyModelTest : TestBase<IStringIdTableRespository>
    {
        public StringKeyModelTest()
        {
            base.Kernel.Bind<IStringIdTableRespository>().To<StringIdTableRespository>();
        }

        [Fact]
        public void Count()
        {
            var respository = Get();
            respository.Count<ISession>(null).ShouldBe(2);
        }

        [Fact]
        public async void CountAsync()
        {
            var respository = Get();
            (await respository.CountAsync<ISession>(null)).ShouldBe(2);
        }

        [Fact]
        public void GetAll()
        {
            var respository = Get();
            var list = respository.GetAll<ISession>(null);
            Assert.True(list.Count() == 2);
            Assert.True(list.ToList()[0].Id == "asdasd");
        }

        [Fact]
        public async void GetAllAsync()
        {
            var respository = Get();
            var list = await respository.GetAllAsync<ISession>(null);

            Assert.True(list.Count() == 2);
            Assert.True(list.ToList()[0].Id == "asdasd");
        }

        [Fact]
        public void GetKey()
        {
            var respository = Get();
            var row = respository.GetKey<ISession>(null, "asdasd");
            Assert.True(row.Id == "asdasd" && row.Name == "1");
        }

        [Fact]
        public async void GetKeyAsync()
        {
            var respository = Get();
            var row = await respository.GetKeyAsync<ISession>(null, "asdasd").ConfigureAwait(false);
            Assert.True(row.Id == "asdasd" && row.Name == "1");
        }

        [Fact]
        public void DeleteKey()
        {
            var respository = Get();
            respository.DeleteKey<ISession>(null, "asdasd").ShouldBe(true);
            respository.DeleteKey<ISession>(null, "").ShouldBe(false);
            respository.Add<ISession>(null, new StringIdTable
            {
                Id = "asdasd",
                Name = "1"
            });
        }

        [Fact]
        public async void DeleteKeyAsync()
        {
            var respository = Get();
            (await respository.DeleteKeyAsync<ISession>(null, "asdasd")).ShouldBe(true);
            (await respository.DeleteKeyAsync<ISession>(null, "")).ShouldBe(false);
            respository.Add<ISession>(null, new StringIdTable
            {
                Id = "asdasd",
                Name = "1"
            });
        }

        [Fact]
        public void Delete()
        {
            var respository = Get();
            respository.Delete<ISession>(null, new StringIdTable { Id = "asdasd" }).ShouldBe(true);
            respository.Delete<ISession>(null, new StringIdTable { Id = "" }).ShouldBe(false);
            respository.Add<ISession>(null, new StringIdTable
            {
                Id = "asdasd",
                Name = "1"
            });
        }

        [Fact]
        public async void DeleteAsync()
        {
            var respository = Get();
            (await respository.DeleteAsync<ISession>(null, new StringIdTable { Id = "asdasd" })).ShouldBe(true);
            (await respository.DeleteAsync<ISession>(null, new StringIdTable { Id = "" })).ShouldBe(false);
            respository.Add<ISession>(null, new StringIdTable
            {
                Id = "asdasd",
                Name = "1"
            });
        }

        [Fact]
        public void Update()
        {
            var respository = Get();
            respository.SaveOrUpdate<ISession>(null, new StringIdTable { Id = "asdasd", Name = "3" }).ShouldBe("asdasd");
            respository.SaveOrUpdate<ISession>(null, new StringIdTable { Id = "asdasd", Name = "1" }).ShouldBe("asdasd");
        }

        [Fact]
        public async void UpdateAsync()
        {
            var respository = Get();
            (await respository.SaveOrUpdateAsync<ISession>(
                null, new StringIdTable { Id = "asdasd", Name = "3" })).ShouldBe("asdasd");
            (await respository.SaveOrUpdateAsync<ISession>(
                null, new StringIdTable { Id = "asdasd", Name = "1" })).ShouldBe("asdasd");
        }
    }
}
