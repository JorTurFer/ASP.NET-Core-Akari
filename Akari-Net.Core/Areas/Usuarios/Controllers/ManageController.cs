using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Usuarios.Models.Helpers;
using Akari_Net.Core.Areas.Usuarios.Models.Services;
using Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Akari_Net.Core.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    //[Authorize]
    [Route("[area]/[controller]/[action]")]
    public class ManageController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        IPoliciesManager _policiesManager;

        public ManageController(RoleManager<IdentityRole> roleManager, IPoliciesManager policiesManager)
        {
            _roleManager = roleManager;
            _policiesManager = policiesManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManageUsers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManageRoles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRole(string roleName)
        {
            //Compruebo que no exista
            var exists = _roleManager.Roles.Any(x => string.Compare(x.Name, roleName, false) == 0);
            if (exists)
                return Json(false);
            //Si no existe lo creo
            IdentityRole role = new IdentityRole();
            role.Name = roleName;
            await _roleManager.CreateAsync(role);
            return Json(role.Id);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return Json(false);
            //Si no existe lo creo
            var res = await _roleManager.DeleteAsync(role);
            return Json(res.Succeeded);
        }

        [HttpPost]
        public async Task<IActionResult> ClaimsManage(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return Json(false);
            var claims = await _roleManager.GetClaimsAsync(role);
            ClaimsManageViewModel model = ClaimsManageHelper.GetClaimsManageViewModel(claims,role, _policiesManager);
           
            return View(model);
        }
    }
}