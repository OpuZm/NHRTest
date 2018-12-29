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
    public class UserRepository : MultiDbRepository<UserModel, int>, IUserRepository
    {
        public UserRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByUserCodeAndGroupIdSql = @"SELECT * FROM Users WHERE UserCode = @UserCode AND GroupId = @GroupId";
        

        public UserModel GetByUserCodeAndGroupId(string userCode, int groupId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = session.QueryFirstOrDefault<UserModel>(GetByUserCodeAndGroupIdSql, new UserModel { UserCode = userCode, GroupId = groupId });
                return result;
            }
        }

        public async Task<UserModel> GetByUserCodeAndGroupIdAsync(string userCode, int groupId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<UserModel>(GetByUserCodeAndGroupIdSql, new UserModel { UserCode = userCode, GroupId = groupId });
                return model;
            }
        }

        public async Task<bool> AddModel(UserModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);
            return result > 0;
        }

        public async Task<bool> UpdateModel(UserModel model, IUnitOfWork uow = null)
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
