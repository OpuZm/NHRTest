using OPUPMS.Domain.Hotel.Model;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using Smooth.IoC.UnitOfWork;
using Dapper;

namespace OPUPMS.Domain.Hotel.Repository
{
    public class RoomRoutineRepository : MultiDbRepository<FhswModel, int>, IRoomRoutineRepository
    {
        public RoomRoutineRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        readonly string GetRoomNoByStatusAndDateSql = "SELECT fhswfh00 FROM dbo.fhsw WHERE fhswbzs0='Y' AND fhswswdm IN @Status AND (DATEDIFF(DAY, fhswzzrq, @BeginDate) < 0 OR DATEDIFF(DAY, fhswksrq, @EndDate) > 0)) GROUP BY fhswfh00";

        public async Task<List<RoomRoutineInfo>> GetRoomNoListByStatusAndDateAsync(string token, string[] status, DateTime beginDate, DateTime endDate)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<FhswModel>(GetRoomNoByStatusAndDateSql,
                        new { Status = status, BeginDate = beginDate, EndDate = endDate });

                return ConvertToInfoList(result);
            }
        }

        #region 模型转换

        private List<RoomRoutineInfo> ConvertToInfoList(IEnumerable<FhswModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<RoomRoutineInfo> list = new List<RoomRoutineInfo>();
            foreach (FhswModel model in sourceList)
            {
                //调用AutoMapper映射
                RoomRoutineInfo customer = ConvertToInfo(model);

                list.Add(customer);
            }
            return list;
        }

        private FhswModel ConvertToModel(RoomRoutineInfo guestInfo)
        {
            if (guestInfo == null)
                return null;

            FhswModel model = AutoMapper.Mapper.Map<RoomRoutineInfo, FhswModel>(guestInfo);
            return model;
        }

        private RoomRoutineInfo ConvertToInfo(FhswModel model)
        {
            if (model == null)
                return null;

            RoomRoutineInfo info = AutoMapper.Mapper.Map<FhswModel, RoomRoutineInfo>(model);
            return info;
        }

        #endregion
    }
}
