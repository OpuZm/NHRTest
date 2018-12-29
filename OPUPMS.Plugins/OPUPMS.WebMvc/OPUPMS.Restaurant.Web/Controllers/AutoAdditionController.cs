using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class AutoAdditionController : AuthorizationController
    {
        // GET: AutoAddition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AutoAdditionCreateDTO req)
        {
            return View();
        }

        [HttpPost]
        public ActionResult IsDelete(int id = 0)
        {
            return View();
        }
    }
}