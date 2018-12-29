using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.Hotel.Model.Dtos;
using OPUPMS.Domain.Hotel.Model.IServices;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Web;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Domain.Hotel.Model.ConvertModels;

namespace OPUPMS.Web.Hotel.Controllers
{
    //[RoutePrefix("HRoomManage")]
    public class RoomManageController : AuthorizationController
    {
        readonly IRoomManageService _roomService;
        readonly IGuestAccountingRepository _accRepository;
        readonly IGuestRoutineRepository _guestRoutineRep;
        public RoomManageController(IRoomManageService roomService, IGuestAccountingRepository accRepository, IGuestRoutineRepository guestRoutineRepository)
        {
            _roomService = roomService;
            _accRepository = accRepository;
            _guestRoutineRep = guestRoutineRepository;
        }

        #region 房态图页面操作
        // GET: RoomManage
        [ActionName("Index")]
        public ActionResult Index()
        {
            try
            {
                return View("RoomList");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 加载房态数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[HandlerAjaxOnly]
        [ActionName("RoomList")]
        public async Task<ActionResult> RoomList()
        {
            try
            {
                var operatorProvider = OperatorProvider.Provider.GetCurrent();
                var list = await _roomService.LoadRoomListAsync(operatorProvider.ConnectToken);
                list.ResponseResult = new AjaxResult() { Status = ResultType.Success.ToString() };
                var json = list.ToJson();
                return Content(json);
            }
            catch (Exception ex)
            {
                RoomListDto obj = new RoomListDto();
                obj.ResponseResult = new AjaxResult() { Status = ResultType.Error.ToString(), Message = ex.Message };
                return Content(obj.ToJson());
                //throw new Exception(ex.Message);
            }
        }
        #endregion
        
        
        #region 房间业务操作，开房，查看房间等

        /// <summary>
        /// 开房房间信息页面
        /// </summary>
        /// <returns></returns>
        //[ActionName("RoomInfo")]
        public ActionResult RoomInfo()
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var obj = _roomService.InitRoomInfo(operatorProvider.ConnectToken);
            ViewBag.CountryList = obj.CountrySourceList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code, Selected = x.Code == "CN" });
            ViewBag.ClientSourceList = obj.ClientSourceTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.CredentialList = obj.CredentialCategoryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code, Selected = x.Code == "01" });
            ViewBag.GenderList = obj.GenderTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.GuestCategoryList = obj.GuestCategoryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.GuestTypeList = obj.GuestTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.PaymentMethodList = obj.PaymentMethodList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.RoomPriceCategoryList = obj.RoomPriceCategoryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.RoomPriceStructureList = obj.RoomPriceStructureList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            //return Content(list.ToJson());
            return View();
        }

        public async Task<ActionResult> ApplyOpenRoom(string jsonObj)
        {
            try
            {
                var operatorProvider = OperatorProvider.Provider.GetCurrent();
                bool result = false;
                if (!jsonObj.IsEmpty())
                {
                    SumbitCheckInDto checkinObj = jsonObj.ToObject<SumbitCheckInDto>();
                    result = await _roomService.ApplyCheckinAsync(operatorProvider.ConnectToken, checkinObj);
                }

                return NewtonSoftJson(new JsonMessage<int, object> { Status = 1, Message = "开房成功" }, "text/html", JsonRequestBehavior.AllowGet, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #region 房间详情操作

        /// <summary>
        /// 打开房间页面
        /// </summary>
        /// <returns></returns>
        //[Route("Details")]
        [ActionName("Details")]
        public ActionResult Details()
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var obj = _roomService.InitRoomInfo(operatorProvider.ConnectToken);
            ViewBag.CountryList = obj.CountrySourceList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code, Selected = x.Code == "CN" });
            ViewBag.ClientSourceList = obj.ClientSourceTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.CredentialList = obj.CredentialCategoryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code, Selected = x.Code == "01" });
            ViewBag.GenderList = obj.GenderTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.GuestCategoryList = obj.GuestCategoryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.GuestTypeList = obj.GuestTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.PaymentMethodList = obj.PaymentMethodList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.RoomPriceCategoryList = obj.RoomPriceCategoryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });
            ViewBag.RoomPriceStructureList = obj.RoomPriceStructureList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code });

            return View("RoomDetails");
        }

        /// <summary>
        /// 房态页面双击（或点击联房客人）后，获取房间客人信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [Route("LoadGuestDetails/{guestId:int}/roomId")]
        public async Task<ActionResult> LoadGuestDetails(int guestId, string roomId)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var obj = await _roomService.GetGuestDetailsByRoomNoAsync(operatorProvider.ConnectToken, roomId, guestId);

            return Content(obj.ToJson());
        }

        /// <summary>
        /// 获取房间详情(左侧房间客人列表)关联的联房及客人信息
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns></returns>
        [Route("Details/GetLinkRoomAndGuestList")]
        public ActionResult GetLinkRoomAndGuestList(int guestId)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var obj = _roomService.GetLinkRoomListAsync(operatorProvider.ConnectToken, guestId);

            return Content(obj.ToJson());
        }

        /// <summary>
        /// 获取房间客人的账务详情
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [Route("Details/GetAccDetails")]
        //[ActionName("Details/AccDetails")]
        public async Task<ActionResult> GetAccDetails(int guestId)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var obj = await _accRepository.GetListByGuestIdsAsync(operatorProvider.ConnectToken, new int[] { guestId });

            return Content(obj.ToJson());
        } 

        /// <summary>
        /// 获取房间客人事务信息
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns></returns>
        [Route("Details/GetGuestRoutineList")]
        public async Task<ActionResult> GetGuestRoutineList(int guestId)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var obj = await _guestRoutineRep.GetListByGuestIdAsync(operatorProvider.ConnectToken, guestId);

            return Content(obj.ToJson());
        }

        /// <summary>
        /// 新增房间客人账务信息
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns></returns>
        [Route("Details/AddGuestBillInfo")]
        public async Task<ActionResult> AddGuestBillInfo(string jsonObj)
        {
            try
            {
                var operatorProvider = OperatorProvider.Provider.GetCurrent();
                GuestAccountingInfo obj = jsonObj.ToObject<GuestAccountingInfo>();
                var result = await _accRepository.AddNewOrUpdateAccountingInfo(operatorProvider.ConnectToken, obj);
                if (result)
                    return NewtonSoftJson(new JsonMessage<int, object> { Status = 1, Message = "已成功添加客人账务信息！" }, "text/html", JsonRequestBehavior.AllowGet, true);

                return NewtonSoftJson(new JsonMessage<int, object> { Status = 0, Message = "新增账务信息失败" }, "text/html", JsonRequestBehavior.AllowGet, true);
            }
            catch (Exception ex)
            {
                return NewtonSoftJson(new JsonMessage<int, object> { Status = 0, Message = ex.Message }, "text/html", JsonRequestBehavior.AllowGet, true);
            }
        }
        #endregion

        #endregion

    }
}
