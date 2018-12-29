using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于云后台 Department 
    /// </summary>
    public class CloudHotelInfo
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public int HotelId2 { get; set; }

        public int HotelId3 { get; set; }

        public string State { get; set; }

        public int OrderBy { get; set; }

        public string ConnectionString { get; set; }

        public string DBName { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string HotelSerialNO { get; set; }

        public string HotelCode { get; set; }
    }
}
