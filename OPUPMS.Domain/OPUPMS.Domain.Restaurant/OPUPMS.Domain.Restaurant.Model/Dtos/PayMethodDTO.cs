using System;

namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    /// <summary>
    /// 创建支付方式实体
    /// </summary>
    public class PayMethodCreateDTO : R_PayMethod
    {

    }

    /// <summary>
    /// 支付列表实体
    /// </summary>
    public class PayMethodListDTO : R_PayMethod
    {
        public string ParentName { get; set; }
    }

    public class PayMethodSearchDTO : BaseSearch
    {
        public int Pid { get; set; }
        public string Name { get; set; }
    }
}
