using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class PackageRepository : SqlSugarService, IPackageRepository
    {
        public PackageRepository()
        {
        }

        public int Create(PackageCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int result = 0;
                R_Package model = Mapper.Map<PackageCreateDTO, R_Package>(req);
                model.Property = req.IsDiscount + req.IsChangePrice + req.IsGive;
                var insertResult = db.Insert(model);
                if (insertResult != null)
                {
                    result = Convert.ToInt32(insertResult);
                }

                return result;
            }
        }

        public bool DetailCreate(PackageCreateDTO cytc, List<PackageDetailCreateDTO> list)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;

                try
                {
                    db.BeginTran();
                    db.Update<R_Package>(new { Price = cytc.Price, CostPrice = cytc.CostPrice }, p => p.Id == cytc.Id);
                    if (list != null && list.Any())
                    {
                        db.Update<R_PackageDetail>(new
                        {
                            IsDelete = true
                        }, p => p.R_Package_Id == cytc.Id);

                        foreach (var item in list)
                        {
                            if (item.Id > 0)
                            {
                                db.Update<R_PackageDetail>(new R_PackageDetail
                                {
                                    Id = item.Id,
                                    R_Package_Id = cytc.Id,
                                    R_ProjectDetail_Id = item.R_ProjectDetail_Id,
                                    Num = item.Num,
                                    IsDelete = false
                                });
                            }
                            else
                            {
                                R_PackageDetail model = new R_PackageDetail()
                                {
                                    R_Package_Id = cytc.Id,
                                    R_ProjectDetail_Id = item.R_ProjectDetail_Id,
                                    IsDelete = false,
                                    Num = item.Num
                                };
                                db.Insert<R_PackageDetail>(model);
                            }
                        }
                        db.Update<R_Package>(new
                        {
                            CostPrice = cytc.CostPrice,
                            Price = cytc.Price
                        }, p => p.Id == cytc.Id);
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

        public List<PackageListDTO> GetList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<PackageListDTO> list = new List<PackageListDTO>();
                var data = db.Sqlable().From<R_Package>("s1").Where(" s1.IsDelete = 0 ");
                list = data.SelectToList<PackageListDTO>("s1.*");
                //list = data.SelectToPageList<PackageListDTO>(
                //    "s1.*", order, (req.offset / req.limit) + 1, req.limit, null);
                return list;
            }
        }

        public List<PackageListDTO> GetList(out int total, PackageSearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                List<PackageListDTO> list = new List<PackageListDTO>();
                int totalCount = 0;
                string order = "s1.Id desc";

                var data = db.Sqlable().From<R_Package>("s1").Where(" s1.IsDelete = 0 ");
                data = data.Where("s1.R_Company_Id=" + companyId);
                if (!string.IsNullOrEmpty(req.Name))
                {
                    data = data.Where("s1.Name like '%" + req.Name + "%'");
                }

                if (req.MinPrice > 0)
                {
                    data = data.Where("s1.Price>=" + req.MinPrice);
                }

                if (req.MaxPrice > 0)
                {
                    data = data.Where("s1.Price <= " + req.MaxPrice);
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

                totalCount = data.Count();
                list = data.SelectToPageList<PackageListDTO>(
                    "s1.*", order, (req.offset / req.limit) + 1, req.limit, null);
                total = totalCount;

                return list;
            }
        }

        public PackageCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                PackageCreateDTO model = null;
                R_Package data = db.Queryable<R_Package>().InSingle(id);

                if (data != null)
                {
                    model = Mapper.Map<R_Package, PackageCreateDTO>(data);
                    model.IsDiscount = data.Property & (int)CytcProperty.是否可打折;
                    model.IsChangePrice = data.Property & (int)CytcProperty.是否可改价;
                    model.IsGive = data.Property & (int)CytcProperty.是否可赠送;

                    //var cytcMxList = db.Queryable<R_PackageDetail>()
                    //    .JoinTable<R_ProjectDetail>((s1,s2)=>s1.R_ProjectDetail_Id==s2.Id,JoinType.Left)
                    //    .JoinTable<R_ProjectDetail,R_Project>((s1,s2,s3)=>s2.R_Project_Id==s3.Id,JoinType.Left)
                    //    .Where<R_Project>(p => p.R_Package_Id == model.Id && p.IsDelete == false).ToList();
                    var cytcMxList = db.Queryable<R_PackageDetail>()
                        .JoinTable<R_ProjectDetail>((s1, s2) => s1.R_ProjectDetail_Id == s2.Id, JoinType.Left)
                        .JoinTable<R_ProjectDetail, R_Project>((s1, s2, s3) => s2.R_Project_Id == s3.Id, JoinType.Left)
                        .Where<R_Project>((s1,s3)=>s1.R_Package_Id == model.Id && s1.IsDelete == false && s3.IsEnable==true)
                        .Select("s1.*").ToList();

                    model.PackageDetails = cytcMxList;
                }

                return model;
            }
        }

        public int Update(PackageCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int result = 0;
                var baseModel = db.Queryable<R_Package>()
                    .First(p => p.Id == req.Id);
                R_Package model = new R_Package()
                {
                    Id = req.Id,
                    Name = req.Name,
                    IsOnSale = req.IsOnSale,
                    Property = req.IsDiscount + req.IsChangePrice + req.IsGive,
                    Describe = req.Describe,
                    CostPrice = baseModel.CostPrice,
                    Price = baseModel.Price,
                    R_Category_Id=req.R_Category_Id
                };
                result = db.Update(model) ? req.Id : 0;
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
                result = db.Update<R_Package>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }

    }
}