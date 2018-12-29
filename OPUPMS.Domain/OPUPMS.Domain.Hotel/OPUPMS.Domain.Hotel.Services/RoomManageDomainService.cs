using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Dapper;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Domain.Hotel.Model.Dtos;
using OPUPMS.Domain.Hotel.Model.IServices;
using OPUPMS.Domain.Hotel.Model.IRepositories;
using OPUPMS.Domain.Hotel.Repository;
using OPUPMS.Domain.Hotel.Model.ConvertModels;
using Smooth.IoC.UnitOfWork;
using System.Data;

namespace OPUPMS.Domain.Hotel.Services
{
    public class RoomManageDomainService : IRoomManageDomainService
    {
        public RoomManageDomainService(IMultiDbDbFactory factory, IRoomSymbolRepository roomCodeRep, ISystemCodeRepository systemRep, IOperateLogRepository logRep, IGuestAccountingRepository accountingRep, IGuestDataRepository guestRep, IRoomRoutineRepository routineRep)
        {
            RoomFactory = factory;
            RoomCodeRep = roomCodeRep;
            SysCodeRep = systemRep;
            LogRepository = logRep;
            AccRepository = accountingRep;
            GuestRepository = guestRep;
            RoomRoutineRep = routineRep;
        }

        public IMultiDbDbFactory RoomFactory { get; private set; }

        /// <summary>
        /// 房号代码接口对象
        /// </summary>
        public IRoomSymbolRepository RoomCodeRep { get; private set; }

        /// <summary>
        /// 房间事务接口对象
        /// </summary>
        public IRoomRoutineRepository RoomRoutineRep { get; private set; }

        /// <summary>
        /// 系统代码接口对象
        /// </summary>
        public ISystemCodeRepository SysCodeRep { get; private set; }

        /// <summary>
        /// 操作记录接口对象
        /// </summary>
        public IOperateLogRepository LogRepository { get; private set; }

        /// <summary>
        /// 客人账务接口对象
        /// </summary>
        public IGuestAccountingRepository AccRepository { get; private set; }
        
        /// <summary>
        /// 客人信息接口对象
        /// </summary>
        public IGuestDataRepository GuestRepository { get; private set; }


        #region 获取房间列表信息方法

        public RoomListDto LoadRoomList(string token)
        {
            var roomCodeList = RoomCodeRep.LoadRoomSymbolList(token);
            var sysCodeList = SysCodeRep.GetCodeListByMutliTypes(token, new string[] { SystemCodeTypes.ROOM_CATEGORY, SystemCodeTypes.ROOM_GALLERY_CODE, SystemCodeTypes.ROOM_FLOOR_CODE, SystemCodeTypes.ROOM_STATE_TYPE });
            var guestList = GuestRepository.GetListByStatus(token, GuestInfoState.I);  //在住客人列表

            var customerNOArray = guestList.Select(x => x.Id).ToArray();//在住客人账号列表
            var guestAccList = AccRepository.GetListByGuestIds(token, customerNOArray);

            var guestBookingList = GuestRepository.GetListByStatus(token, GuestInfoState.N);
            guestBookingList = guestBookingList.Where(x => x.CheckInDate == DateTime.Today && !x.RoomNo.IsEmpty()).ToList();//今日预低客人信息

            RoomListDto roomListGuiDto = new RoomListDto();
            roomListGuiDto.RoomList = BuildRoomList(sysCodeList, roomCodeList, guestAccList, guestList, guestBookingList);

            roomListGuiDto.GalleryList = BuildGalleryList(sysCodeList, roomCodeList);
            roomListGuiDto.RoomTypeList = BuildRoomTypeList(sysCodeList);
            roomListGuiDto.RoomStateList = BuildRoomStateList(sysCodeList);
            roomListGuiDto.RoomStatistic = BuildRoomStatisticInfo(roomCodeList);

            return roomListGuiDto;
        }

        public async Task<RoomListDto> LoadRoomListAsync(string token)
        {
            var roomCodeList = await RoomCodeRep.LoadRoomSymbolListAsync(token);
            var sysCodeList = await SysCodeRep.GetCodeListByMutliTypesAsync(token, new string[] { SystemCodeTypes.ROOM_CATEGORY, SystemCodeTypes.ROOM_GALLERY_CODE, SystemCodeTypes.ROOM_FLOOR_CODE, SystemCodeTypes.ROOM_STATE_TYPE });

            var guestList = await GuestRepository.GetListByStatusAsync(token, GuestInfoState.I);  //在住客人列表

            var customerNOArray = guestList.Select(x => x.Id).ToArray();//在住客人账号列表
            var guestAccList = await AccRepository.GetListByGuestIdsAsync(token, customerNOArray);

            var guestBookingList = await GuestRepository.GetListByStatusAsync(token, GuestInfoState.N);
            guestBookingList = guestBookingList.Where(x => (x.CheckInDate.HasValue && x.CheckInDate == DateTime.Today) && !x.RoomNo.IsEmpty()).ToList();//今日预低客人信息

            RoomListDto roomListGuiDto = new RoomListDto();
            roomListGuiDto.RoomList = BuildRoomList(sysCodeList, roomCodeList, guestAccList, guestList, guestBookingList);

            roomListGuiDto.GalleryList = BuildGalleryList(sysCodeList, roomCodeList);
            roomListGuiDto.RoomTypeList = BuildRoomTypeList(sysCodeList); 
            roomListGuiDto.RoomStateList = BuildRoomStateList(sysCodeList);
            roomListGuiDto.RoomStatistic = BuildRoomStatisticInfo(roomCodeList);

            return roomListGuiDto;
        }

        /// <summary>
        /// 构造房间及客人详细信息
        /// </summary>
        /// <param name="sysCodeList"></param>
        /// <param name="roomCodeList"></param>
        /// <param name="guestAccList"></param>
        /// <param name="guestList"></param>
        /// <param name="bookingList"></param>
        /// <returns></returns>
        private List<RoomInfoDto> BuildRoomList(List<SystemCodeInfo> sysCodeList, List<RoomSymbolInfo> roomCodeList, List<GuestAccountingInfo> guestAccList, List<GuestDataInfo> guestList, List<GuestDataInfo> bookingList)
        {
            List<RoomInfoDto> roomInfoList = new List<RoomInfoDto>();

            #region 遍历生成房间及客人信息
            foreach (var room in roomCodeList)
            {
                var guest = guestList.Where(x => x.RoomNo == room.RoomNo).FirstOrDefault();//获取当前在住房客人信息
                var bookingGuest = bookingList.Where(x => x.RoomNo == room.RoomNo).FirstOrDefault();//今日预低客人信息
                RoomInfoDto roomInfo = new RoomInfoDto();

                roomInfo.RoomId = room.RoomNo;
                roomInfo.GalleryCode = room.GalleryCode;
                roomInfo.FloorCode = room.FloorCode;
                roomInfo.RoomTypeCode = room.RoomTypeCode;
                roomInfo.Price = room.Price;
                roomInfo.Status = room.Status;
                roomInfo.IsClean = room.IsClean;
                roomInfo.RoomState = room.RoomState;
                roomInfo.RoomTypeName = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_CATEGORY && x.SysCode == room.RoomTypeCode).Select(x => x.SysCodeName).FirstOrDefault();

                roomInfo.GalleryName = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_GALLERY_CODE && x.SysCode == room.GalleryCode).Select(x => x.SysCodeName).FirstOrDefault();

                roomInfo.FloorName = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_FLOOR_CODE && x.SysCode == room.FloorCode).Select(x => x.SysCodeName).FirstOrDefault();

                //在住客人信息
                if (guest != null)
                {
                    roomInfo.GuestId = guest.Id;
                    roomInfo.PayBillId = guest.PayBillId;
                    roomInfo.ChummageId = guest.ChummageId;
                    roomInfo.LinkRoomId = guest.LinkRoomId;
                    roomInfo.ChineseName = guest.ChineseName;

                    roomInfo.IsVIP = guest.VipType;
                    roomInfo.CheckInDate = guest.CheckInDate;
                    roomInfo.CheckInTime = guest.CheckInTime;
                    roomInfo.CheckOutDate = guest.CheckOutDate;
                    roomInfo.GuestRemark = guest.GuestRemark;
                    //roomDto.Remark1 = "";
                    roomInfo.Phone = guest.Phone;
                    roomInfo.RoomPriceCategory = guest.RoomPriceCategory;
                    //roomDto.Breakfast = room.Fhdmft00.Trim();
                    roomInfo.PayBillMethod = guest.PayBillMethod.Trim();
                    roomInfo.RoomPriceCategoryName = roomInfo.RoomPriceCategory;
                    //roomData.MsgTitle = new BKrlyService().GetKrlyTitileByZh(roomData.krzh);
                    //roomData.MsgText = new BKrlyService().GetKrlyTextByZh(roomData.krzh);
                    roomInfo.GuestCategory = guest.GuestCategory.Trim();
                }

                if (bookingGuest != null)
                {
                    roomInfo.BookingNo = bookingGuest.BookingId;
                    roomInfo.BookingGuestName = bookingGuest.ChineseName;
                    roomInfo.BookingCheckInDate = bookingGuest.CheckInDate;
                    roomInfo.BookingCheckOutDate = bookingGuest.CheckOutDate;
                    roomInfo.BookingPhone = bookingGuest.Phone;
                    roomInfo.BookingRemark = bookingGuest.GuestRemark;
                }

                if (guest != null && guest.Id > 0)
                {
                    roomInfo.Gender = guest.Gender == GenderDescTypes.Female ? GenderDescTypes.Female.GetDescription(typeof(GenderDescTypes), "Female") : GenderDescTypes.Male.GetDescription(typeof(GenderDescTypes), "Male");

                    //总消费
                    var totalAmount = guestAccList.Where(x => x.GuestId == guest.Id && x.BillType == PayBillTypes.C && x.AccountingType != GuestAccType.X && x.AccountingType != GuestAccType.H && x.AccountingType != GuestAccType.Y && x.Operation.IsEmpty() && x.CalculateAmount != 0 && x.AccId02 == 0).Select(x => x.CalculateAmount).Sum();

                    //总付款
                    var totalPayment = guestAccList.Where(x => x.GuestId == guest.Id && x.BillType == PayBillTypes.D && (x.AccountingType == GuestAccType.A || x.AccountingType == GuestAccType.H || x.AccountingType == GuestAccType.Z) && x.Status != AccStateSign.H).Select(x => x.CalculateAmount).Sum();

                    roomInfo.TotalAmount = totalAmount.ToDecimal();
                    roomInfo.TotalPayment = totalPayment.ToDecimal();
                }
                else
                {
                    roomInfo.Gender = "";
                    roomInfo.TotalAmount = 0;
                    roomInfo.TotalPayment = 0;
                }
                roomInfo.Balance = roomInfo.TotalPayment - roomInfo.TotalAmount;

                roomInfoList.Add(roomInfo);
            } 
            #endregion
            
            return roomInfoList;
        }

        /// <summary>
        /// 根据现有的房间代码信息，构造房间关联的楼座楼层信息
        /// </summary>
        /// <param name="sysCodeList"></param>
        /// <param name="roomCodeList"></param>
        /// <returns></returns>
        private List<GalleryDto> BuildGalleryList(List<SystemCodeInfo> sysCodeList, List<RoomSymbolInfo> roomCodeList)
        {
            var galleryCodeInfo = roomCodeList.Select(x => x.GalleryCode).Distinct().ToList();
            var sysGalleryInfo = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_GALLERY_CODE).ToList();

            List<GalleryDto> galleryList = new List<GalleryDto>();
            foreach (var code in galleryCodeInfo)
            {
                var result = sysGalleryInfo.Where(x => x.SysCode == code).FirstOrDefault();
                if (result != null)
                {
                    GalleryDto gallery = new GalleryDto();
                    gallery.Code = code;
                    gallery.Name = result.SysCodeName;

                    var floorCodeInfo = roomCodeList.Where(x => x.GalleryCode == code).Select(x => x.FloorCode).ToList();
                    var sysFloorInfo = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_FLOOR_CODE).ToList();
                    //获取关联的楼层信息列表
                    var floorList = sysFloorInfo.Where(x => floorCodeInfo.Contains(x.SysCode))
                        .Select(x => new FloorDto
                        {
                            Code = x.SysCode,
                            Name = x.SysCodeName,
                            GalleryCode = code,
                        }).ToList();


                    gallery.FloorList = floorList.OrderBy(x => x.Code).ToList();
                    galleryList.Add(gallery);
                }
            }
            return galleryList.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// 构造房型信息列表
        /// </summary>
        /// <param name="sysCodeList"></param>
        /// <returns></returns>
        private List<RoomTypeDto> BuildRoomTypeList(List<SystemCodeInfo> sysCodeList)
        {
            var roomTypeInfo = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_CATEGORY).ToList();
            List<RoomTypeDto> roomTypeList = roomTypeInfo.Select(x => new RoomTypeDto() { Code = x.SysCode, Name = x.SysCodeName }).ToList();

            return roomTypeList;
        }

        /// <summary>
        /// 构造房态描述信息
        /// </summary>
        /// <param name="sysCodeList"></param>
        /// <returns></returns>
        private List<RoomStateDto> BuildRoomStateList(List<SystemCodeInfo> sysCodeList)
        {
            var roomStateInfo = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_STATE_TYPE).ToList();
            List<RoomStateDto> roomStateList = roomStateInfo.Select(x => new RoomStateDto() { Code = x.SysCode, Name = x.SysCodeName }).ToList();

            return roomStateList;
        }

        /// <summary>
        /// 生成房态统计信息  （待处理完善）
        /// </summary>
        /// <returns></returns>
        private RoomStatisticDto BuildRoomStatisticInfo(List<RoomSymbolInfo> roomCodeList)
        {
            RoomStatisticDto statsInfo = new RoomStatisticDto();
            statsInfo.AverageRoomPrices = 256;
            statsInfo.ComplimentaryRooms = 0;
            statsInfo.ExpectArrivalRooms = 6;
            statsInfo.ExpectDepartureRooms = 9;
            statsInfo.HourRooms = 0;
            statsInfo.HouseUseRooms = 0;
            statsInfo.LockedRooms = 0;
            statsInfo.MemberGuests = 12;
            statsInfo.NetworkGuests = 20;
            statsInfo.OccupancyRate = 68;
            statsInfo.OccupiedRooms = 16;
            statsInfo.OutOfOrderRooms = 2;
            statsInfo.TotalRoomPrices = 5680;
            statsInfo.TotalRooms = roomCodeList.Count;
            statsInfo.VacantRooms = 10;
            statsInfo.WalkinGuests = 18;

            return statsInfo;
        }

        #endregion


        #region 房间操作

        public InitRoomDto InitRoomInfo(string token)
        {
            var sysCodeList = SysCodeRep.GetCodeListByMutliTypes(token, new string[] { SystemCodeTypes.MARKET_TYPE, SystemCodeTypes.COUNTRY_CODE, SystemCodeTypes.CREDENTIAL_CATEGORY, SystemCodeTypes.GENDER, SystemCodeTypes.GUEST_TYPE, SystemCodeTypes.PAYMENT_METHOD, SystemCodeTypes.PRICE_CATEGORY, SystemCodeTypes.ROOM_PRICE_STRUCT
            });

            sysCodeList = sysCodeList.Where(x => x.SysCodeState == "Y").ToList();

            InitRoomDto obj = new InitRoomDto();
            //obj.BookingTypeList = null;
            var clientTypeList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.MARKET_TYPE)
                                        .Select(x => new ClientSourceTypeDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            clientTypeList.Insert(0, new ClientSourceTypeDto() { Code = "", Name = "请选择" });
            obj.ClientSourceTypeList = clientTypeList;

            obj.CountrySourceList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.COUNTRY_CODE)
                                        .Select(x => new CountrySourceDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.CountrySourceList.Insert(0, new CountrySourceDto() { Code = "", Name = "请选择" });

            obj.CredentialCategoryList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.CREDENTIAL_CATEGORY)
                                        .Select(x => new CredentialCategoryDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.CredentialCategoryList.Insert(0, new CredentialCategoryDto() { Code = "", Name = "请选择" });

            obj.GenderTypeList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.GENDER)
                                        .Select(x => new GenderTypeDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.GenderTypeList.Insert(0, new GenderTypeDto() { Code = "", Name = "请选择" });

            obj.GuestCategoryList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.GUEST_TYPE)
                                        .Select(x => new GuestCategoryDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.GuestCategoryList.Insert(0, new GuestCategoryDto() { Code = "", Name = "请选择" });

            obj.GuestTypeList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.TEAM_TYPE)
                                        .Select(x => new GuestTypeDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.GuestTypeList.Insert(0, new GuestTypeDto() { Code = "", Name = "请选择" });

            obj.PaymentMethodList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.PAYMENT_METHOD)
                                        .Select(x => new PaymentMethodDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.PaymentMethodList.Insert(0, new PaymentMethodDto() { Code = "", Name = "请选择" });

            obj.RoomPriceCategoryList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.PRICE_CATEGORY)
                                        .Select(x => new RoomPriceCategoryDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.RoomPriceCategoryList.Insert(0, new RoomPriceCategoryDto() { Code = "", Name = "请选择" });

            obj.RoomPriceStructureList = sysCodeList.Where(x => x.SysCodeType == SystemCodeTypes.ROOM_PRICE_STRUCT)
                                        .Select(x => new RoomPriceStructureDto { Code = x.SysCode, Name = x.SysCodeName }).OrderBy(x => x.Code).ToList();
            obj.RoomPriceStructureList.Insert(0, new RoomPriceStructureDto() { Code = "", Name = "请选择" });

            return obj;
        }

        public async Task<bool> ApplyCheckinAsync(string token, SumbitCheckInDto checkinObj)
        {
            bool result = false;
            //if (1 == 1)
            //    return result;

            string[] openRoomList = new string[] { checkinObj.RoomNo };

            var routineRoomList = await RoomRoutineRep.GetRoomNoListByStatusAndDateAsync(token, new string[] { RoomRoutineTypes.B, RoomRoutineTypes.I }, checkinObj.CheckinDate, checkinObj.CheckoutDate);

            var guestRoomList = await GuestRepository.GetRoomNoListByStatusAndDateAsync(token, new string[] { GuestInfoState.I, GuestInfoState.N }, checkinObj.CheckinDate, checkinObj.CheckoutDate);

            var roomList = await RoomCodeRep.LoadRoomSymbolListAsync(token);
            var roomCodeList = roomList.Select(y => y.RoomNo).Where(x => !x.IsEmpty()).ToList();

            //从现有的房号列表中取对应的客人房号
            var limitedGuestRooms = guestRoomList.Select(x => x.RoomNo).Where(x => roomCodeList.Contains(x)).ToList();
            
            //从现有的房号列表中取对应的房务房号
            var limitedRoutineRooms = routineRoomList.Select(x => x.RoomNo).Where(x => roomCodeList.Contains(x)).ToList();

            var existsGuestRooms = limitedGuestRooms.Where(x => openRoomList.Contains(x));
            var existsRoutineRooms = limitedRoutineRooms.Where(x => openRoomList.Contains(x));
            var invalidRooms = existsGuestRooms.Union(existsRoutineRooms).ToList();
            if ((invalidRooms != null && invalidRooms.Count() > 0))
                throw new Exception(string.Format("所选房间({0})已被占用，请重新选择！", invalidRooms.Join()));

            //事务（工作单元）操作
            using (IUnitOfWork uow = RoomFactory.Create<IUnitOfWork, ISession>(token, IsolationLevel.ReadCommitted))
            {
                foreach (var guest in checkinObj.GuestList)
                {
                    result = await GuestRepository.AddNewGuestAsync(token, guest, uow);
                    if (!result)
                        break;
                }

                int chummageId = checkinObj.GuestList.Where(x => x.AccountType == 1).Select(x => x.Id).FirstOrDefault();// 主结人帐号
                foreach (var guest in checkinObj.GuestList)
                {
                    guest.ChummageId = chummageId;
                    result = await GuestRepository.UpdateGuestAsync(token, guest, uow);
                    if (!result)
                        break;
                }

                //OperateLogInfo logInfo = new OperateLogInfo();
                //logInfo.OperateType = "Z_2";
                //logInfo.OperateTime = DateTime.Now;
                //logInfo.UserCode = "";
                //logInfo.Remark = "于" + DateTime.Now.ToString() + "登陆，电脑名称-" + Net.Host + "，登陆IP地址-" + Net.Ip; ;
                //logInfo.OperateRemark = "登录";
                //logInfo.ActionName = "系统登录-" + user.UserName;

                //await LogRepository.SaveLog(token, logInfo);//写入操作日志记录
            }
            
            return result;
        }

        public async Task<GuestDataInfo> GetGuestDetailsByRoomNoAsync(string token, string roomNo, int guestId)
        {
            var guestList = await ((GuestDataRepository)GuestRepository).GetListByStatusAndRoomNoAsync(token, GuestInfoState.I, roomNo);
            GuestDataInfo guest = null;
            if (guestId > 0)
                guest = guestList.Where(x => x.Id == guestId).FirstOrDefault();
            else
                guest = guestList.Where(x => x.AccountType == 1).FirstOrDefault();
            
            return guest;
        }

        public async Task<List<LinkRoomDto>> GetLinkRoomListAsync(string token, int guestId)
        {
            var guestList = await GuestRepository.GetLinkRoomListByGuestIdAsync(token, guestId);

            string[] roomArray = guestList.Select(x => x.RoomNo).Distinct().ToArray();
            var roomList = await RoomCodeRep.GetRoomInfoListByNoAsync(token, roomArray);

            string[] sysCodeArray = roomList.Select(x => x.RoomTypeCode).Distinct().ToArray();
            var roomTypeList = await SysCodeRep.GetCodeListByMutliTypesAsync(token, new string[] { SystemCodeTypes.ROOM_CATEGORY });
            roomTypeList = roomTypeList.Where(x => sysCodeArray.Contains(x.SysCode)).ToList();

            List<LinkRoomDto> list = new List<LinkRoomDto>();
            foreach (var item in roomList)
            {
                LinkRoomDto roomDto = new LinkRoomDto();
                roomDto.GuestList = guestList.Where(x => x.RoomNo == item.RoomNo)
                    .Select(x => new LinkGuestDto { GuestId = x.Id, GuestName = x.ChineseName, RoomNo = x.RoomNo }).ToList();
                roomDto.RoomName = roomTypeList.Where(x => x.SysCode == item.RoomNo).Select(x => x.SysCodeName).FirstOrDefault();
                roomDto.RoomNo = item.RoomNo;
                list.Add(roomDto);
            }

            return list;
        }
        #endregion
    }
}
