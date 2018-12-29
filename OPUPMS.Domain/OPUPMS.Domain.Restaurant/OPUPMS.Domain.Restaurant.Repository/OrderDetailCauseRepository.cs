using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using AutoMapper;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class OrderDetailCauseRepository : SqlSugarService, IOrderDetailCauseRepository
    {
        public bool Edit(OrderDetailCauseDTO req)
        {
            using (var db= new SqlSugarClient(Connection))
            {
                bool result = false;
                try
                {
                    var model = Mapper.Map<OrderDetailCauseDTO, R_OrderDetailCause>(req);
                    if (req.Id>0)
                    {
                        result = db.Update(model);
                    }
                    else
                    {
                        result = db.Insert(model) != null ? true : false;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return result;
            }
        }

        public List<OrderDetailCauseListDTO> GetList(out int total, OrderDetailCauseSearch req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = string.Empty;
                List<OrderDetailCauseListDTO> list = new List<OrderDetailCauseListDTO>();

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
                var data = db.Sqlable()
                    .From<R_OrderDetailCause>("")
                    .Where("IsDelete = 0  ");
                if (req.CauseType > 0)
                {
                    data = data.Where("CauseType=" + req.CauseType);
                }
                totalCount = data.Count();
                list = data.SelectToPageList<OrderDetailCauseListDTO>(
                    @"*,(CASE CauseType WHEN 1 THEN '赠送' WHEN 2 THEN '退菜' end) AS CauseTypeName",
                    order, (req.offset / req.limit) + 1, req.limit, null);
                total = totalCount;
                return list;
            }
        }

        public OrderDetailCauseDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                try
                {
                    var model = db.Queryable<R_OrderDetailCause>().InSingle(id);
                    var result = Mapper.Map<R_OrderDetailCause, OrderDetailCauseDTO>(model);
                    return result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool Delete(List<int> ids)
        {
            using (var db =  new SqlSugarClient(Connection))
            {
                bool result = false;
                try
                {
                    result = db.Update<R_OrderDetailCause>(new { IsDelete = true }, x => ids.Contains(x.Id));
                }
                catch (Exception e)
                {
                    throw e;
                }
                return result;
            }
        }

        public List<OrderDetailCauseListDTO> GetAllList()
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var result = db.Queryable<R_OrderDetailCause>().Where(p => p.IsDelete == false)
                    .Select(p => new OrderDetailCauseListDTO()
                    {
                        Id=p.Id,
                        CauseType=p.CauseType,
                        IsDelete=p.IsDelete,
                        Remark=p.Remark,
                    }).ToList();
                return result;
            }
        }
    }
}
