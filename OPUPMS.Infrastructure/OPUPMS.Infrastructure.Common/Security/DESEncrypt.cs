/********************************************************************************
 * Copyright © 2017 OPUPMS.Framework 版权所有
 * Author: OPUPMS - Justin
 * Description: OPUPMS快速开发平台
 * Website：http://www.opu.com.cn
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OPUPMS.Infrastructure.Common.Security
{
    /// <summary>
    /// DES加密、解密帮助类
    /// </summary>
    public class DESEncrypt
    {
        private static string DESKey = "opupms_desencrypt_2017";//"nfine_desencrypt_2016";

        #region ========加密========
        public static string GetMD5(string encypStr)
        {
            if (String.IsNullOrEmpty(encypStr))
            {
                return ""; //空密码
            }
            //byte[] bitData = Encoding.Default.GetBytes(encypStr);    //tbPass为输入密码的文本框
            byte[] bitData = Encoding.GetEncoding("GB2312").GetBytes(encypStr);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(bitData);
            string result = BitConverter.ToString(output).Replace("-", "");

            return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, DESKey);
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// MD5 按位数加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string MD5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }

            return strEncrypt;
        }

        /// <summary>
        /// 将字符串加密成Base64 编码字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string EncryptToBase64(string inputString)
        {
            string codeString = string.Empty;
            byte[] arrCodes = Encoding.Default.GetBytes(inputString);
            codeString = Convert.ToBase64String(arrCodes);
            return codeString;
        }

        /// <summary>
        /// 16进制加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string Rc4PassHex(string inputString)
        {
            byte[] xKey, IKey;
            byte t, j;
            byte nI, nJ, nK;
            int len;
            string Ks, rst;
            byte[] KsArray;
            byte[] InsArray;

            rst = "";
            try
            {
                xKey = new byte[256];
                IKey = new byte[256];
                //Ks = UnicodeToAscii("OPU酒店管理系统");
                KsArray = System.Text.Encoding.Default.GetBytes("OPU酒店管理系统");
                InsArray = System.Text.Encoding.Default.GetBytes(inputString);
                len = inputString.Length;
                if (len < 1 || len > 256)
                {
                    return "";
                }
                len = KsArray.Length;
                for (int i = 0; i < 256; i++)
                {
                    IKey[i] = (byte)i;
                    xKey[i] = (byte)KsArray[(i % len)];
                }
                j = 0;
                for (int i = 0; i < 256; i++)
                {
                    j = (byte)(j + IKey[i] + xKey[i] & 0xFF);
                    t = IKey[i];
                    IKey[i] = IKey[j];
                    IKey[j] = t;
                }

                nI = 0; nJ = 0; nK = 0;
                len = InsArray.Length;

                for (int k = 0; k < len; k++)
                {
                    nI = (byte)((nI + 1) & 0xFF);
                    nJ = (byte)((nJ + IKey[nI]) & 0xFF);
                    t = IKey[nI];
                    IKey[nI] = IKey[nJ];
                    IKey[nJ] = t;
                    t = (byte)((IKey[nI] + IKey[nJ]) & 0xFF);
                    int a, b;
                    a = InsArray[k];
                    b = IKey[t];
                    
                    //rst = rst + (char)(InsArray[k] ^ IKey[t]);
                    rst = rst + (a ^ b).ToString("X2");
                }

            }
            finally
            {


            }
            return rst;
        }

        #endregion

        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return Decrypt(text, DESKey);
            }
            else
            {
                return "";
            }
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 解密base64 字符串
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static string DecryptFromBase64(string base64String)
        {
            string codeString = string.Empty;
            if (string.IsNullOrEmpty(base64String))
                return codeString;

            byte[] arrCodes = Convert.FromBase64String(base64String);
            codeString = Encoding.Default.GetString(arrCodes);
            return codeString;
        }
        #endregion
    }
}
