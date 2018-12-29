using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.ConvertModels
{
    /// <summary>
    /// 对应于房号代码表实体 Fhdm
    /// </summary>
    public class RoomSymbolInfo
    {
        /// <summary>
        /// 房号代码 Fhdmdm00
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 楼座代码 Fhdmlz00
        /// </summary>
        public string GalleryCode { get; set; }

        /// <summary>
        /// 楼层代码 Fhdmlc00
        /// </summary>
        public string FloorCode { get; set; }

        /// <summary>
        /// 房型代码 Fhdmlx00
        /// </summary>
        public string RoomTypeCode { get; set; }

        /// <summary>
        /// 单价 Fhdmdj00
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 状态 Fhdmzt00
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 状态01  Fhdmzt01
        /// </summary>
        public string Status01 { get; set; }

        /// <summary>
        /// 状态02  Fhdmzt02
        /// </summary>
        public string Status02 { get; set; }

        /// <summary>
        /// 其它状态  Fhdmqtzt
        /// </summary>
        public string OtherStatus { get; set; }

        /// <summary>
        /// 是否清洁 Fhdmcd00
        /// </summary>
        public string IsClean { get; set; }

        /// <summary>
        /// 房间状态 Fhdmft00
        /// </summary>
        public string RoomState { get; set; }

        /// <summary>
        /// 可住人数 Fhdmkzrs
        /// </summary>
        public short AllowStayGuests { get; set; }

        /// <summary>
        /// 实住人数 Fhdmszrs
        /// </summary>
        public short ActualStayGuests { get; set; }

        /// <summary>
        /// 备注 Fhdmbz00
        /// </summary>
        public string Remark { get; set; }
    }
}
