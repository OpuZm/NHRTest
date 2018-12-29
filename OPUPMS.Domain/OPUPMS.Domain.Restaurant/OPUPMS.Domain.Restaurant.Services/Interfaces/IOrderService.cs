using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// 取消订单操作
        /// </summary>
        /// <param name="opreateDTO"></param>
        /// <returns></returns>
        bool CancelOrderHandle(CancelOrderOperateDTO operateDTO);

        /// <summary>
        /// 保存预定订单信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tableIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        ReserveCreateDTO SaveReserveOrderHandle(ReserveCreateDTO req, List<int> tableIds, out string msg);


        /// <summary>
        /// 返回预定预测信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        ForecastInfoDTO ForecastSearch(ForecastSearchDTO req);

        /// <summary>
        /// 退定金操作处理
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool RefundDepositHandler(RefundDepositDTO req);
        /// <summary>
        /// 创建订单发票
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool CreateOrderInvoice(InvoiceCreateDTO req);
        InvoiceCreateDTO GetInvoice(int id);
    }
}
