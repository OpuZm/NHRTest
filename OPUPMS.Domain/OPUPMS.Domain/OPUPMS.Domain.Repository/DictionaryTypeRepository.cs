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
    public class DictionaryTypeRepository : MultiDbRepository<DictionaryTypeModel, int>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByIdSql = @"SELECT * FROM DictionaryItems WHERE Id = @Id ";
        protected static readonly string GetByTypeCodeSql = @"SELECT * FROM DictionaryItems WHERE TypeCode = @TypeCode ";
        
        
        public async Task<bool> SaveModel(DictionaryTypeModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }
        
        public async Task<DictionaryTypeModel> GetModel(int Id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<DictionaryTypeModel>(GetByIdSql, new { Id = Id });
                return result.FirstOrDefault();
            }
        }

        public async Task<DictionaryTypeModel> GetModel(string typeCode, int? status = null)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<DictionaryTypeModel>(GetByTypeCodeSql, new { TypeCode = typeCode });
                if (status.HasValue)
                    result = result.Where(x => x.Status == status.Value);

                return result.FirstOrDefault();
            }
        }
    }
}
