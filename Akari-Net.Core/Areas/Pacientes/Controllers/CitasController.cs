using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akari_Net.Core.Areas.Citas.Controllers
{
    [Area("Pacientes")]
    [Authorize(Policy = "CitasManager")]
    public class CitasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}