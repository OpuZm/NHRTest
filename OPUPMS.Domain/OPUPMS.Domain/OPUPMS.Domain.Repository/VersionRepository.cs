using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Dapper;
using Smooth.IoC.UnitOfWork;
using Dapper;

namespace OPUPMS.Domain.Repository
{
    public class VersionRepository : MultiDbRepository<VersionModel, int>, IVersionRepository
    {
        private readonly string GetVersionByCodeVersionSql = @"SELECT * FROM dbo.Versions WHERE CodeVersion=@CodeVersion";
        private readonly string GetVersionAllSql = @"SELECT * FROM dbo.Versions";

        public VersionRepository(IMultiDbDbFactory factory) : base(factory) { }

        public async Task<bool> AddVersionsAsync(VersionModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }
        

        public async Task<List<VersionModel>> GetVersionAllAsync()
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<VersionModel>(GetVersionAllSql);
                var list = result.ToList();
                return list;

            }
        }

        public VersionModel GetVersionByCodeVersion(string codeVersion)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<VersionModel>(GetVersionByCodeVersionSql, new VersionModel { CodeVersion = codeVersion });
                return model;

            }
        }

        public async Task<bool> UpdateVersionsAsync(VersionModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
            {
                result = await SaveOrUpdateAsync<ISession>(model);

            }
            else
            {
                result = await SaveOrUpdateAsync(model, uow);
            }
            return result > 0;
        }
    }
}
