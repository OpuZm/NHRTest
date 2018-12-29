using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using System.Collections.Generic;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface ICheckOutRepository
    {
        /// <summary>
        /// 根据订单详细id修改菜品单价
        /// </summary>
        /// <param name="orderDetailId">订单详细id</param>
        /// <param name="newPrice">新单价</param>
        /// <returns>是否修改成功</returns>
        bool UpdatePriceById(int orderDetailId, decimal newPrice);

        /// <summary>
        /// 根据订单id获得订单台号列表
        /// </summary>
        /// <param name="orderNo">订单id</param>
        /// <returns></returns>
        List<CheckOutOrderTableDTO> GetOrderTableListBy(int orderId);

        /// <summary>
        /// 根据订单id和桌号id集合查询订单台号集合
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="tableIdList">桌号id集合</param>
        /// <returns>订单台号集合</returns>
        List<CheckOutOrderTableDTO> GetOrderTableListBy(int orderId, List<int> tableIdList, OrderTableStatus oStatus);

        /// <summary>
        /// 根据订单id获得订单实体
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>订单实体类</returns>
        CheckOutOrderDTO GetOrderById(int orderId);

        /// <summary>
        /// 根据订单台号id集合获得订单明细集合
        /// </summary>
        /// <param name="tableOrderIds">订单台号id集合</param>
        /// <returns>订单明细集合</returns>
        List<CheckOutOrderDetailDTO> GetOrderDetailListBy(List<int> tableOrderIds);

        /// <summary>
        /// 根据订单明细id集合获取拓展集合
        /// </summary>
        /// <param name="orderDetailIdList">订单明细id集合</param>
        /// <returns>拓展集合</returns>
        List<OrderDetailExtendDTO> GetExtendListBy(List<int> orderDetailIdList);

        /// <summary>
        /// 根据订单明细id集合获取操作记录集合
        /// </summary>
        /// <param name="orderDetailIdList">订单明细id集合</param>
        /// <returns>操作记录集合</returns>
        List<R_OrderDetailRecord> GetRecordListBy(List<int> orderDetailIdList);

        /// <summary>
        /// 根据订单明细id集合获取订单套餐明细集合
        /// </summary>
        /// <param name="orderDetailIdList">订单明细id集合</param>
        /// <returns>订单套餐明细集合</returns>
        List<R_OrderDetailPackageDetail> GetPackageDetailListBy(List<int> orderDetailIdList);

        /// <summary>
        /// 根据餐饮项目id集合获取餐饮项目集合
        /// </summary>
        /// <param name="projectIdList">餐饮项目id集合</param>
        /// <returns>餐饮项目集合</returns>
        List<ProjectJoinDetailDTO> GetProjectListBy(List<int> projectIdList);

        /// <summary>
        /// 根据餐饮套餐id集合获取餐饮套餐集合
        /// </summary>
        /// <param name="packageIdList">餐饮套餐id集合</param>
        /// <returns>餐饮套餐集合</returns>
        List<R_Package> GetPackageListBy(List<int> packageIdList);
        /// <summary>
        /// 反结账单并重置账单相关数据
        /// </summary>
        /// <param name="mainPayId"></param>
        /// <returns></returns>
        CheckOutResultDTO ReverseOrder(ReverseOrderDTO req);
        OrderMainPayDTO GetPreOrderMainPay(int orderId);
    }
}
