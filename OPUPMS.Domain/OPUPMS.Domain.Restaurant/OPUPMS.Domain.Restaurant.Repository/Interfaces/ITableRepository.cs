using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface ITableRepository
    {
        bool Create(TableCreateDTO req);

        bool Update(TableCreateDTO req);

        TableCreateDTO GetModel(int id);

        List<TableListDTO> GetList(out int total, TableSearchDTO req);
        List<TableListIndexDTO> GetList(TableSearchDTO req);

        List<TableStatusDTO> GetStatus();
        List<TableListDTO> GetReseverChoseList(TableChoseSearchDTO req);
        List<TableListDTO> GetOpenTableChoseList(TableSearchDTO req);

        /// <summary>
        /// 根据相关条件获取餐台信息
        /// </summary>
        /// <returns>返回餐台列表</returns>
        List<R_Table> GetTables(int[] restraurantIds, int? areaId, CythStatus statusType, bool inCludVirtual = true);
        /// <summary>
        /// 根据订单ID查询下面的台号信息
        /// </summary>
        /// <returns>返回餐台列表</returns>
        List<R_Table> GetTables( int orderId);

        /// <summary>
        /// 根据Id 获取餐台信息
        /// </summary>
        /// <returns>返回餐台</returns>
        R_Table GetTable(int tableId);

        /// <summary>
        /// 根据台号id集合获取台号列表
        /// </summary>
        /// <param name="tableIds"></param>
        /// <returns></returns>
        List<TableListDTO> GetlistByIds(List<int> tableIds);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id, out string msg);
        List<R_Table> GetTables(int[] restraurantIds, int? areaId);
    }
}
