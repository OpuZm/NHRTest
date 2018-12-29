using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    /// <summary>
    /// 出品统计
    /// </summary>
    public class ProducedStatisticsDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Num { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
    }

    public class ProducedSearchDTO
    {
        public int ParentCategoryId { get; set; }
        public int CategoryId { get; set; }
        public ProducedType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RestaurantId { get; set; }
    }
    
    public enum ProducedType
    {
        菜品大类=1,
        菜品分类=2,
        菜品信息=3,
        套餐=4
    }

    public class TurnDutySearchDTO
    {
        public int RestaurantId { get; set; }
        public DateTime? StartDate { get; set; }
    }

    public class TurnDutyStatisticsGroupDto
    {
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<TurnDutyStatisticsDTO> List { get; set; }
    }

    public class TurnDutyStatisticsDTO
    {
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public int CyddJzType { get; set; }
    }

    public class ReportListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
