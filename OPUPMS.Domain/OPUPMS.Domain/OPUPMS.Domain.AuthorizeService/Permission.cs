using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.AuthorizeService
{
    public enum Permission
    {
        None = 0,
        餐饮系统设置 = 1,
        商品特殊要求管理 = 2,
        餐饮项目管理 = 4,
        套餐管理 = 8,
        自定义折扣管理 = 16,
        订单管理 = 32,
        预定 = 64,
        开台 = 128,
        预定记录 = 256,
        点餐 = 512,
        换桌 = 1024,
        并台 = 2048,
        拆台 = 4096,
        拼台 = 8192,
        结账 = 16384,
        取消订单 = 32768,
        设为空置 = 65536,
        可打折 = 131072,
        可强制打折 = 262144,
        估清 = 524288,
        删除订单= 1048576,
        查看删除订单=2097152,
        菜品推荐设置 = 4194304,
        夜审 = 8388608,
        赠菜 = 16777216,
        退菜 = 33554432
    }
}
