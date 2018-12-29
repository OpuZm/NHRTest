using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OPUPMS.Domain.Base.Dtos;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class RestaurantCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int R_Company_Id { get; set; }
    }

    public class RestaurantListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AreaNum { get; set; }
        public int BoxNum { get; set; }
        public int TableNum { get; set; }
        public int SeatNum { get; set; }
        public string Description { get; set; }
        public decimal? ServerRate { get; set; }
        public int R_Company_Id { get; set; }

        /// <summary>
        /// 分市列表
        /// </summary>
        public List<MarketListDTO> MarketList { get; set; }
    }

    public class RestaurantSearchDTO : BaseSearch
    {
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// 餐厅工作台信息处理类
    /// </summary>
    public class RestaurantPlatformDTO
    {
        /// <summary>
        /// 区域信息列表
        /// </summary>
        public List<AreaListDTO> AreaList { get; set; }

        /// <summary>
        /// 餐台信息列表
        /// </summary>
        public List<TableListIndexDTO> TableList { get; set; }

        /// <summary>
        /// 餐台状态类型列表
        /// </summary>
        public List<BaseDto> TableStatusList { get; set; }

        /// <summary>
        /// 餐厅当前总客数
        /// </summary>
        public int CurrentTotalGuestNum { get; set; }

        /// <summary>
        /// 餐台当前使用率
        /// </summary>
        public string CurrentTableUsedRate { get; set; }

        /// <summary>
        /// 餐厅当前订单总金额
        /// </summary>
        public decimal CurrentTotalAmount { get; set; }

        /// <summary>
        /// 营业日期
        /// </summary>
        public string BusinessDate { get; set; }

        /// <summary>
        /// 登录分市Id
        /// </summary>
        public int LoginMarketId { get; set; }
    }

    public class VerifyUserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        /// <summary>
        /// 所属餐厅Id
        /// </summary>
        public int RestaurantId { get; set; }
        public int CompanyId { get; set; }
    }

}
