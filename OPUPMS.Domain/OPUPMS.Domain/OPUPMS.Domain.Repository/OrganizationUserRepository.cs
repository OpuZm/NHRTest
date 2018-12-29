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
    public class OrganizationUserRepository : MultiDbRepository<OrganizationUserModel, int>, IOrganizationUserRepository
    {
        public OrganizationUserRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByGroupIdSql = @"SELECT * FROM OrganizationUsers WHERE GroupId = @GroupId";

        public async Task<bool> DelModel(OrganizationUserModel model, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
                result = await DeleteAsync<ISession>(model);
            else
                result = await DeleteAsync(model, uow);

            return result;
        }

        public async Task<List<OrganizationUserModel>> GetList(int groupId, int userId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<OrganizationUserModel>(GetByGroupIdSql, new { GroupId = groupId });

                if (userId > 0)
                    result = result.Where(x => x.UserId == userId);

                return result.ToList();
            }
        }

        public async Task<bool> SaveModel(OrganizationUserModel model, IUnitOfWork uow = null)
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
