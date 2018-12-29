using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using AutoMapper;


namespace OPUPMS.Domain.Restaurant.Repository
{
    public class AreaRepository : SqlSugarService, IAreaRepository
    {
        public AreaRepository()
        {
        }

        public bool Create(AreaCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Area model = new R_Area()
                {
                    Name = req.Name,
                    Description = req.Description,
                    R_Restaurant_Id = req.Restaurant,
                    ServerRate = req.ServerRate == null ? 0 : req.ServerRate
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<AreaListDTO> GetList(out int total, AreaSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = "Id desc";
                List<AreaListDTO> list = new List<AreaListDTO>();

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
                var sql = db.Queryable<R_Area>()
                    .JoinTable<R_Restaurant>((s1, s2) => s1.R_Restaurant_Id == s2.Id && s2.IsDelete == false)
                    .Select(@"s1.*,Restaurant=s2.Name,
                     BoxNum=(select COUNT(0) from R_Box where IsDelete=0 and R_Area_Id=s1.id),
                     TableNum=(select COUNT(0) from R_Table where IsDelete=0 and R_Area_Id=s1.id)").Where(" s1.IsDelete = 0 ");
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
                        list.Add(new AreaListDTO()
                        {
                            Id = Convert.ToInt32(item["id"]),
                            Name = item["Name"].ToString(),
                            Description = item["Description"].ToString(),
                            Restaurant = item["Restaurant"].ToString(),
                            ServerRate = Convert.ToDecimal(item["ServerRate"]),
                            BoxNum = Convert.ToInt32(item["BoxNum"]),
                            TableNum = Convert.ToInt32(item["TableNum"])
                        });
                    }
                }
                total = totalCount;
                return list;
            }
        }

        public List<AreaListDTO> GetList(int restaurantId = 0)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<AreaListDTO> list = new List<AreaListDTO>();
                var data = db.Queryable<R_Area>();
                data = data.Where(p => p.IsDelete == false);
                if (restaurantId > 0)
                {
                    data = data.Where(p => p.R_Restaurant_Id == restaurantId);
                }

                if (data != null && data.Any())
                {
                    data.ToList().ForEach(p =>
                    {
                        list.Add(new AreaListDTO()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            RestaurantId = p.R_Restaurant_Id,
                            ServerRate = p.ServerRate
                        });
                    });
                }

                return list;
            }
        }

        public AreaCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                AreaCreateDTO result = null;
                var model = db.Queryable<R_Area>().InSingle(id);

                if (model != null)
                {
                    result = new AreaCreateDTO()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Restaurant = model.R_Restaurant_Id,
                        ServerRate = model.ServerRate.Value
                    };
                }

                return result;
            }
        }

        public bool Update(AreaCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();

                    R_Area model = new R_Area()
                    {
                        Name = req.Name,
                        Description = req.Description,
                        Id = req.Id,
                        R_Restaurant_Id = req.Restaurant,
                        ServerRate = req.ServerRate == null ? 0 : req.ServerRate
                    };

                    db.Update(model);

                    if (req.IsUpdate)
                    {
                        db.Update<R_Table>(new
                        {
                            ServerRate = model.ServerRate
                        }, p => p.R_Area_Id == model.Id);
                    }

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

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">区域id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    //区域
                    db.Update<R_Area>(new { IsDelete = true }, x => x.Id == id);
                    //包厢
                    db.Update<R_Box>(new { IsDelete = true }, x => x.R_Area_Id == id);
                    //台号
                    db.Update<R_Table>(new { IsDelete = true }, x => x.R_Area_Id == id);
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

        public bool UpdateWeixinPrint(WeixinPrintDTO req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    R_WeixinPrint model = Mapper.Map<WeixinPrintDTO, R_WeixinPrint>(req);
                    if (model.Id <= 0)
                    {
                        var insertId= Convert.ToInt32(db.Insert<R_WeixinPrint>(model));
                        if (req.PrintAreas!=null && req.PrintAreas.Any())
                        {
                            List<R_WeixinPrintArea> list = new List<R_WeixinPrintArea>();
                            foreach (var item in req.PrintAreas)
                            {
                                list.Add(new R_WeixinPrintArea()
                                {
                                    R_Area_Id = item.R_Area_Id,
                                    R_WeixinPrint_Id = insertId
                                });
                            }
                            db.InsertRange<R_WeixinPrintArea>(list);
                        }
                    }
                    else
                    {
                        db.Update<R_WeixinPrint>(model);
                        db.Delete<R_WeixinPrintArea>(p=>p.R_WeixinPrint_Id==model.Id);
                        if (req.PrintAreas != null && req.PrintAreas.Any())
                        {
                            List<R_WeixinPrintArea> list = new List<R_WeixinPrintArea>();
                            foreach (var item in req.PrintAreas)
                            {
                                list.Add(new R_WeixinPrintArea()
                                {
                                    R_Area_Id = item.R_Area_Id,
                                    R_WeixinPrint_Id = model.Id
                                });
                            }
                            db.InsertRange<R_WeixinPrintArea>(list);
                        }
                    }
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    result = false;
                    db.RollbackTran();
                    throw ex;
                }
                return result;
            }
        }

        public WeixinPrintDTO GetWeixinPrint(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                WeixinPrintDTO result = new WeixinPrintDTO();
                R_WeixinPrint model = db.Queryable<R_WeixinPrint>().InSingle(id);
                if (model != null)
                {
                    result = Mapper.Map<R_WeixinPrint, WeixinPrintDTO>(model);
                    var areas = db.Queryable<R_WeixinPrintArea>().Where(p => p.R_WeixinPrint_Id == id).ToList();
                    result.PrintAreas = areas;
                }
                return result;
            }
        }

        public List<WeixinPrintListDTO> GetWeixinPrints(WeixinPrintSearchDTO req,out int total)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = "s1.Id desc";
                List<WeixinPrintListDTO> list = new List<WeixinPrintListDTO>();

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
                //var data = db.Queryable<R_WeixinPrint>()
                //    .JoinTable<R_Restaurant>((s1, s2) => s1.R_Restaurant_Id == s2.Id)
                //    .JoinTable<Printer>((s1,s3)=>s1.Print_Id==s3.Id)
                //    .Select<R_Restaurant, Printer, WeixinPrintListDTO >((s1, s2 ,s3) => new WeixinPrintListDTO()
                //    {
                //        Id=s1.Id,
                //        R_Restaurant_Id=s2.Id,
                //        Name=s1.Name,
                //        RestaurantName=s1.Name,
                //        Print_Id=s3.Id,
                //        PrintName=s3.Name
                //    });

                var data = db.Sqlable()
                    .From<R_WeixinPrint>("s1")
                    .Join<R_Restaurant>("s2", "s1.R_Restaurant_Id", "s2.Id", JoinType.Left)
                    .Join<Printer>("s3", "s1.Print_Id", "s3.Id", JoinType.Left);
                data = data.Where("s1.PrintType=" + (int)req.PrintType);
                if (req.Restaurant>0)
                {
                    data = data.Where("s1.R_Restaurant_Id=" + req.Restaurant);
                }
                totalCount = data.Count();
                list = data.SelectToPageList<WeixinPrintListDTO>($@"
                    s1.*,s2.Name as RestaurantName,s3.Name as PrintName",
                    order, (req.offset / req.limit) + 1, req.limit, null);
                //list = data.Skip(req.offset).Take(req.limit).OrderBy(order).ToList();
                total = totalCount;
                return list;
            }
        }

        public bool IsDeleteWeixinPrint(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    db.Delete<R_WeixinPrintArea>(x => x.R_WeixinPrint_Id == id);
                    db.Delete<R_WeixinPrint>(x => x.Id == id);
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