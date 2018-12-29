using System;
using System.Collections.Generic;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class OrderRecordRepository : SqlSugarService, IOrderRecordRepository
    {
        public bool Create(OrderRecordDTO req)
        {
            throw new NotImplementedException();
        }

        public List<OrderRecordDetailDTO> GetList(OrderRecordSearchDTO req, out int total)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<OrderRecordDetailDTO> list = new List<OrderRecordDetailDTO>();
                string sql = "SELECT * FROM(" +
                    "    SELECT RE.*, U.czdmmc00 AS UserName, ISNULL(T.Id, 0) AS TableId, T.Name AS TableName FROM dbo.R_OrderRecord RE" +
                    "    LEFT JOIN dbo.czdm U ON U.Id = RE.CreateUser" +
                    "    LEFT JOIN dbo.R_OrderTable OT ON OT.Id = RE.R_OrderTable_Id AND RE.R_OrderTable_Id > 0" +
                    "    LEFT JOIN dbo.R_Table T ON T.Id = OT.R_Table_Id AND OT.R_Table_Id > 0" +
                    "  	 ) Record " +
                    "WHERE Record.R_Order_Id = @orderId";

                if (req.TableId > 0)
                {
                    sql += " AND Record.TableId = @tableId";
                    list = db.SqlQuery<OrderRecordDetailDTO>(sql, new { orderId = req.OrderId, tableId = req.TableId });
                }
                else
                {
                    list = db.SqlQuery<OrderRecordDetailDTO>(sql, new { orderId = req.OrderId });
                }

                total = list.Count();
                return list;
            }
        }

        public List<OrderRecordDetailDTO> GetList(int orderId, int tableId = 0)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                int total = 0;

                List<OrderRecordDetailDTO> list = new List<OrderRecordDetailDTO>();
                string sql = "SELECT * FROM(" +
                    "    SELECT RE.*, U.czdmmc00 AS UserName, ISNULL(T.Id, 0) AS TableId, T.Name AS TableName FROM dbo.R_OrderRecord RE" +
                    "    LEFT JOIN dbo.czdm U ON U.Id = RE.CreateUser" +
                    "    LEFT JOIN dbo.R_OrderTable OT ON OT.Id = RE.R_OrderTable_Id AND RE.R_OrderTable_Id > 0" +
                    "    LEFT JOIN dbo.R_Table T ON T.Id = OT.R_Table_Id AND OT.R_Table_Id > 0" +
                    "  	 ) Record " +
                    "WHERE Record.R_Order_Id = @orderId";

                if (tableId > 0)
                {
                    sql += " AND Record.TableId = @tableId";
                    list = db.SqlQuery<OrderRecordDetailDTO>(sql, new { orderId = orderId, tableId = tableId });
                }
                else
                {
                    list = db.SqlQuery<OrderRecordDetailDTO>(sql, new { orderId = orderId });
                }

                total = list.Count();
                return list;
            }
        }

        public OrderRecordDetailDTO GetModel(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(OrderRecordDTO req)
        {
            throw new NotImplementedException();
        }
    }
}
