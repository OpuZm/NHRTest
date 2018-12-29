using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Infrastructure.Common
{

    public static class EnumHelper
    {
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName<T>(this int status)
        {
            return Enum.GetName(typeof(T), status);
        }
        /// <summary>
        /// 获取枚举名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetNamesArr<T>()
        {
            return Enum.GetNames(typeof(T));
        }
        /// <summary>
        /// 将枚举转换成字典集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<string, EnumInfoContent> GetEnumDic<T>()
        {

            Dictionary<string, EnumInfoContent> resultList = new Dictionary<string, EnumInfoContent>();
            Type type = typeof(T);
            var strList = GetNamesArr<T>().ToList();
            foreach (string key in strList)
            {
                //枚举成员值
                string val = Enum.Format(type, Enum.Parse(type, key), "d");

                //枚举成员Remark属性值
                T _enum = (T)Enum.Parse(typeof(T), key);
                FieldInfo fd = type.GetField(_enum.ToString());
                object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string DisplayValue = string.Empty;
                foreach (DescriptionAttribute attr in attrs)
                {
                    DisplayValue = attr.Value;
                }

                resultList.Add(key, new EnumInfoContent
                {
                    Value = int.Parse(val),
                    Key = key,
                    DisplayValue = DisplayValue
                }
                                  );
            }
            return resultList;
        }


        /// <summary>
        /// Enum类型信息的容器
        /// </summary>
        public class EnumInfoContent
        {
            /// <summary>
            /// 枚举成员名称
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// 枚举成员的整数值
            /// </summary>
            public int Value { get; set; }

            /// <summary>
            /// 枚举成员的页面显示名称
            /// </summary>
            public String DisplayValue { get; set; }
        }//class


    }
}
