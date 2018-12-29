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
    public class AreaLogRepository : MultiDbRepository<AreaLogModel, int>, IAreaLogRepository
    {
        public AreaLogRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByAreaLogIdSql = @"SELECT * FROM AreaLogs WHERE Id = @Id";

        public async Task<AreaLogModel> GetByAreaLogIdAsync(int areaLogId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<AreaLogModel>(GetByAreaLogIdSql, new AreaLogModel { Id = areaLogId });
                return model;
            }
        }

        public async Task<bool> AddNewAreaLog(AreaLogModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<bool> UpdateAreaLog(AreaLogModel model, IUnitOfWork uow = null)
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
