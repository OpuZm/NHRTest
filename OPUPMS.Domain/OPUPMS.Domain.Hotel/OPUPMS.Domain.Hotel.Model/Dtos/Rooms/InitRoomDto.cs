using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    public class InitRoomDto
    {
        /// <summary>
        /// 客人类别列表
        /// </summary>
        public List<GuestCategoryDto> GuestCategoryList { get; set; }

        /// <summary>
        /// 客人类型列表
        /// </summary>
        public List<GuestTypeDto> GuestTypeList { get; set; }

        /// <summary>
        /// 房价类别列表
        /// </summary>
        public List<RoomPriceCategoryDto> RoomPriceCategoryList { get; set; }

        /// <summary>
        /// 房价结构列表
        /// </summary>
        public List<RoomPriceStructureDto> RoomPriceStructureList { get; set; }

        /// <summary>
        /// 支付方式列表
        /// </summary>
        public List<PaymentMethodDto> PaymentMethodList { get; set; }

        /// <summary>
        /// 预订类型列表
        /// </summary>
        public List<BookingTypeDto> BookingTypeList { get; set; }

        /// <summary>
        /// 客户来源市场列表
        /// </summary>
        public List<ClientSourceTypeDto> ClientSourceTypeList { get; set; }

        /// <summary>
        /// 性别列表
        /// </summary>
        public List<GenderTypeDto> GenderTypeList { get; set; }

        /// <summary>
        /// 国籍，国家列表
        /// </summary>
        public List<CountrySourceDto> CountrySourceList { get; set; }

        /// <summary>
        /// 证件类别列表
        /// </summary>
        public List<CredentialCategoryDto> CredentialCategoryList { get; set; }
    }
}
