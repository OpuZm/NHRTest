using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    /// <summary>
    /// 房态统计信息对象
    /// </summary>
    public class RoomStatisticDto
    {
        /// <summary>
        /// 总房数
        /// </summary>
        public int TotalRooms { get; set; }

        /// <summary>
        /// 空房数
        /// </summary>
        public int VacantRooms { get; set; }

        /// <summary>
        /// 预抵房数
        /// </summary>
        public int ExpectArrivalRooms { get; set; }

        /// <summary>
        /// 预离房数
        /// </summary>
        public int ExpectDepartureRooms { get; set; }

        /// <summary>
        /// 自用房数
        /// </summary>
        public int HouseUseRooms { get; set; }

        /// <summary>
        /// 在住房数，占用房数
        /// </summary>
        public int OccupiedRooms { get; set; }

        /// <summary>
        /// 维修房数
        /// </summary>
        public int OutOfOrderRooms { get; set; }

        /// <summary>
        /// 钟点房数
        /// </summary>
        public int HourRooms { get; set; }

        /// <summary>
        /// 锁房数
        /// </summary>
        public int LockedRooms { get; set; }

        /// <summary>
        /// 免费房数
        /// </summary>
        public int ComplimentaryRooms { get; set; }

        /// <summary>
        /// 自来客数（散客数）
        /// </summary>
        public int WalkinGuests { get; set; }

        /// <summary>
        /// 会员客数
        /// </summary>
        public int MemberGuests { get; set; }

        /// <summary>
        /// 网络客人数
        /// </summary>
        public int NetworkGuests { get; set; }

        /// <summary>
        /// 总房价
        /// </summary>
        public decimal TotalRoomPrices { get; set; }

        /// <summary>
        /// 平均房价
        /// </summary>
        public decimal AverageRoomPrices { get; set; }

        /// <summary>
        /// 出租率
        /// </summary>
        public decimal OccupancyRate { get; set; }
    }
}
