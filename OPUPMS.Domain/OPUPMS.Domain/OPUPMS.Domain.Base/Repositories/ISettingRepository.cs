using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Base.Models;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Base.Repositories
{

    /// <summary>
    ///系统属性 
    /// </summary>
    public interface ISettingRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddSettingsAsync(SettingModel model);
       
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        Task<bool> UpdateSettingsAsync(SettingModel model, IUnitOfWork uow = null);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="SettingId"></param>
        /// <returns></returns>
        Task<bool> DeleteSettingsBySettingIdAsync(int settingId,IUnitOfWork uow=null);


        /// <summary>
        /// 根据属性Id查找实体
        /// </summary>
        /// <param name="SettingId"></param>
        /// <returns></returns>
        SettingModel GetSettingBySettingId(int settingId);

        /// <summary>
        /// 根据ID查找实体
        /// </summary>
        /// <param name="SettingId"></param>
        /// <returns></returns>
        SettingModel GetSettingByID(int id);
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        Task<List<SettingModel>> GetAll();
    }
}
