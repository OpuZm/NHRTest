using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Infrastructure.Common;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class ExtendRepository : SqlSugarService, IExtendRepository
    {
        public ExtendRepository()
        {

        }

        public bool Create(ExtendCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;

                R_ProjectExtend model = new R_ProjectExtend()
                {
                    Name = req.Name,
                    Description = req.Description,
                    CyxmKzType = req.CyxmKzType,
                    Price = req.Price,
                    Unit = req.Unit,
                    R_ProjectExtendType_Id = req.ExtendType,
                    R_Company_Id = req.R_Company_Id
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public Dictionary<int, string> GetCategory()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();

            foreach (CyxmKzType item in (CyxmKzType[])Enum.GetValues(typeof(CyxmKzType)))
            {
                list.Add((int)item, item.ToString());
            }

            return list;
        }

        public List<ExtendListDTO> GetList(int type = 0)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ExtendListDTO> list = new List<ExtendListDTO>();
                var data = db.Queryable<R_ProjectExtend>();
                data = data.Where(p => p.IsDelete == false);
                if (type > 0)
                {
                    data = data.Where(p => (int)p.CyxmKzType == type);
                }

                if (data != null && data.Any())
                {
                    data.ToList().ForEach(p =>
                    {
                        list.Add(new ExtendListDTO()
                        {
                            Id = p.Id,
                            CyxmKzType = p.CyxmKzType.ToString(),
                            CyxmKzTypeId = (int)p.CyxmKzType,
                            Description = p.Description,
                            Name = p.Name,
                            Price = p.Price,
                            Unit = p.Unit
                        });
                    });
                }

                return list;
            }
        }

        public List<ExtendListDTO> GetList(out int total, ExtendSearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = string.Empty;
                List<ExtendListDTO> list = new List<ExtendListDTO>();

                var data = db.Sqlable()
                    .From<R_ProjectExtend>("s1")
                    .Join<R_ProjectExtendType>("s2", "s2.IsDelete = 0 and s1.R_ProjectExtendType_Id", "s2.Id", JoinType.Left)
                    .Where(" s1.IsDelete = 0 ");
                data = data.Where("s1.R_Company_Id=" + companyId);
                if (req.CyxmKzType > 0)
                {
                    data = data.Where("s1.CyxmKzType =" + req.CyxmKzType);
                }

                if (req.MinPrice > 0)
                {
                    data = data.Where("s1.Price >=" + req.MinPrice);
                }

                if (req.MaxPrice > 0)
                {
                    data = data.Where("s1.Price <=" + req.MaxPrice);
                }

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

                totalCount = data.Count();
                data.SelectToPageList<ExtendListDTO>(
                    "s1.*,s2.Id as ExtendTypeId,s2.Name as ExtendType",
                    order, (req.offset / req.limit) + 1, req.limit, null).ForEach(p =>
                   {
                       list.Add(new ExtendListDTO()
                       {
                           Id = p.Id,
                           CyxmKzType = Enum.GetName(typeof(CyxmKzType), int.Parse(p.CyxmKzType)),
                           CyxmKzTypeId = int.Parse(p.CyxmKzType),
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Unit = p.Unit,
                           ExtendTypeId = p.ExtendTypeId,
                           ExtendType = p.ExtendType
                       });
                   });

                total = totalCount;
                return list;
            }
        }

        public ExtendCreateDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ExtendCreateDTO model = null;
                var data = db.Queryable<R_ProjectExtend>().InSingle(id);

                if (data != null)
                {
                    model = new ExtendCreateDTO()
                    {
                        Id = data.Id,
                        CyxmKzType = data.CyxmKzType,
                        Description = data.Description,
                        Name = data.Name,
                        Price = data.Price,
                        Unit = data.Unit,
                        ExtendType = data.R_ProjectExtendType_Id
                    };
                }

                return model;
            }
        }

        public bool Update(ExtendCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_ProjectExtend model = new R_ProjectExtend()
                {
                    Name = req.Name,
                    Description = req.Description,
                    Id = req.Id,
                    CyxmKzType = req.CyxmKzType,
                    Price = req.Price,
                    Unit = req.Unit,
                    R_ProjectExtendType_Id = req.ExtendType
                };
                result = db.Update(model);
                return result;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                result = db.Update<R_ProjectExtend>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }

        #region [扩展类别]
        public bool CreateExtendType(ExtendTypeCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_ProjectExtendType model = new R_ProjectExtendType()
                {
                    Name = req.Name,
                    R_Company_Id = req.R_Company_Id
                };

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<ExtendTypeListDTO> GetExtendTypeList(int type = 0)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ExtendTypeListDTO> list = new List<ExtendTypeListDTO>();
                var data = db.Queryable<R_ProjectExtendType>().Where(p => p.IsDelete == false);

                if (type > 0)
                {
                    data = data.Where(p => (int)p.R_Company_Id == type);
                }

                if (data != null && data.Any())
                {
                    data.ToList().ForEach(p =>
                    {
                        list.Add(new ExtendTypeListDTO()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            R_Company_Id = p.R_Company_Id
                        });
                    });
                }

                return list;
            }
        }

        public List<ExtendTypeListDTO> GetExtendTypeList(out int total, ExtendTypeSearchDTO req)
        {
            var companyId = OperatorProvider.Provider.GetCurrent().CompanyId.ToInt();
            using (var db = new SqlSugarClient(Connection))
            {
                int totalCount = 0;
                string order = string.Empty;
                List<ExtendTypeListDTO> list = new List<ExtendTypeListDTO>();
                var data = db.Queryable<R_ProjectExtendType>().Where(p => p.IsDelete == false && p.R_Company_Id==companyId);

                if (!string.IsNullOrWhiteSpace(req.Name))
                {
                    data = data.Where(p => p.Name.Contains(req.Name));
                }

                if (!string.IsNullOrEmpty(req.Sort))
                {
                    order = string.Format("{0} {1}", req.Sort, req.Order);
                }
                else
                {
                    order = "Id desc";
                }

                totalCount = data.Count();

                if (totalCount > 0)
                {
                    data = data.OrderBy(order).Skip(req.offset).Take(req.limit);
                    data.ToList().ForEach(p =>
                    {
                        list.Add(new ExtendTypeListDTO()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            R_Company_Id = p.R_Company_Id
                        });
                    });
                }

                total = totalCount;
                return list;
            }
        }

        public ExtendTypeCreateDTO GetExtendTypeModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ExtendTypeCreateDTO model = null;
                var data = db.Queryable<R_ProjectExtendType>().InSingle(id);

                if (data != null)
                {
                    model = new ExtendTypeCreateDTO()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        R_Company_Id = data.R_Company_Id
                    };
                }

                return model;
            }
        }

        public bool UpdateExtendType(ExtendTypeCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                R_ProjectExtendType model = new R_ProjectExtendType()
                {
                    Name = req.Name,
                    Id = req.Id
                };
                result = db.Update(model);
                return result;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool IsDeleteExtendType(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                result = db.Update<R_ProjectExtendType>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }

        #endregion
    }
}