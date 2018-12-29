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
    public class BookingDetailRepository : MultiDbRepository<DfmxModel, int>, IBookingDetailRepository
    {
        public BookingDetailRepository(IMultiDbDbFactory factory): base(factory) { }

        #region 模型转换
        private DfmxModel ConvertToModel(BookingDetailInfo info)
        {
            if (info == null)
                return null;

            DfmxModel model = AutoMapper.Mapper.Map<BookingDetailInfo, DfmxModel>(info);
            return model;
        }

        private BookingDetailInfo ConvertToInfo(DfmxModel model)
        {
            if (model == null)
                return null;

            BookingDetailInfo info = AutoMapper.Mapper.Map<DfmxModel, BookingDetailInfo>(model);
            return info;
        }

        private List<BookingDetailInfo> ConvertToInfoList(IEnumerable<DfmxModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<BookingDetailInfo> list = new List<BookingDetailInfo>();
            foreach (var item in sourceList)
            {
                BookingDetailInfo info = ConvertToInfo(item);
                list.Add(info);
            }
            return list;
        }
        #endregion

        public Task<bool> AddNewOrUpdateBookingDetailAsync(string token, BookingDetailInfo info, IUnitOfWork uow = null)
        {
            DfmxModel model = ConvertToModel(info);
            return SaveOrUpdateBookingDetailAsync(token, model, uow);
        }

        private async Task<bool> SaveOrUpdateBookingDetailAsync(string token, DfmxModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(token, model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }

        readonly string GetListByBookingIdSQL = @"SELECT * FROM dbo.dfmx WHERE dfmxzt00 = @Status AND dfmxydh0 = @BookingId";

        public async Task<List<BookingDetailInfo>> GetListByBookingIdAsync(string token, int bookingId)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<DfmxModel>(GetListByBookingIdSQL, new { Status = GuestInfoState.N, BookingId = bookingId });

                return ConvertToInfoList(result);
            }
        }
    }
}
