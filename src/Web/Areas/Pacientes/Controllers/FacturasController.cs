using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Facturas.Services.Referencias;
using Web.Areas.Pacientes.Data;
using Web.Areas.Pacientes.Models.ViewModels.Facturas;

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

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new CreateOrEditFacturaViewModel()
            {
                NombrePaciente = "",
                Factura = new FacturasHeader()
                {
                    Codigo = "",
                    Fecha = DateTime.Now,
                    IRPF = 0,
                    Descuento = 0
                }
            };

            return View(vm);
        }



        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string paciente,FacturasHeader factura,FacturaLine[] lineas)
        {
            factura.Lineas = lineas.ToList();
            if (await _facturasServices.CreateFacturaAsync(factura, paciente))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var factura = await _facturasServices.FindFacturaByIdForEditAsync(id);
            var vm = new CreateOrEditFacturaViewModel()
            {
                NombrePaciente = factura.Paciente.Nombre,
                Factura = factura
            };

            return View(vm);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string paciente, FacturasHeader factura, FacturaLine[] lineas)
        {
            factura.Lineas = lineas.ToList();
            if (await _facturasServices.UpdateFacturaAsync(factura, paciente))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _facturasServices.FindFacturaByIdAsync(id);
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