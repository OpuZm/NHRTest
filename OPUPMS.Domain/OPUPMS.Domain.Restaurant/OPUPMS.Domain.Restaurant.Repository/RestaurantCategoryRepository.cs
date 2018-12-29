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
    public class RestaurantCategoryRepository : SqlSugarService, IRestaurantCategoryRepository
    {
        public bool Create(RestaurantCategoryCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    List<R_RestaurantCategory> list = new List<R_RestaurantCategory>();
                    foreach (var item in req.CategoryIds)
                    {
                        list.Add(new R_RestaurantCategory()
                        {
                            R_Restaurant_Id = req.R_Restaurant_Id,
                            R_Category_Id=item
                        });
                    }
                    db.InsertRange<R_RestaurantCategory>(list);
                    res = true;
                }
                catch (Exception ex)
                {
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public List<RestaurantCategoryListDTO> GetList(out int total, RestaurantCategorySearchDTO req)
        {
            total = 1;
            return null;
        }

        public RestaurantCategoryCreateDTO GetModel(int restaurantId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                RestaurantCategoryCreateDTO res = new RestaurantCategoryCreateDTO();
                var restaurant = db.Queryable<R_Restaurant>().Where(p => p.Id == restaurantId).First();
                var data = db.Queryable<R_RestaurantCategory>()
                    .Where(p => p.R_Restaurant_Id == restaurantId).ToList();
                res.CategoryIds = data.Select(p => p.R_Category_Id).ToList();
                res.R_Restaurant_Id = restaurantId;
                res.R_Restaurant_Name = restaurant.Name;
                return res;
            }
        }

        public bool Update(RestaurantCategoryCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    db.BeginTran();
                    db.Delete<R_RestaurantCategory>(p => p.R_Restaurant_Id == req.R_Restaurant_Id);
                    List<R_RestaurantCategory> list = new List<R_RestaurantCategory>();
                    foreach (var item in req.CategoryIds)
                    {
                        list.Add(new R_RestaurantCategory()
                        {
                            R_Restaurant_Id = req.R_Restaurant_Id,
                            R_Category_Id = item
                        });
                    }
                    db.InsertRange<R_RestaurantCategory>(list);
                    db.CommitTran();
                    res = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return res;
            }
        }

        public List<ProjectAndDetailListDTO> FilterRestaurantCategorys(List<ProjectAndDetailListDTO> req,int restaurantId)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                List<ProjectAndDetailListDTO> res = new List<ProjectAndDetailListDTO>();
                var parentCategorys = db.Queryable<R_RestaurantCategory>()
                    .Where(p => p.R_Restaurant_Id == restaurantId)
                    .Select(p => p.R_Category_Id).ToList();
                var childCategorys = db.Queryable<R_Category>()
                    .Where(p => parentCategorys.Contains(p.PId) && p.PId != 0)
                    .Select(p => p.Id).ToList();
                res = req.Where(p => childCategorys.Contains(p.Category)).ToList();
                return res;   
            }
        }

        public List<AllCategoryListDTO> FilterRestaurantCategorys(List<AllCategoryListDTO> req, int restaurantId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<AllCategoryListDTO> res = new List<AllCategoryListDTO>();
                var parentCategorys = db.Queryable<R_RestaurantCategory>()
                    .Where(p => p.R_Restaurant_Id == restaurantId)
                    .Select(p => p.R_Category_Id).ToList();
                res = req.Where(p => parentCategorys.Contains(p.Id)).ToList();
                return res;
            }
        }
    }
}
