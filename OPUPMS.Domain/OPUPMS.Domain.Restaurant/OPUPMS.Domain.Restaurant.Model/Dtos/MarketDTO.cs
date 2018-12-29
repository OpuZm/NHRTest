using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class MarketCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]   
        [Required(ErrorMessage = "您需要填写{0}")]  
        [StringLength(200)]       
        public string Name { get; set; }
        [Display(Name = "描述信息")]
        [StringLength(4000)]
        public string Description { get; set; }
        public int Restaurant { get; set; }
        [Display(Name = "开始时间")] 
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(50)]
        public string StartTime { get; set; }
        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(50)]
        public string EndTime { get; set; }
    }

    public class MarketSearchDTO : BaseSearch
    {
        public int Restaurant { get; set; }
    }

    public class MarketListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restaurant { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int RestaurantId { get; set; }

        public bool IsDefault { get; set; }
    }
}
