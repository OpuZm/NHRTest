using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Infrastructure.Common;
using SqlSugar;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Base.Models;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class StatisticsRepository : SqlSugarService,IStatisticsRepository
    {
        public List<ProducedStatisticsDTO> Produced(ProducedSearchDTO req)
        {
            using (var db= new SqlSugarClient(Connection))
            {
                List<ProducedStatisticsDTO> res = new List<ProducedStatisticsDTO>();
                try
                {
                    req.StartDate = req.StartDate == null ? DateTime.MinValue : req.StartDate;
                    if (req.EndDate != null)
                    {
                        req.EndDate = req.EndDate.Value.AddDays(1);
                    }
                    var dataBase = db.Queryable<R_Project>()
                        .JoinTable<R_ProjectDetail>((s1, s2) => s1.Id == s2.R_Project_Id)
                        .JoinTable<R_ProjectDetail, R_OrderDetail>((s1, s2, s3) => s2.Id == s3.CyddMxId)
                        .JoinTable<R_Category>((s1,s4)=>s1.R_Category_Id==s4.Id)
                        .JoinTable<R_OrderDetail,R_OrderTable>((s1,s3,s5)=>s3.R_OrderTable_Id==s5.Id)
                        .JoinTable<R_OrderTable, R_Order>((s1,s5,s6)=>s5.R_Order_Id==s6.Id)
                        .Where<R_OrderDetail, R_Order>((s1, s3,s6) => s3.CyddMxType == CyddMxType.餐饮项目
                        && (s6.R_Restaurant_Id==req.RestaurantId && s6.IsDelete==false)
                        && (req.StartDate.Value == null || s3.CreateDate >= req.StartDate.Value)
                        && (req.EndDate.Value == null || s3.CreateDate <= req.EndDate.Value)
                        && s3.CyddMxStatus!=CyddMxStatus.保存)
                        .Select<ProducedStatisticsDTO>("s1.Id,s1.Name,s1.R_Category_Id as CategoryId,s4.Pid as ParentCategoryId,sum(s3.num) as Num")
                        .GroupBy("s1.Id,s1.Name,s1.R_Category_Id,s4.Pid").ToList();

                    switch (req.Type)
                    {
                        case ProducedType.菜品大类:
                            var parentCategorys = db.Queryable<R_Category>().Where(p => p.PId == 0).ToList();
                            foreach (var item in parentCategorys)
                            {
                                ProducedStatisticsDTO model = new ProducedStatisticsDTO();
                                if (dataBase.Any(p=>p.ParentCategoryId==item.Id))
                                {
                                    model = dataBase.Where(p => p.ParentCategoryId == item.Id)
                                        .GroupBy(p => new { p.ParentCategoryId })
                                    .Select(p => new ProducedStatisticsDTO
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        ParentCategoryId = item.Id,
                                        Num = p.Sum(g => g.Num)
                                    }).FirstOrDefault();
                                }
                                else
                                {
                                    model = new ProducedStatisticsDTO()
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        ParentCategoryId = item.Id,
                                        Num = 0
                                    };
                                }
                                res.Add(model);
                            }
                            break;
                        case ProducedType.菜品分类:
                            var categorys = db.Queryable<R_Category>().Where(p => p.PId==req.ParentCategoryId).ToList();
                            foreach (var item in categorys)
                            {
                                ProducedStatisticsDTO model = new ProducedStatisticsDTO();
                                if (dataBase.Any(p => p.CategoryId == item.Id))
                                {
                                    model = dataBase.Where(p => p.CategoryId == item.Id)
                                        .GroupBy(p => new { p.CategoryId })
                                    .Select(p => new ProducedStatisticsDTO
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        CategoryId = item.Id,
                                        Num = p.Sum(g => g.Num)
                                    }).FirstOrDefault();
                                }
                                else
                                {
                                    model = new ProducedStatisticsDTO()
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        CategoryId = item.Id,
                                        Num = 0
                                    };
                                }
                                res.Add(model);
                            }
                            break;
                        case ProducedType.菜品信息:
                            var projects = db.Queryable<R_Project>()
                                .Where(p => p.R_Category_Id == req.CategoryId).ToList();
                            foreach (var item in projects)
                            {
                                ProducedStatisticsDTO model = new ProducedStatisticsDTO();
                                if (dataBase.Any(p => p.Id == item.Id))
                                {
                                    model = dataBase.FirstOrDefault(p => p.Id == item.Id);
                                }
                                else
                                {
                                    model = new ProducedStatisticsDTO()
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        CategoryId = item.R_Category_Id,
                                        Num = 0
                                    };
                                }
                                res.Add(model);
                            }
                            break;
                        case ProducedType.套餐:
                            var packages = db.Queryable<R_Package>().ToList();
                            var packageBase = db.Queryable<R_Package>()
                        .JoinTable<R_OrderDetail>((s1, s2) => s1.Id == s2.CyddMxId)
                        .JoinTable<R_OrderDetail, R_OrderTable>((s1, s2, s3) => s2.R_OrderTable_Id == s3.Id)
                        .JoinTable<R_OrderTable, R_Order>((s1, s3, s4) => s3.R_Order_Id == s4.Id)
                        .Where<R_OrderDetail, R_Order>((s1, s2, s4) => s2.CyddMxType == CyddMxType.餐饮套餐
                        && (s4.R_Restaurant_Id == req.RestaurantId && s4.IsDelete==false)
                        && (req.StartDate.Value == null || s2.CreateDate >= req.StartDate.Value)
                        && (req.EndDate.Value == null || s2.CreateDate <= req.EndDate.Value)
                        && s2.CyddMxStatus != CyddMxStatus.保存)
                        .Select<ProducedStatisticsDTO>("s1.Id,s1.Name,sum(s2.Num) as Num")
                        .GroupBy("s1.Id,s1.Name").ToList();
                            foreach (var item in packages)
                            {
                                ProducedStatisticsDTO model = new ProducedStatisticsDTO();
                                if (packageBase.Any(p=>p.Id==item.Id))
                                {
                                    model = packageBase.FirstOrDefault(p => p.Id == item.Id);
                                }
                                else
                                {
                                    model = new ProducedStatisticsDTO()
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        Num = 0
                                    };
                                }
                                res.Add(model);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return res;
            }
        }

        public List<TurnDutyStatisticsGroupDto> GetTurnDuty(TurnDutySearchDTO req)
        {
            using (var db= new SqlSugarClient(Connection))
            {
                List<TurnDutyStatisticsGroupDto> res = new List<TurnDutyStatisticsGroupDto>();
                try
                {
                    req.StartDate = req.StartDate == null ? DateTime.Now : req.StartDate;
                    var marketList = db.Queryable<R_Market>()
                        .Where(p => p.R_Restaurant_Id == req.RestaurantId).ToList();
                    var dataBase = db.Queryable<R_OrderPayRecord>()
                    .JoinTable<R_Order>((s1, s2) => s1.R_Order_Id == s2.Id)
                    .JoinTable<R_Order, R_Restaurant>((s1, s2, s3) => s2.R_Restaurant_Id == s3.Id)
                    .JoinTable<R_Market>((s1, s4) => s1.R_Market_Id == s4.Id)
                    .JoinTable<Czdm>((s1, s5) => s1.CreateUser == s5.Id)
                    .Where<R_Order>((s1, s2) => s2.R_Restaurant_Id == req.RestaurantId && s2.IsDelete==false
                    && (req.StartDate.Value == null || s1.BillDate == req.StartDate.Value)
                    && (s1.CyddJzType==CyddJzType.定金 || s1.CyddJzType==CyddJzType.消费 || s1.CyddJzType==CyddJzType.转结 || s1.CyddJzType==CyddJzType.找零))
                    .Select<TurnDutyStatisticsDTO>("s4.Id as MarketId,s5.Id as UserId,s5.Czdmmc00 as UserName,s1.CyddJzType,sum(s1.PayAmount) as TotalAmount")
                    .GroupBy("s4.Id,s4.Name,s5.Id,s5.Czdmmc00,s1.CyddJzType").ToList();
                    TurnDutyStatisticsGroupDto model = null;
                    foreach (var item in marketList)
                    {
                        List<TurnDutyStatisticsDTO> users = new List<TurnDutyStatisticsDTO>();
                        decimal marketTotal = 0;
                        var userAllList = dataBase.Where(p => p.MarketId == item.Id)
                            .GroupBy(p => new { p.UserId, p.UserName, p.CyddJzType })
                            .Select(g => new TurnDutyStatisticsDTO
                            {
                                UserId=g.Key.UserId,
                                UserName=g.Key.UserName,
                                TotalAmount=g.Sum(p=>p.TotalAmount),
                                CyddJzType=g.Key.CyddJzType
                            }).ToList();
                        var userGroups = userAllList.GroupBy(p => new { p.UserId,p.UserName }).Select(p=> new TurnDutyStatisticsDTO { UserId=p.Key.UserId,UserName=p.Key.UserName}).ToList();
                        foreach (var use in userGroups)
                        {
                            decimal userTotal = 0;
                            var userList = userAllList.Where(p => p.UserId == use.UserId);
                            if (userList != null && userList.Any())
                            {
                                foreach (var ub in userList)
                                {
                                    if (ub.CyddJzType == (int)CyddJzType.折扣 || ub.CyddJzType == (int)CyddJzType.抹零)
                                    {
                                        marketTotal = marketTotal - ub.TotalAmount;
                                        userTotal = userTotal - ub.TotalAmount;
                                    }
                                    else
                                    {
                                        marketTotal = marketTotal + ub.TotalAmount;
                                        userTotal = userTotal + ub.TotalAmount;
                                    }
                                }
                                users.Add(new TurnDutyStatisticsDTO()
                                {
                                    UserId = use.UserId,
                                    MarketId = item.Id,
                                    MarketName = item.Name,
                                    TotalAmount= userTotal,
                                    UserName=use.UserName
                                });
                            }
                        }
                        model = new TurnDutyStatisticsGroupDto()
                        {
                            MarketId = item.Id,
                            MarketName = item.Name,
                            TotalAmount = marketTotal,
                            List = users
                        };
                        
                        res.Add(model);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return res;
            }
        }

        public List<ReportListDTO> GetReportList()
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var data = db.Sqlable().From("pobb", "s1")
                    .Where("s1.pobbsslb='05' and s1.pobbbzs0='Y'")
                    .SelectToList<ReportListDTO>("s1.pobbid00 as Id,s1.pobbmc00 as Name,s1.pobbsslb as Category");
                return data;
            }
        }
    }
}
