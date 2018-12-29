using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.Restaurant.Services;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Infrastructure.Common.Security;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Infrastructure.Common.Operator;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Base;
using OPUPMS.Infrastructure.Common.Net;
using System.Threading.Tasks;

namespace OPUPMS.Web.Restaurant.Controllers
{
    public class PluginController : BaseController
    {
        readonly ICyxmService cyxmService;

        public PluginController(ICyxmService _cyxmService)
        {
            cyxmService = _cyxmService;
        }

        // GET: Plugin
        public ActionResult Index()
        {
            return View();
        }
    }
}