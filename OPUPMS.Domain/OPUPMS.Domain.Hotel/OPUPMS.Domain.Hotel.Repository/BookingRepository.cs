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
    public class BookingRepository : MultiDbRepository<DfzlModel, int>, IBookingRepository
    {
        public BookingRepository(IMultiDbDbFactory factory): base(factory) { }

        #region 模型转换
        private DfzlModel ConvertToModel(BookingInfo info)
        {
            if (info == null)
                return null;

            DfzlModel model = AutoMapper.Mapper.Map<BookingInfo, DfzlModel>(info);
            return model;
        }

        private BookingInfo ConvertToInfo(DfzlModel model)
        {
            if (model == null)
                return null;

            BookingInfo info = AutoMapper.Mapper.Map<DfzlModel, BookingInfo>(model);
            return info;
        }

        private List<BookingInfo> ConvertToInfoList(IEnumerable<DfzlModel> sourceList)
        {
            if (sourceList == null)
                return null;

            List<BookingInfo> list = new List<BookingInfo>();
            foreach (var item in sourceList)
            {
                BookingInfo info = ConvertToInfo(item);
                list.Add(info);
            }
            return list;
        } 
        #endregion

        public Task<bool> AddNewOrUpdateBookingAsync(string token, BookingInfo info, IUnitOfWork uow = null)
        {
            DfzlModel model = null;
            return SaveOrUpdateBookingAsync(token, model, uow);
        }

        private async Task<bool> SaveOrUpdateBookingAsync(string token, DfzlModel model, IUnitOfWork uow = null)
        {
            int result = 0;
            if (uow == null)
                result = await SaveOrUpdateAsync<ISession>(token, model);
            else
                result = await SaveOrUpdateAsync(model, uow);

            return result > 0;
        }

        readonly string GetListByStatusSQL = @"SELECT * FROM dbo.dfzl WHERE dfzlzt00 = @Status";

        public async Task<List<BookingInfo>> GetListByStatusAsync(string token, string status)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = await session.QueryAsync<DfzlModel>(GetListByStatusSQL, new { Status = status });

                return ConvertToInfoList(result);
            }
        }
    }
}
