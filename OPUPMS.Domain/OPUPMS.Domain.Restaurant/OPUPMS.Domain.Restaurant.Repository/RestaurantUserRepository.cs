using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Repository.OldRepository;
using OPUPMS.Infrastructure.Dapper;
using Smooth.IoC.UnitOfWork;
using SqlSugar;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.AuthorizeService;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class RestaurantUserRepository : UserRepository_Old
    {
        public RestaurantUserRepository(IMultiDbDbFactory factory) : base(factory)
        {
        }

        private readonly static string ByNameORCodeSql = @"SELECT * FROM SUsers WHERE (UserCode = @UserCode OR UserName = @UserName)";
        private readonly static string GetUserRestaurants = @"SELECT s1.Id as RestaurantId FROM dbo.R_Restaurant s1 LEFT JOIN dbo.UserRestaurant s2 ON s1.Id=s2.RestaurantId where s1.R_Company_Id=@R_Company_Id and s2.UserId=@UserId";
        private readonly static string IsHaveRestaurant = @"SELECT count(0) FROM dbo.R_Restaurant where R_Company_Id=@CompanyId";
        private readonly static string GetAllRestaurant = @"SELECT s1.Id as RestaurantId FROM dbo.R_Restaurant s1 LEFT JOIN dbo.UserRestaurant s2 ON s1.Id=s2.RestaurantId where s1.R_Company_Id=@R_Company_Id";
        private readonly static string InsertRestaurantInit = @"INSERT INTO dbo.R_Restaurant
        ( Name ,
          SeatNum ,
          TableNum ,
          ServiceRate ,
          Description ,
          R_Company_Id ,
          IsDelete
        )
VALUES  ( @Name , -- Name - nvarchar(200)
          0 , -- SeatNum - int
          0 , -- TableNum - int
          0 , -- ServiceRate - decimal
          @Name , -- Description - ntext
          @R_Company_Id , -- R_Company_Id - int
          0  -- IsDelete - bit
        ) select @@identity";

        private readonly static string InsertMarket = @"INSERT INTO dbo.R_Market
        ( Name ,
          R_Restaurant_Id ,
          StartTime ,
          EndTime ,
          Description ,
          IsDelete
        )
VALUES  ( @Name , -- Name - nvarchar(200)
          @R_Restaurant_Id , -- R_Restaurant_Id - int
          @StartTime , -- StartTime - varchar(50)
          @EndTime , -- EndTime - varchar(50)
          @Name , -- Description - nvarchar(4000)
          0  -- IsDelete - bit
        ) select @@identity";

        public override UserInfo GetByUserName(string token, string userName, int companyId)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var result = session.QueryFirstOrDefault<CzdmModel>(
                    ByNameORCodeSql, new CzdmModel
                    {
                        UserCode = userName,
                        UserName = userName
                    });
                IEnumerable<UserRestaurant> userRestaurants = null;
                var userinfo = ConvertToInfo(result);
                if (userinfo != null)
                {
                    if (userinfo.IsTechnicalAssistance)
                    {
                        var needInit = session.ExecuteScalar(IsHaveRestaurant, new
                        {
                            CompanyId=companyId
                        }).ObjToInt();
                        if (userinfo.Permission <= 0)
                        {
                            var permissionList = EnumToList.ConvertEnumToList(typeof(Permission));
                            var permissionSum = permissionList.Sum(p => p.Key);
                        }
                        if (needInit<=0)
                        {
                            var restaurantId = session.Execute(InsertRestaurantInit, new R_Restaurant()
                            {
                                Name="初始化餐厅",
                                R_Company_Id=companyId
                            });
                            var marketId = session.Execute(InsertMarket, new R_Market()
                            {
                                Name="早市",
                                R_Restaurant_Id=restaurantId,
                                StartTime="06:00",
                                EndTime="12:00"
                            });
                        }
                        userRestaurants = session.Query<UserRestaurant>(GetAllRestaurant, new
                        {
                            R_Company_Id = companyId
                        });
                    }
                    else
                    {
                        userRestaurants = session.Query<UserRestaurant>(GetUserRestaurants, new
                        {
                            UserId = userinfo.UserId,
                            R_Company_Id = companyId
                        });
                    }

                    userinfo.ManagerRestaurant = string.Join(",", userRestaurants.Select(p => p.RestaurantId));
                }
                return userinfo;
                //return ConvertToInfo(result);
            }
        }

        public override async Task<UserInfo> GetByUserNameAsync(string token, string userName)
        {
            using (var session = Factory.Create<ISession>(token))
            {
                var model = await session.QueryFirstOrDefaultAsync<CzdmModel>(
                    GetByUserNameSql, new CzdmModel
                    {
                        UserName = userName
                    });

                return ConvertToInfo(model);
            }
        }

        public override UserInfo GetByUserId(int userId)
        {
            return base.GetByUserId(userId);
        }

        public override List<UserInfo> GetByUserIds(List<int> userIds)
        {
            return base.GetByUserIds(userIds);
        }


    }
}