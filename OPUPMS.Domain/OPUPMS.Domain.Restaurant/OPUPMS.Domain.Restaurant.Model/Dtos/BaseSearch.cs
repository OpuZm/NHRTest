namespace OPUPMS.Domain.Restaurant.Model.Dtos
{
    public class BaseSearch
    {
        public int offset
        {
            get;
            set;
        }

        public int limit
        {
            get;
            set;
        }

        private string sort;

        public string Sort
        {
            get { return string.IsNullOrEmpty(sort) ? "ID" : sort; }
            set { sort = value; }
        }
        private string order;

        public string Order
        {
            get { return string.IsNullOrEmpty(order) ? "desc" : order; }
            set { order = value; }
        }

        /// <summary>
        /// 新旧页面列表框架  0 -- 表示旧界面列表，1 -- 新界面列表
        /// </summary>
        public int ListType { get; set; }
    }
}
