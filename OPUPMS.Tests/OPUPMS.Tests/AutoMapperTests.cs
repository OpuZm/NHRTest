using AutoMapper;
using AutoMapper.Attributes;
using OPUPMS.Web.Framework.Core.ObjectMapping;
using Shouldly;
using Xunit;

namespace OPUPMS.Tests
{

    public class AModel
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
    }

    public class BModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Attribute 方式，参考 https://github.com/schneidenbach/AutoMapper.Attributes
    /// </summary>
    [MapsTo(typeof(BModel))]
    public class CModel
    {
        [MapsToProperty(typeof(BModel), nameof(BModel.ID))]
        public int OtherId { get; set; }

        [MapsToProperty(typeof(BModel), nameof(BModel.Name))]
        public string OtherName { get; set; }
    }


    public class TestProfile : Profile
    {
        public TestProfile() : base()
        {
            CreateMap<AModel, BModel>().ForMember(
                dest => dest.ID, opt => opt.MapFrom(source => source.OrderId));
        }
    }

    public class AutoMapperTests : TestBase<IMapper>
    {
        [Fact]
        public void Test()
        {
            // 使用 IMapper 依赖注入 public TestController(IMapper mapper) ...
            var mapper = base.Get();

            Assert.NotNull(mapper);

            var amodel = new AModel
            {
                OrderId = 100,
                Name = "AModel"
            };

            var bmodel = mapper.Map<BModel>(amodel);

            bmodel.ID.ShouldBe(amodel.OrderId);
            bmodel.Name.ShouldBe(amodel.Name);

            var cmodel = new CModel
            {
                OtherId = 1000,
                OtherName = "CModel"
            };

            bmodel = Mapper.Map<BModel>(cmodel); //使用 AutoMapper
            bmodel.ID.ShouldBe(cmodel.OtherId);
            bmodel.Name.ShouldBe(cmodel.OtherName);

            var cmodel1 = new CModel
            {
                OtherId = 100000,
                OtherName = "CModel-1"
            };

            bmodel = cmodel1.MapTo<BModel>(); //使用扩展方法
            bmodel.ID.ShouldBe(cmodel1.OtherId);
            bmodel.Name.ShouldBe(cmodel1.OtherName);
        }
    }
}
