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
    /// 数据字典类型信息接口
    /// </summary>
    public interface IDictionaryTypeRepository
    {
        /// <summary>
        /// 保存数据字典类型信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> SaveModel(DictionaryTypeModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 获取数据字典类型信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <returns></returns>
        Task<DictionaryTypeModel> GetModel(int Id);

        /// <summary>
        /// 根据类型代码 获取数据字典类型信息
        /// </summary>
        /// <param name="typeCode">类型代码</param>
        /// <returns></returns>
        Task<DictionaryTypeModel> GetModel(string typeCode, int? status = null);
    }
}
