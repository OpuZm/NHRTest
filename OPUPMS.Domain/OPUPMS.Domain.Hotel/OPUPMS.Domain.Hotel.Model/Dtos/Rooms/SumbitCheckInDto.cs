using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Hotel.Model.ConvertModels;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    /// <summary>
    /// 提交开房的对象信息
    /// </summary>
    public class SumbitCheckInDto
    {
        /// <summary>
        /// 开房客人信息
        /// </summary>
        public List<GuestDataInfo> GuestList { get; set; }

        /// <summary>
        /// 开房房号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime CheckinDate { get; set; }

        /// <summary>
        /// 离店日期
        /// </summary>
        public DateTime CheckoutDate { get; set; }
    }
}
