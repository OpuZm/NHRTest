using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class RestaurantRepository : SqlSugarService, IRestaurantRepository
    {
        public RestaurantRepository()
        {

        }

        public bool Create(RestaurantCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Restaurant model = new R_Restaurant
                {
                    Name = req.Name,
                    Description = req.Description,
                    R_Company_Id = req.R_Company_Id
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<RestaurantListDTO> GetList()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();
            var company = currentUser.CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                List<RestaurantListDTO> list = new List<RestaurantListDTO>();

                db.Queryable<R_Restaurant>().Where(p => p.IsDelete == false && p.R_Company_Id==company)
                    .ToList().ForEach(p =>
                    {
                        list.Add(new RestaurantListDTO()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description
                        });
                    });

                return list;
            }
        }

        public List<RestaurantListDTO> GetList(out int total, RestaurantSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = string.Empty;
                List<RestaurantListDTO> list = new List<RestaurantListDTO>();

                if (!string.IsNullOrEmpty(req.Sort))
                {
                    order = string.Format("{0} {1}", req.Sort, req.Order);
                }

                var data = db.Queryable<R_Restaurant>()
                    .Select(@"R_Restaurant.Id,R_Restaurant.Name,R_Restaurant.Description,
                      AreaNum=(select COUNT(0) from R_Area a where a.R_Restaurant_Id=R_Restaurant.id and a.IsDelete=0),
                      BoxNum=(select count(0) from R_Box where IsDelete=0 and R_Area_Id in 
                             (select id from R_Area where IsDelete=0 and R_Restaurant_Id=R_Restaurant.Id)),
                      TableNum=(select COUNT(0) from R_Table c where c.IsDelete=0 and  c.R_Restaurant_Id=R_Restaurant.id),
                      SeatNum=(select isnull(SUM(SeatNum),0) from R_Table d where d.IsDelete=0 and d.R_Restaurant_Id=R_Restaurant.id)")
                      .Where(p => p.IsDelete == false && p.R_Company_Id==req.CompanyId).ToDataTable();

                if (data != null && data.Rows.Count > 0)
                {
                    DataView dtv = data.DefaultView;
                    dtv.Sort = order;
                    data = dtv.ToTable();
                    totalCount = data.Rows.Count;
                    var rows = data.Rows.Cast<DataRow>();
                    var curRows = rows.Skip(req.offset).Take(req.limit).ToArray();

                    foreach (DataRow item in curRows)
                    {
                        list.Add(new RestaurantListDTO()
                        {
                            Id = Convert.ToInt32(item["id"]),
                            Name = item["Name"].ToString(),
                            Description = item["Description"].ToString(),
                            AreaNum = Convert.ToInt32(item["AreaNum"]),
                            BoxNum = Convert.ToInt32(item["BoxNum"]),
                            TableNum = Convert.ToInt32(item["TableNum"]),
                            SeatNum = Convert.ToInt32(item["SeatNum"])
                        });
                    }
                }

                total = totalCount;
                return list;
            }
        }

        public List<RestaurantListDTO> GetList(int companyId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var result = db.Queryable<R_Restaurant>()
                    .Where(x => x.R_Company_Id == companyId && x.IsDelete == false)
                    .Select<RestaurantListDTO>(p => new RestaurantListDTO
                    {
                        Id=p.Id,
                        Name=p.Name,
                        Description=p.Description,
                        R_Company_Id=p.R_Company_Id
                    });
                return result.ToList();
            }
        }

        public List<R_Restaurant> GetList(string[] ids)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var result = db.Queryable<R_Restaurant>()
                    .Where(x => ids.Contains(x.Id.ToString()) && x.IsDelete == false);
                return result.ToList();
            }
        }

        public RestaurantCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                RestaurantCreateDTO result = null;
                var model = db.Queryable<R_Restaurant>().InSingle(id);

                if (model != null)
                {
                    result = new RestaurantCreateDTO
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        R_Company_Id = model.R_Company_Id
                    };
                }

                return result;
            }
        }

        public bool Update(RestaurantCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;

                result = db.Update<R_Restaurant>(
                    new R_Restaurant
                    {
                        Description = req.Description,
                        R_Company_Id = req.R_Company_Id,
                        Name = req.Name
                    }, x => x.Id == req.Id);

                return result;
            }
        }

        public List<RestaurantListDTO> FilterCompanyRestaurant(List<RestaurantListDTO> req, int companyId)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var ids = req.Select(p => p.Id).ToArray();
                var companyResIds = db.Queryable<R_Restaurant>().Where(p => p.R_Company_Id == companyId && ids.Contains(p.Id)).Select(p => p.Id).ToList();
                var res = req.Where(p => companyResIds.Contains(p.Id)).ToList();
                return res;
            }
        }
    }
}