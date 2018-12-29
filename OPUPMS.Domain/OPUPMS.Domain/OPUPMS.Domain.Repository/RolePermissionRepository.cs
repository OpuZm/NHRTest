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
    public class RolePermissionRepository : MultiDbRepository<RolePermissionModel, int>, IRolePermissionRepository
    {

        public RolePermissionRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        private readonly string GetRolePermissionAllSql = @"SELECT * FROM dbo.RolePermissions";
        private readonly string GetRolePermissionByIDSql = @"SELECT * FROM dbo.RolePermissions WHERE Id=@Id";
        private readonly string GetRolePermissionByPermissionIdSql = @"SELECT * FROM dbo.RolePermissions WHERE PermissionId=@PermissionId";
        private readonly string GetRolePermissionByRoleIdSql = @"SELECT * FROM dbo.RolePermissions WHERE RoleId=@RoleId";

        public async Task<bool> AddRolePermission(RolePermissionModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<bool> DeleteRolePermissionById(int id, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
            {
                result = await DeleteKeyAsync<ISession>(id);
            }
            else
            {
                result = await DeleteKeyAsync(id, uow);
            }
            return result;

        }

        public async Task<List<RolePermissionModel>> GetRolePermissionAll()
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<RolePermissionModel>(GetRolePermissionAllSql);
                var list = result.ToList();
                return list;
            }
        }

        public RolePermissionModel GetRolePermissionByID(int id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirst<RolePermissionModel>(GetRolePermissionByIDSql, new RolePermissionModel { Id = id });

                return model;
            }
        }

        public async Task<List<RolePermissionModel>> GetRolePermissionByPermissionId(int permissionId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<RolePermissionModel>(GetRolePermissionByPermissionIdSql, new RolePermissionModel { PermissionId = permissionId });
                var list = result.ToList();
                return list;
            }
        }

        public async Task<List<RolePermissionModel>> GetRolePermissionByRoleId(int roleId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<RolePermissionModel>(GetRolePermissionByRoleIdSql, new RolePermissionModel { RoleId = roleId });
                var list = result.ToList();
                return list;
            }
        }

        public async Task<bool> UpdateRolePermission(RolePermissionModel model, IUnitOfWork uow = null)
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
