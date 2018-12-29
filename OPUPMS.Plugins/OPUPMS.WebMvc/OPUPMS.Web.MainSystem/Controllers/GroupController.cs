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
using OPUPMS.Domain.Base.Repositories;
using OPUPMS.Web.MainSystem.Models.Dtos;
using OPUPMS.Domain.Base.Models;

namespace OPUPMS.Web.MainSystem.Controllers
{
    public class GroupController : AuthorizationController
    {
        readonly IGroupRepository _groupRep;
        public GroupController(IGroupRepository groupRep)
        {
            _groupRep = groupRep;
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
        
        // GET: Login
        [HttpGet]
        public async Task<ActionResult> List()
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var list = await _groupRep.GetAllList();
            ViewBag.GroupList = list;
            return View();
        }
        
        [HttpGet]
        public ActionResult AddGroup()
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            if(id.HasValue && !id.IsEmpty())
            {
                var result = await _groupRep.GetByGroupAsync(id.Value);
                GroupInputDto dto = new GroupInputDto();
                dto.Id = result.Id;
                dto.Name = result.Name;
                dto.Address = result.Address;
                dto.Code = result.Code;
                dto.ContactTel = result.ContactTel;
                dto.Content = result.Content;
                dto.Email = result.Email;
                dto.Fax = result.Fax;
                dto.FullName = result.FullName;
                dto.Manager = result.Manager;
                dto.Phone = result.Phone;
                dto.Status = result.Status;
                
                return View(dto);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveGroup(GroupInputDto obj)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            if (ModelState.IsValid)
            {
                var checkCodeObj = _groupRep.GetByGroupCode(obj.Code);
                if (obj.Id == 0)
                {
                    if (checkCodeObj != null && checkCodeObj.Id > 0)
                        return JavaScript("</script>alert('此集团代码已被使用，请重新设置！');history.go(-1);</script>");
                }
                else
                {
                    var model = await _groupRep.GetByGroupAsync(obj.Id);
                    if(checkCodeObj != null && checkCodeObj.Id != model.Id)
                        return JavaScript("<script>alert('此集团代码已被使用，请重新设置！');history.go(-1);</script>");
                }
                GroupModel group = new GroupModel();
                group.Id = obj.Id;
                group.Name = obj.Name;
                group.FullName = obj.FullName;
                group.Code = obj.Code;
                group.Address = obj.Address;
                group.ContactTel = obj.ContactTel;
                group.Content = obj.Content;
                group.Email = obj.Email;
                group.Fax = obj.Fax;
                group.Manager = obj.Manager;
                group.Phone = obj.Phone;

                bool result = false;
                if (obj.Id > 0)
                {
                    result = await _groupRep.UpdateModel(group);
                }
                else
                {
                    group.Status = 1;
                    result = await _groupRep.AddModel(group);
                }

                if (result)
                    return JavaScript("<script>alert('保存成功');window.location.href = \"/MS/Group/List\";</script>");
                return JavaScript("<script>alert('保存失败');history.go(-1);</script>");
            }
            return JavaScript("<script>alert('请填写完整信息！');history.go(-1);</script>");
        }
    }
}