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
    public class UserRoleRepository : MultiDbRepository<UserRoleModel, int>, IUserRoleRepository
    {
        public UserRoleRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByIdSql = @"SELECT * FROM UserRoles WHERE 1=1 ";

        public async Task<bool> DelModel(int id, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
                result = await DeleteKeyAsync<ISession>(id);
            else
                result = await DeleteKeyAsync(id, uow);

            return result;
        }
        
        public async Task<bool> SaveModel(UserRoleModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }

        public async Task<List<UserRoleModel>> GetList(int roleId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<UserRoleModel>(GetByIdSql, new { RoleId = roleId });
                
                return result.ToList();
            }
        }

        public async Task<List<UserRoleModel>> GetList(int? roleId, int? userId)
        {
            using (var session = Factory.Create<ISession>())
            {
                string whereRoleSql = "AND RoleId = @RoleId ";
                string whereUserSql = "AND UserId = @UserId ";
                string sql = "";
                IEnumerable<UserRoleModel> result = null;
                if (roleId.HasValue && userId.HasValue)
                {
                    sql = GetByIdSql + whereRoleSql + whereRoleSql;
                    result = await session.QueryAsync<UserRoleModel>(sql, new { RoleId = roleId, UserId = userId });
                }
                else if (roleId.HasValue && !userId.HasValue)
                {
                    sql = GetByIdSql + whereRoleSql;
                    result = await session.QueryAsync<UserRoleModel>(sql, new { RoleId = roleId });
                }
                else if (!roleId.HasValue && userId.HasValue)
                {
                    sql = GetByIdSql + whereUserSql;
                    result = await session.QueryAsync<UserRoleModel>(sql, new { UserId = userId });
                }
                else
                    throw new Exception("无效参数");

                return result.ToList();
            }
        }
    }
}
