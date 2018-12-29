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
    /// 扩展子项 信息接口
    /// </summary>
    public interface IExtendItemSettingRepository
    {
        /// <summary>
        /// 根据Id 删除指定扩展子项属性的关联信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> DelModel(int id, IUnitOfWork uow = null);

        /// <summary>
        /// 新增一个子项属性关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> AddModel(ExtendItemSettingModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 根据公司与类型 获取其关联的扩展子项属性列表信息
        /// </summary>
        /// <param name="extendItemId">扩展项Id</param>
        /// <param name="settingId">属性ID</param>
        /// <returns></returns>
        Task<List<ExtendItemSettingModel>> GetModelList(int extendItemId, int settingId);
    }
}
