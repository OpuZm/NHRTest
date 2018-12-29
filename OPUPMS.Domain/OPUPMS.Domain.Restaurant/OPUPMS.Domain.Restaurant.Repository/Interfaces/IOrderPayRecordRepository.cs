using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using System;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IOrderPayRecordRepository
    {
        List<OrderPayDeposit> GetOrderPayDepositList();
        List<DayMarketStatistics> GetDayStallStatistics(List<int> UserIds,DateTime? date);

        /// <summary>
        /// 根据订单Id 获取订单支付记录
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<R_OrderPayRecord> GetPayListByOrderId(int orderId);

        List<OrderPayRecordDTO> GetPaidRecordListByOrderId(int orderId);

        /// <summary>
        /// 根据支付Id 获取指定支付记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderPayRecordDTO GetModelByPayId(int id);

        /// <summary>
        /// 根据订单Id 获取主结账记录信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<OrderMainPayDTO> GetMainPayListByOrderId(int orderId);
    }
}
