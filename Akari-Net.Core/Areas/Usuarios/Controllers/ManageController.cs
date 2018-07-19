using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akari_Net.Core.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    //[Authorize]
    [Route("[area]/[controller]/[action]")]
    public class ManageController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ManageRoles()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ManageUsers()
        {
            return View();
        }

    }

}