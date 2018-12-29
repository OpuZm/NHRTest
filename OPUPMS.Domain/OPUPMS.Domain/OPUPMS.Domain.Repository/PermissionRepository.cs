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
    public class PermissionRepository : MultiDbRepository<PermissionModel, int>, IPermissionRepository
    {


        public PermissionRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }
        private readonly string GetPermissionAllSql = @"SELECT * FROM dbo.Permissions";
        private readonly string GetPermissionByCodeSql = @"SELECT * FROM dbo.Permissions WHERE Code=@Code";
        private readonly string GetPermissionByParentIdSql = @"SELECT * FROM dbo.Permissions WHERE ParentId=@ParentId";
        public async Task<bool> AddPermission(PermissionModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<List<PermissionModel>> GetPermissionAll()
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<PermissionModel>(GetPermissionAllSql);
                var list = result.ToList();
                return list;

            }
        }

        public PermissionModel GetPermissionByCode(string code)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<PermissionModel>(GetPermissionByCodeSql, new PermissionModel { Code = code });
                return model;

            }
        }

        public async Task<List<PermissionModel>> GetPermissionByParentId(int parentId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<PermissionModel>(GetPermissionByParentIdSql, new PermissionModel { ParentId = parentId });
                var list = result.ToList();
                return list;

            }
        }

        public async Task<bool> UpdatePermission(PermissionModel model, IUnitOfWork uow = null)
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
