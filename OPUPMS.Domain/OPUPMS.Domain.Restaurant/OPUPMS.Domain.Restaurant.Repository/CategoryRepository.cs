using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class CategoryRepository : SqlSugarService, ICategoryRepository
    {
        public CategoryRepository()
        {
        }

        public bool Create(CategoryCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Category model = new R_Category()
                {
                    Name = req.Name,
                    Description = req.Description,
                    DiscountRate = req.DiscountRate,
                    PId = req.Pid,
                    IsDiscount = req.IsDiscount,
                    Sorted=req.Sorted,
                    R_Company_Id=req.R_Company_Id
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<CategoryListDTO> GetChildList(bool includeDelete = false)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<CategoryListDTO> res = new List<CategoryListDTO>();
                var data = db.Queryable<R_Category>()
                    .Where(p=>p.PId > 0);
                if (!includeDelete)
                {
                    data = data.Where(p => p.IsDelete == false);
                }
                res=data.Select<R_Category, CategoryListDTO>((s1) => new CategoryListDTO()
                {
                    Id = s1.Id,
                    Description = s1.Description,
                    DiscountRate = s1.DiscountRate,
                    Name = s1.Name,
                    Pid = s1.PId,
                    IsDiscount = s1.IsDiscount
                }).ToList();
                #region
                //if (!includeDelete)
                //{
                //    res = db.Queryable<R_Category>()
                //    .Where<R_Category>((s1) => s1.PId > 0 && s1.IsDelete == false)
                //    .Select<R_Category, CategoryListDTO>((s1) => new CategoryListDTO()
                //    {
                //        Id = s1.Id,
                //        Description = s1.Description,
                //        DiscountRate = s1.DiscountRate,
                //        Name = s1.Name,
                //        Pid = s1.PId,
                //        IsDiscount = s1.IsDiscount
                //    }).ToList();
                //}
                //else
                //{
                //    res = db.Queryable<R_Category>()
                //    .Where<R_Category>((s1) => s1.PId > 0)
                //    .Select<R_Category, CategoryListDTO>((s1) => new CategoryListDTO()
                //    {
                //        Id = s1.Id,
                //        Description = s1.Description,
                //        DiscountRate = s1.DiscountRate,
                //        Name = s1.Name,
                //        Pid = s1.PId,
                //        IsDiscount = s1.IsDiscount
                //    }).ToList();
                //}
                #endregion
                return res;
            }
        }

        public List<CategoryListDTO> GetList(int pid = 0)
        {
            var companyId= OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                List<CategoryListDTO> res = new List<CategoryListDTO>();
                var data = db.Queryable<R_Category>().Where(p => p.IsDelete == false && p.R_Company_Id==companyId);

                if (pid > 0)
                {
                    data = data.Where(p => p.PId == pid);
                }

                res = data.Select(p => new CategoryListDTO()
                {
                    Id = p.Id,
                    Description = p.Description,
                    DiscountRate = p.DiscountRate,
                    Name = p.Name,
                    Pid = p.PId,
                    IsDiscount = p.IsDiscount
                }).ToList();
                return res;
            }
        }
        /// <summary>
        /// 点餐菜品所有分类，包含父类和子类
        /// </summary>
        public List<AllCategoryListDTO> GetAllCategoryList(bool includeDelete = false)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                List<AllCategoryListDTO> res = new List<AllCategoryListDTO>();
                var data = db.Queryable<R_Category>();
                var dataList = data.ToList();
                if (!includeDelete)
                {
                    data = data.Where(p => p.IsDelete == false);
                }
                res=data.Where(p=>p.PId==0 && p.R_Company_Id==companyId).OrderBy(p=>p.Sorted)
                    .Select(p => new AllCategoryListDTO()
                    {
                        Id = p.Id,
                        Description = p.Description,
                        DiscountRate = p.DiscountRate,
                        Name = p.Name,
                        Pid = p.PId,
                        IsDiscount = p.IsDiscount,
                        Sorted = p.Sorted
                    }).ToList() ?? new List<AllCategoryListDTO>();
                #region
                //if (!includeDelete)
                //{
                //    res = data.Where(p => p.PId == 0 && p.IsDelete == false).OrderBy(p => p.Sorted)
                //    .Select(p => new AllCategoryListDTO()
                //    {
                //        Id = p.Id,
                //        Description = p.Description,
                //        DiscountRate = p.DiscountRate,
                //        Name = p.Name,
                //        Pid = p.PId,
                //        IsDiscount = p.IsDiscount,
                //        Sorted = p.Sorted
                //    }).ToList() ?? new List<AllCategoryListDTO>();
                //}
                //else
                //{
                //    res = data.Where(p => p.PId == 0).OrderBy(p => p.Sorted)
                //    .Select(p => new AllCategoryListDTO()
                //    {
                //        Id = p.Id,
                //        Description = p.Description,
                //        DiscountRate = p.DiscountRate,
                //        Name = p.Name,
                //        Pid = p.PId,
                //        IsDiscount = p.IsDiscount,
                //        Sorted = p.Sorted
                //    }).ToList() ?? new List<AllCategoryListDTO>();
                //}
                #endregion
                foreach (var item in res)
                {
                    item.ChildList = dataList.Where(p => p.PId == item.Id && p.IsDelete == false).OrderBy(p => p.Sorted)
                        .Select(p => new CategoryListDTO()
                    {
                        Id = p.Id,
                        Description = p.Description,
                        DiscountRate = p.DiscountRate,
                        Name = p.Name,
                        Pid = p.PId,
                        IsDiscount = p.IsDiscount,
                        Sorted=p.Sorted
                    }).ToList() ?? new List<CategoryListDTO>();
                }
                return res;
            }
        }

        public List<CategoryListDTO> GetList(out int total, CategorySearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = "s1.Id desc";
                List<CategoryListDTO> list = new List<CategoryListDTO>();

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

                var data = db.Queryable<R_Category>()
                    .JoinTable<R_Category>((s1, s2) => s1.PId == s2.Id && s2.IsDelete == false && s1.R_Company_Id==companyId)
                    .Select("s1.*,s2.Name as Pname")
                    .Where(" s1.IsDelete = 0  ");
                    //.OrderBy(order).ToDataTable();
                if (req.Pid > 0)
                {
                    data = data.Where("s1.Pid=" + req.Pid);
                }
                var _data=data.OrderBy(order).ToDataTable();
                if (data != null && _data.Rows.Count > 0)
                {
                    totalCount = _data.Rows.Count;
                    var rows = _data.Rows.Cast<DataRow>();
                    var curRows = rows.Skip(req.offset).Take(req.limit).ToArray();

                    foreach (DataRow item in curRows)
                    {
                        list.Add(new CategoryListDTO()
                        {
                            Id = Convert.ToInt32(item["id"]),
                            Name = item["Name"].ToString(),
                            Description = item["Description"].ToString(),
                            DiscountRate = Convert.ToDecimal(item["DiscountRate"]),
                            Pid = Convert.ToInt32(item["Pid"]),
                            Pname = item["Pname"].ToString(),
                            IsDiscount = Convert.ToBoolean(item["IsDiscount"])
                        });
                    }
                }

                total = totalCount;
                return list;
            }
        }

        public CategoryCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                CategoryCreateDTO result = null;
                var model = db.Queryable<R_Category>().InSingle(id);

                if (model != null)
                {
                    result = new CategoryCreateDTO()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        DiscountRate = model.DiscountRate,
                        Pid = model.PId,
                        IsDiscount = model.IsDiscount,
                        Sorted=model.Sorted
                    };
                }

                return result;
            }
        }

        public bool Update(CategoryCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Category model = new R_Category()
                {
                    Id = req.Id,
                    Name = req.Name,
                    Description = req.Description,
                    DiscountRate = req.DiscountRate,
                    PId = req.Pid,
                    IsDiscount = req.IsDiscount,
                    Sorted=req.Sorted
                };

                result = db.Update(model);
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
                result = db.Update<R_Category>(new { IsDelete = true }, x => x.Id == id);
                var pid = db.Queryable<R_Category>().Where(p => p.Id == id).FirstOrDefault();
                if (pid.PId == 0)//说明删除的是父类，子类也要删除
                {
                    db.Update<R_Category>(new { IsDelete = true }, x => x.PId == id);
                }
                return result;
            }
        }

    }
}
