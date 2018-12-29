using System;

namespace OPUPMS.Web.Framework.Core.Permission
{
    /// <summary>
    /// 指定需要进行权限认证的Action描述。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        private string _description;

        /// <summary>
        /// 设指定需要进行权限认证的Action描述。
        /// </summary>
        public PermissionAttribute()
            : this(string.Empty)
        {
        }

        public PermissionAttribute(string description)
        {
            _description = description;
        }

        /// <summary>
        /// Action功能说明。
        /// </summary>
        public virtual string Description
        {
            get { return DescriptionValue; }
        }

        protected string DescriptionValue
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}