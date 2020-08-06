using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("PacientesManager", "Gestión de Pacientes")]
    [Route("[area]/[controller]/[action]")]
    public class PacientesController : Controller
    {
        private readonly IPacientesService _pacientesService;

        public PacientesController(IPacientesService pacientesService)
        {
            _pacientesService = pacientesService;
        }

        // GET: Pacientes/Pacientes
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _pacientesService.FindPacienteByIdAsync(id.Value);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Pacientes/Create
        public IActionResult Create()
        {
            var vm = _pacientesService.GetPacienteDataViewModel(0);
            return View(vm);
        }

        // POST: Pacientes/Pacientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                await _pacientesService.AddAsync(paciente);
                return RedirectToAction(nameof(Index));
            }
            var vm = await _pacientesService.GetPacienteDataViewModelAsync(0);
            vm.Paciente = paciente;
            return View(vm);
        }

        // GET: Pacientes/Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vm = await _pacientesService.GetPacienteDataViewModelAsync(id.Value);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _pacientesService.UpdateAsync(paciente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_pacientesService.PacienteExists(paciente.IdPaciente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var vm = await _pacientesService.GetPacienteDataViewModelAsync(paciente.IdPaciente);
            vm.Paciente = paciente;
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var paciente = await _pacientesService.FindPacienteByIdAsync(id);
            await _pacientesService.RemoveAsync(paciente);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPacientesGrid(GridPacientesViewModel vm)
        {
            var pageData = await _pacientesService.GetPacientesPageAsync(vm.Text, vm.Page, vm.PageSize, vm.Sort, vm.Ascending);
            vm.TotalPacientes = pageData.TotalPacientes;
            vm.Pacientes = pageData.Pacientes;
            return View(vm);
        }

        [Permission("PacientesCitas", "Visualizar citas")]
        public async Task<IActionResult> Citas(int id)
        {
            var citas = await _pacientesService.GetCitasViewModelAsync(id);

            return View(citas);
        }

        [HttpPost]
        [Permission("PacientesManager", "Gestión de Pacientes")]
        public async Task<JsonResult> GetPatientNames(string Nombre)
        {
            //Para evitar sobrecarga, solo busco si se han escrito 4 o mas letras
            if (Nombre.Length > 2)
            {
                var pacientes = await _pacientesService.GetPatientNamesAsync(Nombre);
                return Json(pacientes);
            }
            else
            {
                return Json("");
            }
        }
    }
}
