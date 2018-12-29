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
    public class DepartmentUserRepository : MultiDbRepository<DepartmentUserModel, int>, IDepartmentUserRepository
    {
        public DepartmentUserRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByDepartmentUserIdSql = @"SELECT * FROM DepartmentUsers WHERE Id = @Id";
        protected static readonly string GetByDepartmentIdSql = @"SELECT * FROM DepartmentUsers WHERE DepartmentId = @DepartmentId";
        protected static readonly string GetByUserIdSql = @"SELECT * FROM DepartmentUsers WHERE UserId = @UserId";


        public DepartmentUserModel GetByDepartmentId(int departmentId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<DepartmentUserModel>(GetByDepartmentIdSql, new DepartmentUserModel { DepartmentId = departmentId });
                return model;
            }
        }

        public async Task<DepartmentUserModel> GetByDepartmentIdAsync(int departmentId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<DepartmentUserModel>(GetByDepartmentIdSql, new DepartmentUserModel { DepartmentId = departmentId });
                return model;
            }
        }

        public DepartmentUserModel GetByUserId(int userId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<DepartmentUserModel>(GetByUserIdSql, new DepartmentUserModel { UserId = userId });
                return model;
            }
        }

        public async Task<DepartmentUserModel> GetByUserIdAsync(int userId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<DepartmentUserModel>(GetByUserIdSql, new DepartmentUserModel { UserId = userId });
                return model;
            }
        }

        public async Task<DepartmentUserModel> GetByDepartmentUserIdAsync(int departmentUserId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<DepartmentUserModel>(GetByDepartmentUserIdSql, new DepartmentUserModel { Id = departmentUserId });
                return model;
            }
        }

        public async Task<bool> AddNewDepartmentUser(DepartmentUserModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<bool> UpdateDepartmentUser(DepartmentUserModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow); ;
            return result > 0;
        }
    }
}
