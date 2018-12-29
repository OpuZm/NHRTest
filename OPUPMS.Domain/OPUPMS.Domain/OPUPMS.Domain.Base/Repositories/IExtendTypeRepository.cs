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
    /// 扩展类型信息接口
    /// </summary>
    public interface IExtendTypeRepository
    {
        /// <summary>
        /// 异步保存扩展类型信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> SaveModelAsync(ExtendTypeModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 异步获取扩展类型信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <returns></returns>
        Task<ExtendTypeModel> GetModelAsync(int Id);

        /// <summary>
        /// 根据类型代码 异步获取数据字典类型信息
        /// </summary>
        /// <param name="typeCode">类型代码</param>
        /// <returns></returns>
        Task<ExtendTypeModel> GetModelAsync(string code, bool? status = null);


        Task<List<ExtendTypeModel>> GetAllAsync();


        /// <summary>
        /// 保存扩展类型信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        bool SaveModel(ExtendTypeModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 获取扩展类型信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <returns></returns>
        ExtendTypeModel GetModel(int Id);
    }
}
