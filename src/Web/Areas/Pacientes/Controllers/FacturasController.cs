using System.Threading.Tasks;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Facturas.Services.Referencias;

namespace Web.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("FacturasManager", "Gestión de Facturas")]
    [Route("[area]/[controller]/[action]")]
    public class FacturasController : Controller
    {
        private readonly IFacturasServices _facturasServices;

        public FacturasController(IFacturasServices facturasServices)
        {
            _facturasServices = facturasServices;
        }


        public async Task<IActionResult> Index()
        {
            var availableYears = await _facturasServices.GetAvailableYearsAsync();

            return View(availableYears);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _facturasServices.FindReferenciaByIdAsync(id);
            await _facturasServices.RemoveAsync(factura);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetFacturasGrid(GridFacturasViewModel vm)
        {
            var pageData = _facturasServices.GetReferenciasPageAsync(vm.Text, vm.Page, vm.PageSize, vm.Sort, vm.Ascending,vm.Year);
            vm.TotalReferencias = pageData.TotalFacturas;
            vm.FacturasHeaders = pageData.Facturas;
            return View(vm);
        }
    }
}