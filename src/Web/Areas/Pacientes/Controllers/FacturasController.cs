﻿using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Facturas.Services.Referencias;
using Web.Areas.Pacientes.Data;
using Web.Areas.Pacientes.Models.ViewModels.Facturas;
using Web.Areas.Pacientes.Services.Facturas;

namespace Web.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("FacturasManager", "Gestión de Facturas")]
    [Route("[area]/[controller]/[action]")]
    public class FacturasController : Controller
    {
        private readonly IFacturasServices _facturasServices;
        private readonly IPdfGenerator _pdfGenerator;

        public FacturasController(IFacturasServices facturasServices,  IPdfGenerator pdfGenerator)
        {
            _facturasServices = facturasServices;
            _pdfGenerator = pdfGenerator;
        }


        public async Task<IActionResult> Index()
        {
            var availableYears = await _facturasServices.GetAvailableYearsAsync();

            return View(availableYears);
        }

        [HttpGet]
        [Permission("FacturasWrite", "Permitir crear/editar facturas")]
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


        [Permission("FacturasWrite", "Permitir crear/editar facturas")]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string paciente, FacturasHeader factura, FacturaLine[] lineas)
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

        [Permission("FacturasWrite", "Permitir crear/editar facturas")]
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


        [Permission("FacturasWrite", "Permitir crear/editar facturas")]
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

        [Permission("DeleteFacturas","Permite borrar facturas")]
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
            var pageData = _facturasServices.GetReferenciasPageAsync(vm.Text, vm.Page, vm.PageSize, vm.Sort, vm.Ascending, vm.Year);
            vm.TotalReferencias = pageData.TotalFacturas;
            vm.FacturasHeaders = pageData.Facturas;
            return View(vm);
        }

        [HttpGet, ActionName("Descargar")]
        public async Task<IActionResult> DescargarFactura(int id)
        {
            var factura = await _facturasServices.FindFacturaByIdForEditAsync(id);
            return File(await _pdfGenerator.GeneratePdf(factura), "application/pdf", $"{factura.Codigo}.pdf");
        }

        [HttpGet, ActionName("Ver")]
        public async Task<IActionResult> VerFactura(int id)
        {
            var factura = await _facturasServices.FindFacturaByIdForEditAsync(id);
            return new FileStreamResult(await _pdfGenerator.GeneratePdf(factura), "application/pdf");
        }

        [Permission("Facturacion", "Permitir ver la facturación anual")]
        [HttpGet]
        public async Task<IActionResult> Facturacion()
        {
            var availableYear = await _facturasServices.GetAvailableYearsAsync();
            return View(availableYear);
        }

        [Permission("Facturacion", "Permitir ver la facturación anual")]
        [HttpPost,ActionName("FacturacionYear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FacturacionYear(int year)
        {
            var facturacion = await _facturasServices.GetFacturacion(year);
            return PartialView("_FacturaciónPartial", facturacion);
        }

    }
}