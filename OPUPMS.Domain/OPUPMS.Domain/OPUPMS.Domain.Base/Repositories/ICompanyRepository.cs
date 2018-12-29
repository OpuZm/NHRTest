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
    /// 公司信息接口
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// 根据公司代码获取公司信息
        /// </summary>
        /// <param name="companyCode">公司代码</param>
        /// <returns></returns>
        CompanyModel GetByCompanyCode(string companyCode, int groupId);

        /// <summary>
        /// 根据公司代码获取公司信息
        /// </summary>
        /// <param name="companyCode">公司代码</param>
        /// <returns>异步返回公司信息</returns>
        Task<CompanyModel> GetByCompanyAsync(string companyCode, int groupId);

        /// <summary>
        /// 根据公司ID获取公司信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>异步返回公司信息</returns>
        Task<CompanyModel> GetByCompanyAsync(int companyId);

        /// <summary>
        /// 异步添加一个公司信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> SaveModel(CompanyModel model, IUnitOfWork uow = null);

        /// <summary>
        /// 异步更新一个公司信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uow">事务参数（工作单元）,默认null 不使用事务</param>
        /// <returns></returns>
        Task<bool> UpdateModel(CompanyModel model, IUnitOfWork uow = null);
    }
}
