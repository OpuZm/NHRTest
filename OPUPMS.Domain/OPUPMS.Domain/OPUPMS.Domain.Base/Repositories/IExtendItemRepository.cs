using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Domain.Base.Dtos;

namespace OPUPMS.Domain.Base.Repositories
{
    /// <summary>
    /// 扩展子项 信息接口
    /// </summary>
    public interface IExtendItemRepository
    {
        /// <summary>
        /// 根据Id 异步获取指定扩展子项信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> AddModelAsync(ExtendItemModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 异步删除指定扩展子项的信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> DelModelAsync(int id, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 异步获取指定扩展子项信息
        /// </summary>
        /// <param name="id">类型Id</param>
        /// <returns></returns>
        Task<ExtendItemModel> GetModelAsync(int id);

        /// <summary>
        /// 根据公司与类型 异步获取其关联的扩展子项列表信息
        /// </summary>
        /// <param name="companyId">公司</param>
        /// <param name="typeId">类型ID</param>
        /// <returns></returns>
        Task<List<ExtendItemModel>> GetModelListAsync(int companyId, int typeId);

        /// <summary>
        /// 根据Id 异步获取指定扩展子项信息
        /// </summary>
        /// <param name="Id">类型Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        bool AddModel(ExtendItemModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 异步删除指定扩展子项的信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        bool DelModel(int id, IUnitOfWork uow = null);

        /// <summary>
        /// 根据Id 异步获取指定扩展子项信息
        /// </summary>
        /// <param name="id">类型Id</param>
        /// <returns></returns>
        ExtendItemModel GetModel(int id);
        
        /// <summary>
        /// 根据公司与类型 获取其关联的扩展子项列表信息
        /// </summary>
        /// <param name="companyId">公司</param>
        /// <param name="typeId">类型ID</param>
        /// <returns></returns>
        List<ExtendItemDto> GetModelList(int companyId, int typeId);
        bool UpdateItemValue(int companyId, int typeId, string itemValue);
        bool UpdateXtcs(string xtcsdm, DateTime xtcsrq);
    }
}
