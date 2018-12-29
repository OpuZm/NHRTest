using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class TableCreateDTO
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "您需要填写{0}")]
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "描述信息")]
        [StringLength(4000)]
        public string Description { get; set; }
        public int RestaurantId { get; set; }
        public int SeatNum { get; set; }
        public CythStatus CythStatus { get; set; }
        public Nullable<decimal> ServerRate { get; set; }
        public int RestaurantArea { get; set; }
        public int Box { get; set; }
        public bool IsVirtual { get; set; }
        public int Sorted { get; set; }
    }

    public class TableSearchDTO : BaseSearch
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }
        public int RestaurantId { get; set; }
        public int AreaId { get; set; }
        public CythStatus CythStatus { get; set; }
        public DateTime? ReverDate { get; set; }
        public int Market { get; set; }
        public string TableIds { get; set; }

        public int OrderTableId { get; set; }
        public int Restaurant { get; set; }
        public bool InCludVirtual { get; set; }
    }

    public class TableChoseSearchDTO
    {
        public int RestaurantId { get; set; }
        public int AreaId { get; set; }
        public DateTime ReverDate { get; set; }
        public int Market { get; set; }
        public string TableIds { get; set; }
        public string TableName { get; set; }
        public CythStatus CythStatus { get; set; }
        public int CurrentReservedOrderId { get; set; }
    }

    public class TableListDTO
    {
        public TableListDTO()
        {
            CurrentOrderList = new List<OrderTableDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restaurant { get; set; }
        public int SeatNum { get; set; }
        public decimal? ServerRate { get; set; }
        public string Area { get; set; }
        public string Box { get; set; }
        public int AreaId { get; set; }
        public int RestaurantId { get; set; }

        public bool IsSelected { get; set; }

        public List<OrderTableDTO> CurrentOrderList { get; set; }
        public int Sorted { get; set; }
        public int CythStatus { get; set; }
        public bool IsVirtual { get; set; }
    }

    public class TableListIndexDTO
    {
        public TableListIndexDTO()
        {
            OrderList = new List<BookingTableDTO>();
            OrderNow = new List<OrderTableDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Restaurant { get; set; }
        public int SeatNum { get; set; }
        public decimal? ServerRate { get; set; }
        public int AreaId { get; set; }
        public bool IsVirtual { get; set; }

        public string FirstOrderTime
        {
            get
            {
                return OrderNow.Count > 0 ? OrderNow[0].CreateDate.Value.ToString("HH:mm") : "";
            }
        }

        public string Box { get; set; }
        public List<BookingTableDTO> OrderList { get; set; }
        public CythStatus CythStatus { get; set; }
        public List<OrderTableDTO> OrderNow { get; set; }

        /// <summary>
        /// 获取当前餐台的所有用餐订单总金额
        /// </summary>
        public decimal SumCurrentOrderAmount
        {
            get
            {
                return OrderNow != null ? OrderNow.Sum(x => x.TotalAmount ?? 0) : 0;
            }
        }
        public int Sorted { get; set; }
    }

    public class TableStatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BookingTableDTO
    {
        public int OrderTableId { get; set; }
        public int TableId { get; set; }
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string BookingDate { get; set; }
        public string MarketName { get; set; }
        public int PersonNum { get; set; }
        public string ContactPerson { get; set; }
        public string ContactTel { get; set; }
    }
}
