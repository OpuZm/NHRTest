using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class BoxCreateDTO
    {
        public int Id { get; set; }
        public int Restaurant { get; set; }
        public int RestaurantArea { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class BoxSearchDTO : BaseSearch
    {
        public int Restaurant { get; set; }
    }

    public class BoxListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RestaurantArea { get; set; }
        public int TableNum { get; set; }
        public string Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public int RestaurantAreaId { get; set; }
    }
}
