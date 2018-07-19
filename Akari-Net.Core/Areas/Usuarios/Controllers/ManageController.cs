using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ManageController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
            var res = await _roleManager.CreateAsync(role);
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

    }

}