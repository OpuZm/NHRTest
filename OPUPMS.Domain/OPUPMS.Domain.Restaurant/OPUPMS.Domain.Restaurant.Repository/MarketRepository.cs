using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class MarketRepository : SqlSugarService, IMarketRepository
    {
        public MarketRepository()
        {

        }

        public bool Create(MarketCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Market model = new R_Market()
                {
                    Name = req.Name,
                    Description = req.Description,
                    R_Restaurant_Id = req.Restaurant,
                    StartTime = req.StartTime,
                    EndTime = req.EndTime
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<MarketListDTO> GetList(out int total, MarketSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = string.Empty;
                List<MarketListDTO> list = new List<MarketListDTO>();

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
                var data = db.Sqlable()
                    .From<R_Market>("s1")
                    .Join<R_Restaurant>("s2", "s1.R_Restaurant_Id", "s2.Id", JoinType.Left)
                    .Where(" s1.IsDelete = 0  ");
                if (req.Restaurant > 0)
                {
                    data = data.Where("s2.Id=" + req.Restaurant);
                }
                totalCount = data.Count();
                list = data.SelectToPageList<MarketListDTO>(
                    @"s1.*,s2.Name as Restaurant",
                    order, (req.offset / req.limit) + 1, req.limit, null);
                //var data = db.Queryable<R_Market>()
                //    .JoinTable<R_Restaurant>((s1, s2) => s1.R_Restaurant_Id == s2.Id)
                //    .Select("s1.*,s2.Name as Restaurant")
                //    .OrderBy(order)
                //    .ToDataTable();

                ////根据餐厅查询
                //if (req.Restaurant > 0)
                //{
                //    DataRow[] dr = data.Select(" R_Restaurant_Id=" + req.Restaurant);
                //    DataTable dtNew = data.Clone();
                //    for (int i = 0; i < dr.Length; i++)
                //    {
                //        dtNew.ImportRow(dr[i]);
                //    }
                //    data = dtNew;//重新赋值                   
                //}

                //if (data != null && data.Rows.Count > 0)
                //{
                //    totalCount = data.Rows.Count;
                //    var rows = data.Rows.Cast<DataRow>();
                //    var curRows = rows.Skip(req.offset).Take(req.limit).ToArray();

                //    foreach (DataRow item in curRows)
                //    {
                //        list.Add(new MarketListDTO()
                //        {
                //            Id = Convert.ToInt32(item["id"]),
                //            Name = item["Name"].ToString(),
                //            Description = item["Description"].ToString(),
                //            Restaurant = item["Restaurant"].ToString(),
                //            RestaurantId = Convert.ToInt32(item["R_Restaurant_Id"]),
                //            StartTime = item["StartTime"].ToString(),
                //            EndTime = item["EndTime"].ToString()
                //        });
                //    }
                //}

                total = totalCount;
                return list;
            }
        }

        public List<MarketListDTO> GetList(int restaurantId = 0)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<MarketListDTO> list = new List<MarketListDTO>();
                var data = db.Queryable<R_Market>();
                data = data.Where(p => p.IsDelete == false);
                if (restaurantId > 0)
                {
                    data = data.Where(p => p.R_Restaurant_Id == restaurantId);
                }

                list = GetBy(data);

                return list;
            }
        }

        private List<MarketListDTO> GetBy(Queryable<R_Market> data)
        {
            List<MarketListDTO> list = new List<MarketListDTO>();
            if (data != null && data.Any())
            {
                var sourceList = data.ToList();
                DateTime time = DateTime.Now;

                //注：前后分市前分市的结束时间与后分市的开始时间设置需相同
                foreach (var item in sourceList)
                {
                    MarketListDTO info = new MarketListDTO();
                    info.Id = item.Id;
                    info.Name = item.Name;
                    info.Description = item.Description;
                    info.RestaurantId = item.R_Restaurant_Id;
                    info.StartTime = item.StartTime;
                    info.EndTime = item.EndTime;
                    info.IsDefault = (DateTime.ParseExact(item.StartTime, "HH:mm", null) < time
                                && time <= DateTime.ParseExact(item.EndTime, "HH:mm", null));
                    list.Add(info);
                }

            }

            return list;
        }

        public List<MarketListDTO> GetList(List<int> resIdList)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<MarketListDTO> list = new List<MarketListDTO>();
                var data = db.Queryable<R_Market>().Where(p => p.IsDelete == false);
                data = data.Where(p => p.IsDelete == false);
                if (resIdList != null && resIdList.Count > 0)
                {
                    data = data.Where(p => resIdList.Contains(p.R_Restaurant_Id));
                }

                list = GetBy(data);

                return list;
            }
        }

        public MarketCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                MarketCreateDTO result = null;
                var model = db.Queryable<R_Market>().InSingle(id);

                if (model != null)
                {
                    result = new MarketCreateDTO()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Restaurant = model.R_Restaurant_Id,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime
                    };
                }

                return result;
            }
        }

        public bool Update(MarketCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Market model = new R_Market()
                {
                    Name = req.Name,
                    Description = req.Description,
                    Id = req.Id,
                    R_Restaurant_Id = req.Restaurant,
                    StartTime = req.StartTime,
                    EndTime = req.EndTime
                };
                result = db.Update(model);

                return result;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">分市id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                result = db.Update<R_Market>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }

    }
}