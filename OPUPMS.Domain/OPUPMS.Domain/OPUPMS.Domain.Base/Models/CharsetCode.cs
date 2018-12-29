using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Models
{
    public class CharsetCode
    {
        public CharsetCode()
        { }
        #region Model
        private int _id;
        private CharsetSource _charsetsource;
        private CharsetType _charsettype;
        private int _sourceid;
        private string _code;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 字符资源(1.餐饮项目)
        /// </summary>
        public CharsetSource CharsetSource
        {
            set { _charsetsource = value; }
            get { return _charsetsource; }
        }
        /// <summary>
        /// 字符类型(1.拼音 2.五笔)
        /// </summary>
        public CharsetType CharsetType
        {
            set { _charsettype = value; }
            get { return _charsettype; }
        }
        /// <summary>
        /// 资源ID
        /// </summary>
        public int SourceId
        {
            set { _sourceid = value; }
            get { return _sourceid; }
        }
        /// <summary>
        /// 字符编码
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        #endregion Model
    }
}
