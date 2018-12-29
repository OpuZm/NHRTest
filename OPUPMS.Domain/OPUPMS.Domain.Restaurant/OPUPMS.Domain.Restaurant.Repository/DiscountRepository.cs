using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class DiscountRepository : SqlSugarService, IDiscountRepository
    {
        public DiscountRepository()
        {

        }

        public bool Create(DiscountCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;

                try
                {
                    db.BeginTran();

                    R_Discount model = new R_Discount()
                    {
                        R_Restaurant_Id = req.Restaurant,
                        R_Area_Id = req.Area,
                        R_Market_Id = req.Market,
                        IsEnable = req.IsEnable,
                        Name = req.Name,
                        StartDate = req.StartDate,
                        EndDate = req.EndDate,
                        R_Company_Id=req.CompanyId
                    };

                    var newId = db.Insert(model);

                    if (req.CyxmZkCp != null)
                    {
                        var list = req.CyxmZkCp.Where(p => p.Id == 0).ToList();
                        list.ForEach(p => p.R_Discount_Id = Convert.ToInt32(newId));
                        //插入之前先判断是否存在，只插入不存在的
                        foreach (var item in list)
                        {
                            //查询是否存在
                            List<R_DiscountCategory> data = db.Queryable<R_DiscountCategory>()
                                .Where(p => p.R_Discount_Id == Convert.ToInt32(newId))
                                .Where(p => p.R_Category_Id == item.R_Category_Id)
                                .ToList();
                            //插入不存在的
                            if (data.Count <= 0)
                            {
                                db.Insert(item);
                            }
                        }
                        //db.InsertRange<R_DiscountCategory>(list);
                    }

                    db.CommitTran();
                }
                catch (Exception)
                {
                    db.RollbackTran();
                }

                return result;
            }
        }

        public List<DiscountDTO> GetList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<DiscountDTO> list = db.Sqlable()
                    .From<R_Discount>("s1")
                    .Join<R_Restaurant>("s2", "s1.R_Restaurant_Id", "s2.Id", JoinType.Left)
                    .Join<R_Market>("s3", "s1.R_Market_Id", "s3.Id", JoinType.Left)
                    .Join<R_Area>("s4", "s1.R_Area_Id", "s4.Id", JoinType.Left)
                    .SelectToList<DiscountDTO>(@"s1.Id,s1.Name,s3.Name as Market,
                        s2.Name as Restaurant,s4.Name as Area,s1.IsEnable");
                return list;
            }
        }
        

        public List<DiscountDTO> GetList(out int total, PayDiscountSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string order = "s1.Id desc";

                var data = db.Sqlable()
                    .From<R_Discount>("s1")
                    .Join<R_Restaurant>("s2", "s1.R_Restaurant_Id", "s2.Id", JoinType.Left)
                    .Join<R_Market>("s3", "s1.R_Market_Id", "s3.Id", JoinType.Left)
                    .Join<R_Area>("s4", "s1.R_Area_Id", "s4.Id", JoinType.Left);

                if (!string.IsNullOrEmpty(req.Name))
                {
                    data = data.Where("s1.Name like '%" + req.Name + "%'");
                }

                if (req.Restaurant > 0)
                {
                    data = data.Where("s2.Id=" + req.Restaurant);
                }

                if (!string.IsNullOrEmpty(req.StartDate))
                {
                    data = data.Where("s1.StartDate>=" + req.StartDate);
                }

                if (!string.IsNullOrEmpty(req.EndDate))
                {
                    data = data.Where("s1.EndDate<=" + req.EndDate);
                }

                if (req.IsEnable==true)
                {
                    data = data.Where("s1.IsEnable=1");
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

                if(!string.IsNullOrEmpty(req.CurrentDate))
                {
                   data = data.Where("'" + req.CurrentDate + "' BETWEEN s1.StartDate  AND s1.EndDate");
                }

                if (!string.IsNullOrEmpty(req.OrderCreateDate))
                {
                    data = data.Where("'" + req.OrderCreateDate + "' BETWEEN s1.StartDate  AND s1.EndDate");

                }

                total = data.Count();
                List<DiscountDTO> list = data.SelectToPageList<DiscountDTO>(
                    @"s1.Id,s1.Name,s3.Name as Market,s2.Name as Restaurant,
                      s4.Name as Area,s1.IsEnable,s1.StartDate,s1.EndDate",
                    order, (req.offset / req.limit) + 1, req.limit, null);

                foreach(var item in list)
                {
                    item.CyxmZkCp = db.Queryable<R_DiscountCategory>().
                        Where(p => p.R_Discount_Id == item.Id).ToList();
                }
                return list;
            }

        }

        public List<DiscountDTO> GetList(out int total, DiscountSearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                string order = "s1.Id desc";

                var data = db.Sqlable()
                    .From<R_Discount>("s1")
                    .Join<R_Restaurant>("s2", "s2.IsDelete = 0 and s1.R_Restaurant_Id", "s2.Id", JoinType.Left)
                    .Join<R_Market>("s3", "s3.IsDelete = 0 and s1.R_Market_Id", "s3.Id", JoinType.Left)
                    .Join<R_Area>("s4", " s4.IsDelete = 0 and  s1.R_Area_Id", "s4.Id", JoinType.Left)
                    .Where(" s1.IsDelete = 0   ");
                data = data.Where("s1.R_Company_Id=" + companyId);
                if (!string.IsNullOrEmpty(req.Name))
                {
                    data = data.Where("s1.Name like '%" + req.Name + "%'");
                }

                if (req.Restaurant > 0)
                {
                    data = data.Where("s2.Id=" + req.Restaurant);
                }

                if (!string.IsNullOrEmpty(req.StartDate))
                {
                    data = data.Where("s1.StartDate>='" + req.StartDate+"'");
                }

                if (!string.IsNullOrEmpty(req.EndDate))
                {
                    data = data.Where("s1.EndDate<='" + req.EndDate + "'");
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

                total = data.Count();
                List<DiscountDTO> list = data.SelectToPageList<DiscountDTO>(
                    @"s1.Id,s1.Name,s3.Name as Market,s2.Name as Restaurant,
                      s4.Name as Area,s1.IsEnable,s1.StartDate,s1.EndDate",
                    order, (req.offset / req.limit) + 1, req.limit, null);

                return list;
            }
        }

        public DiscountCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                DiscountCreateDTO model = null;
                R_Discount data = db.Queryable<R_Discount>().InSingle(id);

                if (data != null)
                {
                    model = new DiscountCreateDTO()
                    {
                        Id = data.Id,
                        Area = data.R_Area_Id,
                        Restaurant = data.R_Restaurant_Id,
                        Market = data.R_Market_Id,
                        Name = data.Name,
                        IsEnable = data.IsEnable,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate
                    };
                    model.CyxmZkCp = db.Queryable<R_DiscountCategory>().
                        Where(p => p.R_Discount_Id == data.Id).ToList();
                }

                return model;
            }
        }

        public bool Update(DiscountCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();

                    R_Discount model = new R_Discount()
                    {
                        Id = req.Id,
                        R_Restaurant_Id = req.Restaurant,
                        R_Area_Id = req.Area,
                        R_Market_Id = req.Market,
                        IsEnable = req.IsEnable,
                        Name = req.Name,
                        StartDate = req.StartDate,
                        EndDate = req.EndDate
                    };
                    db.Update(model);

                    if (req.CyxmZkCp != null)
                    {
                        var list = req.CyxmZkCp.Where(p => p.Id == 0).ToList();
                        //db.InsertRange<R_DiscountCategory>(list);

                        //插入之前先判断是否存在，只插入不存在的
                        foreach (var item in list)
                        {
                            //查询是否存在
                            List<R_DiscountCategory> data = db.Queryable<R_DiscountCategory>()
                                .Where(p => p.R_Discount_Id == req.Id)
                                .Where(p => p.R_Category_Id == item.R_Category_Id).ToList();
                            //插入不存在的
                            if (data.Count <= 0)
                            {
                                db.Insert(item);
                            }
                        }

                    }

                    db.CommitTran();
                }
                catch (Exception)
                {
                    result = false;
                }

                return result;
            }
        }


        public List<SchemeDiscountDTO> GetSchemeDiscountList(SchemeDiscountSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var paras = SqlSugarTool.GetParameters(new
                {
                    RestaurantId = req.RestaurantId,
                    MarketId = req.MarketId,
                    OrderId=req.OrderId==0?33:req.OrderId
                });

                string sql = $@" SELECT D.*, ISNULL(R.Id, 0) AS RestaurantId, ISNULL(M.Id, 0) AS MarketId, 
                                ISNULL(A.Id, 0) AS AreaId, ISNULL(A.Name, '') AS AreaName 
                            FROM dbo.R_Discount D 
                            LEFT JOIN dbo.R_Restaurant R ON R.Id = D.R_Restaurant_Id
                            LEFT JOIN dbo.R_Market M ON M.Id = D.R_Area_Id
                            LEFT JOIN dbo.R_Area A ON A.Id = D.R_Area_Id
                            left join dbo.R_Order OD on R.Id=OD.R_Restaurant_Id
                            WHERE (GETDATE() BETWEEN D.StartDate AND D.EndDate) AND D.IsEnable = 1 
                                AND D.R_Restaurant_Id = @RestaurantId and D.IsDelete=0 
                                and OD.Id=@OrderId
                                and (OD.R_Market_Id=D.R_Market_Id OR 2=0)";
                //AND (D.R_Market_Id = @MarketId OR D.R_Market_Id = 0)
                var list = db.SqlQuery<SchemeDiscountDTO>(sql, paras);

                if (list != null && list.Any())
                {
                    var discountIds = list.Select(x => x.Id).ToArray().Join(",");
                    var detailParas = SqlSugarTool.GetParameters(new { DiscountIds = discountIds });
                    string detailSql = "SELECT DC.*, D.Id AS SchemeId, D.[Name] AS SchemeName, " +
                                        "D.R_Area_Id AS AreaId, D.R_Market_Id AS MarketId, " +
                                        "isnull(C.Id,0) AS CategoryId, isnull(C.Name,'全部') AS CategoryName " +
                                "FROM dbo.R_DiscountCategory DC " +
                                "left JOIN dbo.R_Discount D ON D.Id = DC.R_Discount_Id " +
                                "left JOIN dbo.R_Category C ON C.Id = DC.R_Category_Id " +
                                "WHERE DC.R_Discount_Id IN ({0})";
                    detailSql = string.Format(detailSql, discountIds);
                    var detailList = db.SqlQuery<SchemeDiscountDetailDTO>(detailSql);

                    foreach (var item in list)
                    {
                        item.DetailList = detailList.Where(x => x.SchemeId == item.Id).ToList();
                    }
                }

                return list;
            }
        }

        public List<SchemeDiscountDetailDTO> GetSchemeDetailListById(int discountId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string detailSql = "SELECT DC.*, D.Id AS SchemeId, D.[Name] AS SchemeName, " +
                                    "D.R_Area_Id AS AreaId, D.R_Market_Id AS MarketId, " +
                                    "C.Id AS CategoryId, C.Name AS CategoryName " +
                            "FROM dbo.R_DiscountCategory DC " +
                            "INNER JOIN dbo.R_Discount D ON D.Id = DC.R_Discount_Id " +
                            "INNER JOIN dbo.R_Category C ON C.Id = DC.R_Category_Id " +
                            "WHERE DC.R_Discount_Id = @DiscountId";
                var detailList = db.SqlQuery<SchemeDiscountDetailDTO>(detailSql, new { DiscountId = discountId });

                return detailList;
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
                result = db.Update<R_Discount>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }

    }
}