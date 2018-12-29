using OPUPMS.Web.Framework.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPUPMS.Web.Hotel.Controllers
{
    //[RoutePrefix("HBookingManage")]
    public class BookingManageController : AuthorizationController
    {
        // GET: BookingManage
        [ActionName("Details")]
        public ActionResult Details()
        {
            return View("BookingDetails");
        }

        // GET: BookingManage
        [ActionName("List")]
        public ActionResult List()
        {
            try
            {
                return View("BookingList");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}