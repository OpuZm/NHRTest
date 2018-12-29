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
    /// 系统代码信息接口
    /// </summary>
    public interface ISystemCodeRepository : IMultiDbRepository<XtdmModel, string>
    {
        /// <summary>
        /// 获取系统代码信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回系统代码列表</returns>
        List<SystemCodeInfo> GetCodeListByMutliTypes(string token, string[] paras);

        /// <summary>
        /// 异步获取系统代码信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>异步返回系统代码列表</returns>
        Task<List<SystemCodeInfo>> GetCodeListByMutliTypesAsync(string token, string[] paras);
    }
}
