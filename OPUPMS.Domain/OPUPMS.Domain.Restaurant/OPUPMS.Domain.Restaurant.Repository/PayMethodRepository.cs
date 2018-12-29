using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using AutoMapper;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class PayMethodRepository : SqlSugarService, IPayMethodRepository
    {
        public bool Create(PayMethodCreateDTO req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool result = false;
                try
                {
                    R_PayMethod model= Mapper.Map<PayMethodCreateDTO, R_PayMethod>(req);
                    model.CreateDate = DateTime.Now;
                    model.IsDelete = false;
                    result = Convert.ToBoolean(db.Insert<R_PayMethod>(model));
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        public List<PayMethodListDTO> GetList(out int total, PayMethodSearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = string.Empty;
                List<PayMethodListDTO> list = new List<PayMethodListDTO>();
                if (req.Sort.Equals("id", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(req.Sort))
                {
                    order = "s1.Id desc";
                }
                else
                {
                    order = string.Format("{0} {1}", req.Sort, req.Order);
                }
                //var data = db.Queryable<R_PayMethod>()
                //    .JoinTable<R_PayMethod>((s1, s2) => s1.Pid == s2.Id && s2.IsDelete == false)
                //    .Select<PayMethodListDTO>("s1.*,s2.Name as ParentName");
                var data = db.Sqlable().From<R_PayMethod>("s1")
                    .Join<R_PayMethod>("s2", "s1.Pid", "s2.Id", JoinType.Left)
                    .Where("s1.IsDelete = 0");
                data = data.Where("(s1.IsSystem=1 or s1.R_Company_Id=" + companyId + ")");
                if (!string.IsNullOrWhiteSpace(req.Name))
                {
                    data = data.Where("s1.Name like '%" + req.Name + "%'");
                }
                if (req.Pid > 0)
                {
                    data = data.Where("s1.Pid=" + req.Pid + "");
                }
                totalCount = data.Count();
                list = data.SelectToPageList<PayMethodListDTO>(
                    "s1.*,s2.Name as ParentName", order, (req.offset / req.limit) + 1, req.limit, null);
                total = totalCount;
                return list;
            }
        }

        public List<PayMethodListDTO> GetList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string order = "s1.Id asc";
                List<PayMethodListDTO> list = new List<PayMethodListDTO>();
                var data = db.Sqlable().From<R_PayMethod>("s1")
                    .Join<R_PayMethod>("s2", "s1.Pid", "s2.Id", JoinType.Left)
                    .Where("s1.IsDelete = 0").OrderBy(order);
                list = data.SelectToList<PayMethodListDTO>(
                    "s1.*,s2.Name as ParentName");
                return list;
            }
        }

        public bool Update(PayMethodCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = false;
                try
                {
                    var updateModel = db.Queryable<R_PayMethod>().InSingle(req.Id);
                    if (updateModel.IsSystem)
                    {
                        throw new Exception("该支付方式为系统配置，不可修改");
                    }
                    updateModel.Name = req.Name;
                    updateModel.Remark = req.Remark;
                    updateModel.Pid = req.Pid;
                    result = db.Update<R_PayMethod>(updateModel);
                }
                catch (Exception ex)
                {
                    result = false;
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }

        public List<PayMethodListDTO> GetParents()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var data = db.Sqlable().From<R_PayMethod>("s1")
                    .Where("s1.Pid=0 and s1.IsDelete=0")
                    .SelectToList<PayMethodListDTO>("s1.*");
                return data;
            }
        }

        public PayMethodCreateDTO GetModel(int id)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var data = db.Queryable<R_PayMethod>().InSingle(id);
                PayMethodCreateDTO model= Mapper.Map<R_PayMethod, PayMethodCreateDTO>(data);
                return model;
            }
        }

        public bool Delete(int id)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    var model = db.Queryable<R_PayMethod>().InSingle(id);
                    if (model.IsSystem)
                    {
                        throw new Exception("该支付项为系统配置，不能删除");
                    }
                    res = db.Update<R_PayMethod>(new { IsDelete = true }, p => p.Id == id || p.Pid == id);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return res;
            }
        }

        public List<int> GetCheckOutRemovePayType()
        {
            try
            {
                List<int> result = new List<int>();
                if (!string.IsNullOrEmpty(CheckOutRemovePayType))
                {
                    result = new List<int>(CheckOutRemovePayType.Split(',').Select(p=>int.Parse(p)));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
