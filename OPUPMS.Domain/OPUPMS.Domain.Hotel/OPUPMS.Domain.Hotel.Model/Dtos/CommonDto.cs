using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    class CommonDto
    {
    }

    /// <summary>
    /// 数据源对象
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 客人类别数据对象
    /// </summary>
    public class GuestCategoryDto : BaseDto
    {
    }

    /// <summary>
    /// 客人类型数据对象
    /// </summary>
    public class GuestTypeDto : BaseDto
    {
    }

    /// <summary>
    /// 房价类别数据对象
    /// </summary>
    public class RoomPriceCategoryDto : BaseDto
    {
    }

    /// <summary>
    /// 房价结构数据对象
    /// </summary>
    public class RoomPriceStructureDto : BaseDto
    {
    }

    /// <summary>
    /// 支付方式数据对象
    /// </summary>
    public class PaymentMethodDto : BaseDto
    {
    }

    /// <summary>
    /// 预订类型数据对象
    /// </summary>
    public class BookingTypeDto : BaseDto
    {
    }

    /// <summary>
    /// 客户来源市场数据对象
    /// </summary>
    public class ClientSourceTypeDto : BaseDto
    {
    }

    /// <summary>
    /// 性别数据对象
    /// </summary>
    public class GenderTypeDto : BaseDto
    {
    }

    /// <summary>
    /// 国籍，国家数据对象
    /// </summary>
    public class CountrySourceDto : BaseDto
    {
    }

    /// <summary>
    /// 证件类别数据对象
    /// </summary>
    public class CredentialCategoryDto : BaseDto
    {
    }

    /// <summary>
    /// 楼座数据对象
    /// </summary>
    public class GalleryDto : BaseDto
    {
        /// <summary>
        /// 楼座的楼层列表
        /// </summary>
        public List<FloorDto> FloorList { get; set; }
    }

    /// <summary>
    /// 楼层数据对象
    /// </summary>
    public class FloorDto : BaseDto
    {
        /// <summary>
        /// 楼座代码
        /// </summary>
        public string GalleryCode { get; set; }
    }

    /// <summary>
    /// 房型数据对象
    /// </summary>
    public class RoomTypeDto : BaseDto
    {
    }

    /// <summary>
    /// 房间状态数据对象
    /// </summary>
    public class RoomStateDto : BaseDto
    {
    }
}
