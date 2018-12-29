using OPUPMS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    /// <summary>
    /// 房态界面信息对象
    /// </summary>
    public class RoomListDto
    {
        /// <summary>
        /// 房间列表
        /// </summary>
        public List<RoomInfoDto> RoomList { get; set; }

        /// <summary>
        /// 房态统计信息
        /// </summary>
        public RoomStatisticDto RoomStatistic { get; set; }

        /// <summary>
        /// 楼座楼层信息列表
        /// </summary>
        public List<GalleryDto> GalleryList { get; set; }

        /// <summary>
        /// 房间状态信息列表
        /// </summary>
        public List<RoomStateDto> RoomStateList { get; set; }

        /// <summary>
        /// 房型信息列表
        /// </summary>
        public List<RoomTypeDto> RoomTypeList { get; set; }

        /// <summary>
        /// 响应的结果
        /// </summary>
        public AjaxResult ResponseResult { get; set; }
    }
}
