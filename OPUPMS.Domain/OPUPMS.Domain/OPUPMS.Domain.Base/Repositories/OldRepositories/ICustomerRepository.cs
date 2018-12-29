using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.ConvertModels;

namespace OPUPMS.Domain.Base.Repositories.OldRepositories
{
    /// <summary>
    /// 客户信息接口
    /// </summary>
    public interface ICustomerRepository : IMultiDbRepository<LxdmModel, int>
    {
        /// <summary>
        /// 获取客户代码信息
        /// </summary>
        /// <param name="status">lxdmgzbz包含{X,Y,Z}状态；status为空时默认状态包含{X,Y}</param>
        /// <returns>返回客户代码列表</returns>
        List<TypeCodeInfo> GetListByStatus(string status);

        /// <summary>
        /// 获取客户代码信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回客户代码信息</returns>
        TypeCodeInfo GetCustomerInfoById(int id);
        List<TypeCodeInfoSimple> GetListByStatusSimple(string status);
    }
}
