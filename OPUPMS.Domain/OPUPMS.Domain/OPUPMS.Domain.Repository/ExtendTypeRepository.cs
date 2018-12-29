using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Repository
{
    public class ExtendTypeRepository : MultiDbRepository<ExtendTypeModel, int>, IExtendTypeRepository
    {
        public ExtendTypeRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByIdSql = @"SELECT * FROM ExtendTypes WHERE Id = @Id ";
        protected static readonly string GetByCodeSql = @"SELECT * FROM ExtendTypes WHERE Code = @Code ";
        protected static readonly string GetAllSql = @"SELECT * FROM ExtendTypes  ";


        public async Task<bool> SaveModelAsync(ExtendTypeModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }
        
        public async Task<ExtendTypeModel> GetModelAsync(int Id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<ExtendTypeModel>(GetByIdSql, new { Id = Id });
                return result.FirstOrDefault();
            }
        }

        public async Task<ExtendTypeModel> GetModelAsync(string code, bool? status = null)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<ExtendTypeModel>(GetByCodeSql, new { Code = code });
                if (status.HasValue)
                    result = result.Where(x => x.Status == status.Value);

                return result.FirstOrDefault();
            }
        }

        public async Task<List<ExtendTypeModel>> GetAllAsync()
        {
            using (var session = Factory.Create<ISession>())
            {
                var result =  session.Query<ExtendTypeModel>(GetAllSql);
                var list = result.ToList();
                return list;
            }
        }
        
        public bool SaveModel(ExtendTypeModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = SaveOrUpdate<ISession>(model);
            else
                result = SaveOrUpdate(model, uow);

            return result > 0;
        }

        public ExtendTypeModel GetModel(int Id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = session.Query<ExtendTypeModel>(GetByIdSql, new { Id = Id });
                return result.FirstOrDefault();
            }
        }
    }
}
