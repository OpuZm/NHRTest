using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smooth.IoC.UnitOfWork;
using Dapper;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Domain.Hotel.Model.CloudModels;
using OPUPMS.Domain.Hotel.Model.ConvertModels;

namespace OPUPMS.Domain.Hotel.Repository
{
    public class CloudHotelRepository : MultiDbRepository<DepartmentModel, int>, ICloudHotelRepository
    {
        public CloudHotelRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        static readonly string SelectCloudHotelInfoSql = @"SELECT Did, Dname, Dpid, Ddata, DNO, DEndDate, Dxlh FROM dbo.Department WHERE Dpid = @Dpid";

        public CloudHotelInfo GetHotelByCode(string hotelCode)
        {
            using(var session = Factory.Create<ISession>())
            {
                var result = session.QueryFirstOrDefault<DepartmentModel>(SelectCloudHotelInfoSql, new DepartmentModel { Dpid = hotelCode.ToInt() });

                return ConvertToInfo(result);
            }
        }

        public async Task<CloudHotelInfo> GetHotelByCodeAsync(string hotelCode)
        {
            using (var session = Factory.Create<ISession>())
            {
                var model = await session.QueryFirstOrDefaultAsync<DepartmentModel>(SelectCloudHotelInfoSql, new DepartmentModel { Dpid = hotelCode.ToInt() });
                var info = ConvertToInfo(model);

                if (info != null)
                    Factory.SetConnectionCache(info.HotelId2.ToString(), info.ConnectionString);
                return info;
            }
        }

        private CloudHotelInfo ConvertToInfo(DepartmentModel model)
        {
            if (model == null)
                return null;

            CloudHotelInfo hotel = new CloudHotelInfo();
            hotel.ConnectionString = model.Ddata;
            hotel.DBName = model.DNO;
            hotel.ExpireDate = model.DEndDate;
            hotel.HotelCode = model.Djdm;
            hotel.HotelId = model.Id;
            hotel.HotelId2 = model.Dpid;
            hotel.HotelId3 = model.Dppid;
            hotel.HotelName = model.Dname;
            hotel.HotelSerialNO = model.Dxlh;
            hotel.OrderBy = model.Dorder;
            hotel.State = model.Dstatus;

            return hotel;
        }
        
    }
}
