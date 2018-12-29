using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Repository
{
    public class AreaCodeRepository : MultiDbRepository<AreaCodeModel, int>, IAreaCodeRepository
    {
        public AreaCodeRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByAreaCodeSql = @"SELECT * FROM AreaCodes WHERE Code = @Code";
        protected static readonly string GetByAreaCodeIdSql = @"SELECT * FROM AreaCodes WHERE Id = @Id";

        
        public AreaCodeModel GetByAreaCode(string areaCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<AreaCodeModel>(GetByAreaCodeSql, new AreaCodeModel { Code = areaCode });
                return model;
            }
        }

        public async Task<AreaCodeModel> GetByAreaCodeAsync(string areaCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<AreaCodeModel>(GetByAreaCodeSql, new AreaCodeModel { Code = areaCode });
                return model;
            }
        }

        public async Task<AreaCodeModel> GetByAreaCodeIdAsync(int areaCodeId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<AreaCodeModel>(GetByAreaCodeIdSql, new AreaCodeModel { Id = areaCodeId });
                return model;
            }
        }

        public async Task<bool> AddNewAreaCode(AreaCodeModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<bool> UpdateAreaCode(AreaCodeModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow); ;
            return result > 0;
        }
    }
}
