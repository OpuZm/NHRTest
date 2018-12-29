using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Web;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class TableRepository : SqlSugarService, ITableRepository
    {
        public TableRepository()
        {

        }

        public bool Create(TableCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();

                    R_Table model = new R_Table()
                    {
                        Name = req.Name,
                        Describe = req.Description,
                        R_Restaurant_Id = req.RestaurantId,
                        ServerRate = req.ServerRate,
                        R_Area_Id = req.RestaurantArea,
                        CythStatus = CythStatus.空置,
                        SeatNum = req.SeatNum,
                        IsVirtual = req.IsVirtual,
                        Sorted=req.Sorted
                    };

                    var insert = db.Insert(model);

                    if (req.Box > 0)
                    {
                        db.Insert(new R_BoxTable()
                        {
                            R_Table_Id = Convert.ToInt32(insert),
                            R_Box_Id = req.Box
                        });
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

        public List<TableListDTO> GetList(out int total, TableSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = "Id desc";
                List<TableListDTO> list = new List<TableListDTO>();

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
                var sql = db.Queryable<R_Table>()
                    .JoinTable<R_Restaurant>((s1, s2) => s1.R_Restaurant_Id == s2.Id && s2.IsDelete == false)
                    .JoinTable<R_Area>((s1, s3) => s1.R_Area_Id == s3.Id && s3.IsDelete == false)
                    .JoinTable<R_BoxTable>((s1, s4) => s1.Id == s4.R_Table_Id)
                    .JoinTable<R_BoxTable, R_Box>((s1, s4, s5) => s4.R_Box_Id == s5.Id)
                    .Select("s1.*,Restaurant=s2.Name,Area=s3.Name,Box=s5.Name").Where(" s1.IsDelete = 0 ");
                if (req.Restaurant > 0)
                {
                    sql.Where(" s1.R_Restaurant_Id=" + req.Restaurant);
                }
                var data = sql.ToDataTable();

                //根据餐厅查询
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
                        list.Add(new TableListDTO()
                        {
                            Id = Convert.ToInt32(item["id"]),
                            Name = item["Name"].ToString(),
                            Description = item["Describe"].ToString(),
                            Restaurant = item["Restaurant"].ToString(),
                            ServerRate = item.IsNull("ServerRate") ? 0 : Convert.ToDecimal(item["ServerRate"]),
                            Area = item["Area"].ToString(),
                            SeatNum = Convert.ToInt32(item["SeatNum"]),
                            Box = item["Box"].ToString(),
                            Sorted= Convert.ToInt32(item["Sorted"])
                        });
                    }
                }

                total = totalCount;
                return list;
            }
        }

        public List<TableListDTO> GetReseverChoseList(TableChoseSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string date = req.ReverDate.ToString("yyyyMMdd");
                List<TableListDTO> list = new List<TableListDTO>();

                var data = db.Sqlable().From<R_Table>("s1");

                if (req.RestaurantId > 0)
                {
                    data = data.Where("s1.R_Restaurant_Id=" + req.RestaurantId);
                }

                if (req.AreaId > 0)
                {
                    data = data.Where("s1.R_Area_Id=" + req.AreaId);
                }
                data = data.Where("s1.IsDelete=0");
                list = data.SelectToList<TableListDTO>("s1.*,s1.R_Area_Id as AreaId");

                //取已存在的预订记录
                var records = db.SqlQuery<BookingTableDTO>($@"
                    select p2.R_Table_Id as TableId, p2.R_Order_Id as OrderId, p1.R_Market_Id as MarketId,
                           convert(varchar(8), p1.ReserveDate, 112) as BookingDate 
                    from R_Order p1
                    inner join R_OrderTable p2 on p1.Id=p2.R_Order_Id
                    where p1.CyddStatus={(int)CyddStatus.预定} and p1.R_Market_Id={req.Market}
                          and convert(varchar(8), p1.ReserveDate, 112) = '{date}'");

                var ids = records.Select(x => x.TableId).ToArray();
                if (req.CurrentReservedOrderId > 0)//过滤非当前预订单关联的TableId
                {
                    ids = records
                        .Where(x => x.OrderId != req.CurrentReservedOrderId)
                        .Select(x => x.TableId).ToArray();
                }

                list = list.Where(x => !ids.Contains(x.Id)).ToList();
                return list;
            }
        }

        public List<TableListDTO> GetOpenTableChoseList(TableSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<TableListDTO> list = new List<TableListDTO>();
                var data = db.Sqlable().From<R_Table>("s1");


                if (req.RestaurantId > 0)
                {
                    data = data.Where("s1.R_Restaurant_Id=" + req.RestaurantId);
                }

                if (req.AreaId > 0)
                {
                    data = data.Where("s1.R_Area_Id=" + req.AreaId);
                }

                if ((int)req.CythStatus > 0)
                {
                    data = data.Where("s1.CythStatus=" + (int)req.CythStatus);
                }

                list = data.SelectToList<TableListDTO>("s1.*,s1.R_Area_Id as AreaId");
                return list;
            }
        }

        public List<TableListIndexDTO> GetList(TableSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<TableListIndexDTO> list = new List<TableListIndexDTO>();
                var data = db.Queryable<R_Table>();
                data = data.Where(p => p.IsDelete == false);
                if (req.RestaurantId > 0)
                {
                    data = data.Where(p => p.R_Restaurant_Id == req.RestaurantId);
                }

                if (req.AreaId > 0)
                {
                    data = data.Where(p => p.R_Area_Id == req.AreaId);
                }

                if ((int)req.CythStatus > 0)
                {
                    data = data.Where("CythStatus =" + (int)req.CythStatus);
                }
                data = data.OrderBy(p => p.Sorted);
                if (data.Any())
                {
                    list = data.Select<TableListIndexDTO>(s1 =>
                            new TableListIndexDTO()
                            {
                                Id = s1.Id,
                                Name = s1.Name,
                                CythStatus = s1.CythStatus,
                                Description = s1.Describe,
                                SeatNum = s1.SeatNum,
                                ServerRate = s1.ServerRate,
                                Restaurant = s1.R_Restaurant_Id,
                                AreaId = s1.R_Area_Id,
                                IsVirtual = s1.IsVirtual,
                                Sorted = s1.Sorted
                            }).ToList();

                    if (list.Any())
                    {
                        var tabIds = list.Select(p => p.Id).ToArray();
                        string tabIdsStr = string.Join(",", tabIds);

                        var orderTabList = db.Queryable<R_OrderTable>()
                            .JoinTable<R_Order>((s1, s2) => s1.R_Order_Id == s2.Id, JoinType.Left)
                            .Where<R_Order>((s1, s2) => tabIds.Contains(s1.R_Table_Id) && s2.IsDelete == false
                                    && (s2.CyddStatus == CyddStatus.开台 || s2.CyddStatus == CyddStatus.点餐
                                    || s2.CyddStatus == CyddStatus.送厨 || s2.CyddStatus == CyddStatus.用餐中 || s2.CyddStatus==CyddStatus.反结))
                            .Select<R_Order, TableLinkOrderDTO>((s1, s2) =>
                                new TableLinkOrderDTO()
                                {
                                    Id = s1.Id,
                                    CreateDate = s1.CreateDate,
                                    IsCheckOut = s1.IsCheckOut,
                                    IsOpen = s1.IsOpen,
                                    OrderCallType = s2.CyddCallType,
                                    OrderNo = s2.OrderNo,
                                    OrderPersonNum = s2.PersonNum,
                                    OrderStatus = s2.CyddStatus,
                                    OrderCreatedTime = s2.CreateDate,
                                    PersonNum = s1.PersonNum,
                                    R_Order_Id = s1.R_Order_Id,
                                    R_Table_Id = s1.R_Table_Id,
                                    IsControl = s1.IsControl,
                                    IsLock=s1.IsLock,
                                    MemberCardId=s2.MemberCardId,
                                    ContactPerson=s2.ContactPerson,
                                    ContactTel=s2.ContactTel
                                }
                            ).ToList();

                        #region 预定列表  

                        var orderTabReverList = db.SqlQuery<BookingTableDTO>($@" 
                                    SELECT OT.Id AS OrderTableId, OT.R_Table_Id AS TableId, 
                                        O.Id AS OrderId, O.ReserveDate AS BookingDate, O.PersonNum, O.ContactPerson, O.ContactTel,
                                        M.Id AS MarketId, M.Name AS MarketName 
                                    FROM dbo.R_OrderTable OT 
                                    INNER JOIN dbo.R_Order O ON O.Id = OT.R_Order_Id 
                                    INNER JOIN dbo.R_Market M ON M.Id = O.R_Market_Id 
                                    WHERE OT.R_Table_Id IN ({tabIdsStr}) AND O.IsDelete=0 AND O.CyddStatus = {(int)CyddStatus.预定} AND DATEDIFF(DAY, GETDATE(), O.ReserveDate)=0 ");

                        #endregion

                        var orderIds = (from od in orderTabList
                                        group od by od.R_Order_Id into g
                                        select g.Key).ToArray();

                        var mxList = db.Queryable<R_OrderDetail>()
                            .JoinTable<R_OrderTable>((s1, s2) => s1.R_OrderTable_Id == s2.Id)
                            .Where<R_OrderTable>((s1, s2) => orderIds.Contains(s2.R_Order_Id))
                            .Select<R_OrderTable, OrderTableProjectDTO>((s1, s2) =>
                                new OrderTableProjectDTO()
                                {
                                    MxId = s1.Id,
                                    OrderTableId = s2.Id,
                                    TableId = s2.R_Table_Id,
                                    Price = s1.Price,
                                    Num = s1.Num,
                                    OrderId = s2.R_Order_Id
                                }).ToList();

                        if (orderTabList.Any() || orderTabReverList.Any())
                        {
                            var mxIds = mxList.Select(p => p.MxId).ToArray();

                            #region 订单明细操作表
                            //var mxRecordList = db.Queryable<R_OrderDetailRecord>().Where(
                            //    p => mxIds.Contains(p.R_OrderDetail_Id) &&
                            //    (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出 || p.CyddMxCzType == CyddMxCzType.退菜)).ToList();
                            #endregion

                            var extendList = db.Queryable<R_OrderDetailExtend>()
                                .Where(p => mxIds.Contains(p.R_OrderDetail_Id))
                                .ToList();

                            foreach (var item in list)
                            {
                                item.OrderList = orderTabReverList.Where(p => p.TableId == item.Id).ToList();

                                //获取当前餐台已开台未结账的关联订单信息
                                var orderTableNows = orderTabList
                                    .Where(p => p.R_Table_Id == item.Id && !p.IsCheckOut && p.IsOpen);

                                item.OrderNow = new List<OrderTableDTO>();

                                if (orderTableNows.Any())
                                {
                                    foreach (var otn in orderTableNows)
                                    {
                                        var otnModel = new OrderTableDTO()
                                        {
                                            Id = otn.Id,
                                            OrderId = otn.R_Order_Id,
                                            IsCheckOut = otn.IsCheckOut,
                                            IsOpen = otn.IsOpen,
                                            PersonNum = otn.PersonNum,
                                            CreateDate = otn.CreateDate, //订单台号表 记录的时间
                                            OrderNo = otn.OrderNo,
                                            OrderStatusDesc = otn.OrderStatusDesc,
                                            IsControl = otn.IsControl,
                                            IsLock=otn.IsLock,
                                            IsWeiXin = otn.MemberCardId > 0 ? true : false,
                                            TableId = otn.R_Table_Id,
                                            ContactTel = otn.ContactTel,
                                            ContactPerson = otn.ContactPerson
                                        };

                                        var totalAmount = mxList.Where(
                                                p => p.OrderTableId == otn.Id && p.OrderId == otn.R_Order_Id)
                                            .Sum(p => p.Price * p.Num);

                                        var totalAmountExtend =
                                            (from ext in extendList
                                             join mx in mxList on ext.R_OrderDetail_Id equals mx.MxId
                                             where mx.OrderTableId == otn.Id && mx.OrderId == otn.R_Order_Id
                                             && mx.MxId == ext.R_OrderDetail_Id
                                             select new OrderTableDTO
                                             {
                                                 TotalAmount = ext.Price * mx.Num
                                             }).ToList().Sum(p => p.TotalAmount);

                                        otnModel.TotalAmount = totalAmount + totalAmountExtend;
                                        item.OrderNow.Add(otnModel);
                                    }
                                }
                            }
                        }
                    }
                }

                return list;
            }
        }

        public TableCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                TableCreateDTO model = db.Queryable<R_Table>()
                    .JoinTable<R_BoxTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                    .Where<R_Table>((s1) => s1.Id == id)
                    .Select<R_Table, R_BoxTable, TableCreateDTO>((s1, s2) =>
                        new TableCreateDTO
                        {
                            Id = s1.Id,
                            Name = s1.Name,
                            Description = s1.Describe,
                            RestaurantId = s1.R_Restaurant_Id,
                            RestaurantArea = s1.R_Area_Id,
                            ServerRate = s1.ServerRate,
                            CythStatus = s1.CythStatus,
                            SeatNum = s1.SeatNum,
                            Box = s2.R_Box_Id,
                            IsVirtual = s1.IsVirtual,
                            Sorted=s1.Sorted
                        })
                    .FirstOrDefault();
                return model;
            }
        }

        public bool Update(TableCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    db.BeginTran();

                    R_Table model = db.Queryable<R_Table>().First(p => p.Id == req.Id);

                    model.Name = req.Name;
                    model.Describe = req.Description;
                    model.R_Restaurant_Id = req.RestaurantId;
                    model.ServerRate = req.ServerRate;
                    model.R_Area_Id = req.RestaurantArea;
                    model.SeatNum = req.SeatNum;
                    model.IsVirtual = req.IsVirtual;
                    model.Sorted = req.Sorted;

                    result = db.Update(model);

                    var cybxth = db.Queryable<R_BoxTable>()
                        .Where(p => p.R_Table_Id == model.Id)
                        .FirstOrDefault();

                    if (req.Box > 0)
                    {
                        if (cybxth != null)
                        {
                            if (cybxth.R_Box_Id != req.Box)
                            {
                                cybxth.R_Box_Id = req.Box;
                                db.Update<R_BoxTable>(cybxth);
                            }
                        }
                        else
                        {
                            db.Insert<R_BoxTable>(new R_BoxTable()
                            {
                                R_Table_Id = model.Id,
                                R_Box_Id = req.Box
                            });
                        }
                    }
                    else
                    {
                        if (cybxth != null)
                        {
                            db.Delete<R_BoxTable>(p => p.Id == cybxth.Id);
                        }
                    }

                    db.CommitTran();
                }
                catch (Exception)
                {
                    db.RollbackTran();
                    result = false;

                    throw;
                }

                return result;
            }
        }

        public List<TableStatusDTO> GetStatus()
        {
            List<TableStatusDTO> res = new List<TableStatusDTO>();
            foreach (int myCode in Enum.GetValues(typeof(CythStatus)))
            {
                string strName = Enum.GetName(typeof(CythStatus), myCode);//获取名称
                res.Add(new TableStatusDTO()
                {
                    Id = myCode,
                    Name = strName
                });
            }
            return res;
        }

        public List<R_Table> GetTables(int[] restraurantIds, int? areaId, CythStatus statusType,bool inCludVirtual=true)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.Queryable<R_Table>()
                    .Where(x => restraurantIds.Contains(x.R_Restaurant_Id) && x.IsDelete == false);

                if (areaId.HasValue && areaId.Value > 0)
                    list = list.Where(x => x.R_Area_Id == areaId.Value);
                if ((int)statusType > 0)
                    list = list.Where(x => x.CythStatus == statusType);

                if (!inCludVirtual)
                {
                    list = list.Where(p => p.IsVirtual == false);
                }
                return list.ToList();
            }
        }

        public List<R_Table> GetTables(int[] restraurantIds, int? areaId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.Queryable<R_Table>()
                    .Where(x => restraurantIds.Contains(x.R_Restaurant_Id) && x.IsDelete == false);

                if (areaId.HasValue && areaId.Value > 0)
                    list = list.Where(x => x.R_Area_Id == areaId.Value);

                return list.ToList();
            }
        }


        /// <summary>
        /// 根据订单ID查询下面的台号信息
        /// </summary>
        /// <returns>返回餐台列表</returns>
        public List<R_Table> GetTables(int orderId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var list = db.SqlQuery<R_Table>($@"
                        SELECT * FROM dbo.R_Table WHERE Id IN 
                            (SELECT R_Table_Id FROM R_OrderTable WHERE R_Order_Id={orderId})");
                return list;
            }
        }

        public R_Table GetTable(int tableId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var model = db.Queryable<R_Table>()
                .Where(x => x.Id == tableId)
                .FirstOrDefault();
                return model;
            }
        }

        public List<TableListDTO> GetlistByIds(List<int> tableIds)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<TableListDTO> res = new List<TableListDTO>();
                var list = db.Queryable<R_Table>();
                if (tableIds.Any())
                {
                    list = list.Where(p => tableIds.Contains(p.Id));
                }
                list.ToList().ForEach(p =>
                {
                    res.Add(new TableListDTO() { Id = p.Id, Name = p.Name });
                });
                return res;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">台号id</param>
        /// <returns></returns>
        public bool IsDelete(int id, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                msg = "";
                //先查询是否此台号还存在未结账的订单
                var orderTableIds = db.Queryable<R_OrderTable>()
                    .JoinTable<R_Order>((s1, s2) => s1.R_Order_Id == s2.Id)
                    .Where(" s1.R_Table_Id = " + id + " and s2.CyddStatus not in (6,7) and s2.IsDelete=0").ToList();
                if (orderTableIds != null && orderTableIds.Count > 0)
                {
                    result = false;
                    msg = "此台号还存在未结账的订单,删除失败！";
                }
                else
                {
                    //删除台号
                    result = db.Update<R_Table>(new { IsDelete = true }, x => x.Id == id);
                }
                return result;
            }
        }

    }
}