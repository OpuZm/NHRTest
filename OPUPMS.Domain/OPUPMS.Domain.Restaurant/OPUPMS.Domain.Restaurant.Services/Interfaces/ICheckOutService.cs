using OPUPMS.Domain.Restaurant.Model.Dtos;
using System.Collections.Generic;
using SqlSugar;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Base.Dtos;

namespace OPUPMS.Domain.Restaurant.Services.Interfaces
{
    public interface ICheckOutService
    {
        /// <summary>
        /// 根据订单台号id集合查询订单明细
        /// </summary>
        /// <param name="tableList">订单台号id集合</param>
        /// <returns>订单明细集合</returns>
        List<CheckOutOrderDetailDTO> GetOrderDetailByOrderTableId(List<int> tableList);


        /// <summary>
        /// 整单全部结账或部分结账
        /// </summary>
        CheckOutResultDTO WholeOrPartialCheckout(WholeOrPartialCheckoutDto req, CyddCzjlUserType userType = CyddCzjlUserType.员工);

        /// <summary>
        /// 订单反结处理
        /// </summary>
        /// <param name="req"></param>
        void ReCheckout(WholeOrPartialCheckoutDto req);

        /// <summary>
        /// 根据订单ID和该订单下的台号ID集合获取CheckOutOrderDTO
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tableIdList"></param>
        /// <returns></returns>
        CheckOutOrderDTO GetCheckOutOrderDTO(int orderId, List<int> tableIdLis,OrderTableStatus oStatus);
        void VerifyOrderInfo(WholeOrPartialCheckoutDto requestDto);
        void VerifyAndCalcDetailInfo(WholeOrPartialCheckoutDto requestDto, CheckOutOrderDTO verifyObj);

        /// <summary>
        /// 获取订单支付记录信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<OrderPaymentDTO> GetOrderPaymentInfo(int orderId);

        /// <summary>
        /// 验证转客房或挂账 信息是否有效
        /// </summary>
        /// <param name="verifySource"></param>
        /// <returns></returns>
        List<string> VerifyOutsideInfo(VerifySourceInfoDTO verifySource);

        List<string> VerifyOutsideInfo(VerifySourceInfoDTO verifySource, SqlSugarClient db);

        /// <summary>
        /// 结账时判断用户挂账总金额是否小于限额
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool CheckoutVaild(List<OrderPayRecordDTO> req,int restaurant);

        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        List<MemberInfoDTO> SearchMemberBy(string text);
        List<SearchKrzlInfo> SearchRoomBy(string text);
        /// <summary>
        /// 反结账单
        /// </summary>
        /// <param name="mainPayId"></param>
        /// <returns></returns>
        CheckOutResultDTO ReverseOrder(ReverseOrderDTO req);
        bool BeforeWholeOrPartialCheckout(WholeOrPartialCheckoutDto req);

        CheckOutOrderDTO GetWeixinPayDTO(CheckOutOrderDTO model);
        List<ProjectCheckOutStaticsDTO> GetCheckOutStatics(List<CheckOutOrderTableDTO> req);
    }
}
