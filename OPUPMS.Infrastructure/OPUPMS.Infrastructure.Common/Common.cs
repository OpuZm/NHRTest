/********************************************************************************
 * Copyright © 2017 OPUPMS.Framework 版权所有
 * Author: OPUPMS - Justin
 * Description: OPUPMS快速开发平台
 * Website：http://www.opu.com.cn
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace OPUPMS.Infrastructure.Common
{
    /// <summary>
    /// 常用公共类
    /// </summary>
    public class Common
    {
        #region Stopwatch计时器
        /// <summary>
        /// 计时器开始
        /// </summary>
        /// <returns></returns>
        public static Stopwatch TimerStart()
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            return watch;
        }
        /// <summary>
        /// 计时器结束
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            double costtime = watch.ElapsedMilliseconds;
            return costtime.ToString();
        }
        #endregion

        #region 删除数组中的重复项
        /// <summary>
        /// 删除数组中的重复项
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string[] RemoveDup(string[] stringArray)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < stringArray.Length; i++)//遍历数组成员
            {
                if (!list.Contains(stringArray[i]))
                {
                    list.Add(stringArray[i]);
                };
            }
            return list.ToArray();
        }
        #endregion

        #region 自动生成编号
        /// <summary>
        /// 获取一个全局唯一标识符 (GUID)。
        /// </summary>
        /// <returns></returns>
        public static string GuId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 自动生成编号 示例：201705051145409865
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random random = new Random();
            string strRandom = random.Next(1000, 10000).ToString(); //生成编号 
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
            return code;
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int codeNum)
        {
            StringBuilder sb = new StringBuilder(codeNum);
            Random rand = new Random();
            for (int i = 1; i < codeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        #region 删除最后一个字符之后的字符
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strChar)
        {
            return str.Substring(0, str.LastIndexOf(strChar));
        }

        /// <summary>
        /// 删除最后结尾的长度
        /// </summary>
        /// <param name="str">需操作的字符串</param>
        /// <param name="length">需删除的字符串长度</param>
        /// <returns>返回一个新字符串</returns>
        public static string DelLastLength(string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - length);
            return str;
        }
        #endregion

        #region 图片与Base64 编码字符串相互转换

        /// <summary>
        /// 将图片转换成Base64编码字符串
        /// </summary>
        /// <param name="imgFilePathName">图片名称路径</param>
        /// <returns></returns>
        public static string ImgToBase64String(string imgFilePathName)
        {
            string base64String = string.Empty;
            if (string.IsNullOrEmpty(imgFilePathName))
                return base64String;

            Bitmap bmp = new Bitmap(imgFilePathName);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            base64String = Convert.ToBase64String(arr);
            return base64String;
        }

        /// <summary>
        /// 将Base64字符串转换成 Bitmp 图片
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Bitmap Base64StringToImg(string base64String)
        {
            byte[] arrs = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(arrs);
            Bitmap bmp = new Bitmap(ms);
            ms.Close();
            return bmp;
        }
        #endregion
    }
}
