using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Repository.OldRepository;
using OPUPMS.Infrastructure.Dapper;

namespace OPUPMS.Domain.Hotel.Repository
{
    public class HotelUserRepository //: UserRepository_Old
    {
        //public HotelUserRepository(IMultiDbDbFactory factory) : base(factory)
        //{
        //}

        //public override UserInfo GetByUserName(string token, string userName)
        //{
        //    using (var session = Factory.Create<ISession>(token))
        //    {
        //        var result = session.QueryFirstOrDefault<CzdmModel>(GetByUserNameSql, new CzdmModel { Czdmmc00 = userName });

        //        return ConvertToInfo(result);
        //    }
        //}

        //public override async Task<UserInfo> GetByUserNameAsync(string token, string userName)
        //{
        //    using (var session = Factory.Create<ISession>(token))
        //    {
        //        var model = await session.QueryFirstOrDefaultAsync<CzdmModel>(GetByUserNameSql, new CzdmModel { Czdmmc00 = userName });

        //        return ConvertToInfo(model);
        //    }
        //}
    }
}
