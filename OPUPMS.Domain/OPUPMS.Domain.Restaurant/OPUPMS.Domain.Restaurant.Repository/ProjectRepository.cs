using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using SqlSugar;
using System.IO;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class ProjectRepository : SqlSugarService, IProjectRepository
    {
        public ProjectRepository()
        {

        }

        public int Create(ProjectCreateDTO req, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int result = 0;
                try
                {
                    msg = string.Empty;
                    db.BeginTran();

                    R_Project model = Mapper.Map<ProjectCreateDTO, R_Project>(req);
                    model.CreateDate = DateTime.Now;
                    model.Property = req.IsDiscount + req.IsOnSale +
                        req.IsQzdz + req.IsRecommend + req.IsCustomer +
                        req.IsGive + req.IsChangePrice + req.IsChangeNum + req.IsServiceCharge;

                    var newId = db.Insert(model);

                    if (newId != null)
                    {
                        int Id = Convert.ToInt32(newId);
                        #region 初始化餐饮项目规格
                        if (!string.IsNullOrEmpty(req.BaseUnit))
                        {
                            db.Insert<R_ProjectDetail>(new R_ProjectDetail()
                            {
                                R_Project_Id = Id,
                                CostPrice = req.CostPrice,
                                Price = req.Price,
                                Unit = req.BaseUnit,
                                IsDelete = false,
                                UnitRate = 1
                            });
                        }
                        #endregion

                        #region 添加餐饮项目编码
                        CharsetCodeHelp cch = new CharsetCodeHelp();
                        List<CharsetCode> codeList = new List<CharsetCode>
                        {
                            new CharsetCode()
                            {
                                CharsetSource = Base.CharsetSource.餐饮项目,
                                CharsetType = Base.CharsetType.拼音,
                                SourceId = Id,
                                Code = cch.GetSpellCode(model.Name)
                            },
                            new CharsetCode()
                            {
                                CharsetSource = Base.CharsetSource.餐饮项目,
                                CharsetType = Base.CharsetType.五笔,
                                SourceId = Id,
                                Code = cch.GetWBCode(model.Name)
                            }
                        };

                        db.InsertRange<CharsetCode>(codeList);
                        #endregion
                    }
                    db.CommitTran();
                    result = Convert.ToInt32(newId);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    db.RollbackTran();
                }
                return result;
            }
        }

        public bool DetailCreate(ProjectDetailCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_ProjectDetail model = Mapper.Map<ProjectDetailCreateDTO, R_ProjectDetail>(req);

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public bool ExtendCreate(ProjectExtendCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;

                try
                {
                    db.BeginTran();

                    db.Delete<R_ProjectExtendAttribute>(p => p.R_Project_Id == req.Id);

                    if (req.Extends != null && req.Extends.Any())
                    {
                        List<R_ProjectExtendAttribute> listInsert = new List<R_ProjectExtendAttribute>();
                        req.Extends.ToList().ForEach(p =>
                        {
                            listInsert.Add(new R_ProjectExtendAttribute()
                            {
                                R_Project_Id = req.Id,
                                R_ProjectExtend_Id = p
                            });
                        });

                        if (listInsert != null && listInsert.Any())
                        {
                            db.InsertRange<R_ProjectExtendAttribute>(listInsert);
                        }
                    }

                    db.CommitTran();
                }
                catch (Exception)
                {
                    result = false;
                    db.RollbackTran();

                    throw;
                }

                return result;
            }
        }

        public List<ProjectDetailListDTO> GetDetailList(int category, bool isHasPackage = false)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ProjectDetailListDTO> list = new List<ProjectDetailListDTO>();
                var data = db.Sqlable()
                    .From<R_ProjectDetail>("s1")
                    .Join<R_Project>("s2", "s1.R_Project_Id", "s2.Id", JoinType.Inner)
                    .Join<R_Category>("s3", "s2.R_Category_Id", "s3.Id", JoinType.Inner)
                    //.Join<R_ProjectImage>("s4","s2.Id", "s4.Source_Id",JoinType.Left)
                    .Where("s1.IsDelete=0 and s2.IsDelete=0 and s2.IsEnable=1 and s3.IsDelete=0");

                if (category > 0)
                {
                    data = data.Where("s3.Id=" + category);
                }

                list = data.SelectToList<ProjectDetailListDTO>(@"s1.*,s2.Name as ProjectName,
                    s2.R_Category_Id as Category,1 as CyddMxType,s2.Property & 2 as IsDiscount,
                    s2.Property & 4 as IsQzdz,s2.Property & 16 as IsCustomer,s2.Property & 32 as IsGive,
                    s2.Property & 64 as IsChangePrice,s2.Property & 128 as IsChangeNum,s2.Code as Code,CoverUrl=(select top 1 Url from R_ProjectImage where (CyxmTpSourceType=1 and Source_Id=s2.Id and IsCover=1) order by IsCover desc,Sorted asc)");

                int[] proIds = list.Select(p => p.R_Project_Id).ToArray();
                var charsetCodeList = db.Queryable<CharsetCode>()
                    .Where(p => p.CharsetSource == Base.CharsetSource.餐饮项目 && proIds.Contains(p.SourceId)).ToList();
                var project = db.Queryable<R_Project>().Where(p => p.IsDelete == false && proIds.Contains(p.Id)).ToList();
                foreach (var item in list)
                {
                    item.CharsetCodeList = charsetCodeList.Where(p => p.SourceId == item.R_Project_Id).ToList() ?? new List<CharsetCode>();
                    var projectCode = project.Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                    if (projectCode != null && !string.IsNullOrEmpty(projectCode.Code))
                    {
                        item.CharsetCodeList.Add(new CharsetCode()
                        {
                            Code = projectCode.Code,
                            SourceId = item.Id
                        });
                    }
                }

                if (isHasPackage)
                {
                    //var packages = db.Queryable<R_Package>()
                    //    .Where(p => p.IsOnSale == true && p.IsDelete==false)
                    //    .Select(p => new ProjectDetailListDTO()
                    //    {
                    //        Id = p.Id,
                    //        ProjectName = p.Name,
                    //        Price = p.Price,
                    //        CyddMxType = (int)CyddMxType.餐饮套餐,
                    //        CostPrice = p.CostPrice,
                    //        Description = p.Describe,
                    //        R_Project_Id = p.Id,
                    //        IsDiscount = p.Property & (int)CytcProperty.是否可打折,
                    //        IsChangePrice = p.Property & (int)CytcProperty.是否可改价
                    //    })
                    //    .ToList();
                    var packages = db.Queryable<R_Package>()
                        .JoinTable<R_ProjectImage>((s1,s2)=>s1.Id==s2.Source_Id)
                        .Where<R_ProjectImage>((s1,s2)=> s1.IsOnSale == true && s1.IsDelete == false && 
                        (s2.CyxmTpSourceType==2 && s2.IsCover==true))
                        .Select<R_ProjectImage, ProjectDetailListDTO>((s1,s2) => new ProjectDetailListDTO()
                        {
                            Id = s1.Id,
                            ProjectName = s1.Name,
                            Price = s1.Price,
                            CyddMxType = (int)CyddMxType.餐饮套餐,
                            CostPrice = s1.CostPrice,
                            Description = s1.Describe,
                            R_Project_Id = s1.Id,
                            IsDiscount = s1.Property & (int)CytcProperty.是否可打折,
                            IsChangePrice = s1.Property & (int)CytcProperty.是否可改价,
                            CoverUrl=s2.Url
                        })
                        .ToList();
                    list = list.Concat(packages).ToList();
                }

                return list;
            }
        }

        public List<ProjectAndDetailListDTO> GetProjectAndDetailList(int category, bool isHasPackage = false)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ProjectAndDetailListDTO> list = new List<ProjectAndDetailListDTO>();
                //var data = db.Queryable<R_Project>().OrderBy(p => p.Sorted);
                var data = db.Queryable<R_Project>()
                    .JoinTable<R_Category>((s1, s2) => s1.R_Category_Id == s2.Id, JoinType.Inner)
                    .Where<R_Category>((s1, s2) => s2.IsDelete == false && s1.IsDelete==false && s1.IsEnable==true)
                    .Select("s1.*")
                    .OrderBy("s1.Sorted");
                //data = data.Where(p=>p.IsDelete == false && p.IsEnable==true);
                if (category > 0)
                {
                    //data = data.Where(p => p.R_Category_Id == category);
                    data = data.Where("s1.R_Category_Id=" + category);
                }

                List<R_Project> projectList = data.ToList();

                int[] proIds = projectList.Select(p => p.Id).ToArray();
                var projectImages = db.Queryable<R_ProjectImage>()
                    .Where(p => p.IsCover == true && p.CyxmTpSourceType == 1 && proIds.Contains(p.Source_Id)).ToList();
                var charsetCodeList = db.Queryable<CharsetCode>()
                    .Where(p => p.CharsetSource == Base.CharsetSource.餐饮项目 && proIds.Contains(p.SourceId)).ToList();   //字符集

                List<ProjectExtendDTO> extendList = db.Queryable<R_ProjectExtend>()
                    .JoinTable<R_ProjectExtendAttribute>((s1, s2) =>
                        s1.Id == s2.R_ProjectExtend_Id, JoinType.Inner)
                    .Where<R_ProjectExtendAttribute>((s1, s2) => proIds.Contains(s2.R_Project_Id) && s1.IsDelete==false)
                    .Select<R_ProjectExtendAttribute, ProjectExtendDTO>((s1, s2) =>
                    new ProjectExtendDTO
                    {
                        Id = s1.Id,
                        ExtendType = s1.CyxmKzType,
                        Project = s2.R_Project_Id,
                        ProjectExtend = s2.R_ProjectExtend_Id,
                        ProjectExtendName = s1.Name,
                        Price = s1.Price,
                        Unit = s1.Unit
                    }).ToList();    //餐饮项目扩展属性列表

                var detailList = db.Queryable<R_ProjectDetail>()
                    .Where(p => proIds.Contains(p.R_Project_Id) && p.IsDelete == false)
                    .ToList();  //餐饮项目明细

                ProjectAndDetailListDTO model = null;
                string imageUrl = string.Empty;
                string imageExt = string.Empty;
                string smallImage = string.Empty;
                foreach (var item in projectList)
                {
                    var projectImage = projectImages.FirstOrDefault(p => p.Source_Id == item.Id);
                    imageUrl = projectImage != null ? projectImage.Url : "";
                    imageExt = projectImage!=null? Path.GetExtension(imageUrl):"";
                    smallImage = projectImage != null ? imageUrl + "320x" + imageExt : "";
                    model = new ProjectAndDetailListDTO()
                    {
                        Id = item.Id,
                        Category = item.R_Category_Id,
                        Name = item.Name,
                        Stock = item.Stock,
                        IsStock = item.IsStock,
                        CyddMxType = 1,
                        CostPrice = item.CostPrice,
                        IsDiscount = item.Property & (int)CyxmProperty.是否可打折,
                        IsQzdz = item.Property & (int)CyxmProperty.是否强制打折,
                        IsCustomer = item.Property & (int)CyxmProperty.是否自定义,
                        IsGive = item.Property & (int)CyxmProperty.是否可赠送,
                        IsChangePrice = item.Property & (int)CyxmProperty.是否可改价,
                        IsChangeNum = item.Property & (int)CyxmProperty.送厨后可否更改数量,
                        IsRecommend = item.Property & (int)CyxmProperty.是否推荐,
                        IsUpStore=item.Property & (int)CyxmProperty.是否上架,
                        IsSpecialOffer = item.Property & (int)CyxmProperty.今日特价,
                        ProjectDetailList = detailList.Where(p => p.R_Project_Id == item.Id).ToList() ?? new List<R_ProjectDetail>(),
                        CharsetCodeList = charsetCodeList.Where(p => p.SourceId == item.Id).ToList() ?? new List<CharsetCode>(),
                        ExtendList = extendList.Where(p => p.Project == item.Id).ToList() ?? new List<ProjectExtendDTO>(),
                        Code=item.Code,
                        CoverUrl = projectImage == null ? "" : projectImage.Url,
                        Describe = item.Description,
                        smallImage = smallImage,
                        chosenNum=0,
                        practiceList= extendList.Where(p => p.Project == item.Id && p.ExtendType==CyxmKzType.做法).ToList()?? new List<ProjectExtendDTO>(),
                        requirementList= extendList.Where(p => p.Project == item.Id && p.ExtendType == CyxmKzType.要求).ToList() ?? new List<ProjectExtendDTO>(),
                        garnishList= extendList.Where(p => p.Project == item.Id && p.ExtendType == CyxmKzType.配菜).ToList() ?? new List<ProjectExtendDTO>()
                    };

                    if (!string.IsNullOrEmpty(item.Code))
                    {
                        model.CharsetCodeList.Add(new CharsetCode()
                        {
                            Code = item.Code,
                            SourceId = item.Id
                        });
                    }

                    list.Add(model);
                }

                if (isHasPackage)
                {
                    var packageImages = db.Queryable<R_ProjectImage>()
                        .Where(p => p.CyxmTpSourceType == 2 && p.IsCover == true).ToList();
                    var packages = db.Queryable<R_Package>().Where(p => p.IsDelete == false)
                        .Select(p => new ProjectAndDetailListDTO()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            CyddMxType = (int)CyddMxType.餐饮套餐,
                            CostPrice = p.CostPrice,
                            IsDiscount = p.Property & (int)CytcProperty.是否可打折,
                            IsChangePrice = p.Property & (int)CytcProperty.是否可改价,
                            IsGive = p.Property & (int)CytcProperty.是否可赠送,
                            IsUpStore = p.IsOnSale.ObjToInt(),
                            Category = p.R_Category_Id,
                            Describe=p.Describe
                        }).ToList();

                    foreach (var item in packages)
                    {
                        var packageImage = packageImages.FirstOrDefault(p => p.Source_Id == item.Id);
                        imageUrl = packageImage != null ? packageImage.Url : "";
                        imageExt = packageImage != null ? Path.GetExtension(imageUrl) : "";
                        smallImage = packageImage != null ? imageUrl + "320x" + imageExt : "";
                        item.PackageDetailList = db.Sqlable()
                                                .From<R_Project>("s1")
                                                .Join<R_ProjectDetail>("s2", "s1.Id", "s2.R_Project_Id", JoinType.Inner)
                                                .Join<R_PackageDetail>("s3", "s2.Id", "s3.R_ProjectDetail_Id", JoinType.Inner)
                                                .Where(" s1.IsDelete=0 and s2.IsDelete=0 and s3.IsDelete=0 and s1.IsEnable=1 and  s3.R_Package_Id=" + item.Id)
                                                .SelectToList<R_OrderDetailPackageDetail>("(s1.Name+'('+s2.Unit+')') as Name,s3.Num as Num,s2.Id as R_ProjectDetail_Id") ?? new List<R_OrderDetailPackageDetail>();
                        item.CoverUrl = packageImage == null ? "" : packageImage.Url;
                        item.chosenNum = 0;
                        item.smallImage = smallImage;
                    }
                    list = list.Concat(packages).ToList();
                }
                return list;
            }
        }


        public List<ProjectListDTO> GetList(int category)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var data = db.Queryable<R_Project>()
                    .JoinTable<R_Category>((s1, s2) => s1.R_Category_Id == s2.Id)
                    .Where<R_Project, R_Category>((s1, s2) => s1.R_Category_Id == s2.Id);

                if (category > 0)
                {
                    data.Where<R_Project, R_Category>((s1, s2) => s1.R_Category_Id == category);
                }

                // IsCustomer = s1.IsCustomer, IsDelete = s1.IsDelete, IsDiscount = s1.IsDiscount, IsGive = s1.IsGive, IsOnSale = s1.IsOnSale, IsQzdz = s1.IsQzdz, IsRecommend = s1.IsRecommend, 
                var list = data.Select<R_Project, R_Category, ProjectListDTO>((s1, s2) =>
                    new ProjectListDTO()
                    {
                        Id = s1.Id,
                        Name = s1.Name,
                        Description = s1.Description,
                        CostPrice = s1.CostPrice,
                        CreateDate = s1.CreateDate,
                        CategoryId = s1.R_Category_Id,
                        IsStock = s1.IsStock,
                        Price = s1.Price,
                        Stock = s1.Stock,
                        Category = s2.Name,
                        IsOnSale = (s1.Property & (int)CyxmProperty.是否上架).ObjToBool()
                    }).ToList();

                return list;
            }
        }

        public List<ProjectListDTO> GetList(out int total, ProjectSearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                string order = "s1.Id desc";
                List<ProjectListDTO> list = new List<ProjectListDTO>();

                var data = db.Sqlable()
                    .From<R_Project>("s1")
                    .Join<R_Category>("s2", "s1.R_Category_Id", "s2.Id", JoinType.Left)
                    .Where(" s1.IsDelete = 0 ");
                data = data.Where("s1.R_Company_Id=" + companyId);
                if (req.Category > 0)
                {
                    data = data.Where("s1.R_Category_Id=" + req.Category);
                }

                if (!string.IsNullOrEmpty(req.Name))
                {
                    data = data.Where("s1.Name like '%" + req.Name + "%'");
                }

                if (!string.IsNullOrEmpty(req.Sort))
                {
                    if (req.Sort.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        order = "s1.Id desc";
                    }
                    else
                    {
                        order = string.Format("{0} {1}", req.Sort, req.Order);
                    }
                }

                var paras = new { CyxmLb = req.Category, Name = req.Name };
                total = data.Count();

                list = data.SelectToPageList<ProjectListDTO>($@"
                    s1.*,s1.R_Category_Id as CategoryId,s2.Name as Category,
                    s1.Property & {(int)CyxmProperty.是否上架} as IsOnSale,
                    s1.Property & {(int)CyxmProperty.是否可打折} as IsDiscount,
                    s1.Property & {(int)CyxmProperty.是否可赠送} as IsGive,
                    s1.Property & {(int)CyxmProperty.是否强制打折} as IsQzdz,
                    s1.Property & {(int)CyxmProperty.是否推荐} as IsRecommend,
                    s1.Property & {(int)CyxmProperty.是否自定义} as IsCustomer",
                    order, (req.offset / req.limit) + 1, req.limit, null);
                if (list.Any())
                {
                    var projectIds = list.Select(p => p.Id).ToArray();
                    var ProjectStallStatistics = db.Queryable<R_ProjectStall>().JoinTable<R_Stall>((s1, s2) => s1.R_Stall_Id == s2.Id)
                        .Where<R_Stall>((s1, s2) => projectIds.Contains(s1.R_Project_Id))
                        .Select<R_Stall, ProjectStallStatisticsDTO>((s1, s2) => new ProjectStallStatisticsDTO()
                        {
                            ProjectId = s1.R_Project_Id,
                            BillType = s1.BillType,
                            Name = s2.Name
                        }).ToList();
                    foreach (var item in list)
                    {
                        item.TotalStalls = string.Join(",",ProjectStallStatistics.Where(p => item.Id == p.ProjectId && p.BillType == 2).Select(p => p.Name));
                        item.DetailStalls = string.Join(",", ProjectStallStatistics.Where(p => item.Id == p.ProjectId && p.BillType == 1).Select(p => p.Name));
                        //item.TotalProjectStalls = ProjectStallStatistics.Where(p => item.Id == p.ProjectId && p.BillType==2).Select(p => new ProjectStallDTO()
                        //{
                        //    ProjectId=item.Id,
                        //    BillType=p.BillType,
                        //    Name=p.Name
                        //}).ToList();
                        //item.DetailProjectStalls = ProjectStallStatistics.Where(p => item.Id == p.ProjectId && p.BillType == 1).Select(p => new ProjectStallDTO()
                        //{
                        //    ProjectId = item.Id,
                        //    BillType = p.BillType,
                        //    Name = p.Name
                        //}).ToList();
                    }
                }
                return list;
            }
        }

        public ProjectCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ProjectCreateDTO model = null;
                R_Project data = db.Queryable<R_Project>().InSingle(id);
                if (data != null)
                {
                    model = Mapper.Map<R_Project, ProjectCreateDTO>(data);

                    var extends = db.Queryable<R_ProjectExtendAttribute>()
                        .Where(p => p.R_Project_Id == model.Id)
                        .ToList();

                    var details = db.Queryable<R_ProjectDetail>()
                        .Where(p => p.R_Project_Id == model.Id && p.IsDelete==false)
                        .ToList();

                    model.Extends = extends;
                    model.Details = details;

                    int property = data.Property;
                    model.IsDiscount =
                        (property & (int)CyxmProperty.是否可打折) > 0 ?
                        (int)CyxmProperty.是否可打折 : 0;
                    model.IsQzdz =
                        (property & (int)CyxmProperty.是否强制打折) > 0 ?
                        (int)CyxmProperty.是否强制打折 : 0;
                    model.IsOnSale =
                        (property & (int)CyxmProperty.是否上架) > 0 ?
                        (int)CyxmProperty.是否上架 : 0;
                    model.IsRecommend =
                        (property & (int)CyxmProperty.是否推荐) > 0 ?
                        (int)CyxmProperty.是否推荐 : 0;
                    model.IsGive =
                        (property & (int)CyxmProperty.是否可赠送) > 0 ?
                        (int)CyxmProperty.是否可赠送 : 0;
                    model.IsCustomer =
                        (property & (int)CyxmProperty.是否自定义) > 0 ?
                        (int)CyxmProperty.是否自定义 : 0;
                    model.IsChangePrice =
                        (property & (int)CyxmProperty.是否可改价) > 0 ?
                        (int)CyxmProperty.是否可改价 : 0;
                    model.IsChangeNum =
                        (property & (int)CyxmProperty.送厨后可否更改数量) > 0 ?
                        (int)CyxmProperty.送厨后可否更改数量 : 0;
                    model.IsServiceCharge = (property & (int)CyxmProperty.是否收取服务费) > 0 ?
                        (int)CyxmProperty.是否收取服务费 : 0;
                }
                else
                {
                    model = new ProjectCreateDTO();
                    model.IsEnable = 1;
                    model.IsServiceCharge = (int)CyxmProperty.是否收取服务费;
                }
                return model;
            }
        }

        public List<ProjectExtendDTO> GetProjectExtends(int project)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ProjectExtendDTO> res = db.Queryable<R_ProjectExtend>()
                    .JoinTable<R_ProjectExtendAttribute>((s1, s2) => s1.Id == s2.R_ProjectExtend_Id, JoinType.Inner)
                    .Where<R_ProjectExtendAttribute>((s1, s2) => s2.R_Project_Id == project)
                    .Select<R_ProjectExtendAttribute, ProjectExtendDTO>((s1, s2) =>
                        new ProjectExtendDTO
                        {
                            Id = s1.Id,
                            ExtendType = s1.CyxmKzType,
                            Project = s2.R_Project_Id,
                            ProjectExtend = s2.R_ProjectExtend_Id,
                            ProjectExtendName = s1.Name,
                            Price = s1.Price,
                            Unit = s1.Unit
                        }).ToList();
                return res;
            }
        }

        public bool SpecificationSubmit(int cyxmId, List<R_ProjectDetail> list)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();

                    if (list != null && list.Any())
                    {
                        db.Update<R_ProjectDetail>(new
                        {
                            IsDelete = true
                        }, p => p.R_Project_Id == cyxmId);

                        foreach (var item in list)
                        {
                            if (item.Id > 0)
                            {
                                db.Update<R_ProjectDetail>(new R_ProjectDetail
                                {
                                    Id = item.Id,
                                    R_Project_Id = cyxmId,
                                    Unit = item.Unit,
                                    Price = item.Price,
                                    CostPrice = item.CostPrice,
                                    UnitRate = item.UnitRate,
                                    IsDelete = false
                                });
                            }
                            else
                            {
                                item.R_Project_Id = cyxmId;
                                db.Insert<R_ProjectDetail>(item);
                            }
                        }
                    }

                    db.CommitTran();
                }
                catch (Exception)
                {
                    result = false;
                    db.RollbackTran();

                    throw;
                }

                return result;
            }
        }

        public int Update(ProjectCreateDTO req, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                msg = string.Empty;
                int result = 0;
                try
                {
                    db.BeginTran();
                    R_Project model = Mapper.Map<ProjectCreateDTO, R_Project>(req);
                    model.Property = req.IsDiscount + req.IsOnSale + req.IsQzdz + req.IsRecommend +
                        req.IsCustomer + req.IsGive + req.IsChangePrice + req.IsChangeNum + req.IsServiceCharge;

                    bool controlResult = db.Update(model);

                    if (controlResult)
                    {
                        CharsetCodeHelp cch = new CharsetCodeHelp();
                        string spllCode = cch.GetSpellCode(req.Name);
                        string wbCode = cch.GetWBCode(req.Name);

                        db.Update<CharsetCode>(new
                        {
                            Code = spllCode
                        }, p =>
                            p.CharsetSource == Base.CharsetSource.餐饮项目 &&
                            p.CharsetType == Base.CharsetType.拼音 &&
                            p.SourceId == model.Id);

                        db.Update<CharsetCode>(new
                        {
                            Code = wbCode
                        }, p =>
                            p.CharsetSource == Base.CharsetSource.餐饮项目 &&
                            p.CharsetType == Base.CharsetType.五笔 &&
                            p.SourceId == model.Id);
                    }
                    db.CommitTran();
                    result = req.Id;
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    db.RollbackTran();
                }
                return result;
            }
        }

        public void CharcodeInit()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                try
                {
                    db.BeginTran();
                    var projects = db.Queryable<R_Project>().ToList();
                    CharsetCodeHelp cch = new CharsetCodeHelp();
                    string spellCode = string.Empty;
                    string wbCode = string.Empty;
                    List<CharsetCode> list = new List<CharsetCode>();
                    foreach (var item in projects)
                    {
                        spellCode = cch.GetSpellCode(item.Name);
                        wbCode = cch.GetWBCode(item.Name);
                        list.Add(new CharsetCode()
                        {
                            Code=spellCode,
                            SourceId=item.Id,
                            CharsetSource=Base.CharsetSource.餐饮项目,
                            CharsetType=Base.CharsetType.拼音
                        });
                        list.Add(new CharsetCode()
                        {
                            Code = wbCode,
                            SourceId = item.Id,
                            CharsetSource = Base.CharsetSource.餐饮项目,
                            CharsetType = Base.CharsetType.五笔
                        });
                    }
                    db.InsertRange<CharsetCode>(list);
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                }
            }
        }

        /// <summary>
        /// 获取菜品特殊要求分类列表
        /// </summary>
        /// <returns></returns>
        public ProjectExtendSplitListDTO GetProjectExtendSplitList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ProjectExtendSplitListDTO projectExtendSplitList = new ProjectExtendSplitListDTO();
                var data = db.Queryable<R_ProjectExtend>().Select<ProjectExtendDTO>(
                            (s1) => new ProjectExtendDTO()
                            {
                                Id = s1.Id,
                                ExtendType = s1.CyxmKzType,
                                Price = s1.Price,
                                ProjectExtendName = s1.Name,
                                Unit = s1.Unit
                            })
                        .ToList();
                projectExtendSplitList.Extend = data.Where(p => p.ExtendType == CyxmKzType.做法).ToList() ?? new List<ProjectExtendDTO>();
                projectExtendSplitList.ExtendRequire = data.Where(p => p.ExtendType == CyxmKzType.要求).ToList() ?? new List<ProjectExtendDTO>();
                projectExtendSplitList.ExtendExtra = data.Where(p => p.ExtendType == CyxmKzType.配菜).ToList() ?? new List<ProjectExtendDTO>();
                return projectExtendSplitList;
            }
        }

        public List<OrderProjectExtends> GetProjectExtendSplitListNew()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                //ProjectExtendSplitListDTO projectExtendSplitList = new ProjectExtendSplitListDTO();
                //var data = db.Queryable<R_ProjectExtend>().Select<ProjectExtendDTO>(
                //            (s1) => new ProjectExtendDTO()
                //            {
                //                Id = s1.Id,
                //                ExtendType = s1.CyxmKzType,
                //                Price = s1.Price,
                //                ProjectExtendName = s1.Name,
                //                Unit = s1.Unit
                //            })
                //        .ToList();
                //projectExtendSplitList.Extend = data.Where(p => p.ExtendType == CyxmKzType.做法).ToList() ?? new List<ProjectExtendDTO>();
                //projectExtendSplitList.ExtendRequire = data.Where(p => p.ExtendType == CyxmKzType.要求).ToList() ?? new List<ProjectExtendDTO>();
                //projectExtendSplitList.ExtendExtra = data.Where(p => p.ExtendType == CyxmKzType.配菜).ToList() ?? new List<ProjectExtendDTO>();
                //return projectExtendSplitList;
                List<OrderProjectExtends> res = new List<OrderProjectExtends>();
                var extendTypes = db.Queryable<R_ProjectExtendType>()
                    .Where(p=>p.IsDelete==false).ToList();
                var exnteds= db.Queryable<R_ProjectExtend>().Where(p=>p.IsDelete==false)
                    .Select<ProjectExtendDTO>((s1) => new ProjectExtendDTO()
                            {
                                Id = s1.Id,
                                ExtendType = s1.CyxmKzType,
                                Price = s1.Price,
                                ProjectExtendName = s1.Name,
                                Unit = s1.Unit,
                                R_ProjectExtendType_Id = s1.R_ProjectExtendType_Id
                            })
                        .ToList();
                if (extendTypes!=null&& extendTypes.Any())
                {
                    foreach (var item in extendTypes)
                    {
                        var extendList = exnteds.Where(p => item.Id == p.R_ProjectExtendType_Id && p.ExtendType==CyxmKzType.做法);
                        if (extendList.Any())
                        {
                            res.Add(new OrderProjectExtends()
                            {
                                Id = item.Id,
                                ExtendType = CyxmKzType.做法,
                                Name = item.Name,
                                ProjectExtendDTOList = extendList.ToList()
                            });
                        }
                        var extendRequireList = exnteds.Where(p => item.Id == p.R_ProjectExtendType_Id 
                        && p.ExtendType == CyxmKzType.要求);
                        if (extendRequireList.Any())
                        {
                            res.Add(new OrderProjectExtends()
                            {
                                Id = item.Id,
                                ExtendType = CyxmKzType.要求,
                                Name = item.Name,
                                ProjectExtendDTOList = extendRequireList.ToList()
                            });
                        }
                        var extendExtraList = exnteds.Where(p => item.Id == p.R_ProjectExtendType_Id 
                        && p.ExtendType == CyxmKzType.配菜);
                        if (extendExtraList.Any())
                        {
                            res.Add(new OrderProjectExtends()
                            {
                                Id = item.Id,
                                ExtendType = CyxmKzType.配菜,
                                Name = item.Name,
                                ProjectExtendDTOList = extendExtraList.ToList()
                            });
                        }
                    }
                }
                return res;
            }
        }

        public List<OrderProjectExtends> GetProjectExtendSplitListByProjectId(int projectId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<OrderProjectExtends> res = new List<OrderProjectExtends>();
                var extendTypes = db.Queryable<R_ProjectExtendType>()
                    .Where(p => p.IsDelete == false).ToList();
                var exntends = db.Queryable<R_ProjectExtend>()
                    .Where(p => p.IsDelete == false)
                    .Select<ProjectExtendDTO>((s1) => new ProjectExtendDTO()
                    {
                        Id = s1.Id,
                        ExtendType = s1.CyxmKzType,
                        Price = s1.Price,
                        ProjectExtendName = s1.Name,
                        Unit = s1.Unit,
                        R_ProjectExtendType_Id = s1.R_ProjectExtendType_Id
                    }).ToList();
                var projectAttributes = db.Queryable<R_ProjectExtendAttribute>()
                    .Where(p => p.R_Project_Id == projectId).ToList();
                if (projectAttributes.Any())
                {
                    var attributeExtends = projectAttributes.Select(p => p.R_ProjectExtend_Id).ToArray();
                    foreach (var item in exntends)
                    {
                        if (attributeExtends.Contains(item.Id))
                        {
                            item.IsSelect = true;
                        }
                    }
                }
                if (extendTypes != null && extendTypes.Any())
                {
                    foreach (var item in extendTypes)
                    {
                        var extendList = exntends.Where(p => item.Id == p.R_ProjectExtendType_Id && p.ExtendType == CyxmKzType.做法);
                        if (extendList.Any())
                        {
                            res.Add(new OrderProjectExtends()
                            {
                                Id = item.Id,
                                ExtendType = CyxmKzType.做法,
                                Name = item.Name,
                                ProjectExtendDTOList = extendList.ToList()
                            });
                        }
                        var extendRequireList = exntends.Where(p => item.Id == p.R_ProjectExtendType_Id
                        && p.ExtendType == CyxmKzType.要求);
                        if (extendRequireList.Any())
                        {
                            res.Add(new OrderProjectExtends()
                            {
                                Id = item.Id,
                                ExtendType = CyxmKzType.要求,
                                Name = item.Name,
                                ProjectExtendDTOList= extendRequireList.ToList()
                            });
                        }
                        var extendExtraList = exntends.Where(p => item.Id == p.R_ProjectExtendType_Id
                        && p.ExtendType == CyxmKzType.配菜);
                        if (extendExtraList.Any())
                        {
                            res.Add(new OrderProjectExtends()
                            {
                                Id = item.Id,
                                ExtendType = CyxmKzType.配菜,
                                Name = item.Name,
                                ProjectExtendDTOList=extendExtraList.ToList()
                            });
                        }
                    }
                }
                return res;
            }
        }

        public bool ProjectClearSubmit(List<ProjectClearDTO> req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    if (req != null && req.Any())
                    {
                        foreach (var item in req)
                        {
                            db.Update<R_Project>(new { IsStock = item.IsStock, Stock = item.Stock }, p => p.Id == item.Id);
                        }
                    }
                }
                catch (Exception)
                {
                    result = false;
                    db.RollbackTran();
                }
                return result;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    db.Update<R_Project>(new { IsDelete = true }, x => x.Id == id);
                    db.Update<R_ProjectDetail>(new { IsDelete = true }, x => x.R_Project_Id == id);
                    db.CommitTran();
                }
                catch (Exception)
                {
                    db.RollbackTran();
                    result = false;
                }
                return result;
            }
        }

        public bool IsEnable(List<int> ids,bool enable)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    db.Update<R_Project>(new { IsEnable = enable }, x => ids.Contains(x.Id));
                    db.CommitTran();
                }
                catch (Exception)
                {
                    db.RollbackTran();
                    result = false;
                }
                return result;
            }
        }

        public ProjectImageDTO EditProjectImage(ProjectImageDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ProjectImageDTO res = null;
                R_ProjectImage model = Mapper.Map<ProjectImageDTO, R_ProjectImage>(req);
                if (model.Id > 0)
                {
                    model.IsCover = req.IsCover;
                    model.Sorted = req.Sorted;
                    res = db.Update<R_ProjectImage>(model) ? req : null;
                }
                else
                {
                    int insertId = Convert.ToInt32(db.Insert<R_ProjectImage>(model));
                    if (insertId > 0)
                    {
                        res = req;
                        res.Id = insertId;
                    }
                }
                return res;
            }
        }

        public List<ProjectImageDTO> GetProjectImages(int id,int cyxmTpSourceType)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                List<ProjectImageDTO> res = db.Queryable<R_ProjectImage>()
                    .Where(p => p.Source_Id == id && p.CyxmTpSourceType == cyxmTpSourceType)
                    .OrderBy(p=>p.Sorted)
                    .Select<ProjectImageDTO>(p => new ProjectImageDTO()
                    {
                        Id = p.Id,
                        CyxmTpSourceType = p.CyxmTpSourceType,
                        IsCover = p.IsCover,
                        Sorted = p.Sorted,
                        Source_Id = p.Source_Id,
                        Url = p.Url
                    }).ToList();
                if (res.Any())
                {
                    foreach (var item in res)
                    {
                        item.FilePath = item.Url.Substring(0, item.Url.LastIndexOf('/')+1); ;
                        item.FileName = Path.GetFileNameWithoutExtension(item.Url);
                        item.FileExt = Path.GetExtension(item.Url);
                    }
                }
                return res;
            }
        }

        public bool DeleteProjectImage(int piid)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = db.Delete<R_ProjectImage>(p => p.Id == piid);
                return res;
            }
        }

        public bool BathUpdateProjectImage(List<ProjectImageDTO> req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    if (req.Any())
                    {
                        foreach (var item in req)
                        {
                            db.Update<R_ProjectImage>(new { IsCover=item.IsCover,Sorted=item.Sorted }, p => p.Id == item.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public ProjectImageDTO GetProjectImageModel(int id)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var model = db.Queryable<R_ProjectImage>().InSingle(id);
                ProjectImageDTO res = Mapper.Map<R_ProjectImage, ProjectImageDTO>(model);
                return res;
            }
        }

        public bool ProjectRecommendSubmit(List<ProjectRecomandDTO> req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    var baseProperty = 0;
                    var ids = req.Select(p => p.Id).ToArray();
                    var projects = db.Queryable<R_Project>().Where(p => ids.Contains(p.Id)).ToList();
                    var projectsUpdate = new List<R_Project>();
                    foreach (var item in req)
                    {
                        var model = projects.FirstOrDefault(p => p.Id == item.Id);
                        if (model!=null)
                        {
                            baseProperty = model.Property;
                            if (item.IsRecomand)
                            {
                                baseProperty += (baseProperty & (int)CyxmProperty.是否推荐) <= 0 ? (int)CyxmProperty.是否推荐 : 0;
                            }
                            else
                            {
                                baseProperty += (baseProperty & (int)CyxmProperty.是否推荐) > 0 ? -(int)CyxmProperty.是否推荐 : 0;
                            }
                            if (item.IsSpecialOffer)
                            {
                                baseProperty += (baseProperty & (int)CyxmProperty.今日特价) <= 0 ? (int)CyxmProperty.今日特价 : 0;
                            }
                            else
                            {
                                baseProperty += (baseProperty & (int)CyxmProperty.今日特价) > 0 ? -(int)CyxmProperty.今日特价 : 0;
                            }
                            model.Property = baseProperty;
                            projectsUpdate.Add(model);
                        }
                    }
                    if (projectsUpdate.Any())
                    {
                        db.UpdateRange<R_Project>(projectsUpdate);
                    }
                }
                catch (Exception ex)
                {
                    res = false;
                    db.RollbackTran();
                    throw ex;
                }
                return res;
            }
        }

        public List<PrinterProject> OrderDetailPrintTesting(List<OrderDetailDTO> req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                List<PrinterProject> res = new List<PrinterProject>();
                try
                {
                    var newReq = req.Where(p => p.Id == 0 || (p.Id > 0 && p.CyddMxStatus == CyddMxStatus.保存)).ToList();
                    var projectIds = newReq.Where(p => p.CyddMxType == CyddMxType.餐饮项目).Select(p => p.R_Project_Id).ToList(); //餐饮项目Ids
                    var projectDetailIds = new List<int>();
                    newReq.Where(p => p.CyddMxType == CyddMxType.餐饮套餐).ToList().ForEach(n =>
                    {
                        projectDetailIds = projectDetailIds.Concat(n.PackageDetailList.Select(p => p.R_ProjectDetail_Id).ToList()).ToList();    //套餐餐饮项目明细IDS
                    });
                    var packageProjects = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).Select(p => p.R_Project_Id).ToList();
                    projectIds = projectIds.Concat(packageProjects).ToList();    //连接餐饮项目IDS和套餐包含的餐饮项目IDS
                    projectIds = projectIds.GroupBy(p => p).Select(p => p.Key).ToList();
                    var list = db.Queryable<Printer>()
                        .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id && s2.IsDelete == false)
                        .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                        .Where<R_Stall, R_ProjectStall>((s1, s2, s3) => projectIds.Contains(s3.R_Project_Id) && s1.IsDelete == false && s1.RealStatus==PrintStatus.异常)
                        .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                        {
                            Id = s1.Id,
                            Code = s1.Code,
                            IpAddress = s1.IpAddress,
                            IsDelete = s1.IsDelete,
                            Name = s1.Name,
                            PcName = s1.PcName,
                            PrintPort = s1.PrintPort,
                            Remark = s1.RealStatusRemark,
                            ProjectId = s3.R_Project_Id,
                            BillType = s3.BillType,
                            StallName = s2.Name,
                            StallId = s2.Id
                        }).ToList();    //餐饮项目打印机列表

                    res = (from p in list
                           group p by new { p.Id, p.Name, p.StallName, p.Remark } into g
                           select new PrinterProject()
                           {
                               Id=g.Key.Id,
                               Name=g.Key.Name,
                               StallName=g.Key.StallName,
                               Remark=g.Key.Remark
                           }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return res;
            }
        }

        public bool GetOrderDetailIsTeset()
        {
            return OrderDetailPrintTest;
        }
    }
}