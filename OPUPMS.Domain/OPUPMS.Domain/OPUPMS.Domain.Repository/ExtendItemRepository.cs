using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Repository
{
    public class ExtendItemRepository : MultiDbRepository<ExtendItemModel, int>, IExtendItemRepository
    {
        public ExtendItemRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }

        protected static readonly string GetByIdSql = @"SELECT * FROM ExtendItems WHERE Id = @Id ";
        protected static readonly string GetByCompanyTypeSql = @"
                            SELECT EI.*,ET.Name AS TypeName FROM dbo.ExtendItems EI 
                            INNER JOIN dbo.ExtendTypes ET ON ET.Id = EI.TypeId 
                            WHERE EI.CompanyId = @CompanyId AND EI.TypeId = @TypeId ";
        protected static readonly string UpdateSql = @"
            UPDATE ExtendItems SET ItemValue=@ItemValue WHERE CompanyId=@CompanyId AND TypeId=@TypeId";
        protected static readonly string UpdateXtcsSql = @"update xtcs set xtcsrq00=@xtcsrq00 where xtcsdm00=@xtcsdm00";
        
        #region 异步方法

        public async Task<bool> SaveModel(ExtendItemModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }

        public async Task<ExtendItemModel> GetModelAsync(int id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<ExtendItemModel>(GetByIdSql, new ExtendItemModel { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<List<ExtendItemModel>> GetModelListAsync(int companyId, int typeId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = await session.QueryAsync<ExtendItemModel>(GetByCompanyTypeSql, new ExtendItemModel { CompanyId = companyId, TypeId = typeId });

                return result.ToList();
            }
        } 
        #endregion


        #region 非异步方法

        public async Task<bool> AddModelAsync(ExtendItemModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(model);
            else
                result = await SaveOrUpdateAsync(model, uow); ;
            return result > 0;
        }

        public async Task<bool> DelModelAsync(int id, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
                result = await DeleteKeyAsync<ISession>(id);
            else
                result = await DeleteKeyAsync(id, uow); ;
            return result;
        }

        public bool AddModel(ExtendItemModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = SaveOrUpdate<ISession>(model);
            else
                result = SaveOrUpdate(model, uow); ;
            return result > 0;
        }

        public bool DelModel(int id, IUnitOfWork uow = null)
        {
            bool result;
            if (uow == null)
                result = DeleteKey<ISession>(id);
            else
                result = DeleteKey(id, uow); ;
            return result;
        }

        public ExtendItemModel GetModel(int id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = session.Query<ExtendItemModel>(GetByIdSql, new ExtendItemModel { Id = id });
                return result.FirstOrDefault();
            }
        }

        public List<ExtendItemDto> GetModelList(int companyId, int typeId)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = session.Query<ExtendItemDto>(GetByCompanyTypeSql, new ExtendItemDto { CompanyId = companyId, TypeId = typeId });

                return result.ToList();
            }
        } 

        public bool UpdateItemValue(int companyId,int typeId,string itemValue)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = session.Execute(UpdateSql, new ExtendItemDto { CompanyId = companyId, TypeId = typeId,ItemValue=itemValue });

                return result > 0 ? true : false;
            }
        }

        public bool UpdateXtcs(string xtcsdm,DateTime xtcsrq)
        {
            using (var session=Factory.Create<ISession>())
            {
                var result = session.Execute(UpdateXtcsSql, new { xtcsrq00 = xtcsrq, xtcsdm00 =xtcsdm});
                return result>0?true:false;
            }
        }
        #endregion
    }
}
