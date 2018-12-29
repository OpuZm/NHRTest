using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Repository
{
    public class ExtendItemSettingRepository : MultiDbRepository<ExtendItemSettingModel, int>, IExtendItemSettingRepository
    {
        public ExtendItemSettingRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }
        
        protected static readonly string GetByComanyTypeSql = @"SELECT * FROM ExtendItemSettings WHERE SettingId = @SettingId AND ExtendItemId = @ExtendItemId ";
        

        public async Task<bool> AddModel(ExtendItemSettingModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow); ;
            return result > 0;
        }

        public async Task<bool> DelModel(int id, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
                result = await DeleteKeyAsync<ISession>(id);
            else
                result = await DeleteKeyAsync(id, uow); ;
            return result;
        }        

        public async Task<List<ExtendItemSettingModel>> GetModelList(int extendItemId, int settingId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<ExtendItemSettingModel>(GetByComanyTypeSql, new ExtendItemSettingModel { ExtendItemId = extendItemId, SettingId = settingId });

                return result.ToList();
            }
        }
    }
}
