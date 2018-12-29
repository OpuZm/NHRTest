using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    /// <summary>
    /// 联房房间信息对象
    /// </summary>
    public class LinkRoomDto
    {
        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 房间名称
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// 联房客人列表
        /// </summary>
        public List<LinkGuestDto> GuestList { get; set; }
    }

    /// <summary>
    /// 联房客人信息
    /// </summary>
    public class LinkGuestDto
    {
        /// <summary>
        /// 客人Id
        /// </summary>
        public int GuestId { get; set; }

        /// <summary>
        /// 客人名
        /// </summary>
        public string GuestName { get; set; }

        /// <summary>
        /// 房号
        /// </summary>
        public string RoomNo { get; set; }
    }

}
