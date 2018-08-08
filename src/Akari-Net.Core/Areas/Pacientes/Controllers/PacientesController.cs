﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using AspNetCore.Identity.ByPermissions;

namespace Akari_Net.Core.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("PacientesManager","Gestión de Pacientes")]
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

        // GET: Pacientes/Pacientes/Details/5
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
            return View();
        }

        // POST: Pacientes/Pacientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Nacimiento,Email,Telefono")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                await _pacientesService.AddAsync(paciente);
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Nacimiento,Email,Telefono")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _pacientesService.UpdateAsync(paciente);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_pacientesService.PacienteExists(paciente.Id))
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
            return View(paciente);
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
        public IActionResult GetPacientesGrid(GridPacientesViewModel vm)
        {
            var pageData = _pacientesService.GetPacientesPageAsync(vm.Text, vm.Page, vm.PageSize, vm.Sort, vm.Ascending);
            vm.TotalPacientes = pageData.TotalPacientes;
            vm.Pacientes = pageData.Pacientes;
            return View(vm);
        }

        [Permission( "PacientesCitas",  "Visualizar citas")]
        public async Task<IActionResult> Citas(int id)
        {
            var citas = _pacientesService.GetCitasViewModel(id);

            return View(citas);
        }
    }
}