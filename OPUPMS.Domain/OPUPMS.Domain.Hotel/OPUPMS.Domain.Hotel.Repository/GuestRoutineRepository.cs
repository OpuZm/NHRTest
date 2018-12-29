using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Infrastructure.Dapper;
using Smooth.IoC.UnitOfWork;
using Dapper;

namespace OPUPMS.Domain.Hotel.Repository
{
    public class GuestRoutineRepository : MultiDbRepository<KrswModel, int>, IGuestRoutineRepository
    {
        public GuestRoutineRepository(IMultiDbDbFactory factory): base(factory) { }

        readonly string GetListByGuestIdSql = @"SELECT * FROM Krsw WHERE krswzh00 = @GuestId ";
        
        public async Task<List<GuestRoutineInfo>> GetListByGuestIdAsync(string token, int guestId)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<KrswModel>(GetListByGuestIdSql, new { GuestId = guestId });

                return ConvertToInfoList(result);
            }
        }


        #region 模型转换

        private List<GuestRoutineInfo> ConvertToInfoList(IEnumerable<KrswModel> modelList)
        {
            List<GuestRoutineInfo> list = new List<GuestRoutineInfo>();

            foreach (var item in modelList)
            {
                GuestRoutineInfo info = ConvertToInfo(item);
                list.Add(info);
            }
            return list;
        }

        private GuestRoutineInfo ConvertToInfo(KrswModel model)
        {
            if (model == null)
                return null;

            GuestRoutineInfo info = AutoMapper.Mapper.Map<KrswModel, GuestRoutineInfo>(model);
            return info;
        } 

        private KrswModel ConvertToModel(GuestRoutineInfo info)
        {
            if (info != null)
                return null;

            KrswModel model = AutoMapper.Mapper.Map<GuestRoutineInfo, KrswModel>(info);
            return model;
        }
        #endregion
    }
}
