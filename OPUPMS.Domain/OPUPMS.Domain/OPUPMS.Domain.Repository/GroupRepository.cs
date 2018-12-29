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
    public class GroupRepository : MultiDbRepository<GroupModel, int>, IGroupRepository
    {
        public GroupRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetAllSql = @"SELECT * FROM Groups";
        protected static readonly string GetByGroupCodeSql = @"SELECT * FROM Groups WHERE Code = @Code";
        protected static readonly string GetByGroupIdSql = @"SELECT * FROM Groups WHERE Id = @Id";

        
        public GroupModel GetByGroupCode(string groupCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<GroupModel>(GetByGroupCodeSql, new GroupModel { Code = groupCode });
                return model;
            }
        }

        public async Task<GroupModel> GetByGroupAsync(string groupCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<GroupModel>(GetByGroupCodeSql, new GroupModel { Code = groupCode });
                return model;
            }
        }

        public async Task<GroupModel> GetByGroupAsync(int groupId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<GroupModel>(GetByGroupIdSql, new GroupModel { Id = groupId });
                return model;
            }
        }

        public async Task<bool> AddModel(GroupModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);
            return result > 0;
        }

        public async Task<bool> UpdateModel(GroupModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);
            return result > 0;
        }

        public async Task<List<GroupModel>> GetAllList()
        {
            using (var session = Factory.Create<ISession>())
            {
                var list = await session.QueryAsync<GroupModel>(GetAllSql);
                return list.ToList();
            }
        }
    }
}
