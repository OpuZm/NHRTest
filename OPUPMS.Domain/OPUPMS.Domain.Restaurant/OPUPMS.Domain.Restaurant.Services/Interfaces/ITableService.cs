using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Services.Interfaces
{
    public interface ITableService
    {
        /// <summary>
        /// 并台操作处理
        /// </summary>
        /// <returns></returns>
        bool JoinTableHandle(JoinTableSubmitDTO handleDto);

        /// <summary>
        /// 获取餐台列表
        /// </summary>
        /// <returns></returns>
        List<TableListDTO> GetTableList(TableSearchDTO conditionDto);

        /// <summary>
        /// 开台 拼台操作处理
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tableIds"></param>
        /// <param name="msg"></param>
        /// <param name="reuse">false 开台，true 拼台</param>
        /// <returns></returns>
        OpenTableCreateResultDTO OpenTableHandle(ReserveCreateDTO req, List<int> tableIds, out string msg, bool reuse = false);

        /// <summary>
        /// 换桌操作处理
        /// </summary>
        /// <param name="handleDto"></param>
        /// <returns></returns>
        bool ChangeTableHandle(ChangeTableSubmitDTO handleDto);

        /// <summary>
        /// 拆台操作处理
        /// </summary>
        /// <param name="handleDto"></param>
        /// <returns></returns>
        bool SeparateTableHandle(SeparateTableSubmitDTO handleDto);

        /// <summary>
        /// 根据台号Id（批量或单桌） 更新餐台状态
        /// </summary>
        /// <param name="tableIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateTablesStatus(List<int> tableIds, CythStatus status);

        /// <summary>
        /// 加台操作
        /// </summary>
        /// <param name="handleDto"></param>
        /// <returns></returns>
        List<int> AddTableHandle(AddTableSubmitDTO handleDto);
        /// <summary>
        /// 辙消订单台号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        bool CancelOrderTable(CancelOrderTableSubmitDTO req);
        List<TableListDTO> GetTableListForApi(TableSearchDTO conditionDto);
    }
}
