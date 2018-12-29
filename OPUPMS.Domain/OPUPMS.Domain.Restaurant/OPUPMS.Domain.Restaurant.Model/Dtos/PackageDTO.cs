using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class PackageCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        public string Describe { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsOnSale { get; set; }
        public List<R_PackageDetail> PackageDetails { get; set; }
        public bool IsCustomer { get; set; }
        public int R_Company_Id { get; set; }
        public int IsDiscount { get; set; }
        public int IsChangePrice { get; set; }
        public int R_Category_Id { get; set; }
        public int IsGive { get; set; }
    }

    public class PackageSearchDTO : BaseSearch
    {
        public string Name { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class PackageListDTO
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Describe { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsOnSale { get; set; }
        public int R_Category_Id { get; set; }
    }

    public class PackageDetailCreateDTO
    {
        public int Id { get; set; }
        public int R_Package_Id { get; set; }
        public int R_ProjectDetail_Id { get; set; }
        public decimal Num { get; set; }
        public bool IsDelete { get; set; }
    }
}
