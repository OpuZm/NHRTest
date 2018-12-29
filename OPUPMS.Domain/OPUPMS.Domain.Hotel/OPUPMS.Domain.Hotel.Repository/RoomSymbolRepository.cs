using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Hotel.Model;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using Smooth.IoC.UnitOfWork;
using Dapper;

namespace OPUPMS.Domain.Hotel.Repository
{
    public class RoomSymbolRepository : MultiDbRepository<FhdmModel, string>, IRoomSymbolRepository
    {
        public RoomSymbolRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        //static readonly string GetAllSql = @"SELECT * FROM Fhdm";

        public List<RoomSymbolInfo> LoadRoomSymbolList(string token)
        {
            var result = GetAll<ISession>(token);
            return ConvertToInfoList(result);

            //using (var session = Factory.Create<ISession>(token))
            //{
            //    var result = session.Query<FhdmModel>(GetAllSql);
            //    return ReturnModelList(result.ToList());
            //}
        }

        public async Task<List<RoomSymbolInfo>> LoadRoomSymbolListAsync(string token)
        {
            try
            {
                var result = await GetAllAsync<ISession>(token);
                return ConvertToInfoList(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //using (var session = Factory.Create<ISession>(token))
            //{
            //    var result = await session.QueryAsync<FhdmModel>(GetAllSql);
            //    return ReturnModelList(result.ToList());
            //}
        }


        #region 模型转换

        private List<RoomSymbolInfo> ConvertToInfoList(IEnumerable<FhdmModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<RoomSymbolInfo> list = new List<RoomSymbolInfo>();

            foreach (FhdmModel model in sourceList)
            {
                RoomSymbolInfo roomSymbol = ConvertToInfo(model);

                list.Add(roomSymbol);
            }

            return list;
        }

        private RoomSymbolInfo ConvertToInfo(FhdmModel sourceModel)
        {
            if (sourceModel == null)
                return null;
            RoomSymbolInfo roomInfo = AutoMapper.Mapper.Map<FhdmModel, RoomSymbolInfo>(sourceModel);
            return roomInfo;
        }

        private FhdmModel ConvertToModel(RoomSymbolInfo sourceModel)
        {
            if (sourceModel == null)
                return null;
            FhdmModel roomInfo = AutoMapper.Mapper.Map<RoomSymbolInfo, FhdmModel>(sourceModel);
            return roomInfo;
        } 

        #endregion

        static readonly string GetInfoByRoomNoSql = @"SELECT fhdmdm00 AS Id, * FROM dbo.fhdm WHERE fhdmdm00 = @RoomNo";
        static readonly string GetInfoListByRoomNoArraySql = @"SELECT fhdmdm00 AS Id, * FROM dbo.fhdm WHERE fhdmdm00 IN @RoomNOs";

        public RoomSymbolInfo GetRoomInfoByNo(string token, string roomNo)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = session.Query<FhdmModel>(GetInfoByRoomNoSql, new { RoomNo = roomNo });
                if (result.Count() > 1)
                    throw new Exception("存在多个相同房号的房间信息，请确认此房号的有效性！");
                return ConvertToInfo(result.FirstOrDefault());
            }
        }

        public async Task<bool> UpdateRoomInfoByNoAsync(string token, RoomSymbolInfo room, IUnitOfWork uow = null)
        {
            var model = ConvertToModel(room);
            string result = null;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(token, model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result == null;
        }

        public async Task<RoomSymbolInfo> GetRoomInfoByNoAsync(string token, string roomNo)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<FhdmModel>(GetInfoByRoomNoSql, new { RoomNo = roomNo });
                if (result.Count() > 1)
                    throw new Exception("存在多个相同房号的房间信息，请确认此房号的有效性！");
                return ConvertToInfo(result.FirstOrDefault());
            }
        }

        public async Task<List<RoomSymbolInfo>> GetRoomInfoListByNoAsync(string token, string[] roomNoArray)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<FhdmModel>(GetInfoListByRoomNoArraySql, new { RoomNOs = roomNoArray });

                return ConvertToInfoList(result);
            }
        }
    }
}
