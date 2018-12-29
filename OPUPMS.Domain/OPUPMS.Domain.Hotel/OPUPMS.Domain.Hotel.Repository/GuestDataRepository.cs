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
    public class GuestDataRepository : MultiDbRepository<KrzlModel, int>, IGuestDataRepository
    {
        public GuestDataRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        readonly string GetListByStatusSql = @"SELECT * FROM dbo.krzl WHERE krzlzt00=@Status ";

        readonly string GetRoomNoByStayAndBookingSql = @"SELECT krzlfh00 FROM dbo.krzl WHERE krzlzt00 IN @Status AND (DATEDIFF(DAY, krzlldrq, @BeginDate) < 0 OR DATEDIFF(DAY, krzlrzrq, @EndDate) > 0)) GROUP BY krzlfh00 ";

        readonly string GetLinkRoomListByGuestIdSql = @"SELECT krzlzh00, krzlzhlx, krzltzxh, krzltlxh, krzlfh00, krzlzt00, krzlzwxm, krzlywxm FROM Krzl g WHERE g.krzltlxh IN(SELECT krzltlxh FROM Krzl WHERE krzlzh00 = @GuestId)";

        public List<GuestDataInfo> GetListByStatus(string token, string status)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = session.Query<KrzlModel>(GetListByStatusSql, new { Status = status });
                
                return ConvertToInfoList(result);
            }
        }

        public async Task<List<GuestDataInfo>> GetListByStatusAsync(string token, string status)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<KrzlModel>(GetListByStatusSql, new { Status = status });
                
                return ConvertToInfoList(result);
            }
        }

        public async Task<List<GuestDataInfo>> GetListByStatusAndRoomNoAsync(string token, string status, string roomNo)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                //string sql = @"SELECT  krzlzh00, krzlzhlx, krzlfh00, krzlywy0, krzlzwxm, krzldz00, krzlxb00, 
                //            krzlzjlb, krzlzjhm, krzlgj00, krzlrzrq, krzlldrq, 
                //            krzlkrlb, kl=(select xtdmmc00 from xtdm where xtdmlx00=@GuestType and xtdmdm00=a.krzlkrlb), 
                //            krzlfjlx, fjlx=(select xtdmmc00 from xtdm where xtdmlx00=@RoomType and xtdmdm00=a.krzlfjlx),
                //            krzlfz00=cast(krzlfz00 as decimal(18,2)), 
                //            krzljg00, krzlkrbz, krzlmail, krzldh00, krzlldsj, krzlrzsj, krzllxdm, krzlkh00, krzljzfs, 
                //            krzlgs00, krzlfjlb, krzlzwbz, krzlfjsx,
                //            jgmc=(select xtdmmc00 from xtdm where xtdmlx00=@CityCode and krzljg00=xtdmdm00), krzlattr 
                //            from krzl as a  
                //            WHERE krzlfh00=@RoomNo and krzlzt00=@Status ORDER BY krzlzhlx DESC ";

                var sql = GetListByStatusSql + "AND krzlfh00=@RoomNo ORDER BY krzlzhlx DESC ";
                var result = await session.QueryAsync<KrzlModel>(sql,
                    new
                    {
                        //GuestType = SystemCodeTypes.GUEST_TYPE,
                        //RoomType = SystemCodeTypes.ROOM_CATEGORY,
                        //CityCode = SystemCodeTypes.CITY_CODE,
                        RoomNo = roomNo,
                        Status = status
                    });

                return ConvertToInfoList(result);
            }
        }


        #region 模型转换

        private List<GuestDataInfo> ConvertToInfoList(IEnumerable<KrzlModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<GuestDataInfo> list = new List<GuestDataInfo>();
            foreach (KrzlModel model in sourceList)
            {
                //调用AutoMapper映射
                GuestDataInfo customer = ConvertToInfo(model);

                list.Add(customer);
            }
            return list;
        }

        private KrzlModel ConvertToModel(GuestDataInfo guestInfo)
        {
            if (guestInfo == null)
                return null;

            KrzlModel model = AutoMapper.Mapper.Map<GuestDataInfo, KrzlModel>(guestInfo);
            return model;
        }

        private GuestDataInfo ConvertToInfo(KrzlModel model)
        {
            if (model == null)
                return null;

            GuestDataInfo info = AutoMapper.Mapper.Map<KrzlModel, GuestDataInfo>(model);
            return info;
        } 

        #endregion

        public bool AddNewGuest(string token, GuestDataInfo guest, IUnitOfWork uow = null)
        {
            KrzlModel model = ConvertToModel(guest);
            int result = 0;
            if (uow == null)
                result = SaveOrUpdate<ISession>(token, model);
            else
                result = SaveOrUpdate(model, uow);

            return result > 0;
        }

        public async Task<bool> AddNewGuestAsync(string token, GuestDataInfo guest, IUnitOfWork uow = null)
        {
            KrzlModel model = ConvertToModel(guest);
            return await SaveOrUpdateGuestAsync(token, model, uow);
        }

        public async Task<bool> UpdateGuestAsync(string token, GuestDataInfo guest, IUnitOfWork uow = null)
        {
            KrzlModel model = ConvertToModel(guest);
            return await SaveOrUpdateGuestAsync(token, model, uow);
        }

        private async Task<bool> SaveOrUpdateGuestAsync(string token, KrzlModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(token, model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }

        public async Task<List<GuestDataInfo>> GetRoomNoListByStatusAndDateAsync(string token, string[] status, DateTime beginDate, DateTime endDate)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<KrzlModel>(GetRoomNoByStayAndBookingSql, 
                        new { Status = status, BeginDate = beginDate, EndDate = endDate });

                return ConvertToInfoList(result);
            }
        }

        public async Task<List<GuestDataInfo>> GetLinkRoomListByGuestIdAsync(string token, int guestId)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<KrzlModel>(GetLinkRoomListByGuestIdSql, new { GuestId = guestId });

                return ConvertToInfoList(result);
            }
        }
    }
}
