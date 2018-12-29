/********************************************************************************
 * Copyright © 2017 OPUPMS.Framework 版权所有
 * Author: OPUPMS - Justin
 * Description: OPUPMS快速开发平台
 * Website：http://www.opu.com.cn
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OPUPMS.Infrastructure.Common
{
    public static class ConvertExtend        
    {
        #region 数值转换
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt(this object data)
        {
            if (data == null)
                return 0;
            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为可空整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int? ToIntOrNull(this object data)
        {
            if (data == null)
                return null;
            int result;
            bool isValid = int.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble(this object data)
        {
            if (data == null)
                return 0;
            double result;
            return double.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static double ToDouble(this object data, int digits)
        {
            return Math.Round(ToDouble(data), digits);
        }

        /// <summary>
        /// 转换为可空双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double? ToDoubleOrNull(this object data)
        {
            if (data == null)
                return null;
            double result;
            bool isValid = double.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal ToDecimal(this object data)
        {
            if (data == null)
                return 0;
            decimal result;
            return decimal.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal ToDecimal(this object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits);
        }

        /// <summary>
        /// 转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal? ToDecimalOrNull(this object data)
        {
            if (data == null)
                return null;
            decimal result;
            bool isValid = decimal.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为可空高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal? ToDecimalOrNull(this object data, int digits)
        {
            var result = ToDecimalOrNull(data);
            if (result == null)
                return null;
            return Math.Round(result.Value, digits);
        }

        /// <summary>
        /// 指定数组转换为列表
        /// </summary>
        /// <typeparam name="T">指定数组的数据类型</typeparam>
        /// <param name="array">指定的数组</param>
        /// <returns>返回列表</returns>
        public static List<T> ToList<T>(this T[] array)
        {
            return new List<T>(array); 
        }

        #endregion

        #region 日期转换
        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime ToDate(this object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime? ToDateOrNull(this object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region 布尔转换
        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool ToBool(this object data)
        {
            if (data == null)
                return false;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        private static bool? GetBool(this object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 转换为可空布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool? ToBoolOrNull(this object data)
        {
            if (data == null)
                return null;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            bool isValid = bool.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion


        #region 字符串转换
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="obj">对象</param>
        public static string ToString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString().Trim();
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">当对象为null 时，指定的返回默认值</param>
        /// <returns></returns>
        public static string ToString(this object obj, string defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return obj.ToString();
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToString(this byte[] data)
        {
            string codeString = Encoding.Default.GetString(data);
            return codeString;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string SubString(this string input, int length)
        {
            return input.SubString(length, string.Empty);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">截取长度</param>
        /// <param name="ellipsis">省略符号</param>
        /// <returns></returns>
        public static string SubString(this string input, int length, string ellipsis)
        {
            if (((length >= 1) && !input.IsEmpty()) && (input.Length > length))
            {
                return (input.Substring(0, length) + ellipsis);
            }
            return input;
        }

        /// <summary>
        /// 将指定字符串拆分为字符串数组 
        /// </summary>
        /// <param name="input">指定源字符串</param>
        /// <returns>返回字符串数组</returns>
        public static string[] ToArray(this string input)
        {
            return input.ToArray(",");
        }

        /// <summary>
        /// 将指定字符串拆分为字符串数组 
        /// </summary>
        /// <param name="input">指定源字符串</param>
        /// <param name="separator">指定分隔符号</param>
        /// <returns>返回字符串数组</returns>
        public static string[] ToArray(this string input, string separator)
        {
            return input.ToArray(new string[] { separator });
        }

        /// <summary>
        /// 将指定字符串拆分为字符串数组
        /// </summary>
        /// <param name="input">指定源字符串</param>
        /// <param name="arraySeparaor">指定多个分隔符号数组</param>
        /// <returns>返回字符串数组</returns>
        public static string[] ToArray(this string input, string[] arraySeparaor)
        {
            if (input.IsEmpty())
            {
                return null;
            }
            return input.Split(arraySeparaor, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 将一个包含ASCII编码字符的Byte数组转化为一个完整的String
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToASCIICodeString(this byte[] data)
        {
            ASCIIEncoding enCoding = new ASCIIEncoding();
            string codeString = enCoding.GetString(data);
            return codeString;
        }

        /// <summary>
        /// 将一个包含Unicode编码字符的Byte数组转化为一个完整的String
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToUnicodeString(this byte[] data)
        {
            UnicodeEncoding enCoding = new UnicodeEncoding();
            string codeString = enCoding.GetString(data);
            return codeString;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] data)
        {
            string codeString = string.Empty;
            if(data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    codeString += data[i].ToString("X2");
                }
            }
            return codeString;
        }

        /// <summary>
        /// 连接指定集合中的所有元素
        /// </summary>
        /// <typeparam name="T">指定集合中元素的数据类型</typeparam>
        /// <param name="list">指定要连接的集合对象</param>
        /// <returns>返回List元素连接后的字符串</returns>
        public static string Join<T>(this IList<T> list)
        {
            return list.Join<T>(",");
        }

        /// <summary>
        /// 连接指定数组中的所有元素
        /// </summary>
        /// <typeparam name="T">指定数组的数据类型</typeparam>
        /// <param name="array">指定要连接的数组</param>
        /// <returns>返回数组元素连接后的字符串</returns>
        public static string Join<T>(this T[] array)
        {
            return array.Join<T>(",");
        }

        /// <summary>
        /// 连接指定集合中的所有元素
        /// </summary>
        /// <typeparam name="T">指定集合中元素的数据类型</typeparam>
        /// <param name="list">指定要连接的集合对象</param>
        /// <param name="separator">连接分隔符</param>
        /// <returns>返回List元素连接后的字符串</returns>
        public static string Join<T>(this IList<T> list, string separator)
        {
            if (list.IsEmpty<T>())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }
                builder.Append(list[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 连接指定数组中的所有元素
        /// </summary>
        /// <typeparam name="T">指定数组的数据类型</typeparam>
        /// <param name="array">指定要连接的数组</param>
        /// <param name="separator">连接分隔符</param>
        /// <returns>返回数组元素连接后的字符串</returns>
        public static string Join<T>(this T[] array, string separator)
        {
            if (array.IsEmpty<T>())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            int index = 0;
            int length = array.Length;
            while (index < length)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }
                builder.Append(array[index]);
                index++;
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取中文字符串拼音
        /// </summary>
        /// <param name="text">中文字符串</param>
        /// <returns>返回拼音首字母</returns>
        public static string GetChineseSpell(string text)
        {
            if (text.IsEmpty())
            {
                return string.Empty;
            }
            int length = text.Length;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(GetSingleSpell(text.Substring(i, 1)));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取指定字符串的拼音首字母
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>返回拼音首字母</returns>
        public static string GetFirstSpell(string text)
        {
            if (!text.IsEmpty())
            {
                foreach (char ch in text.ToCharArray())
                {
                    string singleSpell = GetSingleSpell(ch.ToString());
                    if (ValidateExtend.RegexMatch(singleSpell, "[a-zA-Z]+"))
                    {
                        return singleSpell.ToUpper();
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取指定内容文本中的图片列表
        /// </summary>
        /// <param name="content">指定内容文本</param>
        /// <returns>返回图片地址列表</returns>
        public static List<string> GetImageUrls(string content)
        {
            List<string> list = new List<string>();
            if (!content.IsEmpty())
            {
                Regex regex = new Regex("<img\\b[^<>]*?\\bsrc[\\s\\t\\r\\n]*=[\\s\\t\\r\\n]*[\"']?[\\s\\t\\r\\n]*(?<ImageUrl>[^\\s\\t\\r\\n\"'<>]*)[^<>]*?/?[\\s\\t\\r\\n]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match match in regex.Matches(content))
                {
                    string text1 = match.Value;
                    string input = match.Groups["ImageUrl"].Value;
                    if (ValidateExtend.IsImage(input))
                    {
                        list.Add(input);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取一个指定长度的随机字符串
        /// </summary>
        /// <param name="len">要获取的字符串长度</param>
        /// <returns>返回一个随机字符串</returns>
        public static string GetRandomString(int len)
        {
            return GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", len);
        }

        /// <summary>
        /// 获取一个指定长度的随机字符串
        /// </summary>
        /// <param name="words">指定的字符串</param>
        /// <param name="len">要获取的字符串长度</param>
        /// <returns>返回一个随机字符串</returns>
        public static string GetRandomString(string words, int len)
        {
            string str = string.Empty;
            for (int i = 0; i < len; i++)
            {
                using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
                {
                    byte[] data = new byte[4];
                    provider.GetBytes(data);
                    int num2 = new Random(BitConverter.ToInt32(data, 0)).Next(0, words.Length);
                    str = str + words[num2];
                }
            }
            return str;
        }

        /// <summary>
        /// 获取单个字的拼音
        /// </summary>
        /// <param name="singleText">单个字符</param>
        /// <returns>返回对应的拼音</returns>
        public static string GetSingleSpell(string singleText)
        {
            byte[] bytes = Encoding.Default.GetBytes(singleText);
            if (bytes.Length <= 1)
            {
                return singleText;
            }
            int num = bytes[0];
            int num2 = bytes[1];
            int num3 = (num << 8) + num2;
            int[] numArray = new int[] {
            0xb0a1, 0xb0c5, 0xb2c1, 0xb4ee, 0xb6ea, 0xb7a2, 0xb8c1, 0xb9fe, 0xbbf7, 0xbbf7, 0xbfa6, 0xc0ac, 0xc2e8, 0xc4c3, 0xc5b6, 0xc5be,
            0xc6da, 0xc8bb, 0xc8f6, 0xcbfa, 0xcdda, 0xcdda, 0xcdda, 0xcef4, 0xd1b9, 0xd4d1
         };
            for (int i = 0; i < 0x1a; i++)
            {
                int num5 = 0xd7fa;
                if (i != 0x19)
                {
                    num5 = numArray[i + 1];
                }
                if ((numArray[i] <= num3) && (num3 < num5))
                {
                    return Encoding.Default.GetString(new byte[] { (byte)(0x41 + i) });
                }
            }
            return "_";
        }

        #endregion


        #region 数据非空检查

        /// <summary>
        /// 安全返回值
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid? value)
        {
            if (value == null)
                return true;
            return IsEmpty(value.Value);
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid value)
        {
            if (value == Guid.Empty)
                return true;
            return false;
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        } 

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this byte[] value)
        {
            if(value != null && value.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IList<T> value)
        {
            if (value != null)
            {
                return (value.Count < 1);
            }
            return true;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this T[] value)
        {
            if (value != null)
            {
                return (value.Length < 1);
            }
            return true;
        }

        #endregion


        #region Attribute特性处理

        /// <summary>
        /// 获得枚举字段的特性(Attribute)
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="value">一个枚举的实例对象</param>
        /// <returns></returns>
        private static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            Type attributeType = typeof(T);
            if (Attribute.IsDefined(field, attributeType))
            {
                return (Attribute.GetCustomAttribute(field, attributeType) as T);
            }
            return default(T);
        }

        /// <summary>
        /// 获得特定类字段的特性(Attribute)
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="value">一个枚举的实例对象</param>
        /// <returns></returns>
        private static T GetAttribute<T>(this Type type, object name) where T : Attribute
        {
            if (type != null)
            {
                FieldInfo field = type.GetField(name.ToString());
                Type attributeType = typeof(T);
                if (Attribute.IsDefined(field, attributeType))
                {
                    return (Attribute.GetCustomAttribute(field, attributeType) as T);
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取指定对象的描述信息
        /// </summary>
        /// <param name="value">指定枚举</param>
        /// <returns></returns>
        public static string GetDescription(this object value, Type t, string name)
        {
            return t.GetAttribute<DescriptionAttribute>(name).Description;
        }

        /// <summary>
        /// 获取指定对象的HTML描述信息
        /// </summary>
        /// <param name="value">指定枚举</param>
        /// <returns></returns>
        public static string GetHtmlDescription(this object value, Type t, string name)
        {
            return t.GetAttribute<DescriptionAttribute>(name).HtmlDescription;
        }

        /// <summary>
        /// 获取指定枚举对应的指定特性值
        /// </summary>
        /// <param name="value">指定枚举</param>
        /// <returns></returns>
        public static string GetAssignValue(this Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>().AssignValue;
        }

        /// <summary>
        /// 获取指定枚举的描述信息
        /// </summary>
        /// <param name="value">指定枚举</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>().Description;
        }

        /// <summary>
        /// 获取指定枚举的HTML描述信息
        /// </summary>
        /// <param name="value">指定枚举</param>
        /// <returns></returns>
        public static string GetHtmlDescription(this Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>().HtmlDescription;
        }

        #endregion


        /// <summary>
        /// 获取指定类型Type的程序集完整名称（包含：类名, 程序集名），如：OPUPMS.Core, OPUPMS 
        /// </summary>
        /// <param name="type">指定类型 Type</param>
        /// <returns></returns>
        public static string GetAssemblyFullName(this Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }
            string[] strArray = type.Assembly.FullName.ToArray(",");
            return string.Format("{0}, {1}", type.FullName, strArray[0]);
        }
        
        /// <summary>
        /// 获取指定值在数值中的索引位置
        /// </summary>
        /// <typeparam name="T">指定数值的数据类型</typeparam>
        /// <param name="array">指定数值对象</param>
        /// <param name="value">指定要查找的值</param>
        /// <returns>返回该值在数值中的索引位置，如果不存在，则返回 -1</returns>
        public static int IndexOf<T>(this T[] array, T value)
        {
            if (!array.IsEmpty<T>())
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Equals(value))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 返回指定IP值所对应的IP地址
        /// </summary>
        /// <param name="ipValue">IP值</param>
        /// <returns>返回Ip地址</returns>
        public static string IpDecode(long ipValue)
        {
            string[] strArray = new string[] { ((ipValue >> 0x18) & 0xffL).ToString(), ".", ((ipValue >> 0x10) & 0xffL).ToString(), ".", ((ipValue >> 8) & 0xffL).ToString(), ".", (ipValue & 0xffL).ToString() };
            return string.Concat(strArray);
        }

        /// <summary>
        /// 返回指定IP地址所对应的IP值
        /// </summary>
        /// <param name="ipAddress">Ip地址</param>
        /// <returns>返回Ip值</returns>
        public static double IpEncode(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return 0.0;
            }
            string[] strArray = ipAddress.Split(new char[] { '.' });
            long num = 0L;
            foreach (string str in strArray)
            {
                byte num2;
                if (byte.TryParse(str, out num2))
                {
                    num = (num << 8) | num2;
                }
                else
                {
                    return 0.0;
                }
            }
            return (double)num;
        }

    }
}
