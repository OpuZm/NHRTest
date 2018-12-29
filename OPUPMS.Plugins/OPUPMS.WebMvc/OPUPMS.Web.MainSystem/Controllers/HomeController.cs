using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Security;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Base;
using OPUPMS.Infrastructure.Common.Net;

namespace OPUPMS.Web.MainSystem.Controllers
{
    public class HomeController : BaseController
    {        
        public HomeController()
        {
        }

        // GET: Login
        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}