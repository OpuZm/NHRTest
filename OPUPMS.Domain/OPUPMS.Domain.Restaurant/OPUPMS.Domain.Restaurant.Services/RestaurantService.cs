using System;
using System.Collections.Generic;
using System.Linq;
using OPUPMS.Domain.Base.Dtos;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.Repositories;

namespace OPUPMS.Domain.Restaurant.Services
{
    public class RestaurantService : SqlSugarService, IRestaurantService
    {
        readonly ITableRepository _tableRep;
        readonly IAreaRepository _areaRep;
        readonly IExtendItemRepository _extendItemRepository;
        readonly IRestaurantRepository _resRepository;

        public RestaurantService(
            IAreaRepository areaRepository,
            ITableRepository tableRepository, 
            IExtendItemRepository extendTypeRepository,
            IRestaurantRepository restaurantRepository)
        {
            _tableRep = tableRepository;
            _areaRep = areaRepository;
            _extendItemRepository = extendTypeRepository;
            _resRepository = restaurantRepository;
        }

        public RestaurantPlatformDTO LoadPlatformInfo(int restaurantId)
        {
            string errMsg = null;
            if (restaurantId == 0)
                errMsg = "无法读取当前餐厅信息，请确认此餐厅Id 有效!";

            var restaurant = _resRepository.GetModel(restaurantId);
            if (restaurant == null && restaurant.Id <= 0)
                errMsg = "无法读取当前餐厅信息，请确认此餐厅Id 有效!";

            if(errMsg != null)
                throw new Exception(errMsg);

            var areaList = _areaRep.GetList(restaurantId);
            var tableList = _tableRep.GetList(new TableSearchDTO() { RestaurantId = restaurantId });
            var tableStatusList = EnumToList.ConvertEnumToList(typeof(CythStatus));

            tableStatusList.Add(new BaseDto() { Key = 0, Text = "全部" });
            tableStatusList = tableStatusList.OrderBy(x => x.Key).ToList();

            var usedList = tableList.Where(x => x.CythStatus == CythStatus.在用).ToList();
            var totalAmount = usedList.Where(p=>p.IsVirtual==false).Sum(x => x.OrderNow.Sum(y => y.TotalAmount ?? 0));
            var totalGuest = usedList.Where(p=>p.IsVirtual==false).Sum(x => x.OrderNow.Sum(y => y.PersonNum));
            var usedCount = usedList.Where(p=>p.IsVirtual==false).Count();
            
            var dateItem = _extendItemRepository.GetModelList(restaurant.R_Company_Id, 10003).FirstOrDefault();
            
            foreach (var item in tableStatusList)
            {
                if (item.Key == 0)
                    item.Value = tableList.Count.ToString();
                else if (item.Key == (int)CythStatus.空置)
                    item.Value = tableList.Where(x => x.CythStatus == CythStatus.空置).Count().ToString();
                else if (item.Key == (int)CythStatus.在用)
                    item.Value = usedCount.ToString();
                else if (item.Key == (int)CythStatus.清理)
                    item.Value = tableList.Where(x => x.CythStatus == CythStatus.清理).Count().ToString();
            }

            var realUsedCount = tableList.Count(x => x.CythStatus == CythStatus.在用 && x.IsVirtual == false);
            RestaurantPlatformDTO resInfo = new RestaurantPlatformDTO
            {
                AreaList = areaList,
                BusinessDate = dateItem != null ? dateItem.ItemValue : "",
                CurrentTableUsedRate = ((float)usedCount / (float)tableList.Count(p=>p.IsVirtual==false) * 100).ToString("f2"),
                CurrentTotalAmount = totalAmount,
                CurrentTotalGuestNum = totalGuest,
                TableList = tableList,
                TableStatusList = tableStatusList
            };

            return resInfo;
        }
    }
}