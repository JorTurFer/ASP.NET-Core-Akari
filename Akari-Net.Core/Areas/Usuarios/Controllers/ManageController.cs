using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Json(false);
            //Si no existe lo creo
            var res = await _roleManager.DeleteAsync(role);
            return Json(res.Succeeded);
        }

        [HttpPost]
        public async Task<IActionResult> ClaimsManage(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Json(false);
            var claims = await _roleManager.GetClaimsAsync(role);
            ClaimsManageViewModel model = ClaimsManageHelper.GetClaimsManageViewModel(claims, role, _policiesManager);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoleClaims(string roleId, int policyId, bool set)
        {
            //Obtengo el rol
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Json(false);

            //Obtengo la politica con el claim que que hay que actualizar
            var policyItem = _policiesManager.GetPolicyId(policyId);
            if (policyItem == null)
                return Json(false);

            //Si tengo que setear el claim
            if (set)
            {
                //Añado el claim al rol
                var res = await _roleManager.AddClaimAsync(role, new Claim(policyItem.PolicyName, policyItem.PolicyName));
                return Json(res.Succeeded);
            }
            //Si tengo que remover el claim
            else
            {
                //Elimino el claim
                var res = await _roleManager.RemoveClaimAsync(role, new Claim(policyItem.PolicyName, policyItem.PolicyName));
                return Json(res.Succeeded);
            }

            return Json(true);
        }
    }
}