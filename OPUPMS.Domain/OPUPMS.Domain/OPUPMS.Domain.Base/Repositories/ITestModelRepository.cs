using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Infrastructure.Dapper;

namespace OPUPMS.Domain.Base.Repositories
{
    public interface ITestModelRepository : IRepositoryEx<TestModel, int>
    {
        IEnumerable<TestModel> GetList();
    }
}
