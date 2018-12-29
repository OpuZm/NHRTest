using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class AutoAdditionCreateDTO
    {
        public AutoAdditionCreateDTO()
        {
            AutoAdditionProjects = new List<R_AutoAdditionProject>();
        }
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        public int R_Market_Id { get; set; }
        public int R_Restaurant_Id { get; set; }
        public int R_Area_Id { get; set; }
        public bool IsEnable { get; set; }
        public int R_Company_Id { get; set; }
        public bool IsDelete { get; set; }
        public List<R_AutoAdditionProject> AutoAdditionProjects { get; set; }
    }

    public class AutoAdditionListDTO: AutoAdditionCreateDTO
    {
        public string R_Market_Name { get; set; }
        public string R_Restaurant_Name { get; set; }
        public string R_Area_Name { get; set; }
    }

    public class AutoAdditionSearchDTO : BaseSearch
    {
        public int Restaurant { get; set; }
    }
}
