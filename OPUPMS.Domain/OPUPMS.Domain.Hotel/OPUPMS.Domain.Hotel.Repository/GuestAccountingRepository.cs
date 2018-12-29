using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using Smooth.IoC.UnitOfWork;
using Dapper;

namespace OPUPMS.Domain.Hotel.Repository
{
    public class GuestAccountingRepository : MultiDbRepository<KrzwModel, int>, IGuestAccountingRepository
    {
        public GuestAccountingRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        static readonly string GetByMutliIdsSql = @"SELECT * FROM krzw WHERE krzwzh00 IN @IdArray";


        public List<GuestAccountingInfo> GetListByGuestIds(string token, int[] idArray)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = session.Query<KrzwModel>(GetByMutliIdsSql, new { IdArray = idArray });

                return ConvertToInfoList(result);
            }
        }

        public async Task<List<GuestAccountingInfo>> GetListByGuestIdsAsync(string token, int[] idArray)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<KrzwModel>(GetByMutliIdsSql, new { IdArray = idArray });

                return ConvertToInfoList(result);
            }
        }


        #region 模型转换

        private List<GuestAccountingInfo> ConvertToInfoList(IEnumerable<KrzwModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<GuestAccountingInfo> list = new List<GuestAccountingInfo>();
            foreach (KrzwModel model in sourceList)
            {
                //调用AutoMapper映射
                GuestAccountingInfo customer = ConvertToInfo(model);

                list.Add(customer);
            }

            return list;
        }

        private KrzwModel ConvertToModel(GuestAccountingInfo info)
        {
            if (info == null)
                return null;
            KrzwModel model = AutoMapper.Mapper.Map<GuestAccountingInfo, KrzwModel>(info);
            return model;
        }

        private GuestAccountingInfo ConvertToInfo(KrzwModel model)
        {
            if (model == null)
                return null;
            GuestAccountingInfo info = AutoMapper.Mapper.Map<KrzwModel, GuestAccountingInfo>(model);
            return info;
        }
        #endregion

        public async Task<bool> AddNewOrUpdateAccountingInfo(string token, GuestAccountingInfo info)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                KrzwModel model = ConvertToModel(info);
                var result = await SaveOrUpdateAsync<ISession>(token, model);
                return result > 0;
            }
        }
        
    }
}
