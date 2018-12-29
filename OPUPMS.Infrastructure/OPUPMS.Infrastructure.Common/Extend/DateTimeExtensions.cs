using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPUPMS.Infrastructure.Common
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 由日期生成随机名称
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string GetRandomString(this DateTime _this)
        {
            return _this.ToString("yyyyMMddHHmmssffffff") + new Random(DateTime.Now.Millisecond).Next().ToString();
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Int64 GetTimestamp(this DateTime _this)
        {
            DateTime zero = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan ts = _this - zero;
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
