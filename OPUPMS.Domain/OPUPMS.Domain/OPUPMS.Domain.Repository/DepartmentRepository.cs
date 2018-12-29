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
    public class DepartmentRepository : MultiDbRepository<DepartmentModel, int>, IDepartmentRepository
    {
        public DepartmentRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByDepartmentCodeSql = @"SELECT * FROM Departments WHERE Code = @Code";
        protected static readonly string GetByDepartmentIdSql = @"SELECT * FROM Departments WHERE Id = @Id";

        
        public DepartmentModel GetByDepartmentCode(string departmentCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = session.QueryFirstOrDefault<DepartmentModel>(GetByDepartmentCodeSql, new DepartmentModel { Code = departmentCode });
                return model;
            }
        }

        public async Task<DepartmentModel> GetByDepartmentCodeAsync(string departmentCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<DepartmentModel>(GetByDepartmentCodeSql, new DepartmentModel { Code = departmentCode });
                return model;
            }
        }

        public async Task<DepartmentModel> GetByDepartmentIdAsync(int departmentId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<DepartmentModel>(GetByDepartmentIdSql, new DepartmentModel { Id = departmentId });
                return model;
            }
        }

        public async Task<bool> AddNewDepartment(DepartmentModel model)
        {
            var result = await SaveOrUpdateAsync<ISession>(model);
            return result > 0;
        }

        public async Task<bool> UpdateDepartment(DepartmentModel model, IUnitOfWork uow = null)
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
