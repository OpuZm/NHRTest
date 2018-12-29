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
    public class SettingRepository : MultiDbRepository<SettingModel, int>, ISettingRepository
    {
        private readonly string GetAllSql = @"SELECT * FROM dbo.Settings";
        private readonly string GetSettingBySettingIdSql = @"SELECT * FROM dbo.Settings WHERE SettingId=@SettingId";
        private readonly string GetSettingByIDSql = @"SELECT * FROM dbo.Settings WHERE Id=@Id";
        public SettingRepository(IMultiDbDbFactory factory) : base(factory) { }

        public async Task<bool> AddSettingsAsync(SettingModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<bool> DeleteSettingsBySettingIdAsync(int settingId, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
            {
                result = await DeleteKeyAsync<ISession>(settingId);

            }
            else
            {
                result = await DeleteKeyAsync(settingId, uow);
            }
            return result;
        }

        public async Task<List<SettingModel>> GetAll()
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<SettingModel>(GetAllSql);
                var list = result.ToList();
                return list;

            }
        }

        public SettingModel GetSettingByID(int id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<SettingModel>(GetSettingByIDSql, new SettingModel { Id = id });
                return model;

            }
        }

        public SettingModel GetSettingBySettingId(int settingId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<SettingModel>(GetSettingBySettingIdSql, new SettingModel { SettingId = settingId });
                return model;

            }
        }



        public async Task<bool> UpdateSettingsAsync(SettingModel model, IUnitOfWork uow = null)
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
