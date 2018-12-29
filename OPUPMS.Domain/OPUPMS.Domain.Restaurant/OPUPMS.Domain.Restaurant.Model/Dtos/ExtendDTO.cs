using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class ExtendCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "描述信息")]
        [StringLength(4000)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CyxmKzType CyxmKzType { get; set; }
        [Display(Name = "单位")]
        [StringLength(200)]
        public string Unit { get; set; }
        public int ExtendType { get; set; }
        public int R_Company_Id { get; set; }
    }

    public class ExtendSearchDTO : BaseSearch
    {
        public int CyxmKzType { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class ExtendListDTO
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CyxmKzType { get; set; }
        public int CyxmKzTypeId { get; set; }
        [StringLength(200)]
        public string Unit { get; set; }
        public string ExtendType { get; set; }
        public int ExtendTypeId { get; set; }
    }

    public class ExtendTypeCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        public int R_Company_Id { get; set; }
    }

    public class ExtendTypeSearchDTO : BaseSearch
    {
        public string Name { get; set; }
        public int R_Company_Id { get; set; }
    }

    public class ExtendTypeListDTO
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public int R_Company_Id { get; set; }

    }
}
