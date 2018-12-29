using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Api.Controllers
{
    public class ApiPluginController : ApiController
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantService _restaurantService;
        private readonly IMarketRepository _marketRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRestaurantCategoryRepository _restaurantCategoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITableService _tableHandlerSers;

        public ApiPluginController(IRestaurantRepository restaurantRepository,
            IRestaurantService restaurantService, IMarketRepository marketRepository,
            ICategoryRepository categoryRepository, IRestaurantCategoryRepository restaurantCategoryRepository,
            IProjectRepository projectRepository, IOrderRepository orderRepository,
            ITableService tableHandlerSers)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantService = restaurantService;
            _marketRepository = marketRepository;
            _categoryRepository = categoryRepository;
            _restaurantCategoryRepository = restaurantCategoryRepository;
            _projectRepository = projectRepository;
            _orderRepository = orderRepository;
            _tableHandlerSers = tableHandlerSers;
        }

        public string Get()
        {
            return "Hello Starts2000.Web.Framework.WebApi";
        }

        /// <summary>
        /// 获取餐厅和分市信息
        /// </summary>
        /// <returns></returns>
        public JsonResult<Response> GetRestaurants()
        {
            var res = new Response() { Data = null, Successed = false };
            if (ModelState.IsValid)
            {
                try
                {
                    var restaurants = _restaurantRepository.GetList();
                    var markets = _marketRepository.GetList();
                    foreach (var item in restaurants)
                    {
                        item.MarketList = markets.Where(x => x.RestaurantId == item.Id).ToList();
                    }
                    res.Successed = true;
                    res.Data = restaurants;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            else
            {
                res.Message = string.Join(",", ModelState.SelectMany(ms => ms.Value.Errors).Select(e => e.ErrorMessage));
            }

            return Json(res);
        }
    }
}