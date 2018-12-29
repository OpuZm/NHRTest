using OPUPMS.Domain.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Model.Dtos
{
    public class HotelMenuDto : MenuInfo
    {
        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<HotelMenuDto> SubMenus { get; set; }
    }
}
