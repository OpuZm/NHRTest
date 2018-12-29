using AutoMapper;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Models;

namespace OPUPMS.Domain.Base
{
    public class BaseConvertModelProfile : Profile
    {
        public BaseConvertModelProfile()
        {
            //配置AutoMapper
            #region 操作代码对象映射
            CreateMap<CzdmModel, UserInfo>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.UserCode, opt => opt.MapFrom(src => src.UserCode))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.UserPwd, opt => opt.MapFrom(src => src.UserPwd))
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                    //.ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Czdmbm00))
                    //.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Czdmqxzb))
                    //.ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Czdmzkl0))
                    .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.RestaurantAuthority))
                    //.ForMember(dest => dest.MaxClearValue, opt => opt.MapFrom(src => src.Czdmmlje))
                    //.ForMember(dest => dest.ManagerRestaurant, opt => opt.MapFrom(src => src.Czdmtype))
                    .ReverseMap();
            #endregion

            #region 操作记录对象映射
            CreateMap<CzjlModel, OperateLogInfo>()
                    .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.Czjlmc00))
                    .ForMember(dest => dest.OperateRemark, opt => opt.MapFrom(src => src.Czjlczbz))
                    .ForMember(dest => dest.OperateTime, opt => opt.MapFrom(src => src.Czjlczsj))
                    .ForMember(dest => dest.OperateType, opt => opt.MapFrom(src => src.Czjllx00))
                    .ForMember(dest => dest.Remark, opt => opt.MapFrom(src => src.Czjlbz00))
                    .ForMember(dest => dest.UserCode, opt => opt.MapFrom(src => src.Czjlczdm))
                    .ReverseMap(); 
            #endregion

            #region 系统代码对象映射
            CreateMap<XtdmModel, SystemCodeInfo>()//创建映射
                    .ForMember(dest => dest.SysCode, opt => opt.MapFrom(src => src.Xtdmdm00.Trim().ToUpper()))//指定映射属性
                    .ForMember(dest => dest.SysCodeName, opt => opt.MapFrom(src => src.Xtdmmc00))
                    .ForMember(dest => dest.SysCodeState, opt => opt.MapFrom(src => src.Xtdmbzs0))
                    .ForMember(dest => dest.SysCodeType, opt => opt.MapFrom(src => src.Xtdmlx00))
                    .ForMember(dest => dest.CodeBelongCategory, opt => opt.MapFrom(src => src.Xtdmsslb))
                    .ForMember(dest => dest.Remark, opt => opt.MapFrom(src => src.Xtdmbz00))
                    .ReverseMap(); 
            #endregion

            #region 类型代码对象映射
            CreateMap<LxdmModel, TypeCodeInfo>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Lxdmdm00))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Lxdmdz00))
                    .ForMember(dest => dest.AgreementRemark, opt => opt.MapFrom(src => src.Lxdmxybz))
                    .ForMember(dest => dest.Agreements, opt => opt.MapFrom(src => src.Lxdmxy00))
                    .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Lxdmss00))
                    .ForMember(dest => dest.BillType, opt => opt.MapFrom(src => src.Lxdmxz00))
                    .ForMember(dest => dest.ChargeLimitAmount, opt => opt.MapFrom(src => src.Lxdmgzxe))
                    .ForMember(dest => dest.ChargeLimitFlag, opt => opt.MapFrom(src => src.Lxdmgzbz))
                    .ForMember(dest => dest.ChargeLimitRemainAmount, opt => opt.MapFrom(src => src.Lxdmgzye))
                    .ForMember(dest => dest.ChargeLimitTotalAmount, opt => opt.MapFrom(src => src.Lxdmgzze))
                    .ForMember(dest => dest.ClientCategory, opt => opt.MapFrom(src => src.Lxdmglkl))
                    .ForMember(dest => dest.ClientIndustry, opt => opt.MapFrom(src => src.Lxdmhy00))
                    .ForMember(dest => dest.CommisionCode, opt => opt.MapFrom(src => src.Lxdmyjdm))
                    .ForMember(dest => dest.Commission, opt => opt.MapFrom(src => src.Lxdmlxyj))
                    .ForMember(dest => dest.ConsumeAmount, opt => opt.MapFrom(src => src.Lxdmxfze))
                    .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Lxdmlxr0))
                    .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.Lxdmyxrq))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Lxdmmail))
                    .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Lxdmfax0))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Lxdmmc00))
                    .ForMember(dest => dest.PersonalPreference, opt => opt.MapFrom(src => src.Lxdmhp00))
                    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Lxdmdh00))
                    .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Lxdmxp00))
                    .ForMember(dest => dest.Principal, opt => opt.MapFrom(src => src.Lxdmfzr0))
                    .ForMember(dest => dest.RemainAmount, opt => opt.MapFrom(src => src.Lxdmye00))
                    .ForMember(dest => dest.RoomBreakfast, opt => opt.MapFrom(src => src.Lxdmbh00))
                    .ForMember(dest => dest.RoomPriceCategory, opt => opt.MapFrom(src => src.Lxdmxyjl))
                    .ForMember(dest => dest.SellerCode, opt => opt.MapFrom(src => src.Lxdmywy0))
                    .ForMember(dest => dest.SignDate, opt => opt.MapFrom(src => src.Lxdmqdrq))
                    .ForMember(dest => dest.SignImage, opt => opt.MapFrom(src => src.Lxdmqm00))
                    .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Lxdmzip0))
                    //.ForMember(dest => dest, opt => opt.MapFrom(src => src))
                    .ReverseMap(); 
            #endregion

        }
    }
}
