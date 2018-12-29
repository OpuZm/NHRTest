using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Smooth.IoC.UnitOfWork;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Infrastructure.Common;
using System.Configuration;

namespace OPUPMS.Domain.Repository.OldRepository
{
    public class CustomerRepository : MultiDbRepository<LxdmModel, int>, ICustomerRepository
    {
        public CustomerRepository(IMultiDbDbFactory factory) : base(factory)
        {

        }
        protected static readonly string GetDefaultSql = @"SELECT Id, lxdmmc00 FROM Lxdm WHERE lxdmgzbz IN('Y','X')";
        protected static readonly string GetByStatusSql = @"SELECT Id, lxdmmc00 FROM Lxdm WHERE lxdmgzbz = @Status";
        protected static readonly string GetByIdSql = @"SELECT * FROM Lxdm WHERE Id = @Id";
        
        #region 自动映射转换

        /// <summary>
        /// 转换模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected TypeCodeInfo ConvertToInfo(LxdmModel model)
        {
            if (model == null)
                return null;

            TypeCodeInfo user = AutoMapper.Mapper.Map<LxdmModel, TypeCodeInfo>(model);
            return user;
        }

        protected List<TypeCodeInfo> ConvertToInfo(IEnumerable<LxdmModel> sourceList)
        {
            List<TypeCodeInfo> list = new List<TypeCodeInfo>();
            foreach (var item in sourceList)
            {
                list.Add(ConvertToInfo(item));
            }
            return list;
        }

        protected List<TypeCodeInfoSimple> ConvertToInfoSimple(IEnumerable<LxdmModel> sourceList)
        {
            List<TypeCodeInfoSimple> list = new List<TypeCodeInfoSimple>();
            foreach (var item in sourceList)
            {
                list.Add(new TypeCodeInfoSimple() { Id=item.Id,Name=item.Lxdmmc00});
            }
            return list;
        }

        /// <summary>
        /// 自动转换映射
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private LxdmModel ConvertToModel(TypeCodeInfo user)
        {
            LxdmModel model = AutoMapper.Mapper.Map<TypeCodeInfo, LxdmModel>(user);
            return model;
        } 
        #endregion

        public List<TypeCodeInfo> GetListByStatus(string status)
        {
            using (var session = Factory.Create<ISession>())
            {
                IEnumerable<LxdmModel> result = null;
                if (status.IsEmpty())
                    result = session.Query<LxdmModel>(GetDefaultSql);
                else
                    result = session.Query<LxdmModel>(GetByStatusSql, new { Status = status });

                return ConvertToInfo(result);
            }
        }

        public TypeCodeInfo GetCustomerInfoById(int id)
        {
            using (var session = Factory.Create<ISession>())
            {
                var result = session.QueryFirstOrDefault<LxdmModel>(GetByIdSql, new LxdmModel { Id = id });

                return ConvertToInfo(result);
            }
        }

        public List<TypeCodeInfoSimple> GetListByStatusSimple(string status)
        {
            using (var session = Factory.Create<ISession>())
            {
                IEnumerable<LxdmModel> result = null;
                if (status.IsEmpty())
                    result = session.Query<LxdmModel>(GetDefaultSql);
                else
                    result = session.Query<LxdmModel>(GetByStatusSql, new { Status = status });

                return ConvertToInfoSimple(result);
            }
        }
    }
}
