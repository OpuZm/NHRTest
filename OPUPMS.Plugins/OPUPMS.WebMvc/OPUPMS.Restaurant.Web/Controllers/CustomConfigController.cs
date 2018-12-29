using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OPUPMS.Domain.AuthorizeService;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository;
using OPUPMS.Web.Framework.Core.Mvc;
using OPUPMS.Domain.Restaurant.Model;

namespace OPUPMS.Restaurant.Web.Controllers
{
    [CustomerAuthorize(Permission.餐饮系统设置)]
    public class CustomConfigController : AuthorizationController
    {
        private readonly ICustomConfigRepository _customConfigRepository;
        public CustomConfigController(ICustomConfigRepository customConfigRepository)
        {
            _customConfigRepository = customConfigRepository;
        }
        // GET: CustomConfig
        public ActionResult OrderChoseProjectConfig()
        {
            return View();
        }

        public ActionResult GetOrderChoseProjectConfig()
        {
            Response res = new Response();
            var Buttons= _customConfigRepository.GetList(new CustomConfigDTO() { PageModule = (int)PageModule.点餐界面PC端 });
            var Modules = Buttons.GroupBy(p => p.ModuleName).Select(p => p.Key).ToList();

            var ButtonsFlat= _customConfigRepository.GetList(new CustomConfigDTO() { PageModule = (int)PageModule.点餐界面平板端 });
            var ModulesFlat = ButtonsFlat.GroupBy(p => p.ModuleName).Select(p => p.Key).ToList();
            return Json(new
            {
                Buttons=Buttons,
                Modules=Modules,
                ButtonsFlat=ButtonsFlat,
                ModulesFlat=ModulesFlat
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditOrderChoseProjectConfig(List<CustomConfigDTO> req)
        {
            Response res = new Response();
            try
            {
                res.Data = _customConfigRepository.Edit(req);
            }
            catch (Exception e)
            {
                res.Data = false;
                res.Message = e.Message;
            }
            return Json(res);
        }
    }
}