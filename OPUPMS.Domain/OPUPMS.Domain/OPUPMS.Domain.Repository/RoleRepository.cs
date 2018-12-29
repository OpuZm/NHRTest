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
    public class RoleRepository : MultiDbRepository<RoleModel, int>, IRoleRepository
    {
        private readonly string GetRoleByIDSql = @"SELECT * FROM dbo.Roles WHERE Id=@Id";
        private readonly string GetRoleAllSql =@"SELECT * FROM dbo.Roles";


        public RoleRepository(IMultiDbDbFactory factory) : base(factory) { }

        public async Task<bool> AddRole(RoleModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;

        }

        public async Task<bool> DeleteRoleByIdAsync(int Id, IUnitOfWork uow = null)
        {
            bool result ;
            if (uow == null)
            {
                result = await DeleteKeyAsync<ISession>(Id);

            }
            else
            {
                result = await DeleteKeyAsync(Id, uow);
            }
            return result ;
        }

        public async Task<List<RoleModel>> GetRoleAll()
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<RoleModel>(GetRoleAllSql);
                var list = result.ToList();
                return list;

            }
        }

        public RoleModel GetRoleByID(int id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<RoleModel>(GetRoleByIDSql, new PermissionModel { Id = id });
                return model;

            }
        }

        public async Task<bool> UpdateRole(RoleModel model, IUnitOfWork uow = null)
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
