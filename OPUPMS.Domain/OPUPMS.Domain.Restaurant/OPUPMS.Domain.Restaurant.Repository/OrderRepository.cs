using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoMapper;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Operator;
using SqlSugar;
using System.Net;
using System.Data.SqlClient;
using OPUPMS.Domain.Base.ConvertModels;
using OPUPMS.Domain.Base.Models;
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Domain.Base.Repositories.OldRepositories;
using OPUPMS.Infrastructure.Common.Net;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class OrderRepository : SqlSugarService, IOrderRepository
    {
        readonly IExtendItemRepository _extendItemRepository;//可扩展类型项表 
        readonly IOperateLogRepository _userLogRepository;
        public OrderRepository(IExtendItemRepository extendItemRepository, IOperateLogRepository logRepository)
        {
            _extendItemRepository = extendItemRepository;
            _userLogRepository = logRepository;
        }

        /// <summary>
        /// 开台提交
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tableIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public OpenTableCreateResultDTO OpenTableCreate(
            ReserveCreateDTO req, List<int> tableIds, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string messge = string.Empty;
                OpenTableCreateResultDTO res = new OpenTableCreateResultDTO();
                string ids = string.Join(",", tableIds);

                try
                {
                    db.BeginTran();

                    var isCanOpen = db.Queryable<R_Table>()
                        .Any(p => p.CythStatus != CythStatus.空置 && tableIds.Contains(p.Id));

                    if (!isCanOpen)
                    {
                        R_Order model = Mapper.Map<ReserveCreateDTO, R_Order>(req);
                        var insert = db.Insert<R_Order>(model);  //订单主表
                        int insertId = Convert.ToInt32(insert);

                        db.Update<R_Table>(new
                        {
                            CythStatus = (int)CythStatus.在用
                        }, p => tableIds.Contains(p.Id));
                        res.OrderId = insertId;

                        if (tableIds.Any())
                        {
                            res.OrderTableIds = new List<int>();

                            foreach (var item in tableIds)
                            {
                                R_OrderTable obj = new R_OrderTable();
                                obj.R_Order_Id = insertId;
                                obj.R_Table_Id = item;
                                obj.CreateDate = DateTime.Now;
                                obj.IsOpen = true; //开台标识
                                var cyddth = db.Insert(obj);   //订单台号

                                db.Insert(new R_OrderRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    R_Order_Id = insertId,
                                    CreateUser = 0,
                                    CyddCzjlStatus = CyddStatus.开台,
                                    CyddCzjlUserType = CyddCzjlUserType.员工,
                                    Remark = string.Empty,
                                    R_OrderTable_Id = Convert.ToInt32(cyddth)
                                });    //订单操作纪录
                                res.OrderTableIds.Add(Convert.ToInt32(cyddth));
                            }
                        }
                    }
                    else
                    {
                        res = null;
                        messge = "所选台号已被占用，请重新选择";
                    }

                    db.CommitTran();
                }

                catch (Exception e)
                {
                    res = null;
                    messge = e.Message;
                    db.RollbackTran();
                }

                msg = messge;
                return res;
            }
        }

        public bool IsCanChoseProject(int orderId)
        {
            return false;
        }

        /// <summary>
        /// 根据订单桌号获取该桌订单明细点餐项目
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public List<OrderDetailDTO> GetOrderTableProjects(int orderTableId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<OrderDetailDTO> list = db.Sqlable()
                    .From<R_OrderDetail>("s1")
                    .Join<R_ProjectDetail>("s2", "s1.CyddMxId", "s2.Id", JoinType.Left)
                    .Join<R_Project>("s3", "s2.R_Project_Id", "s3.Id", JoinType.Left)
                    .Join<R_Package>("s4", "s1.CyddMxId", "s4.Id", JoinType.Left)
                    .Where("s1.R_OrderTable_Id=" + orderTableId)
                    .OrderBy("s1.Id")
                    .SelectToList<OrderDetailDTO>(@"s1.*,
                        case s1.CyddMxType when 1 then s3.Id when 2 then s4.Id end as R_Project_Id,
                        case s1.CyddMxType when 1 then s3.Property when 2 then s4.Property end as Property, 
                        isnull(s1.ExtendName,s1.CyddMxName) as ProjectName,
                        s1.CyddMxId as Cyxm,CONVERT(varchar(100), s1.CreateDate, 8) as StrTime");

                if (list != null && list.Any())
                {
                    var cyddmxIds = list.Select(p => p.Id).ToArray();
                    List<ProjectExtendDTO> extends = db.Queryable<R_OrderDetailExtend>()
                        .JoinTable<R_ProjectExtend>((s1, s2) => s1.R_ProjectExtend_Id == s2.Id)
                        .JoinTable<R_OrderDetail>((s1, s3) => s1.R_OrderDetail_Id == s3.Id)
                        .JoinTable<R_OrderDetail, R_ProjectDetail>((s1, s3, s4) => s3.CyddMxId == s4.Id)
                        .Where<R_OrderDetailExtend>((s1) => cyddmxIds.Contains(s1.R_OrderDetail_Id))
                        .Select<R_ProjectExtend, R_OrderDetail, R_ProjectDetail, ProjectExtendDTO>(
                            (s1, s2, s3, s4) => new ProjectExtendDTO()
                            {
                                Id = s2.Id,
                                ExtendType = s2.CyxmKzType,
                                Price = s2.Price,
                                ProjectExtendName = s2.Name,
                                Unit = s2.Unit,
                                Project = s4.R_Project_Id,
                                ProjectExtend = s3.CyddMxId,
                                CyddMxId = s3.Id
                            })
                        .ToList();
                    string sql = @"select s1.*,s2.czdmmc00 AS CreateUserName,case s1.CyddMxCzType when 1 then '赠送' 
                        when 2 then '退菜' when 3 then '转入' when 4 then '转出' else '' end as CyddMxCzTypeName from R_OrderDetailRecord s1 
                        left join czdm s2 on s1.CreateUser = s2.Id where s1.IsCalculation = 1 and s1.R_OrderDetail_Id = ";
                    foreach (var item in list)
                    {
                        if (item.CyddMxType == CyddMxType.餐饮项目)
                        {
                            item.IsDiscount = item.Property & (int)CyxmProperty.是否可打折;
                            item.IsQzdz = item.Property & (int)CyxmProperty.是否强制打折;
                            item.IsCustomer = item.Property & (int)CyxmProperty.是否自定义;
                            item.IsGive = item.Property & (int)CyxmProperty.是否可赠送;
                            item.IsChangePrice = item.Property & (int)CyxmProperty.是否可改价;
                            item.IsChangeNum = item.Property & (int)CyxmProperty.送厨后可否更改数量;
                            item.IsRecommend = item.Property & (int)CyxmProperty.是否推荐;
                            item.Extend = new List<ProjectExtendDTO>();
                            item.ExtendRequire = new List<ProjectExtendDTO>();
                            item.ExtendExtra = new List<ProjectExtendDTO>();
                            if (extends != null && extends.Any())
                            {
                                item.Extend = extends.Where(p => p.CyddMxId == item.Id && p.ExtendType == CyxmKzType.做法).ToList() ?? new List<ProjectExtendDTO>();
                                item.ExtendRequire = extends.Where(p => p.CyddMxId == item.Id && p.ExtendType == CyxmKzType.要求).ToList() ?? new List<ProjectExtendDTO>();
                                item.ExtendExtra = extends.Where(p => p.CyddMxId == item.Id && p.ExtendType == CyxmKzType.配菜).ToList() ?? new List<ProjectExtendDTO>();
                            }
                        }
                        else if (item.CyddMxType == CyddMxType.餐饮套餐)
                        {
                            item.PackageDetailList = db.Queryable<R_OrderDetailPackageDetail>().Where(p => p.R_OrderDetail_Id == item.Id).ToList() ?? new List<R_OrderDetailPackageDetail>();
                            item.IsDiscount = item.Property & (int)CytcProperty.是否可打折;
                            item.IsChangePrice = item.Property & (int)CytcProperty.是否可改价;
                            item.IsGive = item.Property & (int)CytcProperty.是否可赠送;
                            item.IsQzdz = 0;
                            item.IsCustomer = 0;
                            item.IsChangeNum = 0;
                            item.IsRecommend = 0;
                        }
                        item.OrderDetailRecordCount = db.Sqlable().From<R_OrderDetailRecord>("s1")
                              .Where("s1.R_OrderDetail_Id=" + item.Id + " and s1.IsCalculation =1 group by s1.CyddMxCzType")
                           .SelectToList<OrderDetailRecordCountDTO>(" sum(s1.Num) as Num,s1.CyddMxCzType") ?? new List<OrderDetailRecordCountDTO>();
                        item.OrderDetailRecord = db.SqlQuery<OrderDetailRecordDTO>(sql + item.Id.ToString());
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 订单明细创建提交
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="orderTableIds">订单台号ID列表</param>
        /// <param name="status">0保存 1落单不打厨 2落单打厨</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public bool OrderDetailCreate(List<OrderDetailDTO> req, List<int> orderTableIds, CyddMxStatus status, OperatorModel userInfo, out string msg,CyddCzjlUserType userType=CyddCzjlUserType.员工,bool isListPrint=false)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string message = string.Empty; //返回操作信息
                bool res = true; //返回是否成功
                bool isStock = (status == CyddMxStatus.已出 || status == CyddMxStatus.未出) ? true : false;  //是否判断库存  保存状态不用判断库存
                bool isPrint = status == CyddMxStatus.已出 ? true : false;    //是否打印
                try
                {
                    #region 条件判断
                    var isCheckOut = db.Queryable<R_OrderTable>().Any(p => orderTableIds.Contains(p.Id) && p.IsCheckOut == true);
                    if (isCheckOut) { msg = "所点台号存在已结账状态，请重新选择台号点菜"; return false; }
                    var isLock = db.Queryable<R_OrderTable>().Any(p => orderTableIds.Contains(p.Id) && p.IsLock == true);
                    if (isLock) { msg = "所选台号已锁定！不能点菜！"; return false; }
                    #endregion

                    db.BeginTran();
                    //string remark = string.Empty;
                    List<string> remarkList = new List<string>();
                    int notContinue = 0; //记录库存是否溢出
                    string cpdyThGuid = string.Empty;   //出品打印按桌号出生成标识
                    List<Cpdy> cydyInsert = new List<Cpdy>();
                    var newReq = req.Where(p => p.Id == 0 || (p.Id > 0 && p.CyddMxStatus == CyddMxStatus.保存)).ToList();    //新添加的菜品列表 与保存的菜品
                    db.Update<R_OrderTable>(new { IsControl = false }, p => orderTableIds.Contains(p.Id));
                    #region 单台点餐，多台点餐都属于同一订单，查询订单信息，分市信息，餐厅信息
                    int thid = orderTableIds[0];
                    var order = db.Queryable<R_Order>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                        .Where<R_OrderTable>((s1, s2) => s2.Id == thid).Select("s1.*").FirstOrDefault();
                    var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == order.R_Market_Id);
                    var resturant = db.Queryable<R_Restaurant>().FirstOrDefault(p => p.Id == order.R_Restaurant_Id);
                    #endregion

                    #region 先删除之前保存的状态的数据 及其扩展。
                    var oldItemIds = new List<int>();
                    if (order.CyddStatus==CyddStatus.预定)
                    {
                        oldItemIds = db.Queryable<R_OrderDetail>().Where(p => orderTableIds.Contains(p.R_OrderTable_Id) && p.CyddMxStatus == CyddMxStatus.保存).Select(p => p.Id).ToList();
                    }
                    else
                    {
                        oldItemIds = db.Queryable<R_OrderDetail>().Where(p => p.R_OrderTable_Id == thid && p.CyddMxStatus == CyddMxStatus.保存).Select(p => p.Id).ToList();
                    }
                    //var oldItemIds = db.Queryable<R_OrderDetail>().Where(p => p.R_OrderTable_Id == thid && p.CyddMxStatus == CyddMxStatus.保存).Select(p => p.Id).ToList();
                    if (oldItemIds != null && oldItemIds.Any())
                    {
                        db.Delete<R_OrderDetail>(p => oldItemIds.Contains(p.Id));//删除该订单台号 已保存菜品的订单明细
                        db.Delete<R_OrderDetailExtend>(p => oldItemIds.Contains(p.R_OrderDetail_Id));//删除订单明细 特殊要求
                        db.Delete<R_OrderDetailRecord>(p => oldItemIds.Contains(p.R_OrderDetail_Id));//删除订单明细赠退记录
                        db.Delete<R_OrderDetailPackageDetail>(p => oldItemIds.Contains(p.R_OrderDetail_Id));//删除订单明细套餐 明细记录表
                    }
                    #endregion

                    #region 下单后更改数量
                    var updateReq = req.Where(p => p.Id > 0 && p.IsUpdateNum == true).ToList(); //需更改数量的菜品列表
                    if (updateReq != null && updateReq.Any())
                    {
                        foreach (var item in updateReq)
                        {
                            if (item.CyddMxType == CyddMxType.餐饮项目)
                            {
                                decimal projectExtendPrice = 0;
                                var projectExtend = db.Queryable<R_OrderDetailExtend>().Where(p => p.R_OrderDetail_Id == item.Id).ToList();
                                if (projectExtend != null && projectExtend.Any())
                                {
                                    projectExtendPrice = projectExtend.Sum(p => p.Price);//特殊要求价格计算
                                }
                                var detailRecordNum = db.Queryable<R_OrderDetailRecord>()
                                   .Where(p => p.R_OrderDetail_Id == item.Id && p.IsCalculation == true &&
                                   (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出
                                   || p.CyddMxCzType == CyddMxCzType.退菜)).GroupBy(p => p.R_OrderDetail_Id)
                                   .Select("sum(Num) as Num").FirstOrDefault();   //订单详情赠菜，转出，退菜总数

                                var oldDetail = db.Queryable<R_OrderDetail>().FirstOrDefault(p => p.Id == item.Id);  //原订单详情
                                var project = db.Queryable<R_Project>().Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                                var projectDetail = db.Queryable<R_ProjectDetail>().Where(p => p.Id == item.CyddMxId).FirstOrDefault();
                                if (project.IsStock)
                                {
                                    var projectUseStock = projectDetail.UnitRate * item.Num * orderTableIds.Count();
                                    var orignalProjectStock = projectDetail.UnitRate * oldDetail.Num * orderTableIds.Count();
                                    var updateProjectStock = project.Stock + orignalProjectStock - projectUseStock;
                                    var stock = updateProjectStock > 0 ? updateProjectStock : 0;  //一般应用于菜品的重量，不是个数,这里不做判断是否会库存溢出
                                    db.Update<R_Project>(new { Stock = stock }, p => p.Id == item.R_Project_Id);//更新库存数
                                }
                                //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)
                                oldDetail.OriginalTotalPrice = (item.Num - (detailRecordNum != null ? detailRecordNum.Num : 0)) * (oldDetail.Price + projectExtendPrice);
                                oldDetail.Num = item.Num;
                                //if (oldDetail.OriginalTotalPrice < 0)
                                //{
                                //    res = false; message = "数量不合法！";
                                //}
                                //else
                                //{
                                //    db.Update<R_OrderDetail>(oldDetail);
                                //}
                                db.Update<R_OrderDetail>(oldDetail);
                            }
                            else if (item.CyddMxType == CyddMxType.餐饮套餐)
                            {
                                //套餐不存在更改数量
                            }
                        }
                    }
                    #endregion

                    #region 下单后更改价格
                    var updatePriceReq = req.Where(p => p.IsUpdatePrice == true).ToList();
                    if (updatePriceReq!=null && updatePriceReq.Any())
                    {
                        foreach (var item in updatePriceReq)
                        {
                            var detailGiveNum = db.Queryable<R_OrderDetailRecord>()
                                .Where(p => p.R_OrderDetail_Id == item.Id && p.IsCalculation == true
                                && p.CyddMxCzType == CyddMxCzType.赠菜)
                                .Select("Num as Num").FirstOrDefault();
                            var detailRecordNum = db.Queryable<R_OrderDetailRecord>()
                               .Where(p => p.R_OrderDetail_Id == item.Id && p.IsCalculation == true &&
                               (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出
                               || p.CyddMxCzType == CyddMxCzType.退菜)).GroupBy(p => p.R_OrderDetail_Id)
                               .Select("sum(Num) as Num").FirstOrDefault();   //订单详情赠菜，转出，退菜总数
                            if (item.CyddMxType == CyddMxType.餐饮项目)
                            {
                                decimal projectExtendPrice = 0;
                                var projectExtend = db.Queryable<R_OrderDetailExtend>().Where(p => p.R_OrderDetail_Id == item.Id).ToList();
                                if (projectExtend != null && projectExtend.Any())
                                {
                                    projectExtendPrice = projectExtend.Sum(p => p.Price);//特殊要求价格计算
                                }
                                var oldDetail = db.Queryable<R_OrderDetail>().FirstOrDefault(p => p.Id == item.Id);  //原订单详情
                                var project = db.Queryable<R_Project>().Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                                var projectDetail = db.Queryable<R_ProjectDetail>().Where(p => p.Id == item.CyddMxId).FirstOrDefault();
                                //if (project.IsStock)
                                //{
                                //    var projectUseStock = projectDetail.UnitRate * item.Num * orderTableIds.Count();
                                //    var orignalProjectStock = projectDetail.UnitRate * oldDetail.Num * orderTableIds.Count();
                                //    var updateProjectStock = project.Stock + orignalProjectStock - projectUseStock;
                                //    var stock = updateProjectStock > 0 ? updateProjectStock : 0;  //一般应用于菜品的重量，不是个数,这里不做判断是否会库存溢出
                                //    db.Update<R_Project>(new { Stock = stock }, p => p.Id == item.R_Project_Id);//更新库存数
                                //}
                                //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)
                                oldDetail.OriginalTotalPrice = (item.Num - (detailRecordNum != null ? detailRecordNum.Num : 0)) * (oldDetail.Price + projectExtendPrice);
                                oldDetail.Price = item.Price;
                                oldDetail.GiveTotalPrice = (detailGiveNum != null ? detailGiveNum.Num : 0) * (oldDetail.Price + projectExtendPrice);
                                db.Update<R_OrderDetail>(oldDetail);
                            }
                            else if (item.CyddMxType == CyddMxType.餐饮套餐)
                            {
                                var oldDetail = db.Queryable<R_OrderDetail>().FirstOrDefault(p => p.Id == item.Id);  //原订单详情
                                oldDetail.OriginalTotalPrice = (item.Num - (detailRecordNum != null ? detailRecordNum.Num : 0)) * (item.Price);
                                oldDetail.Price = item.Price;
                                oldDetail.GiveTotalPrice = (detailGiveNum != null ? detailGiveNum.Num : 0) * oldDetail.Price;
                                db.Update<R_OrderDetail>(oldDetail);
                            }
                        }
                    }
                    #endregion

                    #region 库存判断
                    if (isStock)
                    {
                        foreach (var item in newReq)
                        {
                            /*餐饮项目库存判断 start*/
                            if (item.CyddMxType == CyddMxType.餐饮项目)
                            {
                                var project = db.Queryable<R_Project>().Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                                var projectDetail = db.Queryable<R_ProjectDetail>().Where(p => p.Id == item.CyddMxId).FirstOrDefault();

                                if (project.IsStock)
                                {
                                    var projectUseStock = projectDetail.UnitRate * item.Num * orderTableIds.Count();
                                    if (project.Stock < projectUseStock)
                                    {
                                        notContinue++;
                                        message += string.IsNullOrEmpty(message) ?
                                            item.ProjectName + "超出库存,剩余" + project.Stock + "" :
                                            "," + item.ProjectName + "超出库存,剩余" + project.Stock + "";
                                    }
                                    else
                                    {
                                        db.Update<R_Project>(new
                                        {
                                            Stock = project.Stock - projectUseStock
                                        }, p => p.Id == item.R_Project_Id);
                                    }
                                }
                            }
                            /*餐饮项目库存判断 end*/
                            /*餐饮套餐库存判断 start*/
                            else if (item.CyddMxType == CyddMxType.餐饮套餐)
                            {
                                if (item.PackageDetailList != null && item.PackageDetailList.Any())
                                {
                                    foreach (var pro in item.PackageDetailList)
                                    {
                                        var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == pro.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                        var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                        // var packagedetail = packageDetails.First(p => p.R_ProjectDetail_Id == pro.Id);

                                        if (projects.IsStock)
                                        {
                                            var projectUseStock = projectDetails.UnitRate * item.Num * pro.Num * orderTableIds.Count();    //该餐饮项目共需多少库存
                                            if (projects.Stock < projectUseStock)
                                            {
                                                notContinue++;
                                                message += string.IsNullOrEmpty(message) ?
                                                    item.ProjectName + "-" + projects.Name + "超出库存,剩余" + projects.Stock + "" :
                                                    "," + item.ProjectName + item.ProjectName + "-" + projects.Name + "超出库存,剩余" + projects.Stock + "";
                                            }
                                            else
                                            {
                                                db.Update<R_Project>(new
                                                {
                                                    Stock = projects.Stock - projectUseStock
                                                }, p => p.Id == projects.Id);
                                            }
                                        }
                                    }
                                }
                            }
                            /*餐饮套餐库存判断 end*/
                        }
                    }
                    #endregion

                    if (notContinue <= 0)//库存足够
                    {
                        var orderTables = db.Queryable<R_Table>()
                            .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                            .Where<R_OrderTable>((s1, s2) => orderTableIds.Contains(s2.Id))
                            .Select<R_OrderTable, OrderTableDTO>((s1, s2) => new OrderTableDTO
                            {
                                Id = s2.Id,
                                Name = s1.Name,
                                RestaurantArea=s1.R_Area_Id
                            }).ToList();      //台号列表
                        var detailIds = newReq.Where(p => p.CyddMxType == CyddMxType.餐饮项目)
                            .Select(p => p.CyddMxId).ToArray();  //餐饮项目详情Ids
                        var detailList = db.Queryable<R_ProjectDetail>()
                            .Where(p => detailIds.Contains(p.Id)).ToList();   //餐饮项目详情列表

                        #region 所需打印机列表
                        var projectIds = newReq.Where(p => p.CyddMxType == CyddMxType.餐饮项目).Select(p => p.R_Project_Id).ToList(); //餐饮项目Ids
                        //先全部取出ProjectDetail_Id
                        var projectDetailIds = new List<int>();
                        newReq.Where(p => p.CyddMxType == CyddMxType.餐饮套餐).ToList().ForEach(n =>
                        {
                            projectDetailIds = projectDetailIds.Concat(n.PackageDetailList.Select(p => p.R_ProjectDetail_Id).ToList()).ToList();    //套餐餐饮项目明细IDS
                        });
                        var packageProjects = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).Select(p => p.R_Project_Id).ToList();
                        projectIds = projectIds.Concat(packageProjects).ToList();    //连接餐饮项目IDS和套餐包含的餐饮项目IDS
                        projectIds = projectIds.GroupBy(p => p).Select(p => p.Key).ToList();
                        var printerList = db.Queryable<Printer>()
                            .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id && s2.IsDelete == false)
                            .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                            .Where<R_Stall, R_ProjectStall>((s1, s2, s3) => projectIds.Contains(s3.R_Project_Id) && s1.IsDelete == false )
                            .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                            {
                                Id = s1.Id,
                                Code = s1.Code,
                                IpAddress = s1.IpAddress,
                                IsDelete = s1.IsDelete,
                                Name = s1.Name,
                                PcName = s1.PcName,
                                PrintPort = s1.PrintPort,
                                Remark = s1.Remark,
                                ProjectId = s3.R_Project_Id,
                                BillType = s3.BillType,
                                StallName = s2.Name,
                                StallId = s2.Id
                            }).ToList();    //餐饮项目打印机列表
                        #endregion
                        foreach (var orderTable in orderTableIds)
                        {
                            cydyInsert = new List<Cpdy>();
                            cpdyThGuid = orderTable + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            remarkList = new List<string>();
                           
                            string dishStatus = string.Empty;   //即起|叫起
                            string detailRemark = string.Empty; //手写要求（备注）                           
                            foreach (var detail in newReq)
                            {
                                string yq = string.Empty;   //要求
                                string zf = string.Empty;   //做法
                                string pc = string.Empty;   //配菜
                                decimal giveNum = 0;//记录赠送菜品明细赠送总数量
                                decimal projectExtendPrice = 0;//记录特殊要求总价=要求+做饭+配菜
                                dishStatus = detail.DishesStatus.ToString();
                                detailRemark = string.IsNullOrEmpty(detail.Remark) ? "" : "," + detail.Remark;
                                remarkList.Add(string.Format("{0}{1} * {2}", detail.CyddMxName,
                                    string.IsNullOrEmpty(detail.Unit) ? "" : "(" + detail.Unit + ")", detail.Num));

                                #region 订单明细添加
                                var model = Mapper.Map<OrderDetailDTO, R_OrderDetail>(detail);
                                model.R_OrderTable_Id = orderTable;
                                model.CyddMxStatus = status;
                                model.CyddMxType = detail.CyddMxType;
                                model.CreateDate = DateTime.Now;
                                model.CreateUser = userInfo.UserId;
                                model.DiscountRate = detail.DiscountRate == 0 ? 1 : detail.DiscountRate;
                                model.DishesStatus = detail.DishesStatus;   //添加叫起|即起属性
                                model.IsListPrint = isListPrint;
                                var mxId = db.Insert<R_OrderDetail>(model);
                                int newMxId = Convert.ToInt32(mxId);
                                #endregion

                                #region 套餐详情
                                if (detail.CyddMxType == CyddMxType.餐饮套餐)
                                {
                                    if (detail.PackageDetailList != null && detail.PackageDetailList.Any())
                                    {
                                        detail.PackageDetailList.ForEach(p => p.R_OrderDetail_Id = Convert.ToInt32(mxId));
                                        db.InsertRange<R_OrderDetailPackageDetail>(detail.PackageDetailList);
                                    }
                                }
                                #endregion

                                #region 订单明细扩展添加
                                if (detail.Extend != null && detail.Extend.Any())
                                {
                                    projectExtendPrice = detail.Extend.Sum(p => p.Price);
                                    yq = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.要求).Select(p => p.ProjectExtendName).ToArray());
                                    zf = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.做法).Select(p => p.ProjectExtendName).ToArray());
                                    pc = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.配菜).Select(p => p.ProjectExtendName).ToArray());
                                    List<R_OrderDetailExtend> mxkzList = new List<R_OrderDetailExtend>();
                                    foreach (var extend in detail.Extend)
                                    {
                                        mxkzList.Add(new R_OrderDetailExtend()
                                        {
                                            R_OrderDetail_Id = Convert.ToInt32(mxId),
                                            R_ProjectExtend_Id = extend.Id,
                                            Name = extend.ProjectExtendName,
                                            Price = extend.Price,
                                            Unit = extend.Unit
                                        });
                                    }
                                    db.InsertRange(mxkzList);
                                }
                                #endregion

                                #region 赠送菜品
                                if (detail.OrderDetailRecordCount != null && detail.OrderDetailRecordCount.Any())
                                {
                                    giveNum = detail.OrderDetailRecordCount.Where(p=>p.CyddMxCzType==CyddMxCzType.赠菜).Sum(p => p.Num);
                                    var tableNames = orderTables.Select(p => p.Name).ToList();
                                    List<R_OrderDetailRecord> orderDetailRecordList = new List<R_OrderDetailRecord>();
                                    foreach (var recordCount in detail.OrderDetailRecordCount)
                                    {
                                        orderDetailRecordList.Add(new R_OrderDetailRecord()
                                        {
                                            CreateDate = DateTime.Now,
                                            R_OrderDetail_Id = Convert.ToInt32(mxId),
                                            CreateUser = userInfo.UserId,
                                            Num = recordCount.Num,
                                            CyddMxCzType = recordCount.CyddMxCzType,
                                            IsCalculation = true,
                                            Remark = recordCount.CyddMxCzType.ToString() + ":" + detail.CyddMxName + "*" + recordCount.Num + ",订单id：" + order.Id + ",台号：" + string.Join(",", tableNames)
                                        });
                                        //if (recordCount.CyddMxCzType==CyddMxCzType.赠菜)
                                        //{
                                        //    db.Update<R_OrderDetail>(new { GiveTotalPrice = detail.Price * recordCount.Num }, p => p.Id == newMxId);
                                        //}
                                    }
                                    db.InsertRange(orderDetailRecordList);
                                }
                                #endregion

                                #region 更新菜品原价总额(OriginalTotalPrice)
                                //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)  套餐没有特殊要求和赠送
                                var originalTotalPrice = detail.CyddMxType == CyddMxType.餐饮项目 ? (detail.Num - giveNum) * (detail.Price + projectExtendPrice) :
                                    detail.Num * detail.Price;
                                //if (originalTotalPrice >= 0)
                                //{
                                //    db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice, GiveTotalPrice= giveNum * (detail.Price + projectExtendPrice) }, p => p.Id == Convert.ToInt32(mxId));
                                //}
                                //else
                                //{
                                //    message = "数量不合法！";
                                //    res = false;
                                //}
                                db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice, GiveTotalPrice = giveNum * (detail.Price + projectExtendPrice) }, p => p.Id == Convert.ToInt32(mxId));
                                #endregion

                                #region 厨单出品单打印业务
                                if (isPrint)
                                {
                                    /*打印餐饮项目出品单 start*/
                                    if (detail.CyddMxType == CyddMxType.餐饮项目)
                                    {
                                        var projectPrinters = printerList.Where(p => p.ProjectId == detail.R_Project_Id && p.BillType == 1);   //菜品关联的出品单打印机
                                        if (projectPrinters.Any())
                                        {
                                            projectPrinters.ToList().ForEach(c =>
                                            {
                                                cydyInsert.Add(new Cpdy
                                                {
                                                    cymxxh00 = Convert.ToInt32(mxId),
                                                    cyzdxh00 = order.Id,
                                                    cymxdm00 = detail.CyddMxId.ToString(),
                                                    cymxmc00 = string.IsNullOrEmpty(model.ExtendName) ? model.CyddMxName : model.ExtendName,
                                                    cymxdw00 = detailList.FirstOrDefault(p => p.Id == detail.CyddMxId).Unit,
                                                    cymxsl00 = detail.Num.ToString(),
                                                    cymxdybz = false,
                                                    cymxyj00 = c.IpAddress,
                                                    cymxclbz = "0",
                                                    cymxczrq = DateTime.Now,
                                                    cymxzdbz = "0",
                                                    cymxyq00 = string.IsNullOrEmpty(yq) ? dishStatus + detailRemark : yq + "," + dishStatus + detailRemark,
                                                    cymxzf00 = zf,
                                                    cymxpc00 = pc,
                                                    cymxczy0 = userInfo.UserName,
                                                    cymxfwq0 = c.PcName,
                                                    cymxczdm = userInfo.UserCode,
                                                    cymxje00 = detail.Price.ToString(),
                                                    cymxth00 = orderTables.FirstOrDefault(p => p.Id == orderTable).Name,
                                                    cymxrs00 = order.PersonNum.ToString(),
                                                    cymxct00 = resturant.Name,
                                                    cymxzdid = cpdyThGuid,
                                                    cymxbt00 = "出品单",
                                                    cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                    cpdysdxh = cpdyThGuid,
                                                    cymxdk00 = c.StallName,
                                                    cymxgdbz = false,
                                                    cpdyfsl0 = market.Name
                                                });
                                            });
                                        }
                                    }
                                    /*打印餐饮项目出品单 end*/
                                    /*打印套餐出品单 start*/
                                    else if (detail.CyddMxType == CyddMxType.餐饮套餐)
                                    {
                                        if (detail.PackageDetailList != null && detail.PackageDetailList.Any())
                                        {
                                            detail.PackageDetailList.ForEach(n =>
                                            {
                                                var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == n.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                                var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                                var projectPrinters = printerList.Where(p => p.ProjectId == projects.Id && p.BillType == 1);   //菜品关联的出品单打印机
                                                if (projectPrinters.Any())
                                                {
                                                    projectPrinters.ToList().ForEach(c =>
                                                    {
                                                        cydyInsert.Add(new Cpdy
                                                        {
                                                            cymxxh00 = Convert.ToInt32(mxId),
                                                            cyzdxh00 = order.Id,
                                                            cymxdm00 = projectDetails.Id.ToString(),
                                                            cymxmc00 = n.Name,
                                                            cymxdw00 = projectDetails.Unit,
                                                            cymxsl00 = (detail.Num * n.Num).ToString(),
                                                            cymxdybz = false,
                                                            cymxyj00 = c.IpAddress,
                                                            cymxclbz = "0",
                                                            cymxczrq = DateTime.Now,
                                                            cymxzdbz = "0",
                                                            cymxyq00 = dishStatus,
                                                            cymxzf00 = detail.CyddMxName,
                                                            cymxpc00 = string.Empty,
                                                            cymxczy0 = userInfo.UserName,
                                                            cymxfwq0 = c.PcName,
                                                            cymxczdm = userInfo.UserCode,
                                                            cymxje00 = projectDetails.Price.ToString(),
                                                            cymxth00 = orderTables.FirstOrDefault(p => p.Id == orderTable).Name,
                                                            cymxrs00 = order.PersonNum.ToString(),
                                                            cymxct00 = resturant.Name,
                                                            cymxzdid = cpdyThGuid,
                                                            cymxbt00 = "出品单",
                                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                            cpdysdxh = cpdyThGuid,
                                                            cymxdk00 = c.StallName,
                                                            cymxgdbz = false,
                                                            cpdyfsl0 = market.Name
                                                        });
                                                    });
                                                }
                                            });
                                        }
                                    }
                                    /*打印套餐出品单 end*/
                                }
                                #endregion
                            }
                            #region 厨单总单打印
                            if (isPrint && newReq.Any())
                            {
                                var groupTotalPrint = printerList.Where(p => p.BillType == 2).GroupBy(p => new { p.StallId, p.Id });

                                if (groupTotalPrint.Any())
                                {
                                    groupTotalPrint.ToList().ForEach(c =>
                                    {
                                        var printModel = printerList.FirstOrDefault(p => p.Id == c.Key.Id && p.StallId == c.Key.StallId);
                                        cydyInsert.Add(new Cpdy
                                        {
                                            cymxxh00 = orderTable,
                                            cyzdxh00 = order.Id,
                                            cymxdm00 = "0",
                                            cymxmc00 = "总单",
                                            cymxdw00 = userInfo.UserName,
                                            cymxsl00 = string.Empty,
                                            cymxdybz = false,
                                            cymxyj00 = printModel.IpAddress,
                                            cymxclbz = "0",
                                            cymxczrq = DateTime.Now,
                                            cymxzdbz = "1",
                                            //cymxyq00 = detail.CyddMxName,
                                            //cymxzf00 = detail.CyddMxName,
                                            //cymxpc00 = detail.CyddMxName,
                                            cymxczy0 = userInfo.UserName,
                                            cymxfwq0 = printModel.PcName,
                                            cymxczdm = userInfo.UserCode,
                                            cymxje00 = newReq.Sum(p => p.Price * p.Num).ToString(),
                                            cymxth00 = orderTables.FirstOrDefault(p => p.Id == orderTable).Name,
                                            cymxrs00 = order.PersonNum.ToString(),
                                            cymxct00 = resturant.Name,
                                            cymxzdid = cpdyThGuid,
                                            cymxbt00 = "总单",
                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                            cpdysdxh = cpdyThGuid,
                                            cymxdk00 = printModel.StallName,
                                            cymxgdbz = false,
                                            cpdyfsl0 = market.Name
                                        });
                                    });
                                }

                                var areaId = orderTables.Where(p => p.Id == orderTable).Select(p => p.RestaurantArea).FirstOrDefault();
                                var prints = db.Queryable<Printer>()
                                    .JoinTable<R_WeixinPrint>((s1, s2) => s1.Id == s2.Print_Id)
                                    .JoinTable<R_WeixinPrint, R_WeixinPrintArea>((s1, s2, s3) => s2.Id == s3.R_WeixinPrint_Id)
                                    .Where<R_WeixinPrint, R_WeixinPrintArea>((s1, s2, s3) => s1.IsDelete == false &&
                                    s2.PrintType == PrintType.总单区域出单 && areaId == s3.R_Area_Id).Select("s1.*").ToList();
                                var printerListArea = prints.Distinct().ToList();

                                if (printerListArea.Any())
                                {
                                    foreach (var print in printerListArea)
                                    {
                                        cydyInsert.Add(new Cpdy
                                        {
                                            cymxxh00 = orderTable,
                                            cyzdxh00 = order.Id,
                                            cymxdm00 = "0",
                                            cymxmc00 = "区域总单",
                                            cymxdw00 = userInfo.UserName,
                                            cymxsl00 = string.Empty,
                                            cymxdybz = false,
                                            cymxyj00 = print.IpAddress,
                                            cymxclbz = "0",
                                            cymxczrq = DateTime.Now,
                                            cymxzdbz = "1",
                                            //cymxyq00 = detail.CyddMxName,
                                            //cymxzf00 = detail.CyddMxName,
                                            //cymxpc00 = detail.CyddMxName,
                                            cymxczy0 = userInfo.UserName,
                                            cymxfwq0 = print.PcName,
                                            cymxczdm = userInfo.UserCode,
                                            cymxje00 = newReq.Sum(p => p.Price * p.Num).ToString(),
                                            cymxth00 = orderTables.FirstOrDefault(p => p.Id == orderTable).Name,
                                            cymxrs00 = order.PersonNum.ToString(),
                                            cymxct00 = resturant.Name,
                                            cymxzdid = cpdyThGuid,
                                            cymxbt00 = "区域总单",
                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                            cpdysdxh = cpdyThGuid,
                                            cymxdk00 = print.Name,
                                            cymxgdbz = false,
                                            cpdyfsl0 = market.Name
                                        });
                                    }
                                }
                            }
                            #endregion
                            db.InsertRange<Cpdy>(cydyInsert);   //批量添加菜品打印信息
                            if (status != CyddMxStatus.保存)
                            {
                                db.Insert<R_OrderRecord>(new R_OrderRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    R_Order_Id = order.Id,
                                    CreateUser = userInfo.UserId,
                                    CyddCzjlStatus = CyddStatus.点餐,
                                    CyddCzjlUserType = userType,
                                    Remark = remarkList.Join(","),
                                    R_OrderTable_Id = orderTable
                                });    //订单操作纪录
                            }
                        }

                        db.Update<R_Order>(new
                        {
                            CyddStatus = CyddStatus.点餐
                        }, p => p.Id == order.Id && p.CyddStatus == CyddStatus.开台);  //更新订单状态
                    }
                    else
                    {
                        res = false;
                    }

                    if (res)
                    {
                        db.CommitTran();
                    }
                    else
                    {
                        db.RollbackTran();
                    }

                }
                catch (Exception e)
                {
                    res = false;
                    message = e.Message;
                    db.RollbackTran();
                }

                msg = message;
                return res;
            }
        }

        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="listOrderDetailID">订单明细的ID列表</param>
        /// <returns></returns>
        public List<OrderDetailDTO> GetOrderDetails(List<int> listOrderDetailID)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<OrderDetailDTO> list = db.Sqlable()
                        .From<R_OrderDetail>("s1")
                        .Join<R_ProjectDetail>("s2", "s1.CyddMxId", "s2.Id", JoinType.Inner)
                        .Join<R_Project>("s3", "s2.R_Project_Id", "s3.Id", JoinType.Inner)
                        .Where("s1.Id in (" + string.Join(",", listOrderDetailID) + ")")
                        .SelectToList<OrderDetailDTO>(@"s1.*,isnull(s1.ExtendName,s1.CyddMxName) as ProjectName,
                            s2.Unit as Unit,s1.CyddMxId as Cyxm,CONVERT(varchar(100), s1.CreateDate, 8) as StrTime");

                if (list != null && list.Any())
                {
                    var cyddmxIds = list.Select(p => p.Id).ToArray();
                    List<ProjectExtendDTO> extends = db.Queryable<R_OrderDetailExtend>()
                        .JoinTable<R_ProjectExtend>((s1, s2) => s1.R_ProjectExtend_Id == s2.Id)
                        .JoinTable<R_OrderDetail>((s1, s3) => s1.R_OrderDetail_Id == s3.Id)
                        .JoinTable<R_OrderDetail, R_ProjectDetail>((s1, s3, s4) => s3.CyddMxId == s4.Id)
                        .Where<R_OrderDetailExtend>((s1) => cyddmxIds.Contains(s1.R_OrderDetail_Id))
                        .Select<R_ProjectExtend, R_OrderDetail, R_ProjectDetail, ProjectExtendDTO>(
                            (s1, s2, s3, s4) => new ProjectExtendDTO()
                            {
                                Id = s2.Id,
                                ExtendType = s2.CyxmKzType,
                                Price = s2.Price,
                                ProjectExtendName = s2.Name,
                                Unit = s2.Unit,
                                Project = s4.R_Project_Id,
                                ProjectExtend = s3.CyddMxId,
                                CyddMxId = s3.Id
                            })
                        .ToList();

                    if (extends.Any())
                    {
                        foreach (var item in list)
                        {
                            item.Extend = extends.Where(p => p.CyddMxId == item.Id).ToList();
                        }
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 修改订单明细的折扣
        /// </summary>
        /// <param name="listOrderDetailID">订单明细的ID列表</param>
        /// <returns></returns>
        public bool UpdateOrderDetailDiscounts(List<SingleProductDiscountSetRequestDto> list)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                try
                {
                    foreach (var item in list)
                    {
                        db.Update<R_OrderDetail>(new
                        {
                            DiscountRate = item.DiscountRate
                        }, x => x.Id == item.OrderDetailID);
                        //插入一条餐饮订单明细操作记录

                        #region 插入订单明细修改记录
                        //R_OrderDetailRecord entity = new R_OrderDetailRecord
                        //{
                        //    CreateDate = DateTime.Now,

                        // };
                        // db.Insert<R_OrderDetailRecord>(entity);
                        #endregion

                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 根据订单台号获取台号信息
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public TableListDTO GetTableByOrderTableId(int orderTableId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var res = db.Queryable<R_Table>()
                    .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                    .JoinTable<R_Area>((s1, s3) => s1.R_Area_Id == s3.Id)
                    .Where<R_OrderTable>((s1, s2) => s2.Id == orderTableId)
                    .Select<R_Area, TableListDTO>((s1, s3) => new TableListDTO()
                    {
                        Id = s1.Id,
                        Area = s3.Name,
                        AreaId = s3.Id,
                        Name = s1.Name,
                        SeatNum = s1.SeatNum,
                        ServerRate = s1.ServerRate,
                        RestaurantId = s1.R_Restaurant_Id
                    })
                    .FirstOrDefault();

                return res;
            }
        }

        /// <summary>
        /// 根据台号获取所有预定订单
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public List<ReserveCreateDTO> GetReserveOrdersByTable(int tableId, Nullable<DateTime> minDate)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ReserveCreateDTO> res = new List<ReserveCreateDTO>();
                var data = db.Sqlable()
                    .From<R_Order>("s1")
                    .Where("s1.Id in (select R_Order_Id from R_OrderTable where R_Table_Id="
                        + tableId + ") and cyddstatus=1");

                if (minDate != null)
                {
                    data = data.Where("DateDiff(dd,s1.ReserveDate,'" + minDate + "')<=0");
                }

                res = data.SelectToList<ReserveCreateDTO>("s1.*");

                return res;
            }
        }

        /// <summary>
        /// 获取订单实体
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ReserveCreateDTO GetOrderModel(int orderId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ReserveCreateDTO res = db.Sqlable()
                    .From<R_Order>("s1")
                    .Where("s1.Id=" + orderId + " and cyddstatus=" + (int)CyddStatus.预定 + "")
                    .SelectToList<ReserveCreateDTO>("s1.*")
                    .FirstOrDefault();
                if (res == null)
                    return res;

                res.Tables = new List<TableListDTO>();
                var tables = db.Queryable<R_Table>()
                    .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                    .Where<R_OrderTable>((s1, s2) => s2.R_Order_Id == orderId)
                    .Select<R_Table>("s1.*");

                tables.ToList().ForEach(p =>
                {
                    res.Tables.Add(new TableListDTO()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Describe,
                        SeatNum = p.SeatNum,
                        AreaId = p.R_Area_Id
                    });
                });
                res.OrderTableIds = db.Queryable<R_OrderTable>().Where(p => p.R_Order_Id == res.Id).Select(p => p.Id).ToList();
                return res;
            }
        }

        /// <summary>
        /// 根据订单获取所有定金付款纪录
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderPayHistoryListDTO> GetOrderPayList(int orderId, CyddJzType cyddJzType)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<OrderPayHistoryListDTO> res = new List<OrderPayHistoryListDTO>();
                var data = db.Queryable<R_OrderPayRecord>()
                    .Where(p => p.R_Order_Id == orderId && p.CyddJzType == cyddJzType);

                data.ToList().ForEach(p =>
                {
                    res.Add(new OrderPayHistoryListDTO()
                    {
                        Id = p.Id,
                        CreateDate = p.CreateDate,
                        OrderId = p.R_Order_Id,
                        CyddJzStatus = p.CyddJzStatus.ToString(),
                        CyddJzType = p.CyddJzType,
                        CyddPayType = p.CyddPayType.ToString(),
                        PayAmount = p.PayAmount
                    });
                });

                return res;
            }
        }

        /// <summary>
        /// 创建订单付款纪录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool OrderDepositCreate(OrderPayHistoryDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = true;
                R_OrderPayRecord model = new R_OrderPayRecord();
                model.CreateDate = DateTime.Now;
                model.CreateUser = req.CreateUser;
                model.R_Order_Id = req.OrderId;
                model.CyddJzStatus = req.CyddJzStatus;
                model.CyddJzType = req.CyddJzType;
                model.CyddPayType = req.CyddPayType;
                model.PayAmount = req.PayAmount;
                model.R_Market_Id = req.MarketId;
                model.BillDate = req.BillDate;
                model.R_OrderMainPay_Id = 0;//预订定金不生成当次主结Id
                model.R_Restaurant_Id = req.R_Restaurant_Id;
                model.Remark = req.CyddJzType == CyddJzType.定金 && req.CyddJzStatus == CyddJzStatus.已付
                    ? "预订定金"
                    : (req.CyddJzType == CyddJzType.定金 && req.CyddJzStatus == CyddJzStatus.已退 ? "退回定金" : "");
                res = db.Insert<R_OrderPayRecord>(model) == null ? false : true;
                return res;
            }
        }

        public List<ReserveCreateDTO> GetListByOrderType(int orderType)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var sourceList = db.Queryable<R_Order>().Where(x => x.OrderType == orderType).ToList();
                List<ReserveCreateDTO> list = new List<ReserveCreateDTO>();
                foreach (var item in sourceList)
                {
                    list.Add(Mapper.Map<ReserveCreateDTO>(item));
                }
                return list;
            }
        }

        public List<ReserveCreateDTO> GetListByCustomerSource(int customerSource)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var sourceList = db.Queryable<R_Order>()
                    .Where(x => x.CyddOrderSource == customerSource)
                    .ToList();

                List<ReserveCreateDTO> list = new List<ReserveCreateDTO>();
                foreach (var item in sourceList)
                {
                    list.Add(Mapper.Map<ReserveCreateDTO>(item));
                }
                return list;
            }
        }

        public List<ReserveCreateDTO> GetOrderList(out int total, OrderListSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<ReserveCreateDTO> res = new List<ReserveCreateDTO>();

                string filterSql = " dbo.R_Order ";
                if (req.IsReserveList)
                    filterSql = "(SELECT * FROM dbo.R_Order WHERE CyddStatus = " + (int)CyddStatus.预定 + " and IsDelete=0)";

                string bookingField = string.Format("SELECT R_Order_Id, SUM(PayAmount) SumAmount " +
                    "FROM R_OrderPayRecord WHERE CyddJzType = {0} GROUP BY R_Order_Id", (int)CyddJzType.定金);

                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT O.*, R.Name AS RestaurantName, ISNULL(M.Name, '') AS MarketName, " +
                    "ISNULL(EI1.Name, '') AS OrderTypeName, " +
                    "ISNULL(EI2.Name, '') AS SourceTypeName, " +
                    "ISNULL(P.SumAmount, 0) BookingAmount FROM {0} O " +
                    "LEFT JOIN ({1}) P ON P.R_Order_Id = O.Id " +
                    "LEFT JOIN dbo.R_Market M ON M.Id = O.R_Market_Id AND M.R_Restaurant_Id = O.R_Restaurant_Id " +
                    "LEFT JOIN dbo.R_Restaurant R ON R.Id = O.R_Restaurant_Id " +
                    "LEFT JOIN dbo.ExtendItems EI1 ON EI1.Id = O.OrderType AND EI1.TypeId = 10001 " +
                    "LEFT JOIN dbo.ExtendItems EI2 ON EI2.Id = O.CyddOrderSource AND EI2.TypeId = 10002 " +
                    "WHERE 1=1 ", filterSql, bookingField);

                string whereSql = "";
                List<SqlParameter> paraLsit = new List<SqlParameter>();
                BuildSqlCondition(req, paraLsit, out whereSql);

                sql.AppendLine(whereSql);
                var list = db.SqlQuery<ReserveCreateDTO>(sql.ToString(), paraLsit);
                #region Append TableNames For List
                if (list.Any())
                {
                    var orderIds = list.Select(p => p.Id).ToArray();
                    var tableList = db.Queryable<R_Table>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                        .JoinTable<R_OrderTable, R_Order>((s1, s2, s3) => s2.R_Order_Id == s3.Id)
                        .Where<R_OrderTable, R_Order>((s1, s2, s3) => orderIds.Contains(s3.Id))
                        .Select<R_OrderTable, R_Order, OrderTableDTO>((s1, s2, s3) => new OrderTableDTO()
                        {
                            Id=s2.Id,
                            Name=s1.Name,
                            TableId=s1.Id,
                            OrderId=s3.Id
                        }).ToList();

                    foreach (var item in list)
                    {
                        item.TablesName = string.Join(",",tableList.Where(p => p.OrderId == item.Id).Select(p => p.Name));
                    }
                }
                #endregion
                total = list.Count;
                res = list.Skip(req.offset).Take(req.limit).ToList();
                return res;
            }
        }

        public SearchOrderStatisticalDTO GetOrderStatisticalList(OrderListSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                SearchOrderStatisticalDTO info = new SearchOrderStatisticalDTO();
                string whereSql = "";
                List<SqlParameter> paraLsit = new List<SqlParameter>();

                BuildSqlCondition(req, paraLsit, out whereSql);

                string summarySql = $@"
                        SELECT O.Id, O.OrderNo, O.CyddStatus, O.OrderType, O.CreateDate, O.CyddOrderSource, O.ConAmount,
                                ISNULL(M.Name, '') AS MarketName,
                                ISNULL(EI1.Name, '') AS OrderTypeName, 
                                ISNULL(EI2.Name, '') AS SourceTypeName, 
                                ISNULL(OT.PersomNum, 0) AS SumPeopleNum,
		                        ISNULL(OT.ConAmount, 0) AS SumConAmount,
		                        ISNULL(P.BookingAmount, 0) AS SumBookingAmount,
		                        (ISNULL(P.RealAmount, 0) + ISNULL(P.GiveAmount, 0) + ISNULL(P.BookingAmount, 0)) AS SumRealAmount,
		                        ISNULL(P.FractionAmount, 0) AS SumFractionAmount,
		                        ISNULL(P.ClearAmount, 0) AS SumClearAmount,
		                        ISNULL(P.ServiceAmount, 0) AS SumServiceAmount,
		                        ISNULL(P.DiscountAmount, 0) AS SumDiscountAmount,
                                ISNULL(P.GiveAmount, 0) AS SumGiveAmount,
                                ISNULL(OTN.OrderTableNames, 0) AS OrderTableNames
                        FROM  dbo.R_Order O 
                        LEFT JOIN (SELECT R_Order_Id AS OrderId,
				                        SUM(CASE WHEN CyddJzType = 1 THEN PayAmount ELSE 0 END)AS BookingAmount,
				                        SUM(CASE WHEN (CyddJzType = 2 OR CyddJzType = 3) THEN PayAmount ELSE 0 END)AS RealAmount,
				                        SUM(CASE WHEN CyddJzType = 4 THEN PayAmount ELSE 0 END)AS FractionAmount,
				                        SUM(CASE WHEN CyddJzType = 5 THEN PayAmount ELSE 0 END)AS ClearAmount,
				                        SUM(CASE WHEN CyddJzType = 6 THEN PayAmount ELSE 0 END)AS ServiceAmount,
				                        SUM(CASE WHEN CyddJzType = 7 THEN PayAmount ELSE 0 END)AS DiscountAmount,
                                        SUM(CASE WHEN CyddJzType = 9 THEN PayAmount ELSE 0 END)AS GiveAmount
			                        FROM R_OrderPayRecord WHERE CyddJzStatus = 2 
			                        GROUP BY R_Order_Id
		                        ) P ON P.OrderId = O.Id 
                        LEFT JOIN (SELECT R_Order_Id AS OrderId, SUM(RT.PersonNum) AS PersomNum, 
				                        SUM(ISNULL(Detail.ConAmount, 0)) ConAmount 
			                        FROM R_OrderTable RT 
			                        INNER JOIN (SELECT R_OrderTable_Id AS OrderTabId, 
								                        SUM(ISNULL(OriginalTotalPrice, 0)) ConAmount 
							                        FROM dbo.R_OrderDetail GROUP BY R_OrderTable_Id 
						                        ) Detail ON Detail.OrderTabId = RT.Id
						                        GROUP BY RT.R_Order_Id 
		                        ) OT ON OT.OrderId = O.Id 
                        Left Join (SELECT  Test.R_Order_Id ,OrderTableNames = ( STUFF(( SELECT    ',' + t.Name
                                FROM      R_OrderTable rt  left join R_Table t on rt.R_Table_Id=t.Id
                                where rt.R_Order_Id=Test.R_Order_Id FOR XML PATH('')), 1, 1, '') )
                                FROM    R_OrderTable AS Test
                                GROUP BY Test.R_Order_Id) OTN on OTN.R_Order_Id = O.ID 
                        LEFT JOIN dbo.R_Market M ON M.Id = O.R_Market_Id AND M.R_Restaurant_Id = O.R_Restaurant_Id 
                        LEFT JOIN dbo.ExtendItems EI1 ON EI1.Id = O.OrderType AND EI1.TypeId = 10001 
                        LEFT JOIN dbo.ExtendItems EI2 ON EI2.Id = O.CyddOrderSource AND EI2.TypeId = 10002 
                        WHERE 1=1 ";

                summarySql += whereSql;
                var list = db.SqlQuery<OrderStatisticalDTO>(summarySql, paraLsit);
                info.ListSummaryObj.TotalRecords = list.Count;
                info.ListSummaryObj.TotalClearAmount = list.Sum(x => x.SumClearAmount);
                info.ListSummaryObj.TotalDiscountAmount = list.Sum(x => x.SumDiscountAmount);
                info.ListSummaryObj.TotalFractionAmount = list.Sum(x => x.SumFractionAmount);
                info.ListSummaryObj.TotalPeopleNum = list.Sum(x => x.SumPeopleNum);
                info.ListSummaryObj.TotalRealAmount = list.Sum(x => x.SumRealAmount);
                info.ListSummaryObj.TotalServiceAmount = list.Sum(x => x.SumServiceAmount);
                info.ListSummaryObj.TotalConAmount = list.Sum(x => x.ConAmount);
                info.ListSummaryObj.TotalGiveChangeAmount = list.Sum(x => x.SumGiveAmount);
                info.OrderList = list.Skip(req.offset).Take(req.limit).ToList();
                return info;
            }
        }

        private void BuildSqlCondition(OrderListSearchDTO req, List<SqlParameter> paras, out string whereSql)
        {
            StringBuilder whereSqlStr = new StringBuilder();
            if (!req.IsIncludeDelete)
            {
                whereSqlStr.AppendLine("AND O.IsDelete = @IsDelete ");
                paras.Add(new SqlParameter() { ParameterName = "IsDelete", Value = 0 });
            }
            if (req.Restaurant > 0)
            {
                whereSqlStr.AppendLine("AND O.R_Restaurant_Id = @RestaurantId ");
                int resId = 0;
                if (req.ManagerRestaurant.Contains(req.Restaurant))
                    resId = req.Restaurant;

                paras.Add(new SqlParameter() { ParameterName = "RestaurantId", Value = resId });
            }
            else
            {
                //if (req.ManagerRestaurant != null && req.ManagerRestaurant.Count > 0)
                //    sql.Append(" AND O.R_Restaurant_Id IN (" + req.ManagerRestaurant.Join(",") + ")");
                //else
                whereSqlStr.AppendLine("AND O.R_Restaurant_Id = @RestaurantId ");
                paras.Add(new SqlParameter() { ParameterName = "RestaurantId", Value = 0 });
            }

            if (!req.OrderNo.IsEmpty())
            {
                var likeOrder= "%" + req.OrderNo + "%";
                //req.OrderNo = "%" + req.OrderNo + "%";
                whereSqlStr.AppendLine("AND (O.OrderNo like @LikeOrderNo or CAST(O.Id AS BIGINT)=@OrderNo)");
                paras.Add(new SqlParameter() { ParameterName = "LikeOrderNo", Value = likeOrder });
                paras.Add(new SqlParameter() { ParameterName = "OrderNo", Value = req.OrderNo });
            }

            if (req.CyddOrderSource > 0)
            {
                whereSqlStr.AppendLine("AND O.CyddOrderSource = @OrderSource ");
                paras.Add(new SqlParameter() { ParameterName = "OrderSource", Value = req.CyddOrderSource });
            }

            if (req.CyddStatus > 0)
            {
                whereSqlStr.AppendLine("AND O.CyddStatus = @OrderStatus ");
                paras.Add(new SqlParameter() { ParameterName = "OrderStatus", Value = req.CyddStatus });
            }

            if (req.MarketId > 0)
            {
                whereSqlStr.AppendLine("AND O.R_Market_Id = @MarketId ");
                paras.Add(new SqlParameter() { ParameterName = "MarketId", Value = req.MarketId });
            }

            if (req.OrderType > 0)
            {
                whereSqlStr.AppendLine("AND O.OrderType = @OrderType ");
                paras.Add(new SqlParameter() { ParameterName = "OrderType", Value = req.OrderType });
            }

            if (req.BeginDate.HasValue || req.EndDate.HasValue)
            {
                switch (req.OrderSearchListType)
                {
                    case OrderSearchListType.开单时间:
                        if (req.BeginDate.HasValue)
                        {
                            whereSqlStr.AppendLine("AND O.CreateDate >= @BeginDate ");
                            paras.Add(new SqlParameter() { ParameterName = "BeginDate", Value = req.BeginDate });
                        }
                        if (req.EndDate.HasValue)
                        {
                            whereSqlStr.AppendLine("AND O.CreateDate <= @EndDate ");
                            paras.Add(new SqlParameter() { ParameterName = "EndDate", Value = req.EndDate.Value.AddDays(1) });
                        }
                        break;
                    case OrderSearchListType.开台时间:
                        if (req.BeginDate.HasValue)
                        {
                            whereSqlStr.AppendLine("AND O.OpenDate >= @BeginDate ");
                            paras.Add(new SqlParameter() { ParameterName = "BeginDate", Value = req.BeginDate });
                        }
                        if (req.EndDate.HasValue)
                        {
                            whereSqlStr.AppendLine("AND O.OpenDate <= @EndDate ");
                            paras.Add(new SqlParameter() { ParameterName = "EndDate", Value = req.EndDate.Value.AddDays(1) });
                        }
                        break;
                    default:
                        break;
                }
                if (req.IsReserveList)
                {
                    if (req.BeginDate.HasValue)
                    {
                        whereSqlStr.AppendLine("AND O.ReserveDate >= @BeginDate ");
                        paras.Add(new SqlParameter() { ParameterName = "BeginDate", Value = req.BeginDate });
                    }
                    if (req.EndDate.HasValue)
                    {
                        whereSqlStr.AppendLine("AND O.ReserveDate <= @EndDate ");
                        paras.Add(new SqlParameter() { ParameterName = "EndDate", Value = req.EndDate.Value.AddDays(1) });
                    }
                }
            }

            //if (req.BeginDate.HasValue && req.EndDate.HasValue)
            //{
            //    whereSqlStr.AppendLine("AND (O.CreateDate BETWEEN @BeginDate AND @EndDate) ");
            //    paras.Add(new SqlParameter() { ParameterName = "BeginDate", Value = req.BeginDate });
            //    paras.Add(new SqlParameter() { ParameterName = "EndDate", Value = req.EndDate.Value.AddDays(1) });
            //}
            //else if (req.BeginDate.HasValue)
            //{
            //    whereSqlStr.AppendLine("AND O.CreateDate >= @BeginDate ");
            //    paras.Add(new SqlParameter() { ParameterName = "BeginDate", Value = req.BeginDate });
            //}
            //else if (req.EndDate.HasValue)
            //{
            //    whereSqlStr.AppendLine("AND O.CreateDate <= @EndDate ");
            //    paras.Add(new SqlParameter() { ParameterName = "EndDate", Value = req.EndDate.Value.AddDays(1) });
            //}

            //联系人 联系电话
            if (!req.ContactPerson.IsEmpty())
            {
                req.ContactPerson = "%" + req.ContactPerson + "%";
                whereSqlStr.AppendLine("AND ContactPerson LIKE @Contact ");
                paras.Add(new SqlParameter() { ParameterName = "Contact", Value = req.ContactPerson });
            }

            if (!req.ContactTel.IsEmpty())
            {
                req.ContactTel = "%" + req.ContactTel + "%";
                whereSqlStr.AppendLine("AND ContactTel LIKE @ContactTel ");
                paras.Add(new SqlParameter() { ParameterName = "ContactTel", Value = req.ContactTel });
            }

            if (!string.IsNullOrEmpty(req.TableName))
            {
                whereSqlStr.AppendLine("AND OTN.OrderTableNames in (@TableName)");
                paras.Add(new SqlParameter() { ParameterName = "TableName", Value = req.TableName });
            }

            string order = "ORDER BY ";
            if (!string.IsNullOrEmpty(req.Sort))
            {
                if (req.Sort.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    order += "O.Id DESC";
                }
                else
                {
                    order += string.Format("{0} {1}", req.Sort, req.Order);
                }

            }
            whereSqlStr.AppendLine(order);
            whereSql = whereSqlStr.ToString();
        }

        public List<TableLinkOrderDTO> GetOrderTableListBy(int[] tableIds, int[] orderStatus)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var orderTabList = db.Queryable<R_OrderTable>()
                            .JoinTable<R_Order>((s1, s2) => s1.R_Order_Id == s2.Id && orderStatus.Contains((int)s2.CyddStatus))
                            .Where<R_Order>((s1, s2) => tableIds.Contains(s1.R_Table_Id) && !s1.IsCheckOut && s1.IsOpen)
                            .Select<R_Order, TableLinkOrderDTO>((s1, s2) =>
                                new TableLinkOrderDTO()
                                {
                                    Id = s1.Id,
                                    CreateDate = s1.CreateDate,
                                    IsCheckOut = s1.IsCheckOut,
                                    IsOpen = s1.IsOpen,
                                    OrderCallType = s2.CyddCallType,
                                    OrderNo = s2.OrderNo,
                                    OrderPersonNum = s2.PersonNum,
                                    OrderStatus = s2.CyddStatus,
                                    OrderCreatedTime = s2.CreateDate,
                                    PersonNum = s1.PersonNum,
                                    R_Order_Id = s1.R_Order_Id,
                                    R_Table_Id = s1.R_Table_Id,
                                    IsLock = s1.IsLock,
                                    IsControl=s1.IsControl,
                                    ContactPerson=s2.ContactPerson,
                                    ContactTel=s2.ContactTel
                                }
                            ).ToList();

                return orderTabList;
            }
        }

        public List<R_OrderTable> GetOrderTableListBy(int id, SearchTypeBy searchType)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<R_OrderTable> list = new List<R_OrderTable>();
                if (searchType == SearchTypeBy.订单Id)
                    list = db.Queryable<R_OrderTable>().Where(x => x.R_Order_Id == id).ToList();
                else if (searchType == SearchTypeBy.台号Id)
                    list = db.Queryable<R_OrderTable>().Where(x => x.R_Table_Id == id && !x.IsCheckOut).ToList();
                else if (searchType == SearchTypeBy.订单台号id)
                    list = db.Queryable<R_OrderTable>().Where(x => x.Id == id).ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据订单号获得实体
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ReserveCreateDTO GetOrder(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                ReserveCreateDTO res = new ReserveCreateDTO();
                var data = db.Sqlable().From<R_Order>("s1");
                data = data.Where("s1.id='" + id + "'");
                res = data.SelectToList<ReserveCreateDTO>("s1.*").FirstOrDefault();
                return res;
            }
        }

        /// <summary>
        /// 菜品转台
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="orderTableId">转入哪个订单台号</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public bool ChangeProjectToTable(List<OrderDetailDTO> req, int orderTableId, OperatorModel userInfo, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string message = string.Empty;
                bool result = true;
                try
                {
                    var isLock = db.Queryable<R_OrderTable>().Where(p => p.IsLock == true || p.IsCheckOut==true).Any(p => p.Id == orderTableId || p.Id == req[0].R_OrderTable_Id);
                    if (isLock) { msg = "存在台号已锁定或结账！不能菜品转台！"; return false; }

                    db.BeginTran();
                    var orderTable = db.Queryable<R_OrderTable>().InSingle(orderTableId);   //新订单台号
                    if (orderTable != null)
                    {
                        var oldOrder = db.Queryable<R_Order>()
                            .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                            .Where<R_OrderTable>((s1, s2) => s2.Id == req[0].R_OrderTable_Id)
                            .Select("s1.OrderNo").FirstOrDefault();
                        var newOrder= db.Queryable<R_Order>()
                            .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                            .Where<R_OrderTable>((s1, s2) => s2.Id == orderTableId)
                            .Select("s1.OrderNo").FirstOrDefault();
                        List<string> recordRemark = new List<string>();
                        var oldOrderTable = db.Queryable<R_OrderTable>().InSingle(req[0].R_OrderTable_Id);  //旧订单台号
                        var oldTable = db.Queryable<R_Table>().InSingle(oldOrderTable.R_Table_Id);  //旧台号
                        var newTable = db.Queryable<R_Table>().InSingle(orderTable.R_Table_Id);     //新台号

                        List<R_OrderDetailRecord> records = new List<R_OrderDetailRecord>();
                        var orderDetailIds = req.Select(p => p.Id).ToArray();
                        var orderDetailsOld = db.Queryable<R_OrderDetail>()
                            .Where(p => orderDetailIds.Contains(p.Id)).ToList(); //明细纪录

                        foreach (var item in req)
                        {
                            recordRemark.Add(string.Format("菜品:{0} 数量:{1}",
                                string.IsNullOrEmpty(item.ExtendName) ?
                                item.ProjectName : item.ExtendName, item.Num));
                            var oldNum = orderDetailsOld.FirstOrDefault(p => p.Id == item.Id).Num;  //原纪录数量
                            var newNum = item.Num;  //转移数量
                            var detailRecordNum = db.Queryable<R_OrderDetailRecord>().Where(p => p.R_OrderDetail_Id == item.Id && p.IsCalculation == true
                            && (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出 || p.CyddMxCzType == CyddMxCzType.退菜)).
                                GroupBy(p => p.R_OrderDetail_Id).Select("sum(Num) as Num").FirstOrDefault();   //订单详情赠菜，转出，退菜总数
                            //判断数量是否成立
                            if ((oldNum - (detailRecordNum != null ? detailRecordNum.Num : 0)) < newNum || newNum <= 0)
                            { msg = "数量有误，请重新输入！"; return false; }
                            if (newNum == oldNum) //全部转移
                            {
                                db.Update<R_OrderDetail>(new { R_OrderTable_Id = orderTable.Id }, p => p.Id == item.Id);   //更新至新订单台号
                                //转出的订单台号， 添加 一条订单明细记录
                                records.Add(new R_OrderDetailRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = userInfo.UserId,
                                    CyddMxCzType = CyddMxCzType.转出,
                                    Num = newNum,
                                    R_OrderDetail_Id = item.Id,
                                    IsCalculation = false,
                                    Remark = string.Format("转菜至新订单号:{0},新台号:{1},菜品:{2}", newOrder.OrderNo, newTable.Name, item.ProjectName)
                                });
                                db.Update<R_OrderDetailRecord>(new { IsCalculation = false }, p => p.R_OrderDetail_Id == item.Id);//更新之前的转入 为不记录
                                //转入的订单台号 添加 一条订单明细记录
                                records.Add(new R_OrderDetailRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = userInfo.UserId,
                                    CyddMxCzType = CyddMxCzType.转入,
                                    Num = newNum,
                                    R_OrderDetail_Id = item.Id,
                                    IsCalculation = true,
                                    Remark = string.Format("收到转菜来自旧订单号:{0},旧台号:{1},菜品:{2}", oldOrder.OrderNo, oldTable.Name, item.ProjectName)
                                });
                            }
                            else //部分转移
                            {
                                decimal projectExtendPrice = 0;
                                var oldOrderDetail = orderDetailsOld.FirstOrDefault(p => p.Id == item.Id);  //原纪录
                                var projectExtend = db.Queryable<R_OrderDetailExtend>().Where(p => p.R_OrderDetail_Id == item.Id).ToList();
                                if (projectExtend != null && projectExtend.Any())
                                {
                                    projectExtendPrice = projectExtend.Sum(p => p.Price);//特殊要求价格计算
                                }
                                //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)
                                var originalTotalPrice = (oldNum - (detailRecordNum != null ? detailRecordNum.Num : 0) - newNum) * (oldOrderDetail.Price + projectExtendPrice);
                                oldOrderDetail.OriginalTotalPrice = originalTotalPrice;
                                db.Update<R_OrderDetail>(oldOrderDetail);//更新菜品原价总额
                                // 转入的订单台号 添加 一条订单明细
                                var newOldOrderDetail = oldOrderDetail;
                                newOldOrderDetail.R_OrderTable_Id = orderTable.Id;
                                newOldOrderDetail.Num = newNum;
                                newOldOrderDetail.OriginalTotalPrice = newNum * newOldOrderDetail.Price;
                                var newObj = db.Insert<R_OrderDetail>(newOldOrderDetail);
                                //转入的订单台号 添加 一条订单明细记录
                                records.Add(new R_OrderDetailRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = userInfo.UserId,
                                    CyddMxCzType = CyddMxCzType.转入,
                                    Num = newNum,
                                    R_OrderDetail_Id = Convert.ToInt32(newObj),
                                    IsCalculation = true,
                                    Remark = string.Format("收到转菜来自旧订单号:{0},旧台号:{1},菜品:{2}", oldOrder.OrderNo, oldTable.Name, item.ProjectName)
                                });
                                if (item.CyddMxType == CyddMxType.餐饮套餐)
                                {
                                    if (item.PackageDetailList != null && item.PackageDetailList.Any())
                                    {
                                        item.PackageDetailList.ForEach(p => p.R_OrderDetail_Id = Convert.ToInt32(newObj));
                                        db.InsertRange<R_OrderDetailPackageDetail>(item.PackageDetailList);
                                    }
                                }
                                //转出的订单台号， 添加 一条订单明细记录
                                records.Add(new R_OrderDetailRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = userInfo.UserId,
                                    CyddMxCzType = CyddMxCzType.转出,
                                    Num = newNum,
                                    R_OrderDetail_Id = item.Id,
                                    IsCalculation = true,
                                    Remark = string.Format("转菜至新订单号:{0},新台号:{1},菜品:{2}", newOrder.OrderNo, newTable.Name, item.ProjectName)
                                });
                            }
                        }
                        db.InsertRange<R_OrderDetailRecord>(records);

                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = userInfo.UserId,
                            CyddCzjlStatus = CyddStatus.订单菜品修改,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            R_Order_Id = oldOrderTable.R_Order_Id,
                            R_OrderTable_Id = oldOrderTable.Id,
                            Remark = string.Format("旧台号:{0} 转菜至新订单号:{1},新台号:{2},菜品详情:{3}",
                                oldTable.Name, newOrder.OrderNo, newTable.Name, string.Join(",", recordRemark))
                        });

                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = userInfo.UserId,
                            CyddCzjlStatus = CyddStatus.订单菜品修改,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            R_Order_Id = orderTable.R_Order_Id,
                            R_OrderTable_Id = orderTable.Id,
                            //Remark = string.Format("台号:{0} 转菜至订单ID:{1},台号:{2},菜品详情:{3}", oldTable.Name, orderTable.R_Order_Id, newTable.Name, string.Join(",", recordRemark))
                            Remark = string.Format("新台号:{0} 收到转菜来自旧订单:{1},旧台号:{2},菜品详情:{3}", newTable.Name, oldOrder.OrderNo, oldTable.Name, string.Join(",", recordRemark))
                        });
                    }
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    result = false;
                    db.RollbackTran();
                }

                msg = message;
                return result;
            }
        }

        /// <summary>
        /// 多桌点餐
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="tables">订单台号信息列表</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public bool ChoseProjectToTable(List<OrderDetailDTO> req, List<OrderTableDTO> tables, OperatorModel userInfo, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string message = string.Empty;
                string remark_Categorys = string.Empty;
                string remark_Tables = string.Empty;
                bool result = true;
                try
                {
                    var oldOrder = db.Queryable<R_Order>().JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                        .Where<R_OrderTable>((s1, s2) => s2.Id == req[0].R_OrderTable_Id)
                        .Select("s1.OrderNo").FirstOrDefault();
                    var orderTableIds = tables.Select(a => a.Id).ToList();
                    orderTableIds.Add(req[0].R_OrderTable_Id);
                    var isLock = db.Queryable<R_OrderTable>().Any(p => orderTableIds.Contains(p.Id) && p.IsLock == true);
                    if (isLock) { msg = "存在台号已锁定！不能多桌点餐！"; return false; }

                    db.BeginTran();
                    //先判断库存是否足够
                    int notContinue = 0;
                    foreach (var item in req)
                    {
                        #region 餐饮项目库存判断
                        if (item.CyddMxType == CyddMxType.餐饮项目)
                        {
                            var project = db.Queryable<R_Project>().Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                            var projectDetail = db.Queryable<R_ProjectDetail>().Where(p => p.Id == item.CyddMxId).FirstOrDefault();

                            if (project.IsStock)
                            {
                                var projectUseStock = projectDetail.UnitRate * item.Num * tables.Count();
                                if (project.Stock < projectUseStock)
                                {
                                    notContinue++;
                                    message += string.IsNullOrEmpty(message) ?
                                        item.ProjectName + "超出库存,剩余" + project.Stock + "" :
                                        "," + item.ProjectName + "超出库存,剩余" + project.Stock + "";
                                }
                                else
                                {
                                    db.Update<R_Project>(new
                                    {
                                        Stock = project.Stock - projectUseStock
                                    }, p => p.Id == item.R_Project_Id);
                                }
                            }
                        }
                        #endregion
                        #region 餐饮套餐库存判断
                        else if (item.CyddMxType == CyddMxType.餐饮套餐)
                        {
                            if (item.PackageDetailList != null && item.PackageDetailList.Any())
                            {
                                foreach (var pro in item.PackageDetailList)
                                {
                                    var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == pro.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                    var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                    // var packagedetail = packageDetails.First(p => p.R_ProjectDetail_Id == pro.Id);

                                    if (projects.IsStock)
                                    {
                                        var projectUseStock = projectDetails.UnitRate * item.Num * pro.Num * tables.Count();    //该餐饮项目共需多少库存
                                        if (projects.Stock < projectUseStock)
                                        {
                                            notContinue++;
                                            message += string.IsNullOrEmpty(message) ?
                                                item.ProjectName + "-" + projects.Name + "超出库存,剩余" + projects.Stock + "" :
                                                "," + item.ProjectName + item.ProjectName + "-" + projects.Name + "超出库存,剩余" + projects.Stock + "";
                                        }
                                        else
                                        {
                                            db.Update<R_Project>(new
                                            {
                                                Stock = projects.Stock - projectUseStock
                                            }, p => p.Id == projects.Id);
                                        }
                                    }
                                }
                            }

                        }
                        #endregion
                    }
                    if (notContinue <= 0)
                    {
                        bool round = true;//遍历一遍菜品就行了
                        List<R_OrderDetailRecord> records = new List<R_OrderDetailRecord>();
                        List<Cpdy> cydyInsert = new List<Cpdy>();
                        #region 所需打印机列表
                        int thid = orderTableIds[0]; //多桌点餐订单是同一个
                        var order = db.Queryable<R_Order>()
                            .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                            .Where<R_OrderTable>((s1, s2) => s2.Id == thid).Select("s1.*").FirstOrDefault();
                        var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == order.R_Market_Id);
                        var resturant = db.Queryable<R_Restaurant>().FirstOrDefault(p => p.Id == order.R_Restaurant_Id);

                        var detailIds = req.Where(p => p.CyddMxType == CyddMxType.餐饮项目)
                                   .Select(p => p.CyddMxId).ToArray();  //餐饮项目详情Ids
                        var detailList = db.Queryable<R_ProjectDetail>()
                            .Where(p => detailIds.Contains(p.Id)).ToList();   //餐饮项目详情列表
                        var projectIds = req.Where(p => p.CyddMxType == CyddMxType.餐饮项目).Select(p => p.R_Project_Id).ToList(); //餐饮项目Ids

                        //先全部取出ProjectDetail_Id
                        var projectDetailIds = new List<int>();
                        req.Where(p => p.CyddMxType == CyddMxType.餐饮套餐).ToList().ForEach(n =>
                        {
                            projectDetailIds = projectDetailIds.Concat(n.PackageDetailList.Select(p => p.R_ProjectDetail_Id).ToList()).ToList();    //套餐餐饮项目明细IDS
                        });
                        var packageProjects = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).Select(p => p.R_Project_Id).ToList();
                        projectIds = projectIds.Concat(packageProjects).ToList();    //连接餐饮项目IDS和套餐包含的餐饮项目IDS
                        projectIds = projectIds.GroupBy(p => p).Select(p => p.Key).ToList();
                        var printerList = db.Queryable<Printer>()
                            .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id && s2.IsDelete == false)
                            .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                            .Where<R_ProjectStall>((s1, s3) => projectIds.Contains(s3.R_Project_Id) && s1.IsDelete == false)
                            .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                            {
                                Id = s1.Id,
                                Code = s1.Code,
                                IpAddress = s1.IpAddress,
                                IsDelete = s1.IsDelete,
                                Name = s1.Name,
                                PcName = s1.PcName,
                                PrintPort = s1.PrintPort,
                                Remark = s1.Remark,
                                ProjectId = s3.R_Project_Id,
                                BillType = s3.BillType,
                                StallName = s2.Name,
                                StallId = s2.Id
                            }).ToList();    //餐饮项目打印机列表
                        #endregion

                        foreach (var item0 in tables)  //遍历选中的需要点餐的台号订单
                        {
                            cydyInsert = new List<Cpdy>();
                            string cpdyThGuid = string.Empty;   //出品打印按桌号出生成标识
                            cpdyThGuid = item0.Id + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            // remarkList = new List<string>();
                            
                            string dishStatus = string.Empty;   //即起|叫起
                            string detailRemark = string.Empty; //手写要求（备注）
                            List<R_OrderDetailExtend> mxkzList = new List<R_OrderDetailExtend>();
                            foreach (var item1 in req) //遍历点餐的菜品，将每个菜点餐记录到订单明细里
                            {
                                string yq = string.Empty;   //要求
                                string zf = string.Empty;   //做法
                                string pc = string.Empty;   //配菜
                                decimal projectExtendPrice = 0;
                                dishStatus = item1.DishesStatus.ToString();
                                detailRemark = string.IsNullOrEmpty(item1.Remark) ? "" : "," + item1.Remark;
                                if (round)
                                {                                  
                                    //记录多桌点餐菜品名称和数量
                                    remark_Categorys += string.IsNullOrEmpty(remark_Categorys) ?
                                        item1.CyddMxName + "*" + item1.Num :
                                        "," + item1.CyddMxName + "*" + item1.Num;
                                }
                               
                                #region 订单操作明细
                                var OrderDetailModel = new R_OrderDetail()
                                {
                                    R_OrderTable_Id = item0.Id,
                                    CyddMxType = item1.CyddMxType,
                                    CyddMxId = item1.CyddMxId,
                                    CostPrice = item1.CostPrice,
                                    Price = item1.Price,
                                    Num = item1.Num,
                                    //MakeUser = "0",
                                    CyddMxStatus = item1.CyddMxStatus,
                                    SortNum = item1.SortNum,
                                    RemindNum = item1.RemindNum,
                                    //Remark = "",
                                    DiscountRate = item1.DiscountRate,
                                    CreateDate = DateTime.Now,
                                    CreateUser = userInfo.UserId,
                                    HookNum = 0,
                                    //ExtendName = "",
                                    Unit = item1.Unit,
                                    CyddMxName = item1.CyddMxName,
                                    DishesStatus = item1.DishesStatus,  //添加叫起|即起属性
                                                                        // OriginalTotalPrice= item1.Num*(item1.Price+ projectExtendPrice)
                                };
                                var newObj = db.Insert<R_OrderDetail>(OrderDetailModel);
                                #endregion

                                #region 套餐详情
                                if (item1.CyddMxType == CyddMxType.餐饮套餐)
                                {
                                    if (item1.PackageDetailList != null && item1.PackageDetailList.Any())
                                    {
                                        item1.PackageDetailList.ForEach(p => p.R_OrderDetail_Id = Convert.ToInt32(newObj));
                                        db.InsertRange<R_OrderDetailPackageDetail>(item1.PackageDetailList);
                                    }
                                }
                                #endregion

                                #region 订单明细扩展添加
                                if (item1.Extend != null && item1.Extend.Any())//做法
                                {
                                    projectExtendPrice += item1.Extend.Sum(p => p.Price);
                                    zf = string.Join(",", item1.Extend.Where(p => p.ExtendType == CyxmKzType.做法).Select(p => p.ProjectExtendName).ToArray());
                                    foreach (var extend in item1.Extend)
                                    {
                                        mxkzList.Add(new R_OrderDetailExtend()
                                        {
                                            R_OrderDetail_Id = Convert.ToInt32(newObj),
                                            R_ProjectExtend_Id = extend.Id,
                                            Name = extend.ProjectExtendName,
                                            Price = extend.Price,
                                            Unit = extend.Unit
                                        });
                                    }

                                }
                                if (item1.ExtendRequire != null && item1.ExtendRequire.Any())//要求
                                {
                                    projectExtendPrice += item1.ExtendRequire.Sum(p => p.Price);
                                    yq = string.Join(",", item1.ExtendRequire.Where(p => p.ExtendType == CyxmKzType.要求).Select(p => p.ProjectExtendName).ToArray());
                                    foreach (var extend in item1.ExtendRequire)
                                    {
                                        mxkzList.Add(new R_OrderDetailExtend()
                                        {
                                            R_OrderDetail_Id = Convert.ToInt32(newObj),
                                            R_ProjectExtend_Id = extend.Id,
                                            Name = extend.ProjectExtendName,
                                            Price = extend.Price,
                                            Unit = extend.Unit
                                        });
                                    }
                                }
                                if (item1.ExtendExtra != null && item1.ExtendExtra.Any())//配菜
                                {
                                    projectExtendPrice += item1.ExtendExtra.Sum(p => p.Price);
                                    pc = string.Join(",", item1.ExtendExtra.Where(p => p.ExtendType == CyxmKzType.配菜).Select(p => p.ProjectExtendName).ToArray());
                                    foreach (var extend in item1.ExtendExtra)
                                    {
                                        mxkzList.Add(new R_OrderDetailExtend()
                                        {
                                            R_OrderDetail_Id = Convert.ToInt32(newObj),
                                            R_ProjectExtend_Id = extend.Id,
                                            Name = extend.ProjectExtendName,
                                            Price = extend.Price,
                                            Unit = extend.Unit
                                        });
                                    }
                                }
                                db.InsertRange(mxkzList);
                                #endregion

                                #region 更新菜品原价总额(OriginalTotalPrice)
                                //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)
                                var originalTotalPrice = item1.Num * (item1.Price + projectExtendPrice);
                                if (originalTotalPrice >= 0)
                                {
                                    db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice }, p => p.Id == Convert.ToInt32(newObj));
                                }
                                else
                                {
                                    message = "数量不合法！";
                                    result = false;
                                }
                                #endregion

                                #region 厨单出品单打印业务
                                /*打印餐饮项目出品单 start*/
                                if (item1.CyddMxType == CyddMxType.餐饮项目)
                                {
                                    var projectPrinters = printerList.Where(p => p.ProjectId == item1.R_Project_Id && p.BillType == 1);   //菜品关联的出品单打印机
                                    if (projectPrinters.Any())
                                    {
                                        projectPrinters.ToList().ForEach(c =>
                                        {
                                            cydyInsert.Add(new Cpdy
                                            {
                                                cymxxh00 = Convert.ToInt32(newObj),
                                                cyzdxh00 = order.Id,
                                                cymxdm00 = item1.CyddMxId.ToString(),
                                                cymxmc00 = string.IsNullOrEmpty(OrderDetailModel.ExtendName) ? OrderDetailModel.CyddMxName : OrderDetailModel.ExtendName,
                                                cymxdw00 = detailList.FirstOrDefault(p => p.Id == item1.CyddMxId).Unit,
                                                cymxsl00 = item1.Num.ToString(),
                                                cymxdybz = false,
                                                cymxyj00 = c.IpAddress,
                                                cymxclbz = "0",
                                                cymxczrq = DateTime.Now,
                                                cymxzdbz = "0",
                                                cymxyq00 = string.IsNullOrEmpty(yq) ? dishStatus + detailRemark : yq + "," + dishStatus + detailRemark,
                                                cymxzf00 = zf,
                                                cymxpc00 = pc,
                                                cymxczy0 = userInfo.UserName,
                                                cymxfwq0 = c.PcName,
                                                cymxczdm = userInfo.UserCode,
                                                cymxje00 = item1.Price.ToString(),
                                                cymxth00 = item0.Name,
                                                cymxrs00 = order.PersonNum.ToString(),
                                                cymxct00 = resturant.Name,
                                                cymxzdid = cpdyThGuid,
                                                cymxbt00 = "出品单",
                                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                cpdysdxh = cpdyThGuid,
                                                cymxdk00 = c.StallName,
                                                cymxgdbz = false,
                                                cpdyfsl0 = market.Name
                                            });
                                        });
                                    }
                                }
                                /*打印餐饮项目出品单 end*/
                                /*打印套餐出品单 start*/
                                else if (item1.CyddMxType == CyddMxType.餐饮套餐)
                                {
                                    if (item1.PackageDetailList != null && item1.PackageDetailList.Any())
                                    {
                                        item1.PackageDetailList.ForEach(n =>
                                        {
                                            var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == n.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                            var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                            var projectPrinters = printerList.Where(p => p.ProjectId == projects.Id && p.BillType == 1);   //菜品关联的出品单打印机

                                            if (projectPrinters.Any())
                                            {
                                                projectPrinters.ToList().ForEach(c =>
                                                {
                                                    cydyInsert.Add(new Cpdy
                                                    {
                                                        cymxxh00 = Convert.ToInt32(newObj),
                                                        cyzdxh00 = order.Id,
                                                        cymxdm00 = projectDetails.Id.ToString(),
                                                        cymxmc00 = projects.Name,
                                                        cymxdw00 = projectDetails.Unit,
                                                        cymxsl00 = (item1.Num * n.Num).ToString(),
                                                        cymxdybz = false,
                                                        cymxyj00 = c.IpAddress,
                                                        cymxclbz = "0",
                                                        cymxczrq = DateTime.Now,
                                                        cymxzdbz = "0",
                                                        cymxyq00 = dishStatus,
                                                        cymxzf00 = item1.CyddMxName,
                                                        cymxpc00 = string.Empty,
                                                        cymxczy0 = userInfo.UserName,
                                                        cymxfwq0 = c.PcName,
                                                        cymxczdm = userInfo.UserCode,
                                                        cymxje00 = projectDetails.Price.ToString(),
                                                        cymxth00 = item0.Name,
                                                        cymxrs00 = order.PersonNum.ToString(),
                                                        cymxct00 = resturant.Name,
                                                        cymxzdid = cpdyThGuid,
                                                        cymxbt00 = "出品单",
                                                        cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                        cpdysdxh = cpdyThGuid,
                                                        cymxdk00 = c.StallName,
                                                        cymxgdbz = false,
                                                        cpdyfsl0 = market.Name
                                                    });
                                                });
                                            }
                                        });
                                    }
                                }
                                /*打印套餐出品单 end*/
                                #endregion
                            }
                            #region 厨单总单打印
                            var groupTotalPrint = printerList.Where(p => p.BillType == 2).GroupBy(p => new { p.StallId, p.Id });
                            if (groupTotalPrint.Any())
                            {
                                groupTotalPrint.ToList().ForEach(c =>
                                {
                                    var printModel = printerList.FirstOrDefault(p => p.Id == c.Key.Id);
                                    cydyInsert.Add(new Cpdy
                                    {
                                        cymxxh00 = item0.Id,
                                        cyzdxh00 = order.Id,
                                        cymxdm00 = market.Name,
                                        cymxmc00 = "总单",
                                        cymxdw00 = userInfo.UserName,
                                        cymxsl00 = string.Empty,
                                        cymxdybz = false,
                                        cymxyj00 = printModel.IpAddress,
                                        cymxclbz = "0",
                                        cymxczrq = DateTime.Now,
                                        cymxzdbz = "1",
                                        //cymxyq00 = detail.CyddMxName,
                                        //cymxzf00 = detail.CyddMxName,
                                        //cymxpc00 = detail.CyddMxName,
                                        cymxczy0 = userInfo.UserName,
                                        cymxfwq0 = printModel.PcName,
                                        cymxczdm = userInfo.UserCode,
                                        cymxje00 = req.Sum(p => p.Price * p.Num).ToString(),
                                        cymxth00 = item0.Name,
                                        cymxrs00 = order.PersonNum.ToString(),
                                        cymxct00 = resturant.Name,
                                        cymxzdid = cpdyThGuid,
                                        cymxbt00 = "总单",
                                        cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                        cpdysdxh = cpdyThGuid,
                                        cymxdk00 = printModel.StallName,
                                        cymxgdbz = false,
                                        cpdyfsl0 = market.Name
                                    });
                                });
                            }
                            #endregion
                            db.InsertRange<Cpdy>(cydyInsert);   //批量添加菜品打印信息
                            round = false;
                            remark_Tables += string.IsNullOrEmpty(remark_Tables) ?
                                item0.TableId + "*" + item0.Name :
                                "," + item0.TableId + "*" + item0.Name;
                        }
                        #region 订单操作记录
                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = userInfo.UserId,
                            CyddCzjlStatus = CyddStatus.点餐,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            R_Order_Id = tables[0].OrderId,
                            R_OrderTable_Id = req[0].R_OrderTable_Id,
                            Remark = string.Format("多桌点餐:订单号:{0},台号:{1},菜品:{2}",
                                oldOrder.OrderNo, remark_Tables, remark_Categorys)
                        });
                        #endregion
                        db.InsertRange<R_OrderDetailRecord>(records);
                    }
                    else
                    {
                        result = false;
                    }

                    if (result)
                    {
                        db.CommitTran();
                    }
                    else
                    {
                        db.RollbackTran();
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    result = false;
                    db.RollbackTran();
                }

                msg = message;
                return result;
            }
        }

        /// <summary>
        /// 修改订单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateOrderInfo(ReserveCreateDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;

                R_Order model = this.GetR_OrderById(req.Id);
                //model.Id = req.Id;
                model.Remark = req.Remark;
                model.ContactPerson = req.ContactPerson;
                model.ContactTel = req.ContactTel;
                model.PersonNum = req.PersonNum;
                model.CyddOrderSource = req.CyddOrderSource;
                model.CustomerId = req.CustomerId;
                model.OrderType = req.OrderType;
                model.BillingUser = req.BillingUser;
                model.BillDepartMent = req.BillDepartMent;
                model.R_Market_Id = req.R_Market_Id;
                result = db.Update(model);

                #region 
                var orderTables = db.Queryable<R_OrderTable>().Where(p => p.R_Order_Id == req.Id).ToList();
                int orderTableCount = orderTables.Count();
                int personNumAvg = orderTableCount > 1 ?
req.PersonNum / orderTableCount : req.PersonNum;    //台号人均
                int personNumRemainder = orderTableCount > 1 ?
                    req.PersonNum % orderTableCount : 0;  //台号余人
                int eachRemainder = 0;
                if (orderTables != null && orderTables.Count > 0)
                {
                    foreach (var tId in orderTables)
                    {
                        eachRemainder++;
                        tId.PersonNum = personNumAvg + (personNumRemainder - eachRemainder >= 0 ? 1 : 0);
                        if (!tId.IsCheckOut)
                        {
                            tId.R_Market_Id = req.R_Market_Id;
                        }
                    }
                    db.UpdateRange<R_OrderTable>(orderTables);//更新订单台号人数
                    #endregion
                }
                    return result;
            }
        }

        public R_Order GetR_OrderById(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var data = db.Sqlable().From<R_Order>("s1")
                    .Where("s1.id='" + id + "'")
                    .SelectToList<R_Order>("s1.*").FirstOrDefault();
                return data;
            }
        }

        /// <summary>
        /// 获取订单基本信息
        /// </summary>
        /// <param name="orderId"></param>
        public ReserveCreateDTO GetOrderDTO(int orderId)
        {
            ReserveCreateDTO orderDto = new ReserveCreateDTO();
            using (var db = new SqlSugarClient(Connection))
            {
                string sql = $@" SELECT O.*, ISNULL(R.R_Company_Id, 0) AS CompanyId FROM dbo.R_Order O " +
                    " LEFT JOIN dbo.R_Restaurant R ON R.Id = O.R_Restaurant_Id" +
                    " WHERE O.Id = @orderId";

                orderDto = db.SqlQuery<ReserveCreateDTO>(sql, new { orderId = orderId }).FirstOrDefault();

                return orderDto;

            }
        }

        /// <summary>
        /// 更新订单全单折折扣率
        /// </summary>
        /// <param name="orderId"></param>
        public void Update(ReserveCreateDTO orderDto)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var orderModel = Mapper.Map<ReserveCreateDTO, R_Order>(orderDto);
                db.Update<R_Order>(orderModel);
            }
        }

        /// <summary>
        /// 根据订单台号id获取订单下所有台号列表
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public List<OrderTableDTO> GetTablesByOrderTableId(int orderTableId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var data = db.Sqlable()
                    .From<R_OrderTable>("s1")
                    .Join<R_Table>("s2", "s1.R_Table_Id", "s2.Id", JoinType.Left)
                    .Where("s1.R_Order_Id=(select R_Order_Id from R_OrderTable where Id=" + orderTableId + ")")
                    .SelectToList<OrderTableDTO>(@"s1.Id,s1.R_Table_Id as TableId,
                        s1.R_Order_Id as OrderId,s1.CreateDate,s1.PersonNum,s1.IsCheckOut,s1.IsOpen,s2.Name,s1.IsLock") ?? new List<OrderTableDTO>();
                return data;
            }
        }

        /// <summary>
        /// 根据订单台号ID集合获取台号列表
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public List<OrderTableDTO> GetTablesByOrderTableIds(List<int> orderTableId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var data = db.Queryable<R_Table>().JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                    .Where<R_OrderTable>((s1, s2) => orderTableId.Contains(s2.Id))
                    .Select<R_OrderTable, OrderTableDTO>((s1, s2) => new OrderTableDTO
                    {
                        Name=s1.Name
                    }).ToList();
                //var data = db.Sqlable()
                //    .From<R_OrderTable>("s1")
                //    .Join<R_Table>("s2", "s1.R_Table_Id", "s2.Id", JoinType.Left)
                //    .Where("s1.R_Order_Id=(select R_Order_Id from R_OrderTable where Id=" + orderTableId + ")")
                //    .SelectToList<OrderTableDTO>(@"s1.Id,s1.R_Table_Id as TableId,
                //        s1.R_Order_Id as OrderId,s1.CreateDate,s1.PersonNum,s1.IsCheckOut,s1.IsOpen,s2.Name,s1.IsLock") ?? new List<OrderTableDTO>();
                return data;
            }
        }

        /// <summary>
        /// 根据订单台号id获取订单信息和台号信息
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        public OrderAndTablesDTO GetOrderAndTablesByOrderTableId(int orderTableId)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string sql = "select a.R_Table_Id,c.Name as TableName,f.Name as Restaurant, b.Id AS OrderId, b.OrderNo,a.PersonNum,b.R_Restaurant_Id, d.czdmmc00 AS UserName,e.Name as OrderSourceType,b.CreateDate,a.IsLock,b.CyddStatus,a.IsControl,b.R_Market_Id as MarketId from R_OrderTable  a " +
                    " left join R_Order b on a.R_Order_Id = b.Id " +
                    " left join R_Table c on c.Id = a.R_Table_Id " +
                    " left join czdm d on d.Id=b.CreateUser " +
                    " left join ExtendItems e on e.Id = b.CyddOrderSource " +
                    " left join R_Restaurant f on f.Id = b.R_Restaurant_Id " +
                    " where a.Id = " + orderTableId;
                var data = db.SqlQuery<OrderAndTablesDTO>(sql).FirstOrDefault();
                return data;
            }
        }

        /// <summary>
        /// 餐饮预定信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ForecastReserveInfoDTO> GetForecastList(ForecastSearchDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                var paras = SqlSugarTool.GetParameters(new
                {
                    RestaurantId = req.Restaurant,
                    BeginDate = req.BeginDate,
                    EndDate = req.EndDate.AddDays(1)
                });

                var list = db.SqlQuery<ForecastReserveInfoDTO>($@" SELECT  O.R_Restaurant_Id AS RestaurantId, O.Id AS OrderId ,  
                        T.Id AS TableId, T.Name AS TableName, M.Id AS MarketId, M.Name AS MarketName,
                        O.ReserveDate,
                        O.ContactPerson, O.PersonNum, ISNULL(Pay.PayAmount, 0) BookingAmount
                    FROM dbo.R_OrderTable OT 
                    LEFT JOIN dbo.R_Order O ON OT.R_Order_Id = O.Id 
                    LEFT JOIN dbo.R_Table T ON OT.R_Table_Id = T.Id 
                    LEFT JOIN dbo.R_Market M ON O.R_Market_Id = M.Id 
                    LEFT JOIN (SELECT R_Order_Id AS OrderId, SUM(PayAmount) as PayAmount 
								FROM R_OrderPayRecord  
								WHERE CyddJzStatus = 2 AND CyddJzType = 1 GROUP BY R_Order_Id
								) Pay ON O.Id = Pay.OrderId
                    WHERE O.CyddStatus = 1 AND O.IsDelete=0 AND O.R_Restaurant_Id = @RestaurantId
                        AND O.ReserveDate BETWEEN @BeginDate AND @EndDate", paras);
                return list;
            }

        }


        /// <summary>
        /// 退菜操作
        /// </summary>
        /// <param name="req">订单明细信息</param>
        /// <param name="table">订单台号信息</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public OrderDetailRecordDTO ReturnOrderDetail(OrderDetailDTO req, TableListDTO table, OperatorModel userInfo, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                OrderDetailRecordDTO result = new OrderDetailRecordDTO();
                bool isReturnPackageDetail = false; //是否退套餐菜品详情 
                bool isReturnAllPackage = false;    //是否退完套餐菜品详情
                try
                {
                    if (req.CyddMxType == CyddMxType.餐饮套餐)
                    {
                        var packageDetailCount = db.Queryable<R_OrderDetailPackageDetail>()
                            .Where(p => p.R_OrderDetail_Id == req.Id).Count();
                        var reqPackageDetailCount = req.PackageDetailList != null ? req.PackageDetailList.Count() : 0;
                        if (reqPackageDetailCount>0)
                        {
                            isReturnPackageDetail = true;
                        }
                        if (packageDetailCount == reqPackageDetailCount)
                        {
                            isReturnAllPackage = true;
                        }
                    }
                    /*判断是否锁台  --start*/
                    var isLock = db.Queryable<R_OrderTable>().Single(p => p.Id == req.R_OrderTable_Id).IsLock;
                    if (isLock) { msg = "台号已锁定！不能退菜！"; return null; }
                    decimal originalTotalPrice = 0;
                    /*判断是否锁台  --end*/

                    #region 是否能操作
                    var isCheckout = db.Queryable<R_OrderTable>().Single(p => p.Id == req.R_OrderTable_Id).IsCheckOut;
                    if (isCheckout)
                    {
                        msg = "该台号已经结账！不能退菜";
                        return null;
                    }
                    #endregion
                    #region 条件判断
                    if (!isReturnPackageDetail) //如果不是退套餐菜品详情则无需判断订单详情数量
                    {
                        var detail = db.Queryable<R_OrderDetail>().First(p => p.Id == req.Id);  //订单详情总数
                        var detailRecordNum = db.Queryable<R_OrderDetailRecord>()
                            .Where(p => p.R_OrderDetail_Id == req.Id && p.IsCalculation == true &&
                            (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出
                            || p.CyddMxCzType == CyddMxCzType.退菜)).GroupBy(p => p.R_OrderDetail_Id)
                            .Select("sum(Num) as Num").FirstOrDefault();   //订单详情赠菜，转出，退菜总数
                        if ((detail.Num - (detailRecordNum != null ? detailRecordNum.Num : 0)) < req.Num)
                        {
                            msg = "退菜数量不能大于当前已点数量，如果你有新增数量，请先保存！";
                            return null;
                        }
                        decimal projectExtendPrice = 0;
                        var projectExtend = db.Queryable<R_OrderDetailExtend>().Where(p => p.R_OrderDetail_Id == req.Id).ToList();
                        if (projectExtend != null && projectExtend.Any())
                        {
                            projectExtendPrice = projectExtend.Sum(p => p.Price);//特殊要求价格计算
                        }
                        originalTotalPrice = (detail.Num - (detailRecordNum != null ? detailRecordNum.Num : 0) - req.Num) * (detail.Price + projectExtendPrice);
                        if (originalTotalPrice < 0)
                        {
                            msg = "数量不合法！";
                            return null;
                        }
                    }
                    #endregion

                    db.BeginTran();
                    List<Cpdy> cydyInsert = new List<Cpdy>();
                    var order = db.Queryable<R_Order>()
                    .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                    .Where<R_OrderTable>((s1, s2) => s2.Id == req.R_OrderTable_Id)
                    .Select("s1.*").FirstOrDefault();
                    var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == order.R_Market_Id);
                    string cpdyThGuid = req.R_OrderTable_Id + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    if (!isReturnPackageDetail) //如果不是退套餐菜品详情则无需添加退菜纪录
                    {
                        DateTime timeNow = DateTime.Now;
                        #region 添加退菜纪录
                        var insertId= db.Insert<R_OrderDetailRecord>(new R_OrderDetailRecord()
                        {
                            R_OrderDetail_Id = req.Id,
                            CyddMxCzType = CyddMxCzType.退菜,
                            Num = req.Num,
                            CreateDate = timeNow,
                            CreateUser = userInfo.UserId,
                            IsCalculation = true,
                            Remark = req.Remark +";"+ CyddMxCzType.退菜.ToString() + ":" + req.CyddMxName + "*" + req.Num + ",订单号：" + order.OrderNo + ",台号：" + string.Join(",", table.Name),
                            DetailCauseRemark = req.Remark,
                            R_OrderDetailCause_Id = req.R_OrderDetailCause_Id

                        });
                        result.Id = Convert.ToInt32(insertId);
                        result.CreateDate = timeNow;
                        result.R_OrderDetail_Id = req.Id;

                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateUser = userInfo.UserId,
                            CreateDate=DateTime.Now,
                            CyddCzjlUserType=CyddCzjlUserType.员工,
                            CyddCzjlStatus=CyddStatus.订单菜品修改,
                            Remark= req.Remark +";"+ CyddMxCzType.退菜.ToString() + ":" + req.CyddMxName + "*" + req.Num + ",订单号：" + order.OrderNo + ",台号：" + string.Join(",", table.Name),
                            R_Order_Id=order.Id,
                            R_OrderTable_Id=req.R_OrderTable_Id,
                        });
                        #endregion

                        //更新菜品原价总额(OriginalTotalPrice)
                        db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice }, p => p.Id == req.Id);
                    }
                    else    //如果是退套餐菜品详情则需删除明细
                    {
                        var packageDetailIds = req.PackageDetailList.Select(p => p.Id).ToArray();
                        db.Delete<R_OrderDetailPackageDetail>(p => packageDetailIds.Contains(p.Id));
                        if (isReturnAllPackage)
                        {
                            #region 添加退菜纪录
                            db.Insert<R_OrderDetailRecord>(new R_OrderDetailRecord()
                            {
                                R_OrderDetail_Id = req.Id,
                                CyddMxCzType = CyddMxCzType.退菜,
                                Num = req.Num,
                                CreateDate = DateTime.Now,
                                CreateUser = userInfo.UserId,
                                IsCalculation = true,
                                Remark = CyddMxCzType.退菜.ToString() + ":" + req.CyddMxName + "*" + req.Num + ",订单号：" + order.OrderNo + ",台号：" + string.Join(",", table.Name)
                            });

                            db.Insert<R_OrderRecord>(new R_OrderRecord()
                            {
                                CreateUser = userInfo.UserId,
                                CreateDate = DateTime.Now,
                                CyddCzjlUserType = CyddCzjlUserType.员工,
                                CyddCzjlStatus = CyddStatus.订单菜品修改,
                                Remark = CyddMxCzType.退菜.ToString() + ":" + req.CyddMxName + "*" + req.Num + ",订单号：" + order.OrderNo + ",台号：" + string.Join(",", table.Name),
                                R_Order_Id = order.Id,
                                R_OrderTable_Id = req.R_OrderTable_Id
                            });
                            #endregion

                            //更新菜品原价总额(OriginalTotalPrice)
                            db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice }, p => p.Id == req.Id);
                        }
                    }


                    #region 获取打印机列表
                    List<int> projectIds = new List<int>();
                    if (req.CyddMxType == CyddMxType.餐饮项目)
                    {
                        projectIds.Add(req.R_Project_Id);
                    }
                    else if (req.CyddMxType == CyddMxType.餐饮套餐)
                    {
                        List<int> projectDetailIds = new List<int>();
                        if (isReturnPackageDetail)
                        {
                            projectDetailIds = req.PackageDetailList.Select(p => p.R_ProjectDetail_Id).ToList();
                        }
                        else
                        {
                            projectDetailIds = db.Queryable<R_OrderDetailPackageDetail>()
                            .Where(p => p.R_OrderDetail_Id == req.Id).Select(p => p.R_ProjectDetail_Id).ToList();
                        }
                        projectIds = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).Select(p => p.R_Project_Id).ToList();
                    }
                    var printerList = db.Queryable<Printer>()
                        .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id)
                        .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                        .Where<R_Stall,R_ProjectStall>((s1,s2, s3) => projectIds.Contains(s3.R_Project_Id) && s1.IsDelete==false && s2.IsDelete==false)
                        .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                        {
                            Id = s1.Id,
                            Code = s1.Code,
                            IpAddress = s1.IpAddress,
                            IsDelete = s1.IsDelete,
                            Name = s1.Name,
                            PcName = s1.PcName,
                            PrintPort = s1.PrintPort,
                            Remark = s1.Remark,
                            ProjectId = s3.R_Project_Id,
                            BillType = s3.BillType,
                            StallName = s2.Name,
                            StallId = s2.Id
                        }).ToList();    //餐饮项目打印机列表
                    #endregion

                    #region 退菜明细打印
                    if (req.CyddMxType == CyddMxType.餐饮项目)
                    {
                        var projectPrinters = printerList.Where(p => p.BillType == 1 && req.R_Project_Id == p.ProjectId);
                        projectPrinters.ToList().ForEach(c =>
                        {
                            cydyInsert.Add(new Cpdy
                            {
                                cymxxh00 = Convert.ToInt32(req.Id),
                                cyzdxh00 = order.Id,
                                cymxdm00 = req.CyddMxId.ToString(),
                                cymxmc00 = string.IsNullOrEmpty(req.ExtendName) ? req.CyddMxName : req.ExtendName,
                                cymxdw00 = req.Unit,
                                cymxsl00 = "-" + req.Num.ToString(),
                                cymxdybz = false,
                                cymxyj00 = c.IpAddress,
                                cymxclbz = "0",
                                cymxczrq = DateTime.Now,
                                cymxzdbz = "0",
                                //cymxyq00 = yq,
                                //cymxzf00 = zf,
                                //cymxpc00 = pc,
                                cymxczy0 = userInfo.UserName,
                                cymxfwq0 = c.PcName,
                                cymxczdm = userInfo.UserCode,
                                cymxje00 = req.Price.ToString(),
                                cymxth00 = table.Name,
                                cymxrs00 = order.PersonNum.ToString(),
                                cymxct00 = table.Restaurant,
                                cymxzdid = cpdyThGuid,
                                cymxbt00 = "退菜明细单",
                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                cpdysdxh = cpdyThGuid,
                                cymxdk00 = c.StallName,
                                cymxgdbz = false,
                                cpdyfsl0 = market.Name
                            });
                        });
                    }
                    else if (req.CyddMxType == CyddMxType.餐饮套餐)
                    {
                        if (isReturnPackageDetail)  //退详情
                        {
                            req.PackageDetailList.ForEach(n =>
                            {
                                var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == n.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                var projectPrinters = printerList.Where(p => p.ProjectId == projects.Id && p.BillType == 1);   //菜品关联的出品单打印机
                                if (projectPrinters.Any())
                                {
                                    projectPrinters.ToList().ForEach(c =>
                                    {
                                        cydyInsert.Add(new Cpdy
                                        {
                                            cymxxh00 = Convert.ToInt32(req.Id),
                                            cyzdxh00 = order.Id,
                                            cymxdm00 = projectDetails.Id.ToString(),
                                            cymxmc00 = n.Name,
                                            cymxdw00 = req.Unit,
                                            cymxsl00 = "-" + (req.Num * n.Num).ToString(),
                                            cymxdybz = false,
                                            cymxyj00 = c.IpAddress,
                                            cymxclbz = "0",
                                            cymxczrq = DateTime.Now,
                                            cymxzdbz = "0",
                                            cymxyq00 = req.CyddMxName,
                                            cymxzf00 = req.CyddMxName,
                                            cymxpc00 = req.CyddMxName,
                                            cymxczy0 = userInfo.UserName,
                                            cymxfwq0 = c.PcName,
                                            cymxczdm = userInfo.UserCode,
                                            cymxje00 = projectDetails.Price.ToString(),
                                            cymxth00 = table.Name,
                                            cymxrs00 = order.PersonNum.ToString(),
                                            cymxct00 = table.Restaurant,
                                            cymxzdid = cpdyThGuid,
                                            cymxbt00 = "退菜明细单",
                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                            cpdysdxh = cpdyThGuid,
                                            cymxdk00 = c.StallName,
                                            cymxgdbz = false,
                                            cpdyfsl0 = market.Name
                                        });
                                    });
                                }
                            });
                        }
                        else
                        {
                            var proDetails = db.Queryable<R_ProjectDetail>()
                                .JoinTable<R_OrderDetailPackageDetail>((s1, s2) => s1.Id == s2.R_ProjectDetail_Id)
                                .Where<R_OrderDetailPackageDetail>((s1, s2) => s2.R_OrderDetail_Id == req.Id)
                                .Select<R_OrderDetailPackageDetail, OrderReturnPackageDetailDTO>((s1, s2) => new OrderReturnPackageDetailDTO
                                {
                                    Id=s1.Id,
                                    R_Project_Id=s1.R_Project_Id,
                                    PackageDetailId=s2.Id,
                                    PackageDetailName=s2.Name,
                                    PackageDetailNum=s2.Num,
                                    Price=s1.Price
                                }).ToList();
                            var orderDetalPackageDetails = db.Queryable<R_OrderDetailPackageDetail>()
                                .Where(p => p.R_OrderDetail_Id == req.Id).ToList();
                            if (proDetails!=null && proDetails.Any())
                            {
                                foreach (var prodetail in proDetails)
                                {
                                    var project = db.Queryable<R_Project>().Where(p => p.Id == prodetail.R_Project_Id).FirstOrDefault();
                                    var projectPrinters = printerList.Where(p => p.ProjectId == prodetail.R_Project_Id && p.BillType == 1);   //菜品关联的出品单打印机
                                    if (projectPrinters.Any())
                                    {
                                        projectPrinters.ToList().ForEach(c =>
                                        {
                                            cydyInsert.Add(new Cpdy
                                            {
                                                cymxxh00 = Convert.ToInt32(req.Id),
                                                cyzdxh00 = order.Id,
                                                cymxdm00 = prodetail.Id.ToString(),
                                                cymxmc00 = prodetail.PackageDetailName,
                                                cymxdw00 = req.Unit,
                                                cymxsl00 = "-" + (req.Num * prodetail.PackageDetailNum).ToString(),
                                                cymxdybz = false,
                                                cymxyj00 = c.IpAddress,
                                                cymxclbz = "0",
                                                cymxczrq = DateTime.Now,
                                                cymxzdbz = "0",
                                                cymxyq00 = req.CyddMxName,
                                                cymxzf00 = req.CyddMxName,
                                                cymxpc00 = req.CyddMxName,
                                                cymxczy0 = userInfo.UserName,
                                                cymxfwq0 = c.PcName,
                                                cymxczdm = userInfo.UserCode,
                                                cymxje00 = prodetail.Price.ToString(),
                                                cymxth00 = table.Name,
                                                cymxrs00 = order.PersonNum.ToString(),
                                                cymxct00 = table.Restaurant,
                                                cymxzdid = cpdyThGuid,
                                                cymxbt00 = "退菜明细单",
                                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                cpdysdxh = cpdyThGuid,
                                                cymxdk00 = c.StallName,
                                                cymxgdbz = false,
                                                cpdyfsl0 = market.Name
                                            });
                                        });
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region 退菜总单打印
                    var groupTotalPrint = printerList.Where(p => p.BillType == 2).GroupBy(p => new { p.StallId, p.Id });
                    if (groupTotalPrint.Any())
                    {
                        groupTotalPrint.ToList().ForEach(c =>
                        {
                            var printModel = printerList.FirstOrDefault(p => p.Id == c.Key.Id);
                            cydyInsert.Add(new Cpdy
                            {
                                cymxxh00 = req.R_OrderTable_Id,
                                cyzdxh00 = order.Id,
                                cymxdm00 = "0",
                                cymxmc00 = "退菜总单",
                                cymxdw00 = userInfo.UserName,
                                cymxsl00 = string.Empty,
                                cymxdybz = false,
                                cymxyj00 = printModel.IpAddress,
                                cymxclbz = "0",
                                cymxczrq = DateTime.Now,
                                cymxzdbz = "1",
                                //cymxyq00 = detail.CyddMxName,
                                //cymxzf00 = detail.CyddMxName,
                                //cymxpc00 = detail.CyddMxName,
                                cymxczy0 = userInfo.UserName,
                                cymxfwq0 = printModel.PcName,
                                cymxczdm = userInfo.UserCode,
                                cymxje00 = (req.Price * req.Num).ToString(),
                                cymxth00 = table.Name,
                                cymxrs00 = order.PersonNum.ToString(),
                                cymxct00 = table.Restaurant,
                                cymxzdid = cpdyThGuid,
                                cymxbt00 = "退菜总单",
                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                cpdysdxh = cpdyThGuid,
                                cymxdk00 = printModel.StallName,
                                cymxgdbz = false,
                                cpdyfsl0 = market.Name
                            });
                        });
                    }
                    #endregion
                    db.InsertRange<Cpdy>(cydyInsert);   //批量添加菜品打印信息
                    msg = "提交成功";
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    db.RollbackTran();
                }
                return result;
            }
        }

        /// <summary>
        /// 打厨单
        /// </summary>
        /// <param name="req">保存状态的订单明细列表</param>
        /// <param name="table">订单台号信息</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public bool CookingMenu(List<OrderDetailDTO> req, TableListDTO table, OperatorModel userInfo, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                //1.只能是保存的菜品 2.要判断库存 3.库存足够就出品打印 4.更新订单详情状态
                int errorCount = 0;
                bool result = true;
                msg = "";
                try
                {
                    #region 条件判断和筛选
                    var isLock = db.Queryable<R_OrderTable>().Single(p => p.Id == req[0].R_OrderTable_Id).IsLock;
                    if (isLock) { errorCount = 20; msg = "台号已锁定！不能打厨！"; return false; }
                    /* 
                    var reqIds = req.Select(p => p.Id).ToList();
                    //查询是否还存在保存状态并未打厨的列表
                    var isReq = db.Queryable<R_OrderDetail>().Where(p => p.CyddMxStatus == CyddMxStatus.保存 && reqIds.Contains(p.Id)).ToList();
                    //如果不存在了，则返回提示信息
                    if (isReq == null || !isReq.Any()) { errorCount = 10; msg = "你所选的菜品都已打厨！请勿重复操作。"; return false; }
                    //否则就将保存状态并未打厨的菜品 进行库存判断
                    var isReqIds = isReq.Select(p => p.Id).ToList();
                    var newReq = req.Where(p => isReqIds.Contains(p.Id)).ToList();//保存状态并未打厨的菜品
                    */
                    #endregion
                    var newReq = req;
                    #region 开启事务 业务功能操作
                    db.BeginTran();
                    //判断库存
                    /*
                    foreach (var item in newReq)
                    {
                        #region 餐饮项目库存判断
                        if (item.CyddMxType == CyddMxType.餐饮项目)
                        {
                            var project = db.Queryable<R_Project>().Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                            var projectDetail = db.Queryable<R_ProjectDetail>().Where(p => p.Id == item.CyddMxId).FirstOrDefault();
                            if (project.IsStock)
                            {
                                var projectUseStock = projectDetail.UnitRate * item.Num;
                                if (project.Stock < projectUseStock)
                                {
                                    errorCount++;
                                    msg += string.IsNullOrEmpty(msg) ? item.ProjectName + "超出库存,剩余" + project.Stock + "" :
                                        "," + item.ProjectName + "超出库存,剩余" + project.Stock + "";
                                }
                                else
                                {
                                    db.Update<R_Project>(new { Stock = project.Stock - projectUseStock }, p => p.Id == item.R_Project_Id);
                                }
                            }
                        }
                        #endregion
                        #region 餐饮套餐库存判断
                        else if (item.CyddMxType == CyddMxType.餐饮套餐)
                        {
                            if (item.PackageDetailList != null && item.PackageDetailList.Any())
                            {
                                foreach (var pro in item.PackageDetailList)
                                {
                                    var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == pro.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                    var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                    // var packagedetail = packageDetails.First(p => p.R_ProjectDetail_Id == pro.Id);

                                    if (projects.IsStock)
                                    {
                                        var projectUseStock = projectDetails.UnitRate * item.Num * pro.Num;    //该餐饮项目共需多少库存
                                        if (projects.Stock < projectUseStock)
                                        {
                                            errorCount++;
                                            msg += string.IsNullOrEmpty(msg) ?
                                                item.ProjectName + "-" + projects.Name + "超出库存,剩余" + projects.Stock + "" :
                                                "," + item.ProjectName + item.ProjectName + "-" + projects.Name + "超出库存,剩余" + projects.Stock + "";
                                        }
                                        else
                                        {
                                            db.Update<R_Project>(new
                                            {
                                                Stock = projects.Stock - projectUseStock
                                            }, p => p.Id == projects.Id);
                                        }
                                    }
                                }
                            }

                        }
                        #endregion

                    }
                    */
                    //库存足够 
                    if (errorCount <= 0)
                    {
                        //出品打印
                        //var userInfo = OperatorProvider.Provider.GetCurrent();
                        List<Cpdy> cydyInsert = new List<Cpdy>();
                        var order = db.Queryable<R_Order>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                        .Where<R_OrderTable>((s1, s2) => s2.Id == newReq[0].R_OrderTable_Id)
                        .Select("s1.*").FirstOrDefault();
                        var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == order.R_Market_Id);
                        string cpdyThGuid = newReq[0].R_OrderTable_Id + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        List<string> remarkList = new List<string>();
                        #region 获取打印机列表
                        var projectIds = newReq.Where(p => p.CyddMxType == CyddMxType.餐饮项目).Select(p => p.R_Project_Id).ToList(); //餐饮项目Ids
                        //先全部取出ProjectDetail_Id
                        List<int> projectDetailIds = new List<int>();
                        newReq.Where(p => p.CyddMxType == CyddMxType.餐饮套餐).ToList().ForEach(n =>
                        {
                            projectDetailIds = projectDetailIds.Concat(n.PackageDetailList.Select(p => p.R_ProjectDetail_Id).ToList()).ToList();    //套餐餐饮项目明细IDS
                        });
                        var packageProjects = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).Select(p => p.R_Project_Id).ToList();
                        projectIds = projectIds.Concat(packageProjects).ToList();    //连接餐饮项目IDS和套餐包含的餐饮项目IDS
                        projectIds = projectIds.GroupBy(p => p).Select(p => p.Key).ToList();
                        var printerList = db.Queryable<Printer>()
                            .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id && s2.IsDelete == false)
                            .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                            .Where<R_Stall, R_ProjectStall>((s1, s2, s3) => projectIds.Contains(s3.R_Project_Id) && s1.IsDelete == false )
                            .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                            {
                                Id = s1.Id,
                                Code = s1.Code,
                                IpAddress = s1.IpAddress,
                                IsDelete = s1.IsDelete,
                                Name = s1.Name,
                                PcName = s1.PcName,
                                PrintPort = s1.PrintPort,
                                Remark = s1.Remark,
                                ProjectId = s3.R_Project_Id,
                                BillType = s3.BillType,
                                StallName = s2.Name,
                                StallId = s2.Id
                            }).ToList();    //餐饮项目打印机列表
                        #endregion

                        foreach (var detail in newReq)
                        {
                            var isRepeat = detail.CyddMxStatus == CyddMxStatus.已出 ? true : false;
                            db.Update<R_OrderDetail>(new { CyddMxStatus = CyddMxStatus.已出 }, p => p.Id == detail.Id);  //更新状态
                            string yq = string.Empty;   //要求
                            string zf = string.Empty;   //做法
                            string pc = string.Empty;   //配菜
                            remarkList.Add(string.Format("{0}{1} * {2}", detail.CyddMxName,
    string.IsNullOrEmpty(detail.Unit) ? "" : "(" + detail.Unit + ")", detail.Num));

                            #region 厨单出品单打印业务
                            /*打印餐饮项目出品单 start*/
                            if (detail.CyddMxType == CyddMxType.餐饮项目)
                            {
                                if (detail.Extend != null && detail.Extend.Any())
                                {
                                    zf = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.做法).Select(p => p.ProjectExtendName).ToArray());
                                }
                                if (detail.ExtendExtra != null && detail.ExtendExtra.Any())
                                {
                                    pc = string.Join(",", detail.ExtendExtra.Where(p => p.ExtendType == CyxmKzType.配菜).Select(p => p.ProjectExtendName).ToArray());
                                }
                                if (detail.ExtendRequire != null && detail.ExtendRequire.Any())
                                {
                                    yq = string.Join(",", detail.ExtendRequire.Where(p => p.ExtendType == CyxmKzType.要求).Select(p => p.ProjectExtendName).ToArray());
                                }

                                var projectPrinters = printerList.Where(p => p.ProjectId == detail.R_Project_Id && p.BillType == 1);   //菜品关联的出品单打印机
                                if (projectPrinters.Any())
                                {
                                    projectPrinters.ToList().ForEach(c =>
                                    {
                                        cydyInsert.Add(new Cpdy
                                        {
                                            cymxxh00 = Convert.ToInt32(detail.Id),
                                            cyzdxh00 = order.Id,
                                            cymxdm00 = detail.CyddMxId.ToString(),
                                            cymxmc00 = string.IsNullOrEmpty(detail.ExtendName) ? detail.CyddMxName : detail.ExtendName,
                                            cymxdw00 = detail.Unit,
                                            cymxsl00 = detail.Num.ToString(),
                                            cymxdybz = false,
                                            cymxyj00 = c.IpAddress,
                                            cymxclbz = "0",
                                            cymxczrq = DateTime.Now,
                                            cymxzdbz = "0",
                                            cymxyq00 = yq,
                                            cymxzf00 = zf,
                                            cymxpc00 = pc,
                                            cymxczy0 = userInfo.UserName,
                                            cymxfwq0 = c.PcName,
                                            cymxczdm = userInfo.UserCode,
                                            cymxje00 = detail.Price.ToString(),
                                            cymxth00 = table.Name,
                                            cymxrs00 = order.PersonNum.ToString(),
                                            cymxct00 = table.Restaurant,
                                            cymxzdid = cpdyThGuid,
                                            cymxbt00 = isRepeat ? "[重打]出品单" : "出品单",
                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                            cpdysdxh = cpdyThGuid,
                                            cymxdk00 = c.StallName,
                                            cymxgdbz = false,
                                            cpdyfsl0 = market.Name
                                        });
                                    });
                                }
                            }
                            /*打印餐饮项目出品单 end*/
                            /*打印套餐出品单 start*/
                            else if (detail.CyddMxType == CyddMxType.餐饮套餐)
                            {
                                if (detail.PackageDetailList != null && detail.PackageDetailList.Any())
                                {
                                    detail.PackageDetailList.ForEach(n =>
                                    {
                                        var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == n.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                        var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                        var projectPrinters = printerList.Where(p => p.ProjectId == projects.Id && p.BillType == 1);   //菜品关联的出品单打印机

                                        if (projectPrinters.Any())
                                        {
                                            projectPrinters.ToList().ForEach(c =>
                                            {
                                                cydyInsert.Add(new Cpdy
                                                {
                                                    cymxxh00 = detail.Id,
                                                    cyzdxh00 = order.Id,
                                                    cymxdm00 = projectDetails.Id.ToString(),
                                                    cymxmc00 = projects.Name,
                                                    cymxdw00 = projectDetails.Unit,
                                                    cymxsl00 = (detail.Num * n.Num).ToString(),
                                                    cymxdybz = false,
                                                    cymxyj00 = c.IpAddress,
                                                    cymxclbz = "0",
                                                    cymxczrq = DateTime.Now,
                                                    cymxzdbz = "0",
                                                    cymxyq00 = detail.CyddMxName,
                                                    cymxzf00 = detail.CyddMxName,
                                                    cymxpc00 = detail.CyddMxName,
                                                    cymxczy0 = userInfo.UserName,
                                                    cymxfwq0 = c.PcName,
                                                    cymxczdm = userInfo.UserCode,
                                                    cymxje00 = projectDetails.Price.ToString(),
                                                    cymxth00 = table.Name,
                                                    cymxrs00 = order.PersonNum.ToString(),
                                                    cymxct00 = table.Restaurant,
                                                    cymxzdid = cpdyThGuid,
                                                    cymxbt00 = isRepeat ? "[重打]出品单" : "出品单",
                                                    cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                    cpdysdxh = cpdyThGuid,
                                                    cymxdk00 = c.StallName,
                                                    cymxgdbz = false,
                                                    cpdyfsl0 = market.Name
                                                });
                                            });
                                        }
                                    });
                                }
                            }
                            /*打印套餐出品单 end*/
                            #endregion
                        }
                        #region 打印总单
                        if (newReq.Any())
                        {
                            var groupTotalPrint = printerList.Where(p => p.BillType == 2).GroupBy(p => new { p.StallId, p.Id });
                            if (groupTotalPrint.Any())
                            {
                                groupTotalPrint.ToList().ForEach(c =>
                                {
                                    var printModel = printerList.FirstOrDefault(p => p.Id == c.Key.Id);
                                    cydyInsert.Add(new Cpdy
                                    {
                                        cymxxh00 = newReq[0].R_OrderTable_Id,
                                        cyzdxh00 = order.Id,
                                        cymxdm00 = market.Name,
                                        cymxmc00 = "总单",
                                        cymxdw00 = userInfo.UserName,
                                        cymxsl00 = string.Empty,
                                        cymxdybz = false,
                                        cymxyj00 = printModel.IpAddress,
                                        cymxclbz = "0",
                                        cymxczrq = DateTime.Now,
                                        cymxzdbz = "1",
                                        //cymxyq00 = detail.CyddMxName,
                                        //cymxzf00 = detail.CyddMxName,
                                        //cymxpc00 = detail.CyddMxName,
                                        cymxczy0 = userInfo.UserName,
                                        cymxfwq0 = printModel.PcName,
                                        cymxczdm = userInfo.UserCode,
                                        cymxje00 = newReq.Sum(p => p.Price * p.Num).ToString(),
                                        cymxth00 = table.Name,
                                        cymxrs00 = order.PersonNum.ToString(),
                                        cymxct00 = table.Restaurant,
                                        cymxzdid = cpdyThGuid,
                                        cymxbt00 = "总单",
                                        cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                        cpdysdxh = cpdyThGuid,
                                        cymxdk00 = printModel.StallName,
                                        cymxgdbz = false,
                                        cpdyfsl0 = market.Name
                                    });
                                });
                            }
                        }
                        db.InsertRange<Cpdy>(cydyInsert);
                        #endregion

                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateDate = DateTime.Now,
                            R_Order_Id = order.Id,
                            CreateUser = userInfo.UserId,
                            CyddCzjlStatus = CyddStatus.送厨,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            Remark = "打厨单:" + remarkList.Join(","),
                            R_OrderTable_Id = newReq[0].R_OrderTable_Id
                        });    //订单操作纪录
                        db.CommitTran();
                    }
                    else //库存不够 回滚
                    {
                        result = false;
                        db.RollbackTran();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    result = false;
                    db.RollbackTran();
                }
                return result;
            }
        }

        /// <summary>
        /// 订单明细记录赠送
        /// </summary>
        /// <param name="req">订单明细赠送记录</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public R_OrderDetailRecordDTO CreateOrderDetailRecord(R_OrderDetailRecordDTO req, OperatorModel userInfo, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                R_OrderDetailRecordDTO result = new R_OrderDetailRecordDTO();
                msg = "";
                try
                {
                    #region 条件判断
                    decimal projectExtendPrice = 0;
                    var order = db.Queryable<R_Order>().JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                        .JoinTable<R_OrderTable, R_OrderDetail>((s1, s2, s3) => s2.Id == s3.R_OrderTable_Id)
                        .Where<R_OrderDetail>((s1, s3) => s3.Id == req.R_OrderDetail_Id)
                        .Select("s1.OrderNo,s1.Id").FirstOrDefault();
                    var orderTableId = db.Queryable<R_OrderDetail>().Single(p => p.Id == req.R_OrderDetail_Id).R_OrderTable_Id;
                    var isLock = db.Queryable<R_OrderTable>().Single(p => p.Id == orderTableId).IsLock;
                    if (isLock) { result = null; msg = "台号已锁定！不能赠送！"; return result; }
                    var isCheckout= db.Queryable<R_OrderTable>().Single(p => p.Id == orderTableId).IsCheckOut;
                    if (isCheckout)
                    {
                        result = null; msg = "台号已结账！不能赠送！";
                        return result;
                    }
                    var detail = db.Queryable<R_OrderDetail>().First(p => p.Id == req.R_OrderDetail_Id);  //订单详情
                    var detailRecordNum = db.Queryable<R_OrderDetailRecord>()
                        .Where(p => p.R_OrderDetail_Id == req.R_OrderDetail_Id && p.IsCalculation == true &&
                        (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出
                        || p.CyddMxCzType == CyddMxCzType.退菜)).GroupBy(p => p.R_OrderDetail_Id)
                        .Select("sum(Num) as Num").FirstOrDefault();   //订单详情赠菜，转出，退菜总数
                    var projectExtend = db.Queryable<R_OrderDetailExtend>().Where(p => p.R_OrderDetail_Id == req.R_OrderDetail_Id).ToList();
                    if (projectExtend != null && projectExtend.Any())
                    {
                        projectExtendPrice = projectExtend.Sum(p => p.Price);//特殊要求价格计算
                    }
                    if ((detail.Num - (detailRecordNum != null ? detailRecordNum.Num : 0)) < req.Num)
                    {
                        msg = "赠送数量不能大于当前已点数量，如果你有新增数量，请先保存！";
                        return null;
                    }
                    //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)
                    var originalTotalPrice = (detail.Num - (detailRecordNum != null ? detailRecordNum.Num : 0) - req.Num) * (detail.Price + projectExtendPrice);
                    if (originalTotalPrice < 0)
                    {
                        msg = "数量不合法！";
                        return null;
                    }
                    #endregion
                    db.BeginTran();
                    DateTime timeNow = DateTime.Now;
                    R_OrderDetailRecord model = new R_OrderDetailRecord()
                    {
                        CreateDate = timeNow,
                        R_OrderDetail_Id = req.R_OrderDetail_Id,
                        CreateUser = userInfo.UserId,
                        Num = req.Num,
                        CyddMxCzType = req.CyddMxCzType,
                        IsCalculation = true,
                        Remark = req.Remark + ";" + req.CyddMxCzType.ToString() + ":" + req.CyddMxName + "*" + req.Num + ",订单号：" + order.OrderNo + ",台号：" + string.Join(",", req.TableName),
                        R_OrderDetailCause_Id = req.R_OrderDetailCause_Id,
                        DetailCauseRemark = req.Remark

                    };
                    var insertId = db.Insert<R_OrderDetailRecord>(model);
                    db.Insert<R_OrderRecord>(new R_OrderRecord()
                    {
                        CreateUser = userInfo.UserId,
                        CreateDate = DateTime.Now,
                        CyddCzjlUserType = CyddCzjlUserType.员工,
                        CyddCzjlStatus = CyddStatus.订单菜品修改,
                        Remark = req.Remark + ";" + CyddMxCzType.赠菜.ToString() + ":" + req.CyddMxName + "*" + req.Num + ",订单号：" + order.OrderNo + ",台号：" + string.Join(",", req.TableName),
                        R_Order_Id = order.Id,
                        R_OrderTable_Id = orderTableId
                    });

                    //更新菜品原价总额和赠送金额(OriginalTotalPrice)
                    detail.GiveTotalPrice = detail.GiveTotalPrice + ((detail.Price + projectExtendPrice) * req.Num);
                    detail.OriginalTotalPrice = originalTotalPrice;
                    db.Update<R_OrderDetail>(detail);
                    //db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice, GiveTotalPrice= originalTotalPrice * req.Num }, p => p.Id == req.R_OrderDetail_Id);
                    //if (db.Insert(model) == null)
                    //{
                    //    result = false;
                    //    msg = "赠送失败！";
                    //}
                    //else
                    //{

                    //}
                    db.CommitTran();
                    req.Id = Convert.ToInt32(insertId);
                    result = req;
                    result.CreateDate = timeNow;
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    result = null;
                    db.RollbackTran();
                }
                return result;
            }
        }

        /// <summary>
        /// 判断结账时当前是否还存在保存状态的数据
        /// </summary>
        /// <param name="orderTableInfos">订单台号信息列表</param>
        /// <param name="loginMarketId">当前登录分市id</param>
        /// <param name="itemValue">账务日期</param>
        /// <returns></returns>
        public int JudgeOrderPay(List<OrderTableDTO> orderTableInfos, int loginMarketId, string itemValue, out string msg)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                // bool result = true;
                int num = 0;//1000返回错误  2000正确，并且分市和账务日期都一致  3000正确，分市和账务日期存在不一致
                msg = string.Empty;
                string checkOutStr = string.Empty; //存在结账
                string saveStr = string.Empty;     //存在保存的菜品数据
                string dateStr = string.Empty;     //账务日期不一致
                string marketStr = string.Empty;     //分市不一致
                string dateOrMarketStr = string.Empty;
                //第一步判断
                foreach (var item in orderTableInfos)
                {
                    var orderTable = db.Queryable<R_OrderTable>().Single(p => p.Id == item.Id);
                    if (orderTable.IsCheckOut)
                    {
                        checkOutStr += item.Name + ",";
                        num = 1000;
                    }
                    var data = db.Queryable<R_OrderDetail>().Where(p => p.CyddMxStatus == CyddMxStatus.保存 && p.R_OrderTable_Id == item.Id).ToList();
                    if (data != null && data.Any())
                    {
                        num = 1000;
                        saveStr += item.Name + ",";
                    }
                }
                if (num == 1000)//返回错误信息
                {
                    if (!string.IsNullOrEmpty(checkOutStr))
                    {
                        checkOutStr += "台号已结账，请核查！";
                    }
                    if (!string.IsNullOrEmpty(saveStr))
                    {
                        saveStr += "台号存在未落单的菜品，请核查！";
                    }
                }
                else //否则返回正确信息
                {
                    //第二步查询分市和账务日期是否对的上
                    foreach (var item in orderTableInfos)
                    {
                        var orderTable = db.Queryable<R_OrderTable>().Single(p => p.Id == item.Id);
                        if (orderTable.R_Market_Id != loginMarketId)
                        {
                            marketStr += item.Name + ",";
                            num = 3000;
                        }
                        if (DateTime.Compare(orderTable.BillDate.ToDate(), itemValue.ToDate()) != 0)
                        {
                            dateStr += item.Name + ",";
                            num = 3000;
                        }
                    }
                    if (num == 3000)
                    {
                        if (!string.IsNullOrEmpty(marketStr))
                        {
                            marketStr += "分市与开台时不一致!";
                        }
                        if (!string.IsNullOrEmpty(dateStr))
                        {
                            dateStr += "账务日期与开台时不一致!";
                        }
                        dateOrMarketStr += marketStr + dateStr + "你确定继续结账吗？";
                    }
                    else
                    {
                        num = 2000;
                    }
                }
                msg = checkOutStr + saveStr + dateOrMarketStr;
                return num;
            }
        }

        public bool OrderDelete(OrderDeleteDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();

                    if (dateItem == null)
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

                    DateTime accDate = DateTime.Today;

                    if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                        throw new Exception("餐饮账务日期设置错误，请联系管理员");
                    var orderMainPays = db.Queryable<R_OrderMainPay>().Where(p => p.R_Order_Id == req.Id).ToList();

                    var orderPayRecords = db.Queryable<R_OrderPayRecord>().Any(p => p.R_Order_Id == req.Id && (p.CyddPayType == (int)CyddPayType.挂账 || p.CyddPayType == (int)CyddPayType.会员卡 || p.CyddPayType == (int)CyddPayType.转客房));
                    
                    if (orderPayRecords)
                    {
                        throw new Exception("删除此订单会影响酒店客户账务数据，不充许执行此操作，请联系管理员");
                    }

                    bool orderStatus = db.Queryable<R_Order>().Any(p => p.Id == req.Id && (p.CyddStatus == CyddStatus.结账 || p.CyddStatus == CyddStatus.取消));
                    bool isSameDay = true;
                    foreach (var item in orderMainPays)
                    {
                        if (item.BillDate.Date!=accDate.Date)
                        {
                            isSameDay = false;
                            break;
                        }
                    }

                    if (orderStatus&&isSameDay)
                    {
                        res = db.Update<R_Order>(new { IsDelete = req.IsDelete }, p => p.Id == req.Id);
                    }
                    else
                    {
                        if (!orderStatus)
                        {
                            throw new Exception("请在订单完成结账或取消预定后再执行删除操作");
                        }
                        if (!isSameDay)
                        {
                            throw new Exception("该订单账务日期和当前系统账务日期不符，不能进行删单操作");
                        }
                    }

                    //if (db.Queryable<R_Order>().Any(p=>p.Id==req.Id &&(p.CyddStatus==CyddStatus.结账 || p.CyddStatus == CyddStatus.取消)))
                    //{
                    //    res = db.Update<R_Order>(new { IsDelete = req.IsDelete }, p => p.Id == req.Id);
                    //}
                    //else
                    //{
                    //    throw new Exception("请在订单完成结账或取消预定后再执行删除操作");
                    //}
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return res;
            }
        }

        /// <summary>
        /// 获取业务部门列表
        /// </summary>
        /// <returns></returns>
        public List<SystemCodeInfo> GetDepartList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                string strSql = "select xtdmdm00 as SysCode,xtdmmc00 as SysCodeName from xtdm where xtdmlx00='jdgj'";
                var list = db.SqlQuery<SystemCodeInfo>(strSql);
                return list;
            }
        }

        /// <summary>
        /// 创建订单发票
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateOrderInvoice(InvoiceCreateDTO req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool result = false;
                try
                {
                    if (req.Id>0)
                    {
                        result = db.Update<R_Invoice>(new { Number=req.Number, Company=req.Company, Remark=req.Remark, Title=req.Title, TotalPrice=req.TotalPrice }, p => p.Id == req.Id);
                    }
                    else
                    {
                        R_Invoice model = Mapper.Map<InvoiceCreateDTO, R_Invoice>(req);
                        result = Convert.ToBoolean(db.Insert<R_Invoice>(model));
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 根据订单ID获取发票列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<InvoiceListDTO> GetInvoiceList(int orderId)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var data = db.Queryable<R_Order>()
                    .JoinTable<R_OrderMainPay>((s1, s2) => s1.Id == s2.R_Order_Id)
                    .JoinTable<R_OrderMainPay, R_Market>((s1, s2, s3) => s2.R_Market_Id == s3.Id)
                    .JoinTable<R_OrderMainPay, R_Invoice>((s1, s2, s4) => s2.Id == s4.R_OrderMainPay_Id,JoinType.Inner)
                    .JoinTable<R_Invoice,Czdm>((s1,s4,s5)=>s4.CreateUser==s5.Id)
                    .Where<R_Invoice>((s1, s4) => s1.Id == orderId && s4.IsDelete == false)
                    .Select<R_OrderMainPay, R_Market, R_Invoice, Czdm,InvoiceListDTO>
                    ((s1, s2, s3, s4, s5) => new InvoiceListDTO()
                    {
                        Id = s4.Id,
                        BillDate = s4.BillDate,
                        CreateDate = s4.CreateDate,
                        CreateUserName=s5.Czdmmc00,
                        Number=s4.Number,
                        OrderMainPayMarket=s3.Name,
                        OrderMainPayDate=s2.BillDate,
                        Remark=s4.Remark,
                        Title=s4.Title,
                        TotalPrice=s4.TotalPrice
                    }).ToList();
                return data;
            }
        }

        public InvoiceCreateDTO GetInvoice(int id)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var data = db.Queryable<R_Invoice>().InSingle(id);
                InvoiceCreateDTO model = Mapper.Map<R_Invoice, InvoiceCreateDTO>(data);
                return model;
            }
        }

        public bool DeleteInvoice(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    db.Update<R_Invoice>(new { IsDelete = true }, p => p.Id == id);
                    res = true;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return res;
            }
        }

        public bool UpdateOrderTablePerson(OrderTableDTO req)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    db.BeginTran();
                    var orderTable = db.Queryable<R_OrderTable>().First(p => p.Id == req.Id);
                    var order = db.Queryable<R_Order>().First(p => p.Id == orderTable.R_Order_Id);
                    db.Update<R_OrderTable>(new { PersonNum = req.PersonNum }, p => p.Id == req.Id);
                    db.Update<R_Order>(new { PersonNum= order.PersonNum- orderTable.PersonNum + req.PersonNum }, p => p.Id == orderTable.R_Order_Id);
                    db.CommitTran();
                    res = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    throw ex;
                }
                return res;
            }
        }

        public bool UpdateOrderTableIsControl(List<int> ordertableIds,bool isControl)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    db.Update<R_OrderTable>(new { IsControl = isControl }, p => ordertableIds.Contains(p.Id));
                    res = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return res;
            }
        }

        public bool DeleteOrderDetailRecord(List<R_OrderDetailRecordDTO> req,OperatorModel userInfo)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    db.BeginTran();
                    var orderDetailId = req[0].R_OrderDetail_Id;
                    var orderTableInfoBase = db.Queryable<R_OrderDetail>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.R_OrderTable_Id == s2.Id)
                        .Where<R_OrderTable>((s1, s2) => s1.Id == orderDetailId)
                        .Select<R_OrderTable, OrderTableDTO>((s1, s2) => new OrderTableDTO()
                        {
                            OrderId = s2.R_Order_Id,
                            Id = s2.Id,
                            IsCheckOut = s2.IsCheckOut,
                            IsLock = s2.IsLock
                        }).ToList().First();
                    if (orderTableInfoBase.IsLock||orderTableInfoBase.IsCheckOut)
                    {
                        throw new Exception("该台号已被锁定或已结账，不能执行该操作");
                    }
                    var detail = db.Queryable<R_OrderDetail>().First(p => p.Id == orderDetailId);  //订单详情
                    decimal projectExtendPrice = 0;
                    var detailRecordNum = db.Queryable<R_OrderDetailRecord>()
                        .Where(p => p.R_OrderDetail_Id == orderDetailId && p.IsCalculation == true &&
                        (p.CyddMxCzType == CyddMxCzType.赠菜 || p.CyddMxCzType == CyddMxCzType.转出
                        || p.CyddMxCzType == CyddMxCzType.退菜)).GroupBy(p => p.R_OrderDetail_Id)
                        .Select("sum(Num) as Num").FirstOrDefault();   //订单详情赠菜，转出，退菜总数
                    var projectExtend = db.Queryable<R_OrderDetailExtend>().Where(p => p.R_OrderDetail_Id == orderDetailId).ToList();
                    if (projectExtend != null && projectExtend.Any())
                    {
                        projectExtendPrice = projectExtend.Sum(p => p.Price);//特殊要求价格计算
                    }

                    var ids = req.Select(p => p.Id).ToArray();
                    db.Delete<R_OrderDetailRecord>(p => ids.Contains(p.Id));
                    //db.Update<R_OrderDetailRecord>(new { IsCalculation = false }, p=> ids.Contains(p.Id));
                    var orderTableInfo = db.Queryable<R_OrderDetail>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.R_OrderTable_Id == s2.Id)
                        .Where<R_OrderTable>((s1, s2) => s1.Id == orderDetailId)
                        .Select<R_OrderTable, OrderTableDTO>((s1, s2) => new OrderTableDTO()
                        {
                            OrderId=s2.R_Order_Id,
                            Id=s2.Id
                        }).ToList().First();

                    foreach (var item in req)
                    {
                        db.Insert<R_OrderRecord>(new R_OrderRecord()
                        {
                            CreateUser = userInfo.UserId,
                            CreateDate = DateTime.Now,
                            CyddCzjlUserType = CyddCzjlUserType.员工,
                            CyddCzjlStatus = CyddStatus.订单菜品修改,
                            Remark = string.Format("取消{0}:{1}",item.CyddMxCzType.ToString(),item.Remark),
                            R_Order_Id = orderTableInfo.OrderId,
                            R_OrderTable_Id = orderTableInfo.Id
                        });
                        //	菜品价格
                        var originalTotalPrice = item.Num * (detail.Price + projectExtendPrice);
                        //更新菜品原价总额和赠送金额(OriginalTotalPrice)
                        detail.OriginalTotalPrice = detail.OriginalTotalPrice + originalTotalPrice;
                        if (item.CyddMxCzType==CyddMxCzType.赠菜)
                        {
                            detail.GiveTotalPrice = detail.GiveTotalPrice - originalTotalPrice;
                        }                        
                        db.Update<R_OrderDetail>(detail);
                    }
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public bool UpdateOrderTableListPrint(int orderTableId)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    db.Update<R_OrderDetail>(new { IsListPrint=true }, p => p.R_OrderTable_Id == orderTableId);
                }
                catch (Exception ex)
                {
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public bool WeixinPrint(List<OrderDetailDTO> req, List<int> orderTableIds, CyddMxStatus status, OperatorModel userInfo, CyddCzjlUserType userType = CyddCzjlUserType.员工)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    int thid = orderTableIds[0];
                    var order = db.Queryable<R_Order>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Order_Id)
                        .Where<R_OrderTable>((s1, s2) => s2.Id == thid).Select("s1.*").FirstOrDefault();
                    var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == order.R_Market_Id);
                    var resturant = db.Queryable<R_Restaurant>().FirstOrDefault(p => p.Id == order.R_Restaurant_Id);
                    var tables = db.Queryable<R_Table>()
                        .JoinTable<R_OrderTable>((s1, s2) => s1.Id == s2.R_Table_Id)
                        .Where<R_OrderTable>((s1, s2) => orderTableIds.Contains(s2.Id)).ToList();

                    var newReq = req.Where(p => p.Id == 0 || (p.Id > 0 && p.CyddMxStatus == CyddMxStatus.保存)).ToList();    //新添加的菜品列表 与保存的菜品
                    var areas = db.Queryable<R_Area>()
                        .JoinTable<R_Table>((s1, s2) => s1.Id == s2.R_Area_Id)
                        .JoinTable<R_Table, R_OrderTable>((s1, s2, s3) => s2.Id == s3.R_Table_Id)
                        .Where<R_OrderTable>((s1, s3) => orderTableIds.Contains(s3.Id))
                        .Select("s1.*")
                        .ToList();
                    var areaIds = areas.Select(p => p.Id).ToArray();
                    var prints = db.Queryable<Printer>()
                        .JoinTable<R_WeixinPrint>((s1, s2) => s1.Id == s2.Print_Id)
                        .JoinTable<R_WeixinPrint, R_WeixinPrintArea>((s1, s2, s3) => s2.Id == s3.R_WeixinPrint_Id)
                        .Where<R_WeixinPrint,R_WeixinPrintArea>((s1,s2, s3) => s1.IsDelete == false && s2.PrintType==PrintType.微信区域出单 && areaIds.Contains(s3.R_Area_Id)).Select("s1.*").ToList();
                    var printerList = prints.Distinct().ToList();

                    #region 厨单出品单打印业务
                    if (printerList.Any())
                    {
                        string cpdyThGuid = string.Empty;   //出品打印按桌号出生成标识
                        cpdyThGuid = orderTableIds[0] + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        List<Cpdy> cydyInsert = new List<Cpdy>();
                        foreach (var print in printerList)
                        {
                            foreach (var detail in newReq)
                            {
                                string yq = string.Empty;   //要求
                                string zf = string.Empty;   //做法
                                string pc = string.Empty;   //配菜
                                string dishStatus = string.Empty;   //即起|叫起
                                string detailRemark = string.Empty; //手写要求（备注）

                                if (detail.Extend != null && detail.Extend.Any())
                                {
                                    yq = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.要求).Select(p => p.ProjectExtendName).ToArray());
                                    zf = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.做法).Select(p => p.ProjectExtendName).ToArray());
                                    pc = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.配菜).Select(p => p.ProjectExtendName).ToArray());
                                }

                                var model = Mapper.Map<OrderDetailDTO, R_OrderDetail>(detail);
                                cydyInsert.Add(new Cpdy
                                {
                                    cymxxh00 = orderTableIds[0],
                                    cyzdxh00 = order.Id,
                                    cymxdm00 = detail.CyddMxId.ToString(),
                                    cymxmc00 = string.IsNullOrEmpty(model.ExtendName) ? model.CyddMxName : model.ExtendName,
                                    cymxdw00 = detail.Unit,
                                    cymxsl00 = detail.Num.ToString(),
                                    cymxdybz = false,
                                    cymxyj00 = print.IpAddress,
                                    cymxclbz = "2",
                                    cymxczrq = DateTime.Now,
                                    cymxzdbz = "0",
                                    cymxyq00 = string.IsNullOrEmpty(yq) ? dishStatus + detailRemark : yq + "," + dishStatus + detailRemark,
                                    cymxzf00 = zf,
                                    cymxpc00 = pc,
                                    cymxczy0 = userInfo.UserName,
                                    cymxfwq0 = print.PcName,
                                    cymxczdm = userInfo.UserCode,
                                    cymxje00 = detail.Price.ToString(),
                                    cymxth00 = tables[0].Name,
                                    cymxrs00 = order.PersonNum.ToString(),
                                    cymxct00 = resturant.Name,
                                    cymxzdid = cpdyThGuid,
                                    cymxbt00 = "微信出品单",
                                    cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                    cpdysdxh = cpdyThGuid,
                                    cymxdk00 = print.Name,
                                    cymxgdbz = false,
                                    cpdyfsl0 = market.Name
                                });
                            }

                            #region 厨单总单打印
                            cydyInsert.Add(new Cpdy
                            {
                                cymxxh00 = orderTableIds[0],
                                cyzdxh00 = order.Id,
                                cymxdm00 = market.Name,
                                cymxmc00 = "微信总单",
                                cymxdw00 = userInfo.UserName,
                                cymxsl00 = string.Empty,
                                cymxdybz = false,
                                cymxyj00 = print.IpAddress,
                                cymxclbz = "0",
                                cymxczrq = DateTime.Now,
                                cymxzdbz = "1",
                                //cymxyq00 = detail.CyddMxName,
                                //cymxzf00 = detail.CyddMxName,
                                //cymxpc00 = detail.CyddMxName,
                                cymxczy0 = userInfo.UserName,
                                cymxfwq0 = print.PcName,
                                cymxczdm = userInfo.UserCode,
                                cymxje00 = newReq.Sum(p => p.Price * p.Num).ToString(),
                                cymxth00 = tables[0].Name,
                                cymxrs00 = order.PersonNum.ToString(),
                                cymxct00 = resturant.Name,
                                cymxzdid = cpdyThGuid,
                                cymxbt00 = "微信总单",
                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                cpdysdxh = cpdyThGuid,
                                cymxdk00 = print.Name,
                                cymxgdbz = false,
                                cpdyfsl0 = market.Name
                            });
                            #endregion
                        }
                        db.InsertRange<Cpdy>(cydyInsert);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public int GetOrderCountBeforeNightTrial()
        {
            using (var db=new SqlSugarClient(Connection))
            {
                var count = db.Queryable<R_Order>()
                    .Where(p => p.CyddStatus != CyddStatus.结账 && 
                    (p.CyddStatus != CyddStatus.预定 && p.CyddStatus != CyddStatus.取消))
                    .Count();
                return count;
            }
        }

        public bool NightTrial(int companyId,string userCode)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool res = false;
                try
                {
                    var dateItem = _extendItemRepository.GetModelList(companyId, 10003).FirstOrDefault();

                    if (dateItem == null)
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");

                    DateTime accDate = DateTime.Today;

                    if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                        throw new Exception("餐饮账务日期设置错误，请联系管理员");
                    string itemValue = accDate.Date.AddDays(1).ToString("yyyy-MM-dd");
                    db.BeginTran();
                    db.CommandType = System.Data.CommandType.StoredProcedure;//指定为存储过程可比上面少写EXEC和参数
                    db.ExecuteCommand("p_rcl_cytj_new");
                    db.CommandType = System.Data.CommandType.Text;//还原回默认

                    res = _extendItemRepository.UpdateItemValue(companyId, 10003, itemValue);
                    _extendItemRepository.UpdateXtcs("SYSDATE",Convert.ToDateTime(itemValue));
                    OperateLogInfo logInfo = new OperateLogInfo
                    {
                        OperateType = "YS",
                        OperateTime = DateTime.Now,
                        UserCode = userCode,
                        Remark = "于" + DateTime.Now.ToString() +
        "登陆，电脑名称-" + Net.Host + "，登陆IP地址-" + Net.Ip
                    };

                    logInfo.OperateRemark = "餐饮夜审";
                    logInfo.ActionName = "餐饮夜审";

                    _userLogRepository.SaveLogN("", logInfo);//写入操作日志记录
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public bool FlatOrderSubmit(ReserveCreateDTO req, List<int> tableIds, List<OrderDetailDTO> list, OperatorModel user, CyddMxStatus status,bool isListPrint=false)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool res = true;
                try
                {
                    var newReq = list.Where(p => p.Id == 0 || (p.Id > 0 && p.CyddMxStatus == CyddMxStatus.保存)).ToList();    //新添加的菜品列表 与保存的菜品
                    DateTime accDate = DateTime.Today;
                    var dateItem = _extendItemRepository.GetModelList(req.CompanyId, 10003).FirstOrDefault();
                    if (dateItem == null)
                        throw new Exception("餐饮账务日期尚未初始化，请联系管理员");
                    if (!DateTime.TryParse(dateItem.ItemValue, out accDate))
                        throw new Exception("餐饮账务日期设置错误，请联系管理员");
                    var isCanOpen = db.Queryable<R_Table>()
                        .Any(p => p.CythStatus != CythStatus.空置 && tableIds.Contains(p.Id));
                    if (isCanOpen)
                    {
                        throw new Exception("所选台号已被占用，请重新选择");
                    }
                    var tableList = db.Queryable<R_Table>().Where(p => tableIds.Contains(p.Id)).ToList();

                    var tableNames = tableList.Select(x => x.Name).ToList();

                    var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == req.R_Market_Id);
                    var resturant = db.Queryable<R_Restaurant>().FirstOrDefault(p => p.Id == req.R_Restaurant_Id);

                    #region 所需打印机列表
                    //var projectIds = newReq.Where(p => p.CyddMxType == CyddMxType.餐饮项目).Select(p => p.R_Project_Id).ToList(); //餐饮项目Ids

                    //var projectDetailIds = new List<int>();
                    //newReq.Where(p => p.CyddMxType == CyddMxType.餐饮套餐).ToList().ForEach(n =>
                    //{
                    //    projectDetailIds = projectDetailIds.Concat(n.PackageDetailList.Select(p => p.R_ProjectDetail_Id).ToList()).ToList();    //套餐餐饮项目明细IDS
                    //});
                    //var packageProjects = db.Queryable<R_ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id)).Select(p => p.R_Project_Id).ToList();
                    //projectIds = projectIds.Concat(packageProjects).ToList();    //连接餐饮项目IDS和套餐包含的餐饮项目IDS
                    //projectIds = projectIds.GroupBy(p => p).Select(p => p.Key).ToList();
                    var printerList = db.Queryable<Printer>()
                        .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id && s2.IsDelete == false)
                        .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                        .Where<R_Stall, R_ProjectStall>((s1, s2, s3) => s1.IsDelete == false)
                        .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                        {
                            Id = s1.Id,
                            Code = s1.Code,
                            IpAddress = s1.IpAddress,
                            IsDelete = s1.IsDelete,
                            Name = s1.Name,
                            PcName = s1.PcName,
                            PrintPort = s1.PrintPort,
                            Remark = s1.Remark,
                            ProjectId = s3.R_Project_Id,
                            BillType = s3.BillType,
                            StallName = s2.Name,
                            StallId = s2.Id
                        }).ToList();    //餐饮项目打印机列表
                    #endregion

                    db.BeginTran();

                    #region 菜品库存
                    bool isStock = (status == CyddMxStatus.已出 || status == CyddMxStatus.未出) ? true : false;  //是否判断库存  保存状态不用判断库存
                    bool isPrint = status == CyddMxStatus.已出 ? true : false;    //是否打印
                    string cpdyThGuid = string.Empty;   //出品打印按桌号出生成标识
                    List<Cpdy> cydyInsert = new List<Cpdy>();
                    List<string> remarkList = new List<string>();
                    var projectIds = list.Where(p => p.CyddMxType == CyddMxType.餐饮项目).Select(p => p.R_Project_Id).ToArray();
                    var packageIds= list.Where(p => p.CyddMxType == CyddMxType.餐饮套餐).Select(p => p.R_Project_Id).ToArray();
                    var projectList = db.Queryable<R_Project>()
                        .Where(p => projectIds.Contains(p.Id)).ToList();
                    var packageList = db.Queryable<R_Package>()
                        .Where(p => packageIds.Contains(p.Id)).ToList();
                    var projectDetailList = db.Queryable<R_ProjectDetail>().ToList();
                    if (isStock)
                    {
                        foreach (var item in newReq)
                        {
                            /*餐饮项目库存判断 start*/
                            if (item.CyddMxType == CyddMxType.餐饮项目)
                            {
                                var project = projectList.Where(p => p.Id == item.R_Project_Id).FirstOrDefault();
                                var projectDetail = projectDetailList.Where(p => p.Id == item.CyddMxId).FirstOrDefault();

                                if (project.IsStock)
                                {
                                    var projectUseStock = projectDetail.UnitRate * item.Num * tableIds.Count();
                                    if (project.Stock < projectUseStock)
                                    {
                                        throw new Exception(string.Format("{0} 超出库存,剩余{1}", item.ProjectName, project.Stock));
                                    }
                                    else
                                    {
                                        db.Update<R_Project>(new
                                        {
                                            Stock = project.Stock - projectUseStock
                                        }, p => p.Id == item.R_Project_Id);
                                    }
                                }
                            }
                            /*餐饮项目库存判断 end*/
                            /*餐饮套餐库存判断 start*/
                            else if (item.CyddMxType == CyddMxType.餐饮套餐)
                            {
                                if (item.PackageDetailList != null && item.PackageDetailList.Any())
                                {
                                    foreach (var pro in item.PackageDetailList)
                                    {
                                        var projectDetails = projectDetailList.Where(p => p.Id == pro.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                        var projects = projectList.Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                        // var packagedetail = packageDetails.First(p => p.R_ProjectDetail_Id == pro.Id);

                                        if (projects.IsStock)
                                        {
                                            var projectUseStock = projectDetails.UnitRate * item.Num * pro.Num * tableIds.Count();    //该餐饮项目共需多少库存
                                            if (projects.Stock < projectUseStock)
                                            {
                                                throw new Exception(string.Format("{0}-{1} 超出库存,剩余{2}", item.ProjectName, projects.Name,projects.Stock));
                                            }
                                            else
                                            {
                                                db.Update<R_Project>(new
                                                {
                                                    Stock = projects.Stock - projectUseStock
                                                }, p => p.Id == projects.Id);
                                            }
                                        }
                                    }
                                }
                            }
                            /*餐饮套餐库存判断 end*/
                        }
                    }
                    #endregion

                    R_Order orderModel = Mapper.Map<ReserveCreateDTO, R_Order>(req);
                    orderModel.CyddStatus = CyddStatus.点餐;
                    var insertOrderId = Convert.ToInt32(db.Insert<R_Order>(orderModel));
                    db.Insert<R_OrderRecord>(new R_OrderRecord
                    {
                        CreateDate = DateTime.Now,
                        R_Order_Id = insertOrderId,
                        CreateUser = req.CreateUser,
                        CyddCzjlStatus = CyddStatus.开台,
                        CyddCzjlUserType = req.UserType,
                        Remark = string.Format("平板开台操作-订单（{0}）开台（{1}）",
req.OrderNo, tableNames.Join(",")),
                        R_OrderTable_Id = 0 //订单操作纪录
                    });
                    db.Update<R_Table>(new
                    {
                        CythStatus = (int)CythStatus.在用
                    }, p => tableIds.Contains(p.Id));
                    int personNumAvg = tableIds.Count() > 1 ?
                        req.PersonNum / tableIds.Count() : req.PersonNum;    //台号人均
                    int personNumRemainder = tableIds.Count() > 1 ?
                        req.PersonNum % tableIds.Count() : 0;  //台号余人
                    int eachRemainder = 0;
                    int insertOrderTableId = 0;
                    if (tableIds != null && tableIds.Count > 0)
                    {
                        List<int> otList = new List<int>();
                        foreach (var item in tableIds)
                        {
                            eachRemainder++;
                            R_OrderTable obj = new R_OrderTable();
                            obj.R_Order_Id = insertOrderId;
                            obj.R_Table_Id = item;
                            obj.CreateDate = DateTime.Now;
                            obj.IsOpen = true; //开台标识
                            obj.PersonNum = personNumAvg + (personNumRemainder - eachRemainder >= 0 ? 1 : 0);
                            obj.R_Market_Id = req.CurrentMarketId;
                            obj.BillDate = accDate;
                            insertOrderTableId = Convert.ToInt32(db.Insert<R_OrderTable>(obj));
                            otList.Add(insertOrderTableId);

                            cydyInsert = new List<Cpdy>();
                            cpdyThGuid = insertOrderTableId + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            remarkList = new List<string>();

                            string dishStatus = string.Empty;   //即起|叫起
                            string detailRemark = string.Empty; //手写要求（备注）                           
                            foreach (var detail in newReq)
                            {
                                string yq = string.Empty;   //要求
                                string zf = string.Empty;   //做法
                                string pc = string.Empty;   //配菜
                                decimal giveNum = 0;//记录赠送菜品明细赠送总数量
                                decimal projectExtendPrice = 0;//记录特殊要求总价=要求+做饭+配菜
                                dishStatus = detail.DishesStatus.ToString();
                                detailRemark = string.IsNullOrEmpty(detail.Remark) ? "" : "," + detail.Remark;
                                remarkList.Add(string.Format("{0}{1} * {2}", detail.CyddMxName,
                                    string.IsNullOrEmpty(detail.Unit) ? "" : "(" + detail.Unit + ")", detail.Num));

                                #region 订单明细添加
                                var model = Mapper.Map<OrderDetailDTO, R_OrderDetail>(detail);
                                model.R_OrderTable_Id = insertOrderTableId;
                                model.CyddMxStatus = status;
                                model.CyddMxType = detail.CyddMxType;
                                model.CreateDate = DateTime.Now;
                                model.CreateUser = user.UserId;
                                model.DiscountRate = detail.DiscountRate == 0 ? 1 : detail.DiscountRate;
                                model.DishesStatus = detail.DishesStatus;   //添加叫起|即起属性
                                model.IsListPrint = isListPrint;
                                int newMxId = Convert.ToInt32(db.Insert<R_OrderDetail>(model));
                                #endregion

                                #region 套餐详情
                                if (detail.CyddMxType == CyddMxType.餐饮套餐)
                                {
                                    if (detail.PackageDetailList != null && detail.PackageDetailList.Any())
                                    {
                                        detail.PackageDetailList.ForEach(p => p.R_OrderDetail_Id = newMxId);
                                        db.InsertRange<R_OrderDetailPackageDetail>(detail.PackageDetailList);
                                    }
                                }
                                #endregion

                                #region 订单明细扩展添加
                                if (detail.Extend != null && detail.Extend.Any())
                                {
                                    projectExtendPrice = detail.Extend.Sum(p => p.Price);
                                    yq = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.要求).Select(p => p.ProjectExtendName).ToArray());
                                    zf = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.做法).Select(p => p.ProjectExtendName).ToArray());
                                    pc = string.Join(",", detail.Extend.Where(p => p.ExtendType == CyxmKzType.配菜).Select(p => p.ProjectExtendName).ToArray());
                                    List<R_OrderDetailExtend> mxkzList = new List<R_OrderDetailExtend>();
                                    foreach (var extend in detail.Extend)
                                    {
                                        mxkzList.Add(new R_OrderDetailExtend()
                                        {
                                            R_OrderDetail_Id = newMxId,
                                            R_ProjectExtend_Id = extend.Id,
                                            Name = extend.ProjectExtendName,
                                            Price = extend.Price,
                                            Unit = extend.Unit
                                        });
                                    }
                                    db.InsertRange(mxkzList);
                                }
                                #endregion

                                #region 赠送菜品
                                if (detail.OrderDetailRecordCount != null && detail.OrderDetailRecordCount.Any())
                                {
                                    giveNum = detail.OrderDetailRecordCount.Where(p => p.CyddMxCzType == CyddMxCzType.赠菜).Sum(p => p.Num);
                                    List<R_OrderDetailRecord> orderDetailRecordList = new List<R_OrderDetailRecord>();
                                    foreach (var recordCount in detail.OrderDetailRecordCount)
                                    {
                                        orderDetailRecordList.Add(new R_OrderDetailRecord()
                                        {
                                            CreateDate = DateTime.Now,
                                            R_OrderDetail_Id = newMxId,
                                            CreateUser = user.UserId,
                                            Num = recordCount.Num,
                                            CyddMxCzType = recordCount.CyddMxCzType,
                                            IsCalculation = true,
                                            Remark = recordCount.CyddMxCzType.ToString() + ":" + detail.CyddMxName + "*" + recordCount.Num + ",订单id：" + insertOrderId + ",台号：" + string.Join(",", tableNames)
                                        });
                                        //if (recordCount.CyddMxCzType==CyddMxCzType.赠菜)
                                        //{
                                        //    db.Update<R_OrderDetail>(new { GiveTotalPrice = detail.Price * recordCount.Num }, p => p.Id == newMxId);
                                        //}
                                    }
                                    db.InsertRange(orderDetailRecordList);
                                }
                                #endregion

                                #region 更新菜品原价总额(OriginalTotalPrice)
                                //	原价总额=(菜品数量 - 赠送|退菜|转出数量)*(菜品单价 + 要求|做法|配菜价格)  套餐没有特殊要求和赠送
                                var originalTotalPrice = detail.CyddMxType == CyddMxType.餐饮项目 ? (detail.Num - giveNum) * (detail.Price + projectExtendPrice) :
                                    detail.Num * detail.Price;
                                //if (originalTotalPrice >= 0)
                                //{
                                //    db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice, GiveTotalPrice= giveNum * (detail.Price + projectExtendPrice) }, p => p.Id == Convert.ToInt32(mxId));
                                //}
                                //else
                                //{
                                //    message = "数量不合法！";
                                //    res = false;
                                //}
                                db.Update<R_OrderDetail>(new { OriginalTotalPrice = originalTotalPrice, GiveTotalPrice = giveNum * (detail.Price + projectExtendPrice) }, p => p.Id == newMxId);
                                #endregion

                                #region 厨单出品单打印业务
                                if (isPrint)
                                {
                                    /*打印餐饮项目出品单 start*/
                                    if (detail.CyddMxType == CyddMxType.餐饮项目)
                                    {
                                        var projectPrinters = printerList.Where(p => p.ProjectId == detail.R_Project_Id && p.BillType == 1);   //菜品关联的出品单打印机
                                        if (projectPrinters.Any())
                                        {
                                            projectPrinters.ToList().ForEach(c =>
                                            {
                                                cydyInsert.Add(new Cpdy
                                                {
                                                    cymxxh00 = newMxId,
                                                    cyzdxh00 = insertOrderId,
                                                    cymxdm00 = detail.CyddMxId.ToString(),
                                                    cymxmc00 = string.IsNullOrEmpty(model.ExtendName) ? model.CyddMxName : model.ExtendName,
                                                    cymxdw00 = detail.Unit,
                                                    cymxsl00 = detail.Num.ToString(),
                                                    cymxdybz = false,
                                                    cymxyj00 = c.IpAddress,
                                                    cymxclbz = "0",
                                                    cymxczrq = DateTime.Now,
                                                    cymxzdbz = "0",
                                                    cymxyq00 = string.IsNullOrEmpty(yq) ? dishStatus + detailRemark : yq + "," + dishStatus + detailRemark,
                                                    cymxzf00 = zf,
                                                    cymxpc00 = pc,
                                                    cymxczy0 = user.UserName,
                                                    cymxfwq0 = c.PcName,
                                                    cymxczdm = user.UserCode,
                                                    cymxje00 = detail.Price.ToString(),
                                                    cymxth00 = tableList.FirstOrDefault(p => p.Id == item).Name,
                                                    cymxrs00 = req.PersonNum.ToString(),
                                                    cymxct00 = resturant.Name,
                                                    cymxzdid = cpdyThGuid,
                                                    cymxbt00 = "出品单",
                                                    cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                    cpdysdxh = cpdyThGuid,
                                                    cymxdk00 = c.StallName,
                                                    cymxgdbz = false,
                                                    cpdyfsl0 = market.Name
                                                });
                                            });
                                        }
                                    }
                                    /*打印餐饮项目出品单 end*/
                                    /*打印套餐出品单 start*/
                                    else if (detail.CyddMxType == CyddMxType.餐饮套餐)
                                    {
                                        if (detail.PackageDetailList != null && detail.PackageDetailList.Any())
                                        {
                                            detail.PackageDetailList.ForEach(n =>
                                            {
                                                var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == n.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                                var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                                var projectPrinters = printerList.Where(p => p.ProjectId == projects.Id && p.BillType == 1);   //菜品关联的出品单打印机
                                                if (projectPrinters.Any())
                                                {
                                                    projectPrinters.ToList().ForEach(c =>
                                                    {
                                                        cydyInsert.Add(new Cpdy
                                                        {
                                                            cymxxh00 = newMxId,
                                                            cyzdxh00 = insertOrderId,
                                                            cymxdm00 = projectDetails.Id.ToString(),
                                                            cymxmc00 = n.Name,
                                                            cymxdw00 = projectDetails.Unit,
                                                            cymxsl00 = (detail.Num * n.Num).ToString(),
                                                            cymxdybz = false,
                                                            cymxyj00 = c.IpAddress,
                                                            cymxclbz = "0",
                                                            cymxczrq = DateTime.Now,
                                                            cymxzdbz = "0",
                                                            cymxyq00 = dishStatus,
                                                            cymxzf00 = detail.CyddMxName,
                                                            cymxpc00 = string.Empty,
                                                            cymxczy0 = user.UserName,
                                                            cymxfwq0 = c.PcName,
                                                            cymxczdm = user.UserCode,
                                                            cymxje00 = projectDetails.Price.ToString(),
                                                            cymxth00 = tableList.FirstOrDefault(p => p.Id == item).Name,
                                                            cymxrs00 = req.PersonNum.ToString(),
                                                            cymxct00 = resturant.Name,
                                                            cymxzdid = cpdyThGuid,
                                                            cymxbt00 = "出品单",
                                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                            cpdysdxh = cpdyThGuid,
                                                            cymxdk00 = c.StallName,
                                                            cymxgdbz = false,
                                                            cpdyfsl0 = market.Name
                                                        });
                                                    });
                                                }
                                            });
                                        }
                                    }
                                    /*打印套餐出品单 end*/
                                }
                                #endregion
                            }
                            #region 厨单总单打印
                            if (isPrint && newReq.Any())
                            {
                                var groupTotalPrint = printerList.Where(p => p.BillType == 2).GroupBy(p => new { p.StallId, p.Id });

                                if (groupTotalPrint.Any())
                                {
                                    groupTotalPrint.ToList().ForEach(c =>
                                    {
                                        var printModel = printerList.FirstOrDefault(p => p.Id == c.Key.Id);
                                        cydyInsert.Add(new Cpdy
                                        {
                                            cymxxh00 = insertOrderTableId,
                                            cyzdxh00 = insertOrderId,
                                            cymxdm00 = market.Name,
                                            cymxmc00 = "总单",
                                            cymxdw00 = user.UserName,
                                            cymxsl00 = string.Empty,
                                            cymxdybz = false,
                                            cymxyj00 = printModel.IpAddress,
                                            cymxclbz = "0",
                                            cymxczrq = DateTime.Now,
                                            cymxzdbz = "1",
                                            //cymxyq00 = detail.CyddMxName,
                                            //cymxzf00 = detail.CyddMxName,
                                            //cymxpc00 = detail.CyddMxName,
                                            cymxczy0 = user.UserName,
                                            cymxfwq0 = printModel.PcName,
                                            cymxczdm = user.UserCode,
                                            cymxje00 = newReq.Sum(p => p.Price * p.Num).ToString(),
                                            cymxth00 = tableList.FirstOrDefault(p => p.Id == item).Name,
                                            cymxrs00 = req.PersonNum.ToString(),
                                            cymxct00 = resturant.Name,
                                            cymxzdid = cpdyThGuid,
                                            cymxbt00 = "总单",
                                            cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                            cpdysdxh = cpdyThGuid,
                                            cymxdk00 = printModel.StallName,
                                            cymxgdbz = false,
                                            cpdyfsl0 = market.Name
                                        });
                                    });
                                }
                            }
                            #endregion
                            db.InsertRange<Cpdy>(cydyInsert);   //批量添加菜品打印信息
                            if (status != CyddMxStatus.保存)
                            {
                                db.Insert<R_OrderRecord>(new R_OrderRecord()
                                {
                                    CreateDate = DateTime.Now,
                                    R_Order_Id = insertOrderId,
                                    CreateUser = user.UserId,
                                    CyddCzjlStatus = CyddStatus.点餐,
                                    CyddCzjlUserType = CyddCzjlUserType.员工,
                                    Remark = remarkList.Join(","),
                                    R_OrderTable_Id = insertOrderTableId
                                });    //订单操作纪录
                            }
                        }
                    }
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res = false;
                    throw ex;
                }
                return res;
            }
        }

        public bool RemindOrder(int orderTableId,List<int> detailIds, OperatorModel user)
        {
            using (var db=new SqlSugarClient(Connection))
            {
                bool result = true;
                try
                {
                    var printerList = db.Queryable<Printer>()
                        .JoinTable<R_Stall>((s1, s2) => s1.Id == s2.Print_Id && s2.IsDelete == false)
                        .JoinTable<R_Stall, R_ProjectStall>((s1, s2, s3) => s2.Id == s3.R_Stall_Id)
                        .Where<R_Stall, R_ProjectStall>((s1, s2, s3) => s1.IsDelete == false)
                        .Select<R_Stall, R_ProjectStall, PrinterProject>((s1, s2, s3) => new PrinterProject
                        {
                            Id = s1.Id,
                            Code = s1.Code,
                            IpAddress = s1.IpAddress,
                            IsDelete = s1.IsDelete,
                            Name = s1.Name,
                            PcName = s1.PcName,
                            PrintPort = s1.PrintPort,
                            Remark = s1.Remark,
                            ProjectId = s3.R_Project_Id,
                            BillType = s3.BillType,
                            StallName = s2.Name,
                            StallId = s2.Id
                        }).ToList();    //餐饮项目打印机列表
                    
                    if (detailIds.Any())
                    {
                        List<Cpdy> cydyInsert = new List<Cpdy>();
                        var detailList = db.Queryable<R_OrderDetail>()
                            .JoinTable<R_ProjectDetail>((s1, s2) => s1.CyddMxId == s2.Id)
                            .Where<R_ProjectDetail>((s1, s2) => detailIds.Contains(s1.Id))
                            .Select<OrderDetailDTO>("s1.*,s2.R_Project_Id as R_Project_Id").ToList();
                        var orderTable = GetOrderAndTablesByOrderTableId(orderTableId);
                        var orderDetailRecords = db.Queryable<R_OrderDetailRecord>()
                            .Where(p => detailIds.Contains(p.R_OrderDetail_Id) && p.CyddMxCzType == CyddMxCzType.退菜).ToList();
                        var market = db.Queryable<R_Market>().FirstOrDefault(p => p.Id == orderTable.MarketId);
                        string cpdyThGuid = orderTableId + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        var detailExtends = db.Queryable<R_OrderDetailExtend>()
                            .JoinTable<R_ProjectExtend>((s1, s2) => s1.R_ProjectExtend_Id == s2.Id)
                            .Where<R_ProjectExtend>((s1, s2) => detailIds.Contains(s1.R_OrderDetail_Id))
                            .Select<R_ProjectExtend, ProjectExtendDTO>((s1, s2) => new ProjectExtendDTO()
                            {
                                CyddMxId=s1.R_OrderDetail_Id,
                                ProjectExtendName=s1.Name,
                                ExtendType=s2.CyxmKzType
                            }).ToList();
                        string yq = string.Empty;
                        string zf = string.Empty;
                        string pc = string.Empty;

                        db.BeginTran();
                        foreach (var detail in detailList)
                        {
                            var remindNum = detail.RemindNum + 1;
                            var excludeNum = orderDetailRecords.Where(p => p.R_OrderDetail_Id == detail.Id).Sum(p => p.Num);
                            detail.Num = detail.Num - excludeNum;
                            if (detail.Num > 0)
                            {
                                if (detail.CyddMxType == CyddMxType.餐饮项目)
                                {
                                    var projectPrinters = printerList.Where(p => p.ProjectId == detail.R_Project_Id && p.BillType == 1);   //菜品关联的出品单打印机
                                    if (projectPrinters.Any())
                                    {
                                        yq = string.Join(",", detailExtends.Where(p => p.CyddMxId == detail.Id && p.ExtendType == CyxmKzType.要求).Select(p => p.ProjectExtendName).ToList());
                                        zf = string.Join(",", detailExtends.Where(p => p.CyddMxId == detail.Id && p.ExtendType == CyxmKzType.做法).Select(p => p.ProjectExtendName).ToList());
                                        pc = string.Join(",", detailExtends.Where(p => p.CyddMxId == detail.Id && p.ExtendType == CyxmKzType.配菜).Select(p => p.ProjectExtendName).ToList());
                                        projectPrinters.ToList().ForEach(c =>
                                        {
                                            cydyInsert.Add(new Cpdy
                                            {
                                                cymxxh00 = detail.Id,
                                                cyzdxh00 = orderTable.OrderId,
                                                cymxdm00 = detail.CyddMxId.ToString(),
                                                cymxmc00 = detail.CyddMxName,
                                                cymxdw00 = detail.Unit,
                                                cymxsl00 = detail.Num.ToString(),
                                                cymxdybz = false,
                                                cymxyj00 = c.IpAddress,
                                                cymxclbz = "0",
                                                cymxczrq = DateTime.Now,
                                                cymxzdbz = "0",
                                                cymxyq00 = detail.DishesStatus.ToString(),
                                                cymxzf00 = zf,
                                                cymxpc00 = pc,
                                                cymxczy0 = user.UserName,
                                                cymxfwq0 = c.PcName,
                                                cymxczdm = user.UserCode,
                                                cymxje00 = detail.Price.ToString(),
                                                cymxth00 = orderTable.TableName,
                                                cymxrs00 = orderTable.PersonNum.ToString(),
                                                cymxct00 = orderTable.Restaurant,
                                                cymxzdid = cpdyThGuid,
                                                cymxbt00 = "催菜单 " + remindNum,
                                                cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                cpdysdxh = cpdyThGuid,
                                                cymxdk00 = c.StallName,
                                                cymxgdbz = false,
                                                cpdyfsl0 = market.Name
                                            });
                                        });
                                    }
                                }
                                /*打印餐饮项目催菜单 end*/
                                /*打印套餐出品单 start*/
                                else if (detail.CyddMxType == CyddMxType.餐饮套餐)
                                {
                                    detail.PackageDetailList = db.Queryable<R_OrderDetailPackageDetail>()
                                        .Where(p => detail.CyddMxId == p.R_OrderDetail_Id).ToList();
                                    if (detail.PackageDetailList != null && detail.PackageDetailList.Any())
                                    {
                                        detail.PackageDetailList.ForEach(n =>
                                        {
                                            var projectDetails = db.Queryable<R_ProjectDetail>().Where(p => p.Id == n.R_ProjectDetail_Id && p.IsDelete == false).FirstOrDefault();    //套餐项目详情
                                            var projects = db.Queryable<R_Project>().Where(p => p.Id == projectDetails.R_Project_Id).FirstOrDefault();
                                            var projectPrinters = printerList.Where(p => p.ProjectId == projects.Id && p.BillType == 1);   //菜品关联的出品单打印机
                                            if (projectPrinters.Any())
                                            {
                                                projectPrinters.ToList().ForEach(c =>
                                                {
                                                    cydyInsert.Add(new Cpdy
                                                    {
                                                        cymxxh00 = detail.Id,
                                                        cyzdxh00 = orderTable.OrderId,
                                                        cymxdm00 = detail.CyddMxId.ToString(),
                                                        cymxmc00 = detail.CyddMxName,
                                                        cymxdw00 = projectDetails.Unit,
                                                        cymxsl00 = (detail.Num * n.Num).ToString(),
                                                        cymxdybz = false,
                                                        cymxyj00 = c.IpAddress,
                                                        cymxclbz = "0",
                                                        cymxczrq = DateTime.Now,
                                                        cymxzdbz = "0",
                                                        cymxyq00 = detail.DishesStatus.ToString(),
                                                        cymxzf00 = detail.CyddMxName,
                                                        cymxpc00 = string.Empty,
                                                        cymxczy0 = user.UserName,
                                                        cymxfwq0 = c.PcName,
                                                        cymxczdm = user.UserCode,
                                                        cymxje00 = detail.Price.ToString(),
                                                        cymxth00 = orderTable.TableName,
                                                        cymxrs00 = orderTable.PersonNum.ToString(),
                                                        cymxct00 = orderTable.Restaurant,
                                                        cymxzdid = cpdyThGuid,
                                                        cymxbt00 = "催菜单 " + remindNum,
                                                        cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                                        cpdysdxh = cpdyThGuid,
                                                        cymxdk00 = c.StallName,
                                                        cymxgdbz = false,
                                                        cpdyfsl0 = market.Name
                                                    });
                                                });
                                            }
                                        });
                                    }
                                }
                                db.Update<R_OrderDetail>(new { RemindNum = remindNum }, it => it.Id == detail.Id);
                            }  
                        }

                        #region 厨单总单打印
                        if (detailList.Any())
                        {
                            var groupTotalPrint = printerList.Where(p => p.BillType == 2).GroupBy(p => new { p.StallId, p.Id}).ToList();

                            if (groupTotalPrint.Any())
                            {
                                groupTotalPrint.ForEach(c =>
                                {
                                    var printModel = printerList.FirstOrDefault(p => p.Id == c.Key.Id && p.StallId == c.Key.StallId);
                                    cydyInsert.Add(new Cpdy
                                    {
                                        cymxxh00 = orderTableId,
                                        cyzdxh00 = orderTable.OrderId,
                                        cymxdm00 = "0",
                                        cymxmc00 = "催菜总单",
                                        cymxdw00 = user.UserName,
                                        cymxsl00 = string.Empty,
                                        cymxdybz = false,
                                        cymxyj00 = printModel.IpAddress,
                                        cymxclbz = "0",
                                        cymxczrq = DateTime.Now,
                                        cymxzdbz = "1",
                                        //cymxyq00 = detail.CyddMxName,
                                        //cymxzf00 = detail.CyddMxName,
                                        //cymxpc00 = detail.CyddMxName,
                                        cymxczy0 = user.UserName,
                                        cymxfwq0 = printModel.PcName,
                                        cymxczdm = user.UserCode,
                                        cymxje00 = "",
                                        cymxth00 = orderTable.TableName,
                                        cymxrs00 = orderTable.PersonNum.ToString(),
                                        cymxct00 = orderTable.Restaurant,
                                        cymxzdid = cpdyThGuid,
                                        cymxbt00 = "催菜总单",
                                        cymxzwrq = DateTime.Now.ToString("yyyy-MM-dd"),
                                        cpdysdxh = cpdyThGuid,
                                        cymxdk00 = printModel.StallName,
                                        cymxgdbz = false,
                                        cpdyfsl0 = ""
                                    });
                                });
                            }
                        }
                        db.InsertRange<Cpdy>(cydyInsert);   //批量添加菜品打印信息
                        #endregion
                        db.CommitTran();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    db.RollbackTran();
                    throw ex;
                }
                return result;
            }
        }

        public bool GetAutoListPrint()
        {
            return AutoListPrint;
        }

        public bool GetDefaultPromptly()
        {
            return DefaultPromptly;
        }
    }
}