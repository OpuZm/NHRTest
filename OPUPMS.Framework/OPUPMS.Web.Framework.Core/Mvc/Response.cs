namespace OPUPMS.Web.Framework.Core.Mvc
{
    public class Response : Response<object>
    {
    }

    public class Response<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// 消息-错误/提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
