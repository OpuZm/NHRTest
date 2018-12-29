using System;
using System.Collections.Generic;
using OPUPMS.Domain.Base.Dtos;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// <summary>
    /// 枚举转换处理类
    /// </summary>
    public static class EnumToList
    {
        /// <summary>
        /// 将指定的枚举转换成列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<BaseDto> ConvertEnumToList(Type enumType)
        {
            List<BaseDto> list = new List<BaseDto>();
            var enumArray = Enum.GetValues(enumType);
            foreach (int key in enumArray)
            {
                string strName = Enum.GetName(enumType, key);//获取名称
                BaseDto dto = new BaseDto
                {
                    Key = key,
                    Text = Enum.GetName(enumType, key),
                    Sort = key,
                    Value = key.ToString()
                };
                list.Add(dto);
            }
            return list;
        }
    }

    public enum CythStatus
    {
        空置 = 1,
        在用 = 2,
        清理 = 3
    }

    public enum CyddOrderSource
    {
        自来客 = 1,
        营销推广 = 2,
        协议客户 = 3,
        微信 = 4
    }

    public enum CyddStatus
    {
        预定 = 1,
        开台 = 2,
        点餐 = 3,
        送厨 = 4,
        用餐中 = 5,
        结账 = 6,
        取消 = 7,
        订单菜品修改 = 8,
        并台 = 9,
        换桌 = 10,
        拆台 = 11,
        反结 = 12
    }

    public enum CyddCallType
    {
        叫起 = 1,
        即起 = 2
    }

    public enum CyddMxType
    {
        餐饮项目 = 1,
        餐饮套餐 = 2
    }

    public enum CyddMxPayType
    {
        实收 = 1,
        打折 = 2,
        赠送 = 3
    }

    public enum CyddMxStatus
    {
        保存 = 0,
        未出 = 1,
        已出 = 2
    }

    public enum CyddCzjlUserType
    {
        员工 = 1,
        会员 = 2
    }

    public enum CyddPayType
    {
        系统 = 0,
        现金 = 1,
        银行卡 = 2,
        会员卡 = 3,
        挂账 = 4,
        转客房 = 5,
        代金券 = 6,
        免单 = 7,
        微信 = 8,
        支付宝 = 9
    }

    public enum CyddJzStatus
    {
        未付 = 1,
        已付 = 2,
        已退 = 3,
        已结 = 4,
    }

    public enum CyddJzType
    {
        定金 = 1,
        消费 = 2,
        转结 = 3,
        四舍五入 = 4,
        抹零 = 5,
        服务费 = 6,
        折扣 = 7,
        会员折扣 = 8,
        找零 = 9
    }

    public enum CyxmKzType
    {
        做法 = 1,
        要求 = 2,
        配菜 = 3
    }

    public enum CyddMxCzType
    {
        赠菜 = 1,
        退菜 = 2,
        转入 = 3,
        转出 = 4
    }

    public enum CyxmProperty
    {
        是否上架 = 1,
        是否可打折 = 2,
        是否强制打折 = 4,
        是否推荐 = 8,
        是否自定义 = 16,
        是否可赠送 = 32,
        是否可改价 = 64,
        送厨后可否更改数量 = 128,
        是否急推 = 256,
        今日特价 = 512,
        是否收取服务费 = 1024
    }

    public enum CytcProperty
    {
        是否可打折 = 1,
        是否可改价 = 2,
        是否可赠送 = 4
    }

    public enum SearchTypeBy
    {
        订单Id = 1,
        台号Id = 2,
        订单台号id = 3
    }

    public enum DishesStatus
    {
        即起=1,
        叫起=2
    }

    /// <summary>
    /// 折扣操作方式
    /// </summary>
    public enum DiscountMethods
    {
        无折扣 = 0,
        单品折 = 1,
        全单折 = 2,
        方案折 = 3,
        强制折 = 4,
    }

    public enum AuthOperateTypes
    {
        正常操作 = 0,
        折扣授权 = 1,
        抹零授权 = 2,
        反结操作 = 3,
    }

    public enum OrderTableStatus
    {
        所有=0,
        未结=1,
        已结=2
    }

    public enum AutoAdditionType
    {
        按人数=1,
        按台号=2
    }

    public enum Round
    {
        四舍五入=1,
        只舍不入=2,
        只入不舍=3
    }

    public enum CauseType
    {
        赠菜=1,
        退菜=2
    }

    public enum PrintStatus
    {
        正常=1,
        异常=0
    }

    public enum OrderSearchListType
    {
        开单时间 = 1,
        开台时间 = 2
    }

    public enum PageModule
    {
        点餐界面PC端=1,
        点餐界面平板端=2
    }

    public enum PrintType
    {
        微信区域出单 = 1,
        换台区域出单 = 2,
        总单区域出单 = 3
    }
}
