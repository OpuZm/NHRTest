using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class DiscountCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "分市")]
        [Required(ErrorMessage = "您需要选择{0}")]
        public int Market { get; set; }

        [Display(Name = "餐厅")]
        [Required(ErrorMessage = "您需要选择{0}")]
        public int Restaurant { get; set; }

        //[Display(Name = "区域")]
        //[Required(ErrorMessage = "您需要选择{0}")]
        public int Area { get; set; }
        public bool IsEnable { get; set; }

        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "您需要选择{0}")]
        public Nullable<DateTime> StartDate { get; set; }

        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "您需要选择{0}")]
        public Nullable<DateTime> EndDate { get; set; }

        public string StartDateStr { get { return StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""; } }
        public string EndDateStr { get { return EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""; } }
        public List<R_DiscountCategory> CyxmZkCp { get; set; }
        public int CompanyId { get; set; }
    }

    public class DiscountSearchDTO : BaseSearch
    {
        public int Restaurant { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }


    public class PayDiscountSearchDTO : DiscountSearchDTO
    {
        /// <summary>
        /// 结账时当前所处时间
        /// </summary>
        public string CurrentDate { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string OrderCreateDate { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnable { get; set; }

    }

    public class DiscountDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Market { get; set; }

        public string Restaurant { get; set; }

        public string Area { get; set; }

        public string Category { get; set; }

        public decimal DiscountRate { get; set; }

        public bool IsEnable { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public List<R_DiscountCategory> CyxmZkCp { get; set; }
    }

    public class SchemeDiscountSearchDTO
    {
        public int RestaurantId { get; set; }

        public int MarketId { get; set; }
        public int OrderId { get; set; }
    }

    public class SchemeDiscountDTO
    {
        public SchemeDiscountDTO()
        {
            DetailList = new List<SchemeDiscountDetailDTO>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int MarketId { get; set; }

        public string MarketName { get; set; }

        public int RestaurantId { get; set; }

        public int AreaId { get; set; }

        public string AreaName { get; set; }
        
        public bool IsEnable { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public string StartDateStr { get { return StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd") : ""; } }
        public string EndDateStr { get { return EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : ""; } }

        public List<SchemeDiscountDetailDTO> DetailList { get; set; }
    }

    public class SchemeDiscountDetailDTO
    {
        public int Id { get; set; }

        public int SchemeId { get; set; }

        public string SchemeName { get; set; }

        public decimal DiscountRate { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int AreaId { get; set; }

        public int MarketId { get; set; }
    }
}
