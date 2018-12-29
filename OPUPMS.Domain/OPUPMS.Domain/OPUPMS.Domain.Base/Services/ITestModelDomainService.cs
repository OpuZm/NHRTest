using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;

namespace OPUPMS.Domain.Base.Services
{
    public interface ITestModelDomainService : IDomainService
    {
        IEnumerable<TestModel> ComplexBll();
    }
}
