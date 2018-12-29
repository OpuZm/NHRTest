using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class CategoryCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "描述信息")]
        [StringLength(4000)]
        public string Description { get; set; }
        public int Pid { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsDiscount { get; set; }
        public int R_Company_Id { get; set; }
        public int Sorted { get; set; }
    }

    public class CategorySearchDTO : BaseSearch
    {
        public int Pid { get; set; }
    }

    public class CategoryListDTO
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Pid { get; set; }
        public decimal DiscountRate { get; set; }
        public string Pname { get; set; }
        public bool IsDiscount { get; set; }
        public int Sorted { get; set; }
    }
    /// <summary>
    /// 点餐菜品所有分类，包含父类和子类
    /// </summary>
    public class AllCategoryListDTO
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Pid { get; set; }
        public decimal DiscountRate { get; set; }
        public string Pname { get; set; }
        public bool IsDiscount { get; set; }
        public List<CategoryListDTO> ChildList { get; set; }
        public int Sorted { get; set; }
    }

    public class RestaurantCategoryCreateDTO
    {
        public int Id { get; set; }
        public int R_Restaurant_Id { get; set; }
        public List<int> CategoryIds { get; set; }
        public string R_Restaurant_Name { get; set; }
    }

    public class RestaurantCategoryListDTO 
    {
        public int Id { get; set; }
        public string R_Restaurant_Name { get; set; }
        public string R_Category_Name { get; set; }
    }

    public class RestaurantCategorySearchDTO : BaseSearch
    { }
}
