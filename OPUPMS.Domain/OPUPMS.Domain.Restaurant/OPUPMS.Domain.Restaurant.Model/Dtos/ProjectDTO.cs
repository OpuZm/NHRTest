using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OPUPMS.Domain.Base.Models;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class ProjectCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(100)]
        public string Name { get; set; }
        public int R_Category_Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public int IsOnSale { get; set; }
        public int IsDiscount { get; set; }
        public int IsRecommend { get; set; }
        public int IsCustomer { get; set; }
        public bool IsDelete { get; set; }
        public int IsGive { get; set; }
        public bool IsStock { get; set; }
        public decimal Stock { get; set; }
        public int IsQzdz { get; set; }
        public int IsChangePrice { get; set; }
        public int IsChangeNum { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public int Property { get; set; }
        public int R_Company_Id { get; set; }
        [Display(Name = "自定义编码")]
        [StringLength(100)]
        public string Code { get; set; }
        [Display(Name = "初始单位")]
        [StringLength(20)]
        public string BaseUnit { get; set; }
        public List<R_ProjectExtendAttribute> Extends { get; set; }
        public List<R_ProjectDetail> Details { get; set; }
        public int Sorted { get; set; }
        public int IsEnable { get; set; }
        public int ExtractType { get; set; }
        public decimal ExtractPrice { get; set; }
        public int IsServiceCharge { get; set; }
    }

    public class ProjectSearchDTO : BaseSearch
    {
        public int Category { get; set; }
        public string Name { get; set; }
    }

    public class ProjectListDTO
    {
        public ProjectListDTO()
        {
            TotalProjectStalls = new List<ProjectStallDTO>();
            DetailProjectStalls = new List<ProjectStallDTO>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsDiscount { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsDelete { get; set; }
        public bool IsGive { get; set; }
        public bool IsStock { get; set; }
        public decimal Stock { get; set; }
        public bool IsQzdz { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsChangeNum { get; set; }
        public int Sorted { get; set; }
        public bool IsEnable { get; set; }
        public List<ProjectStallDTO> TotalProjectStalls { get; set; }
        public List<ProjectStallDTO> DetailProjectStalls { get; set; }
        public string TotalStalls { get; set; }
        public string DetailStalls { get; set; }
    }

    public class ProjectExtendCreateDTO
    {
        public int Id { get; set; }
        public IEnumerable<int> Extends { get; set; }
    }

    public class ProjectDetailCreateDTO
    {
        public int Id { get; set; }
        ///<summary>
        /// 餐饮项目ID
        ///</summary>
        public int R_Project_Id { get; set; }
        ///<summary>
        /// 单位
        ///</summary>
        public string Unit { get; set; }
        ///<summary>
        /// 价格
        ///</summary>
        public decimal Price { get; set; }
        ///<summary>
        /// 成本价
        ///</summary>
        public decimal CostPrice { get; set; }
        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }
        ///<summary>
        /// 单位倍率 [1,1.5,2等]
        ///</summary>
        public decimal UnitRate { get; set; }
    }

    public class ProjectDetailListDTO
    {
        public int Id { get; set; }
        ///<summary>
        /// 餐饮项目ID
        ///</summary>
        public int R_Project_Id { get; set; }
        public string ProjectName { get; set; }
        ///<summary>
        /// 单位
        ///</summary>
        public string Unit { get; set; }
        ///<summary>
        /// 价格
        ///</summary>
        public decimal Price { get; set; }
        ///<summary>
        /// 成本价
        ///</summary>
        public decimal CostPrice { get; set; }
        ///<summary>
        /// 描述信息
        ///</summary>
        public string Description { get; set; }
        ///<summary>
        /// 单位倍率 [1,1.5,2等]
        ///</summary>
        public decimal UnitRate { get; set; }
        public bool IsDelete { get; set; }
        public int Category { get; set; }
        public List<CharsetCode> CharsetCodeList { get; set; }

        #region 用于点菜和结账
        /// <summary>
        /// 1=普通菜品 2=套餐
        /// </summary>
        public int CyddMxType { get; set; }
        /// <summary>
        /// 是否可打折
        /// </summary>
        public int IsDiscount { get; set; }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public int IsCustomer { get; set; }
        /// <summary>
        /// 是否可赠送
        /// </summary>
        public int IsGive { get; set; }
        /// <summary>
        /// 是否可强制打折
        /// </summary>
        public int IsQzdz { get; set; }
        /// <summary>
        /// 是否可改价
        /// </summary>
        public int IsChangePrice { get; set; }
        /// <summary>
        /// 送厨后可否更改数量
        /// </summary>
        public int IsChangeNum { get; set; }
        public decimal Num { get; set; }
        #endregion
        public string Code { get; set; }
        public string CoverUrl { get; set; }
    }

    public class ProjectAndDetailListDTO
    {
        public string Name { get; set; }
        public int Category { get; set; }
        public int Id { get; set; }
        public bool IsStock { get; set; }
        public decimal Stock { get; set; }
        public List<CharsetCode> CharsetCodeList { get; set; }
        public List<R_ProjectDetail> ProjectDetailList { get; set; }
        public List<R_OrderDetailPackageDetail> PackageDetailList { get; set; }//套餐菜品明细
        public List<ProjectExtendDTO> ExtendList { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public string Code { get; set; }
        public string CoverUrl { get; set; }
        public string Describe { get; set; }
        public string smallImage { get; set; }
        public int chosenNum { get; set; }
        public List<ProjectExtendDTO> practiceList { get; set; }
        public List<ProjectExtendDTO> requirementList { get; set; }
        public List<ProjectExtendDTO> garnishList { get; set; }
        #region 用于点菜和结账
        /// <summary>
        /// 1=普通菜品 2=套餐
        /// </summary>
        public int CyddMxType { get; set; }
        /// <summary>
        /// 是否可打折
        /// </summary>
        public int IsDiscount { get; set; }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public int IsCustomer { get; set; }
        /// <summary>
        /// 是否可赠送
        /// </summary>
        public int IsGive { get; set; }
        /// <summary>
        /// 是否可强制打折
        /// </summary>
        public int IsQzdz { get; set; }
        /// <summary>
        /// 是否可改价
        /// </summary>
        public int IsChangePrice { get; set; }
        /// <summary>
        /// 送厨后可否更改数量
        /// </summary>
        public int IsChangeNum { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public int IsRecommend { get; set; }
        public int IsUpStore { get; set; }
        public int IsSpecialOffer { get; set; }
        #endregion
    }

    public class ProjectExtendDTO
    {
        public int Id { get; set; }
        public int Project { get; set; }
        public int ProjectExtend { get; set; }
        public string ProjectExtendName { get; set; }
        public CyxmKzType ExtendType { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public int CyddMxId { get; set; }
        public int R_ProjectExtendType_Id { get; set; }
        public bool IsSelect { get; set; }
    }

    public class ProjectExtendSplitListDTO
    {
        public List<ProjectExtendDTO> Extend { get; set; }
        public List<ProjectExtendDTO> ExtendRequire { get; set; }
        public List<ProjectExtendDTO> ExtendExtra { get; set; }
    }

    /// <summary>
    /// 餐饮项目扩展类型
    /// </summary>
    public class ProjectExtendTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int R_Company_Id { get; set; }
    }

    public class ProjectJoinDetailDTO : ProjectDetailCreateDTO
    {
        /// <summary>
        /// 菜品名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 菜品类别
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 菜品属性
        /// </summary>
        public int Property { get; set; }
    }

    /// <summary>
    /// 餐饮项目估清
    /// </summary>
    public class ProjectClearDTO
    {
        public int Id { get; set; }
        public decimal Stock { get; set; }
        public bool IsStock { get; set; }
    }

    /// <summary>
    /// 点餐界面选择特殊要求实体
    /// </summary>
    public class OrderProjectExtends
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CyxmKzType ExtendType { get; set; }
        public List<ProjectExtendDTO> ProjectExtendDTOList { get; set; }
    }

    public class ProjectFromOpu:R_Project
    {
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// 餐饮项目推荐设置
    /// </summary>
    public class ProjectRecomandDTO
    {
        public int Id { get; set; }
        public bool IsRecomand { get; set; }
        public bool IsPush { get; set; }
        public bool IsSpecialOffer { get; set; }
    }

    public class ProjectImageDTO : R_ProjectImage
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
    }

    public class ProjectImageUploadDTO
    {
        ///<summary>
        /// 来源类别(1.餐饮项目;2.餐饮套餐)
        ///</summary>
        public int CyxmTpSourceType { get; set; }
        ///<summary>
        /// 图片路径
        ///</summary>
        public string Url { get; set; }
        ///<summary>
        /// 来源ID
        ///</summary>
        public int Source_Id { get; set; }
        ///<summary>
        /// 是否封面
        ///</summary>
        public bool IsCover { get; set; }

        public int Sorted { get; set; }
    }
}
