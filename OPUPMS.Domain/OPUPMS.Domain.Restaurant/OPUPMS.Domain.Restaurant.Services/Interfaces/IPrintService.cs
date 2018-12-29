using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Services.Interfaces
{
    public interface IPrintService
    {
        /// <summary>
        /// 打印结账单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderTables">订单台号数组</param>
        /// <param name="isLocked">是否进行锁单操作</param>
        /// <returns></returns>
        bool CheckedOut(WholeOrPartialCheckoutDto req,bool isLocked);
        /// <summary>
        /// 打印结账单/预结单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool CheckedOutBill(CheckOutBillDTO req);
        /// <summary>
        /// 订单台号解锁
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool Unlock(CheckOutBillDTO req);
    }
}
