using AutoMapper;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class RestaurantConvertModelProfile : Profile
    {
        public RestaurantConvertModelProfile()
        {
            //配置AutoMapper            
            CreateMap<R_Project, ProjectCreateDTO>().ReverseMap();
            //CreateMap<ProjectCreateDTO, R_Project>();
            CreateMap<R_ProjectDetail, ProjectDetailCreateDTO>().ReverseMap();
            //CreateMap<ProjectDetailCreateDTO, R_ProjectDetail>();
            CreateMap<R_Package, PackageCreateDTO>().ReverseMap();
            //CreateMap<PackageCreateDTO, R_Package>();
            CreateMap<R_PackageDetail, PackageDetailCreateDTO>().ReverseMap();
            //CreateMap<PackageDetailCreateDTO, R_PackageDetail>();
            CreateMap<R_Order, ReserveCreateDTO>().ReverseMap();
            //CreateMap<ReserveCreateDTO, R_Order>();
            CreateMap<R_OrderDetail, OrderDetailDTO>().ReverseMap();
            //CreateMap<OrderDetailDTO, R_OrderDetail>();
            CreateMap<R_OrderPayRecord, OrderPayRecordDTO>().ReverseMap();
            CreateMap<Printer, PrinterDTO>().ReverseMap();
            CreateMap<R_Invoice, InvoiceCreateDTO>().ReverseMap();
            CreateMap<R_PayMethod, PayMethodCreateDTO>().ReverseMap();
            CreateMap<R_Project, ProjectFromOpu>().ReverseMap();
            CreateMap<R_WeixinPrint, WeixinPrintDTO>().ReverseMap();
            CreateMap<R_ProjectImage, ProjectImageDTO>().ReverseMap();
            CreateMap<R_OrderDetailCause, OrderDetailCauseDTO>().ReverseMap();
            CreateMap<R_CustomConfig, CustomConfigDTO>().ReverseMap();
        }
    }
}
