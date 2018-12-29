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
    public class CompanyRepository : MultiDbRepository<CompanyModel, int>, ICompanyRepository
    {
        public CompanyRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByCompanyCodeSql = @"SELECT * FROM Company WHERE Code = @Code AND GroupId = @GroupId";
        protected static readonly string GetByCompanyIdSql = @"SELECT * FROM Company WHERE Id = @GroupId";

        
        public CompanyModel GetByCompanyCode(string companyCode, int groupId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<CompanyModel>(GetByCompanyCodeSql, new CompanyModel { Code = companyCode });
                return model;
            }
        }

        public async Task<CompanyModel> GetByCompanyAsync(string companyCode, int groupId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<CompanyModel>(GetByCompanyCodeSql, new CompanyModel { Code = companyCode, GroupId = groupId });
                return model;
            }
        }

        public async Task<CompanyModel> GetByCompanyAsync(int companyId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<CompanyModel>(GetByCompanyIdSql, new CompanyModel { Id = companyId });
                return model;
            }
        }

        public async Task<bool> SaveModel(CompanyModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);
            return result > 0;
        }

        public async Task<bool> UpdateModel(CompanyModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);
            return result > 0;
        }
        
    }
}
