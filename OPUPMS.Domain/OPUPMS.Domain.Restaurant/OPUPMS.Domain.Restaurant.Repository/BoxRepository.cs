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
    public class BoxRepository : SqlSugarService, IBoxRepository
    {
        public BoxRepository()
        {
        }

        public bool Create(BoxCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Box model = new R_Box()
                {
                    Name = req.Name,
                    Description = req.Description,
                    R_Area_Id = req.RestaurantArea,
                    R_Restaurant_Id = req.Restaurant
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<BoxListDTO> GetList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<BoxListDTO> list = new List<BoxListDTO>();
                var data = db.Queryable<R_Box>();

                if (data != null && data.Any())
                {
                    data.ToList().ForEach(p =>
                    {
                        list.Add(new BoxListDTO()
                        {
                            Id = p.Id,
                            Description = p.Description,
                            Name = p.Name,
                            RestaurantId = p.R_Restaurant_Id,
                            RestaurantAreaId = p.R_Area_Id
                        });
                    });
                }

                return list;
            }
        }

        public List<BoxListDTO> GetList(out int total, BoxSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = "Id desc";
                List<BoxListDTO> list = new List<BoxListDTO>();

                if (!string.IsNullOrEmpty(req.Sort))
                {
                    if (req.Sort.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        order = "Id desc";
                    }
                    else
                    {
                        order = string.Format("{0} {1}", req.Sort, req.Order);
                    }
                }
                var sql = db.Queryable<R_Box>()
                    .JoinTable<R_Restaurant>((s1, s2) => s1.R_Restaurant_Id == s2.Id && s2.IsDelete == false)
                    .JoinTable<R_Area>((s1, s3) => s1.R_Area_Id == s3.Id && s3.IsDelete == false)
                    .Select(@"s1.*,Restaurant=s2.Name,RestaurantArea=s3.Name,
                              TableNum=(select COUNT(0) from R_BoxTable where R_Box_Id=s1.id)").Where(" s1.IsDelete = 0 ");
                if (req.Restaurant > 0)
                {
                    sql.Where(" s1.R_Restaurant_Id=" + req.Restaurant);
                }
                var data = sql.ToDataTable();

                /*性能问题*/
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
                        list.Add(new BoxListDTO()
                        {
                            Id = Convert.ToInt32(item["id"]),
                            Name = item["Name"].ToString(),
                            Description = item["Description"].ToString(),
                            RestaurantArea = item["RestaurantArea"].ToString(),
                            Restaurant = item["Restaurant"].ToString(),
                            TableNum = Convert.ToInt32(item["TableNum"])
                        });
                    }
                }

                total = totalCount;
                return list;
            }
        }

        public BoxCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                BoxCreateDTO result = null;
                var model = db.Queryable<R_Box>().InSingle(id);

                if (model != null)
                {
                    result = new BoxCreateDTO()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        RestaurantArea = model.R_Area_Id,
                        Restaurant = model.R_Restaurant_Id
                    };
                }

                return result;
            }
        }

        public bool Update(BoxCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Box model = new R_Box()
                {
                    Name = req.Name,
                    Description = req.Description,
                    Id = req.Id,
                    R_Restaurant_Id = req.Restaurant,
                    R_Area_Id = req.RestaurantArea
                };

                result = db.Update(model);
                return result;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">包厢id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    //包厢
                    db.Update<R_Box>(new { IsDelete = true }, x => x.Id == id);
                    //删除包厢台号关系表
                    db.Delete<R_BoxTable>(p => p.R_Box_Id == id);
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

    }
}
