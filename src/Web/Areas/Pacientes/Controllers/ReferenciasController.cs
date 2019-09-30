using System.Threading.Tasks;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Facturas.Services.Referencias;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("ReferenciasManager", "Gestión de Referencias")]
    [Route("[area]/[controller]/[action]")]
    public class ReferenciasController : Controller
    {
        private readonly IReferenciasService _referenciasService;

        public ReferenciasController(IReferenciasService referenciasService)
        {
            _referenciasService = referenciasService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View(new Referencia());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(Referencia referencia)
        {
            if (ModelState.IsValid)
            {
                await _referenciasService.AddAsync(referencia);
                return RedirectToAction(nameof(Index));
            }

            return View(referencia);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var referencia = await _referenciasService.FindReferenciaByIdAsync(id);
            if (referencia == null)
            {
                return NotFound();
            }
            return View(referencia);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Referencia referencia)
        {
            if (ModelState.IsValid)
            {
                await _referenciasService.UpdateAsync(referencia);
                return RedirectToAction(nameof(Index));
            }

            return View(referencia);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var referencia = await _referenciasService.FindReferenciaByIdAsync(id);
            await _referenciasService.RemoveAsync(referencia);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetReferenciasGrid(GridReferenciasViewModel vm)
        {
            var pageData = _referenciasService.GetReferenciasPageAsync(vm.Text, vm.Page, vm.PageSize, vm.Sort, vm.Ascending);
            vm.TotalReferencias = pageData.TotalReferencias;
            vm.Referencias = pageData.Referencias;
            return View(vm);
        }


        [HttpPost]
        [Route("FindReferenceByName")]
        public async Task<JsonResult> GetReferenceByNamesAsync(string Nombre)
        {
            //Para evitar sobrecarga, solo busco si se han escrito 4 o mas letras
            if (Nombre.Length > 2)
            {
                var reference = await _referenciasService.GetReferenceNamesAsync(Nombre);
                return Json(reference);
            }
            else
                return Json("");
        }
    }
}