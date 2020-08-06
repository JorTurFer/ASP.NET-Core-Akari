using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Areas.Pacientes.Models.ViewModels.Historial;

namespace Web.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("PacientesHistorial", "Historial de Pacientes")]
    [Route("[area]/[controller]/[action]")]
    public class HistorialController : Controller
    {
        private readonly IPacientesService _pacientesService;

        public HistorialController(IPacientesService pacientesService)
        {
            _pacientesService = pacientesService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var paciente = await _pacientesService.FindPacienteByIdAsync(id);
            var vm = new HistorialViewModel { PacienteId = id,Paciente = paciente.Nombre };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int idPaciente, string registry)
        {
            await _pacientesService.CreateRegistryAsync(idPaciente, registry);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetHistorialGrid(GridHistorialViewModel vm)
        {
            var pageData = await _pacientesService.GetHistorialPageAsync(vm.Id,vm.Text, vm.Page, vm.PageSize, vm.Sort, vm.Ascending);
            vm.TotalRegistros = pageData.TotalRegistros;
            vm.Registros = pageData.Registros;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int idPaciente)
        {
            var historia = await _pacientesService.FindHistoriaByIdAsync(id);
            await _pacientesService.RemoveAsync(historia);
            return RedirectToAction(nameof(Index), new { id = historia.IdPaciente });
        }

        public async Task<string> Detail(int id)
        {
            var historia = await _pacientesService.FindHistoriaByIdAsync(id);
            return historia.Registro;
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string registry)
        {
            await _pacientesService.UpdateRegistryAsync(id,registry);
            return Ok();
        }
    }
}
