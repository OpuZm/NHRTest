using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_AutoAddition
    {
        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 折扣名称
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// 餐饮分市
        ///</summary>
        public int R_Market_Id { get; set; }


        ///<summary>
        /// 餐厅
        ///</summary>
        public int R_Restaurant_Id { get; set; }


        ///<summary>
        /// 餐厅区域
        ///</summary>
        public int R_Area_Id { get; set; }
        ///<summary>
        /// 是否启用
        ///</summary>
        public bool IsEnable { get; set; }
        public int R_Company_Id { get; set; }
        public bool IsDelete { get; set; }
    }
}
