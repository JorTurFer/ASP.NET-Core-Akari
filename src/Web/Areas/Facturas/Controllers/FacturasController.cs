using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Facturas.Controllers
{
    [Area("Facturas")]
    [Permission("FacturasManager", "Gestión de Facturas")]
    [Route("[area]/[action]")]
    public class FacturasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}