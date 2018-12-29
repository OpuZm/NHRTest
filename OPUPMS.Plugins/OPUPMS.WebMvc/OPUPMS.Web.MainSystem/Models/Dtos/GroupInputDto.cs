using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPUPMS.Web.MainSystem.Models.Dtos
{
    public class GroupInputDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "名称不能为空！")]
        public string Name { get; set; }

        [Required(ErrorMessage = "全称不能为空！")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "代码不能为空！")]
        public string Code { get; set; }

        public int Status { get; set; }

        [Required(ErrorMessage = "地址不能为空！")]
        public string Address { get; set; }
        public string ContactTel { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "全称不能为空！")]
        public string Manager { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public string Fax { get; set; }
    }
}