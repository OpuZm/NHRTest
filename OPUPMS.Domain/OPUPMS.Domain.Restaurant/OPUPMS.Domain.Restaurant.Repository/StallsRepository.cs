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
    public class StallsRepository : SqlSugarService, IStallsRepository
    {
        public StallsRepository()
        {

        }

        public bool Create(StallsCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Stall model = new R_Stall()
                {
                    Name = req.Name,
                    Description = req.Description,
                    Print_Id = req.Print_Id
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<StallsListDTO> GetList(out int total, StallsSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = "Id desc";
                List<StallsListDTO> list = new List<StallsListDTO>();

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
                var data = db.Queryable<R_Stall>()
                    .JoinTable<Printer>((S, P) => P.Id == S.Print_Id && P.IsDelete == false, JoinType.Left)
                    .Select(@"S.*, P.Name AS PrinterName, 
                              MainProjectNum=(select count(0) from R_ProjectStall where R_Stall_Id=S.Id AND BillType = 2), 
                              DetailProjectNum=(select count(0) from R_ProjectStall where R_Stall_Id=S.Id AND BillType = 1)")
                    .Where(" S.IsDelete = 0 ").ToDataTable();

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
                        list.Add(new StallsListDTO()
                        {
                            Id = Convert.ToInt32(item["id"]),
                            Name = item["Name"].ToString(),
                            Description = item["Description"].ToString(),
                            MainProjectNum = Convert.ToInt32(item["MainProjectNum"]),
                            DetailProjectNum = Convert.ToInt32(item["DetailProjectNum"]),
                            PrinterName = item["PrinterName"].ToString()
                        });
                    }
                }

                total = totalCount;
                return list;
            }
        }

        public List<StallsListDTO> GetList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<StallsListDTO> list = new List<StallsListDTO>();
                return list;
            }
        }

        public StallsCreateDTO GetModel(int id, int? billType = null)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                StallsCreateDTO result = null;
                if (id == 0)
                    return result;

                var data = db.Queryable<R_Stall>().InSingle(id);

                if (data != null)
                {
                    result = new StallsCreateDTO()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Description = data.Description,
                        Print_Id = data.Print_Id,
                    };

                    if (billType.HasValue)
                    {
                        var cyxmDks = db.Queryable<R_ProjectStall>()
                            .Where(p => p.R_Stall_Id == result.Id && p.BillType == billType.Value)
                            .ToList();
                        result.ProjectStallList = cyxmDks;
                    }
                    else
                    {
                        var cyxmAllDks = db.Queryable<R_ProjectStall>()
                            .Where(p => p.R_Stall_Id == result.Id)
                            .ToList();
                        result.ProjectStallList = cyxmAllDks;
                    }
                }
                return result;
            }
        }

        public bool Update(StallsCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_Stall model = new R_Stall()
                {
                    Id = req.Id,
                    Name = req.Name,
                    Description = req.Description
                };
                //result = db.Update(model);
                result = db.Update<R_Stall>(
                    new
                    {
                        Name = req.Name,
                        Description = req.Description,
                        Print_Id = req.Print_Id,
                        R_Company_Id = req.R_Company_Id,
                    }, x => x.Id == req.Id);
                return result;
            }
        }

        public bool ProjectStallSubmit(int id, int billType, List<R_ProjectStall> req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();

                    result = db.Delete<R_ProjectStall>(p => p.R_Stall_Id == id && p.BillType == billType);

                    if (req != null && req.Count > 0)
                    {
                        req = req.Distinct(new ProjectStallComparer()).ToList();
                        db.InsertRange(req);
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

        public bool ProjectStallSubmitNew(int id, List<R_ProjectStall> req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();
                    db.Delete<R_ProjectStall>(p => p.R_Stall_Id == id);
                    if (req != null && req.Any())
                    {
                        db.InsertRange(req);
                    }
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    result = false;
                    db.RollbackTran();
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">档口id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                //档口
                result = db.Update<R_Stall>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }
    }

    class ProjectStallComparer : IEqualityComparer<R_ProjectStall>
    {
        public bool Equals(R_ProjectStall p1, R_ProjectStall p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.R_Project_Id == p2.R_Project_Id;
        }

        public int GetHashCode(R_ProjectStall p)
        {
            if (p == null)
                return 0;
            return p.R_Project_Id.GetHashCode();
        }
    }
}