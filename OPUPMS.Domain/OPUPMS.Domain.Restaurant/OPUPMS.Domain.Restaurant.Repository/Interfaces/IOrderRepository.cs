using System;
using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Base.ConvertModels;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IOrderRepository
    {
        List<OrderDetailDTO> GetOrderTableProjects(int orderTableId);

        /// <summary>
        /// 订单明细创建提交
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="orderTableIds">订单台号ID列表</param>
        /// <param name="status">0保存 1落单不打厨 2落单打厨</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="message">返回信息</param>
        bool OrderDetailCreate(List<OrderDetailDTO> req, List<int> orderTableIds, CyddMxStatus status, OperatorModel userInfo, out string message,CyddCzjlUserType userType=CyddCzjlUserType.员工, bool isListPrint = false);
        TableListDTO GetTableByOrderTableId(int orderTableId);
        List<ReserveCreateDTO> GetReserveOrdersByTable(int tableId, Nullable<DateTime> minDate);
        ReserveCreateDTO GetOrderModel(int orderId);
        List<OrderPayHistoryListDTO> GetOrderPayList(int orderId, CyddJzType cyddJzType);
        bool OrderDepositCreate(OrderPayHistoryDTO req);

        /// <summary>
        /// 根据订单类型获取订单列表
        /// </summary>
        /// <returns></returns>
        List<ReserveCreateDTO> GetListByOrderType(int orderType);

        /// <summary>
        /// 根据客源获取订单列表
        /// </summary>
        /// <returns></returns>
        List<ReserveCreateDTO> GetListByCustomerSource(int customerSource);

        List<ReserveCreateDTO> GetOrderList(out int total, OrderListSearchDTO req);

        /// <summary>
        /// 根据台号及订单状态 获取订单台号列表
        /// </summary>
        /// <param name="tableIds"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        List<TableLinkOrderDTO> GetOrderTableListBy(int[] tableIds, int[] orderStatus);

        /// <summary>
        /// 根据Id 获取订单台号列表
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sreachType">1/订单Id  2/台号Id</param>
        /// <returns></returns>
        List<R_OrderTable> GetOrderTableListBy(int id, SearchTypeBy searchType);

        /// <summary>
        /// 根据订单号获得实体
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        ReserveCreateDTO GetOrder(int id);

        /// <summary>
        /// 菜品转台
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="orderTableId">转入哪个订单台号</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        bool ChangeProjectToTable(List<OrderDetailDTO> req, int orderTableId, OperatorModel userInfo, out string msg);

        /// <summary>
        /// 修改订单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool UpdateOrderInfo(ReserveCreateDTO req);

        R_Order GetR_OrderById(int id);

        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="listOrderDetailID">订单明细的ID列表</param>
        /// <returns></returns>
        List<OrderDetailDTO> GetOrderDetails(List<int> listOrderDetailID);

        /// <summary>
        /// 修改订单明细的折扣
        /// </summary>
        /// <param name="listOrderDetailID">订单明细的ID列表</param>
        /// <returns></returns>
        bool UpdateOrderDetailDiscounts(List<SingleProductDiscountSetRequestDto> list);

        /// <summary>
        /// 获取订单基本信息
        /// </summary>
        /// <param name="orderId"></param>
        ReserveCreateDTO GetOrderDTO(int orderId);

        /// <summary>
        /// 获取订单基本
        /// </summary>
        /// <param name="orderId"></param>
        void Update(ReserveCreateDTO orderDto);

        /// <summary>
        /// 多桌点餐
        /// </summary>
        /// <param name="req">订单明细列表</param>
        /// <param name="tables">订单台号信息列表</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        bool ChoseProjectToTable(List<OrderDetailDTO> req, List<OrderTableDTO> tables, OperatorModel userInfo, out string msg);

        /// <summary>
        /// 根据订单台号id获取订单下所有台号列表
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        List<OrderTableDTO> GetTablesByOrderTableId(int orderTableId);

        List<ForecastReserveInfoDTO> GetForecastList(ForecastSearchDTO req);

        /// <summary>
        /// 根据订单台号id获取订单和台号信息
        /// </summary>
        /// <param name="orderTableId"></param>
        /// <returns></returns>
        OrderAndTablesDTO GetOrderAndTablesByOrderTableId(int orderTableId);

        /// <summary>
        /// 退菜操作
        /// </summary>
        /// <param name="req">订单明细信息</param>
        /// <param name="table">订单台号信息</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        OrderDetailRecordDTO ReturnOrderDetail(OrderDetailDTO req, TableListDTO table, OperatorModel userInfo, out string msg);

        /// <summary>
        /// 打厨单
        /// </summary>
        /// <param name="req">保存状态的订单明细列表</param>
        /// <param name="table">订单台号信息</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        bool CookingMenu(List<OrderDetailDTO> req, TableListDTO table, OperatorModel userInfo, out string msg);

        /// <summary>
        /// 订单明细记录赠送
        /// </summary>
        /// <param name="req">订单明细赠送记录</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="msg">返回信息</param>
        R_OrderDetailRecordDTO CreateOrderDetailRecord(R_OrderDetailRecordDTO req, OperatorModel userInfo, out string msg);

        /// <summary>
        /// 判断结账时当前是否还存在保存状态的数据
        /// </summary>
        /// <param name="orderTableInfos">订单台号信息列表</param>
        /// <param name="LoginMarketId">当前登录分市id</param>
        /// <param name="ItemValue">账务日期</param>
        int JudgeOrderPay(List<OrderTableDTO> orderTableInfos,int loginMarketId, string itemValue, out string msg);

        /// <summary>
        /// 查单界面列表统计
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        SearchOrderStatisticalDTO GetOrderStatisticalList(OrderListSearchDTO req);
        /// <summary>
        /// 删除(恢复订单)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool OrderDelete(OrderDeleteDTO req);
        List<SystemCodeInfo> GetDepartList();
        /// <summary>
        /// 创建订单发票
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool CreateOrderInvoice(InvoiceCreateDTO req);
        /// <summary>
        /// 根据订单ID获取发票列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<InvoiceListDTO> GetInvoiceList(int orderId);
        InvoiceCreateDTO GetInvoice(int id);
        List<OrderTableDTO> GetTablesByOrderTableIds(List<int> orderTableId);
        bool UpdateOrderTablePerson(OrderTableDTO req);
        bool UpdateOrderTableIsControl(List<int> ordertableIds, bool isControl);
        bool DeleteInvoice(int id);
        bool DeleteOrderDetailRecord(List<R_OrderDetailRecordDTO> req, OperatorModel user);
        bool UpdateOrderTableListPrint(int orderTableId);
        bool WeixinPrint(List<OrderDetailDTO> req, List<int> orderTableIds, CyddMxStatus status, OperatorModel userInfo, CyddCzjlUserType userType = CyddCzjlUserType.员工);
        int GetOrderCountBeforeNightTrial();
        bool NightTrial(int companyId, string userCode);
        bool FlatOrderSubmit(ReserveCreateDTO req, List<int> tableIds, List<OrderDetailDTO> list, OperatorModel user, CyddMxStatus status, bool isListPrint = false);

        bool RemindOrder(int orderTableId, List<int> detailIds, OperatorModel user);
        bool GetAutoListPrint();

        bool GetDefaultPromptly();
    }
}
