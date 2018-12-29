using System;
using System.Collections.Generic;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Base.Models;
using SqlSugar;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Base.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{

    public class OrderPayRecordRepository : SqlSugarService, IOrderPayRecordRepository
    {
        public List<DayMarketStatistics> GetDayStallStatistics(List<int> UserIds, DateTime? date)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var operatorUser = OperatorProvider.Provider.GetCurrent();
                date = date.Value == null ? DateTime.Now : date;
                List<DayMarketStatistics> res = new List<DayMarketStatistics>();
                var queryable = db.Queryable<R_OrderPayRecord>()
                    .JoinTable<R_Order>((s1, s2) => s1.R_Order_Id == s2.Id)
                    .Where("DATEDIFF(day,BillDate,@Date)=0 and s2.R_Restaurant_Id=@Restaurant_Id", new { Date = date, Restaurant_Id = operatorUser.DepartmentId });
                if (UserIds.Any())
                {
                    queryable = queryable.Where(p => UserIds.Contains(p.CreateUser));
                }
                var payRecordList = queryable.Select<OrderPayRecordStatistics>("s1.*,s2.R_Market_Id as MarketId").ToList(); //符合条件的账务列表
                if (payRecordList.Any())
                {
                    var orderGroupIds = payRecordList.GroupBy(p => p.R_Order_Id).Select(p => p.Key).ToArray();
                    var userGroupIds = string.Join(",", payRecordList.GroupBy(p => p.CreateUser).Select(p => p.Key).ToArray());
                    var userList = db.Sqlable().From("czdm", "s1").Where("s1.Id in (" + userGroupIds + ")")
                        .SelectToList<UserDto>("s1.czdmmc00 as UserName,s1.Id as UserId", null);
                    var marketGroupList = db.Queryable<R_Market>()
                        .JoinTable<R_Order>((s1, s2) => s1.Id == s2.R_Market_Id)
                        .Where<R_Order>((s1, s2) => orderGroupIds.Contains(s2.Id))
                        .GroupBy("s1.Id,s1.Name").Select("s1.Id,s1.Name").ToList();
                    if (marketGroupList.Any())
                    {
                        foreach (var item in marketGroupList)
                        {
                            DayMarketStatistics model = new DayMarketStatistics();
                            model.MarketId = item.Id;
                            model.MarketName = item.Name;
                            model.Date = date.Value.ToString("yyyy-MM-dd");
                            model.UserList = (from pay in payRecordList
                                              join use in userList on pay.CreateUser equals use.UserId
                                              where pay.MarketId == item.Id
                                              group new { pay, use }
                                              by new { pay.MarketId, use.UserId, use.UserName } into g
                                              select new UserDayMarketStatistics
                                              {
                                                  UserId = g.Key.UserId,
                                                  UserName = g.Key.UserName,
                                                  Total = g.Sum(t => t.pay.PayAmount)
                                              }).ToList();
                            model.Total = model.UserList.Sum(p => p.Total);
                            res.Add(model);
                        }
                    }
                }
                return res;
            }
        }

        public List<OrderPayDeposit> GetOrderPayDepositList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.SqlQuery<OrderPayDeposit>($@" SELECT R_Order_Id AS OrderId,SUM(PayAmount) as PayAmount  FROM R_OrderPayRecord  WHERE CyddJzStatus=2 AND CyddJzType=1   GROUP BY R_Order_Id");
                return list;
            }
        }

        public List<R_OrderPayRecord> GetPayListByOrderId(int orderId)
        {
            using(var db = new SqlSugarClient(Connection))
            {
                var list = db.Queryable<R_OrderPayRecord>().Where(x => x.R_Order_Id == orderId).ToList();
                return list;
            }
        }


        public List<OrderPayRecordDTO> GetPaidRecordListByOrderId(int orderId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.SqlQuery<OrderPayRecordDTO>($@" SELECT P.*, P.R_Market_Id AS MarketId, P.R_OrderMainPay_Id AS OrderMainPayId,
                        ISNULL(C.czdmmc00, '') AS CreateUserName,
                        RP.Name as PayTypeName
                        FROM R_OrderPayRecord P 
                        LEFT JOIN dbo.czdm C ON C.Id = P.CreateUser 
                        left join R_PayMethod RP on P.CyddPayType = RP.Id
                        WHERE P.R_Order_Id = @orderId", new { orderId = orderId });
                foreach (var item in list)
                {
                    item.SourceName = item.SourceName ?? "";
                    item.Remark = item.Remark ?? "";
                }
                return list;
            }            
        }

        public OrderPayRecordDTO GetModelByPayId(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var model = db.SqlQuery<OrderPayRecordDTO>($@" SELECT P.*, P.R_Market_Id AS MarketId, 
                        ISNULL(C.czdmmc00, '') AS CreateUserName 
                        FROM R_OrderPayRecord P 
                        LEFT JOIN dbo.czdm C ON C.Id = P.CreateUser 
                        WHERE P.Id = " + id).FirstOrDefault();
                
                return model;
            }
        }



        public List<OrderMainPayDTO> GetMainPayListByOrderId(int orderId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.SqlQuery<OrderMainPayDTO>($@"SELECT P.*, ISNULL(P.R_Discount_Id, 0) AS DiscountId, 
                            ISNULL(P.R_Market_Id, 0) AS MarketId, ISNULL(M.Name, '') AS MarketName, 
                            ISNULL(C.czdmmc00, '') AS CreateUserName 
                        FROM dbo.R_OrderMainPay P 
                        LEFT JOIN dbo.czdm C ON C.Id = P.CreateUser 
                        LEFT JOIN dbo.R_Market M ON M.Id = P.R_Market_Id
                        WHERE P.R_Order_Id = @orderId", new { orderId = orderId });
                var orderTableList = db.Queryable<R_OrderTable>().Where(p => p.R_Order_Id == orderId).ToList();
                if (list!=null && list.Any())
                {
                    foreach (var item in list)
                    {
                        item.OrderTableIds = orderTableList.Where(p => p.R_OrderMainPay_Id == item.Id).Select(p => p.Id).ToList();
                    }
                }
                return list;
            }
        }
    }
}
