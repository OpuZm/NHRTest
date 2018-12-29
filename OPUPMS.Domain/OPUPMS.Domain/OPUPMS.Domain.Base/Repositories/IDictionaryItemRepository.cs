using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Base.Repositories
{
    /// <summary>
    /// 数据字典项信息接口
    /// </summary>
    public interface IDictionaryItemRepository
    {
        /// <summary>
        /// 根据Id 获取指定数据字典项信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <returns></returns>
        Task<DictionaryItemModel> GetModel(int Id);

        /// <summary>
        /// 根据类型代码 获取数据字典子项信息
        /// </summary>
        /// <param name="typeCode">类型代码</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task<List<DictionaryItemModel>> GetModelList(string typeCode, bool? enabled = null);
    }
}
