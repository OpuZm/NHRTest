using OPUPMS.Domain.Base.Services;
using OPUPMS.Domain.Hotel.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Hotel.Services
{
    public class MenuManageDomainService : IMenuManageDomainService<HotelMenuDto>
    {
        /// <summary>
        /// 根据UserId加载Web系统菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<HotelMenuDto> GetMenuList(string token, string userId)
        {
            List<HotelMenuDto> sysMenuList = new List<HotelMenuDto>();
            var allMenuList = BuildMainMenuList();
            return allMenuList;
        }

        public Task<List<HotelMenuDto>> GetMenuListAsync(string token, string userId)
        {
            throw new NotImplementedException();
        }

        public List<HotelMenuDto> LoadSysWebMenu(string userId)
        {
            //var list = DPockRepository.Instance.LoadByTypeId(3);

            //var userInfo = new DCzdmRepository(DecodeConnectString).GetByKey(userId);

            List<HotelMenuDto> sysMenuList = new List<HotelMenuDto>();
            var allMenuList = BuildMainMenuList();
            //foreach (var menuItme in allMenuList)
            //{
            //    if (userInfo.Czdmposx.HasValue)
            //    {
            //        if(BCzdmService.HasPermission(null, menuItme.MenuValue, menuItme.MenuType))
            //        {
            //            menuItme.HasPermission = true;
            //        }
            //        foreach (var subMenuItem in menuItme.SubMenus)
            //        {

            //        }
            //    }
            //}

            return allMenuList;
        }

        private List<HotelMenuDto> BuildMainMenuList()
        {
            List<HotelMenuDto> sysMainMenuList = new List<HotelMenuDto>();
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 10, MenuName = "房态管理", MenuValue = 0, MenuType = 0, IconName = "icon-Hotel", MenuUrl = "", SortId = 10, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 10).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 20, MenuName = "预订管理", MenuValue = 1, MenuType = 1, IconName = "icon-order-user", MenuUrl = "", SortId = 30, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 20).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 30, MenuName = "调账应收", MenuValue = 2097152, MenuType = 1, IconName = "icon-order-editmoney", MenuUrl = "", SortId = 40, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 30).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 40, MenuName = "宾客管理", MenuValue = 0, MenuType = 1, IconName = "icon-Customer", MenuUrl = "", SortId = 50, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 40).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 50, MenuName = "短信管理", MenuValue = 0, MenuType = 1, IconName = "icon-email", MenuUrl = "", SortId = 60, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 50).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 60, MenuName = "商品仓库", MenuValue = 0, MenuType = 1, IconName = "icon-kucunguanli", MenuUrl = "", SortId = 70, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 60).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 70, MenuName = "人事管理", MenuValue = 0, MenuType = 1, IconName = "icon-userset", MenuUrl = "", SortId = 80, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 70).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 80, MenuName = "会议室", MenuValue = 0, MenuType = 1, IconName = "icon-Meeting", MenuUrl = "", SortId = 90, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 80).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 90, MenuName = "报表管理", MenuValue = 0, MenuType = 1, IconName = "icon-data_dome", MenuUrl = "", SortId = 120, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 90).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 100, MenuName = "操作日志", MenuValue = 0, MenuType = 1, IconName = "icon-order-set", MenuUrl = "", SortId = 130, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 100).ToList() });
            sysMainMenuList.Add(new HotelMenuDto() { MenuId = 110, MenuName = "系统维护", MenuValue = 8, MenuType = 1, IconName = "icon-Set", MenuUrl = "", SortId = 140, SubMenus = BuildSubMenuList().Where(x => x.ParentMenuId == 110).ToList() });

            return sysMainMenuList;
        }

        private List<HotelMenuDto> BuildSubMenuList()
        {
            List<HotelMenuDto> sysSubMenuList = new List<HotelMenuDto>();
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 110, MenuName = "实时房态", MenuValue = 0, MenuType = 1, IconName = "icon-room_state", MenuUrl = "/H/RoomManage/Index", SortId = 10, ParentMenuId = 10 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 120, MenuName = "收银交班", MenuValue = 1, MenuType = 1, IconName = "icon-jiaojiebanjilu-", MenuUrl = "", SortId = 30, ParentMenuId = 10 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 130, MenuName = "房类预测", MenuValue = 2097152, MenuType = 1, IconName = "icon-yuceshixiang", MenuUrl = "", SortId = 40, ParentMenuId = 10 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 140, MenuName = "物品管理", MenuValue = 0, MenuType = 1, IconName = "icon-materiel", MenuUrl = "", SortId = 50, ParentMenuId = 10 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 210, MenuName = "添加预定", MenuValue = 0, MenuType = 1, IconName = "icon-edit", MenuUrl = "/H/BookingManage/Details", SortId = 60, ParentMenuId = 20 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 220, MenuName = "预定管理", MenuValue = 0, MenuType = 1, IconName = "icon-Reserve", MenuUrl = "/H/BookingManage/List", SortId = 70, ParentMenuId = 20 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 310, MenuName = "调账管理", MenuValue = 0, MenuType = 1, IconName = "icon-tiaozheng", MenuUrl = "", SortId = 80, ParentMenuId = 30 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 320, MenuName = "应收管理", MenuValue = 0, MenuType = 1, IconName = "icon-yingshou", MenuUrl = "", SortId = 90, ParentMenuId = 30 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 330, MenuName = "已结佣金", MenuValue = 0, MenuType = 1, IconName = "icon-jszh", MenuUrl = "", SortId = 120, ParentMenuId = 30 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 610, MenuName = "商品仓库", MenuValue = 0, MenuType = 1, IconName = "icon-prod", MenuUrl = "", SortId = 130, ParentMenuId = 60 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 620, MenuName = "商品毛利统计", MenuValue = 8, MenuType = 1, IconName = "icon-yhq", MenuUrl = "", SortId = 140, ParentMenuId = 60 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 630, MenuName = "客房用品", MenuValue = 8, MenuType = 1, IconName = "icon-riyongpin", MenuUrl = "", SortId = 140, ParentMenuId = 60 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 810, MenuName = "会议室管理", MenuValue = 0, MenuType = 1, IconName = "icon-huiyishiguanli", MenuUrl = "", SortId = 90, ParentMenuId = 80 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 820, MenuName = "添加会议室", MenuValue = 0, MenuType = 1, IconName = "icon-Meeting", MenuUrl = "", SortId = 120, ParentMenuId = 80 });

            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1110, MenuName = "前台客房", MenuValue = 0, MenuType = 1, IconName = "icon-qiantai", MenuUrl = "", SortId = 80, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1120, MenuName = "财务模块", MenuValue = 0, MenuType = 1, IconName = "icon-caiwu", MenuUrl = "", SortId = 90, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1130, MenuName = "操作员", MenuValue = 0, MenuType = 1, IconName = "icon-xingzhengxingye-copy", MenuUrl = "", SortId = 120, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1140, MenuName = "系统设置", MenuValue = 0, MenuType = 1, IconName = "icon-Set", MenuUrl = "", SortId = 130, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1150, MenuName = "佣金设置", MenuValue = 8, MenuType = 1, IconName = "icon-yhq", MenuUrl = "", SortId = 140, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1160, MenuName = "价类管理", MenuValue = 8, MenuType = 1, IconName = "icon-list", MenuUrl = "", SortId = 140, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1170, MenuName = "门锁设置", MenuValue = 0, MenuType = 1, IconName = "icon-wssuomen", MenuUrl = "", SortId = 90, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1180, MenuName = "会员设置", MenuValue = 0, MenuType = 1, IconName = "icon-huiyuan1", MenuUrl = "", SortId = 120, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1170, MenuName = "报表维护", MenuValue = 0, MenuType = 1, IconName = "icon-data_Column", MenuUrl = "", SortId = 90, ParentMenuId = 110 });
            sysSubMenuList.Add(new HotelMenuDto() { MenuId = 1180, MenuName = "签约客户", MenuValue = 0, MenuType = 1, IconName = "icon-kehu", MenuUrl = "", SortId = 120, ParentMenuId = 110 });
            return sysSubMenuList;
        }
    }

}
