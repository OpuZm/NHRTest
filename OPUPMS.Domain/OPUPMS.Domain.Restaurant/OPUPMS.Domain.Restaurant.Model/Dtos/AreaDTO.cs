using System;
using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class AreaCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "名称")]
        [StringLength(4000)]
        public string Description { get; set; }
        public int Restaurant { get; set; }
        public Nullable<decimal> ServerRate { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class AreaSearchDTO : BaseSearch
    {
        /// <summary>
        /// 餐厅
        /// </summary>
        public int Restaurant { get; set; }
    }

    public class AreaListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restaurant { get; set; }
        public Nullable<decimal> ServerRate { get; set; }
        public int BoxNum { get; set; }
        public int TableNum { get; set; }
        public int RestaurantId { get; set; }
    }
}
