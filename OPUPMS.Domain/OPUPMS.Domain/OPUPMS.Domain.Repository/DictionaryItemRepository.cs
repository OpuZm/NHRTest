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
    public class DictionaryItemRepository : MultiDbRepository<DictionaryItemModel, int>, IDictionaryItemRepository
    {
        public DictionaryItemRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByIdSql = @"SELECT * FROM DictionaryItems WHERE Id = @Id ";
        protected static readonly string GetByTypeCodeSql = @"SELECT * FROM DictionaryItems WHERE TypeCode = @TypeCode ";
        
        
        public async Task<bool> SaveModel(DictionaryItemModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }
        
        public async Task<DictionaryItemModel> GetModel(int Id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<DictionaryItemModel>(GetByIdSql, new { Id = Id });
                return result.FirstOrDefault();
            }
        }

        public async Task<List<DictionaryItemModel>> GetModelList(string typeCode, bool? status = null)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<DictionaryItemModel>(GetByTypeCodeSql, new { TypeCode = typeCode });
                if (status.HasValue)
                    result = result.Where(x => x.Enabled == status.Value);

                return result.ToList();
            }
        }
    }
}
