using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace OPUPMS.Infrastructure.Common
{
    public class PrintInvoice
    {
        public XDocument root;
        public PrintInvoice()
        {
            HttpContext HttpCurrent = HttpContext.Current;
            string AppPath = HttpCurrent.Server.MapPath("~");
            try
            {
                root = XDocument.Load(AppPath + "\\bin\\Data\\checkOut.xml");
            }
            catch (Exception)
            {
                throw new Exception("未找到结账打印模板");
            }
            
        }
        public TcpClient GetPrint(string printIp,int printPort)
        {
            var client = new TcpClient();
            client.Connect(printIp, printPort);
            return client;
        }

        public TcpClient GetMyPrint()
        {
            var client = new TcpClient();
            string ip = string.Empty;
            HttpContext httpCurrent = HttpContext.Current;
            string AppPath = httpCurrent.Server.MapPath("~");
            if (httpCurrent.Request.ServerVariables["HTTP_VIA"] != null) // 使用代理 
            {
                ip = httpCurrent.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); // Return real client IP. 
            }
            else// not using proxy or can't get the Client IP 
            {
                ip = httpCurrent.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            ip = ip.Equals("::1") ? "127.0.0.1" : ip;
            try
            {
                XDocument root = XDocument.Load(AppPath + "\\bin\\Data\\print.xml");
                foreach (var item in root.Element("Data").Elements("Print"))
                {
                    string pcIp = item.Attribute("PcIp").Value;
                    if (pcIp.Equals(ip,StringComparison.OrdinalIgnoreCase))
                    {
                        string printIp= item.Attribute("PrintIp").Value;
                        int printPort= Convert.ToInt32(item.Attribute("PrintPort").Value);
                        client.Connect(printIp, printPort);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return client;
        }

        public NetworkStream BuildStream()
        {
            System.Net.Sockets.NetworkStream stream = null;
            return stream;
        }

        #region 套接字和打印机通讯放回通讯流

        /// <summary>

        /// 套接字和打印机通讯放回通讯流
        /// </summary>
        /// <returns></returns>
        public NetworkStream GetStream(TcpClient client, NetworkStream stream)
        {
            byte[] chushihua = new byte[] { 27, 64 };//初始化打印机
            byte[] ziti = new byte[] { 27, 77, 0 };//选择字体n =0,1,48,49
            byte[] zitidaxiao = new byte[] { 29, 33, 0 };//选择字体大小
            byte[] duiqifangshi = new byte[] { 27, 97, 1 };//选择对齐方式0,48左对齐1,49中间对齐2,50右对齐
            stream = client.GetStream(); //是否支持写入
            if (!stream.CanWrite) { stream = null; }
            stream.Write(chushihua, 0, chushihua.Length);//初始化
            stream.Write(ziti, 0, ziti.Length);//设置字体
            stream.Write(zitidaxiao, 0, zitidaxiao.Length);//设置字体大小--关键
            stream.Write(duiqifangshi, 0, duiqifangshi.Length);//居中

            
            return stream;
        }
        #endregion

        #region 把要打印的文字写入打印流
        /// <summary>
        /// 把要打印的文字写入打印流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="output"></param>
        public void PrintText(NetworkStream stream, string output,int left)
        {
            Byte[] data = System.Text.Encoding.Default.GetBytes(output);
            stream.Write(data, left, data.Length);//输出文字
        }
        #endregion

        #region 设置对齐方式0,48左对齐1,49中间对齐2,50右对齐
        /// <summary>
        /// 设置对齐方式0,48左对齐1,49中间对齐2,50右对齐
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="n"></param>
        public void SetDuiQi(NetworkStream stream, byte n)
        {
            byte[] duiqifangshi = new byte[] { 27, 97, 1 };//选择对齐方式0,48左对齐1,49中间对齐2,50右对齐
            stream.Write(duiqifangshi, 0, duiqifangshi.Length);
        }
        #endregion

        #region 设置字体n =0,1,48,49
        /// <summary>
        /// 设置字体n =0,1,48,49
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="n"></param>
        public void SetFont(NetworkStream stream, byte n, PrintPageEventArgs e)
        {
            byte[] ziti = new byte[] { 27, 77, 0 };//选择字体n =0,1,48,49
            stream.Write(ziti, 0, ziti.Length);
        }
        #endregion

        #region 设置加粗1加粗0还原
        /// <summary>
        /// 设置加粗1加粗0还原
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="n"></param>
        public void SetBold(NetworkStream stream, byte n)
        {
            byte[] jiacu = new byte[] { 27, 69, n };//选择加粗模式
            stream.Write(jiacu, 0, jiacu.Length);
        }
        #endregion

        #region 设置字体大小0最小1,2,3
        /// <summary>
        /// 设置字体大小0最小1,2,3
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="n"></param>
        public void SetFontSize(NetworkStream stream, byte n)
        {
            byte[] zitidaxiao = new byte[] { 29, 33, n };//选择字体大小
            stream.Write(zitidaxiao, 0, zitidaxiao.Length);
        }
        #endregion

        #region 切纸
        /// <summary>
        /// 切纸
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="n"></param>
        public void QieZhi(NetworkStream stream)
        {
            byte[] qiezhi = new byte[] { 29, 86, 1, 49 };//切纸
            stream.Write(qiezhi, 0, qiezhi.Length);
        }
        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="client"></param>
        /// <param name="stream"></param>
        public void DiposeStreamClient(TcpClient client, NetworkStream stream)
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
            if (client != null) client.Close();
        }
        #endregion
    }
}
