using System.Web.Mvc;

namespace OPUPMS.Domain.AuthorizeService
{
    public class CustomerAuthorizeAttribute :  FilterAttribute
    {
        public CustomerAuthorizeAttribute(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; set; }
    }
}
