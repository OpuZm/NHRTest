/***********************************************************************
 * Module:  CyddMxXmKz.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyddMxXmKz
 ***********************************************************************/
 
// <summary>
// 餐饮订单明细项目扩展
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
/// 餐饮订单明细项目扩展
public class R_OrderDetailExtend
    {
   
   
   ///<summary>
   ///</summary>
   public int Id {get;set;}
   
   
   ///<summary>
   /// 餐饮订单明细
   ///</summary>
   public int R_OrderDetail_Id { get;set;}
   
   
   ///<summary>
   /// 餐饮项目扩展
   ///</summary>
   public int R_ProjectExtend_Id { get;set;}
   public decimal Price { get; set; }
   public string Name { get; set; }
   public string Unit { get; set; }
}
}