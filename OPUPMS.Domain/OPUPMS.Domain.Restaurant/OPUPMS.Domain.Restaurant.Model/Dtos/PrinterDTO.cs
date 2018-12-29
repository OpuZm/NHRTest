using System.ComponentModel.DataAnnotations;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class PrinterDTO
    {
        public int Id { get; set; }
        [Display(Name = "打印机名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "备注信息")]
        [StringLength(1000)]
        public string Remark { get; set; }

        public string PcName { get; set; }

        [Display(Name = "打印机IP 地址")]
        [Required(ErrorMessage = "您需要填写{0}")]
        public string IpAddress { get; set; }

        [Display(Name = "打印机端口号")]
        [Required(ErrorMessage = "您需要填写{0}")]
        public string PrintPort { get; set; }

        public string Code { get; set; }

        public bool IsDelete { get; set; }
    }

    public class PrinterSearchDTO : BaseSearch
    { }

    public class PrinterProject:PrinterDTO
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// 打印单类型(1=详情单,2=总单)
        /// </summary>
        public int BillType { get; set; }
        public string StallName { get; set; }
        public int StallId { get; set; }
    }

    public class OrderReturnPackageDetailDTO:R_ProjectDetail
    {
        public string PackageDetailName { get; set; }
        public int PackageDetailId { get; set; }
        public decimal PackageDetailNum { get; set; }
    }
}
