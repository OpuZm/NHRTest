using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class StallsCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "描述信息")]
        [StringLength(4000)]
        public string Description { get; set; }
        public int R_Company_Id { get; set; }

        public int Print_Id { get; set; }

        public List<R_ProjectStall> ProjectStallList { get; set; }
    }

    public class StallsSearchDTO : BaseSearch
    { }

    public class StallsListDTO
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int MainProjectNum { get; set; }
        public int DetailProjectNum { get; set; }

        public string PrinterName { get; set; }
    }

    public class ProjectStallDTO
    {
        public int ProjectId { get; set; }
        public int BillType { get; set; }
        public string Name { get; set; }
    }

    public class ProjectStallStatisticsDTO
    {
        public int ProjectId { get; set; }
        public int BillType { get; set; }
        public string Name { get; set; }
    }
}
