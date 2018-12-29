using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_PayMethod
    {
        #region
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int? Pid { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateUser { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public int? R_Company_Id { get; set; }
        /// <summary>
        /// 是否系统设置
        /// </summary>
        public bool IsSystem { get; set; }
        #endregion  
    }
}
