﻿using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Pacientes.Data;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public class PacientesService : IPacientesService
    {
        private readonly PatientsDbContext _context;

        public PacientesService(PatientsDbContext context)
        {
            _context = context;
        }

        public int Add(Paciente paciente)
        {
            _context.Add(paciente);
            return SaveChanges();
        }

        public Task<int> AddAsync(Paciente paciente)
        {
            _context.Add(paciente);
            return SaveChangesAsync();
        }

        public Paciente FindPacienteById(int id)
        {
            return _context.Pacientes.Where(x => x.IdPaciente == id).Include(b => b.Citas).Include(b => b.Pais).Include(b => b.Provincia).FirstOrDefault();
        }

        public Task<Paciente> FindPacienteByIdAsync(int id)
        {
            return _context.Pacientes.Where(x => x.IdPaciente == id).Include(b => b.Citas).Include(b => b.Pais).Include(b => b.Provincia).FirstOrDefaultAsync();
        }

        public List<Paciente> GetPacientes()
        {
            return _context.Pacientes.ToList();
        }

        public Task<List<Paciente>> GetPacientesAsync()
        {
            return _context.Pacientes.ToListAsync();
        }

        public bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.IdPaciente == id);
        }

        public int Remove(Paciente paciente)
        {
            _context.Pacientes.Remove(paciente);
            return SaveChanges();
        }

        public Task<int> RemoveAsync(Paciente paciente)
        {
            _context.Pacientes.Remove(paciente);
            return SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public int Update(Paciente paciente)
        {
            _context.Update(paciente);
            return _context.SaveChanges();
        }

        public Task<int> UpdateAsync(Paciente paciente)
        {
            _context.Update(paciente);
            return _context.SaveChangesAsync();
        }

        public async Task<PacientesPageDataViewModel> GetPacientesPageAsync(string text, int page, int pageSize, string sort, bool ascending)
        {
            IQueryable<Paciente> usersQuery = _context.Pacientes;
            switch (sort.ToLower())
            {
                case "birthdate":
                    usersQuery = ascending
                       ? usersQuery.OrderBy(p => p.Nacimiento)
                       : usersQuery.OrderByDescending(p => p.Nacimiento);
                    break;
                default:
                    usersQuery = ascending
                       ? usersQuery.OrderBy(p => p.Nombre)
                       : usersQuery.OrderByDescending(p => p.Nombre);
                    break;
            }

            if (!String.IsNullOrWhiteSpace(text))
            {
                usersQuery = usersQuery.Where(u => u.Nombre.Contains(text));
            }

            var count = usersQuery.Count();

            var data = await usersQuery.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new PacienteViewModel
            {
                Name = x.Nombre,
                Email = x.Email,
                Id = x.IdPaciente.ToString()
            }).ToListAsync();
            var result = new PacientesPageDataViewModel
            {
                TotalPacientes = count,
                Pacientes = data,
            };
            return result;
        }

        public Task<List<CalendarEvent>> GetCitasAsync(int id)
        {
            return _context.CalendarEvents.Where(x => x.IdPaciente == id).OrderByDescending(x => x.Start).ToListAsync();
        }

        public List<CalendarEvent> GetCitas(int id)
        {
            return _context.CalendarEvents.Where(x => x.IdPaciente == id).OrderByDescending(x => x.Start).ToList();
        }

        public CitasPacienteViewModel GetCitasViewModel(int id)
        {
            var paciente = _context.Pacientes.Where(x => x.IdPaciente == id).Include(b => b.Citas).ThenInclude(c => c.TipoCita).FirstOrDefault();

            return new CitasPacienteViewModel { Citas = paciente.Citas.OrderByDescending(x => x.Start), Paciente = paciente };
        }
        public async Task<CitasPacienteViewModel> GetCitasViewModelAsync(int id)
        {
            var paciente = await _context.Pacientes.Where(x => x.IdPaciente == id).Include(b => b.Citas).ThenInclude(c => c.TipoCita).FirstOrDefaultAsync();

            return new CitasPacienteViewModel { Citas = paciente.Citas.OrderByDescending(x => x.Start), Paciente = paciente };
        }

        public PacienteDataViewModel GetPacienteDataViewModel(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente is null)
            {
                paciente = new Paciente();
            }

            var provincias = _context.Provincias.Select(x => new SelectListItem { Value = x.IdProvincia.ToString(), Text = x.Nombre, Selected = x.IdProvincia == paciente.IdProvincia }).ToList();
            var paises = _context.Paises.Select(x => new SelectListItem { Value = x.IdPais.ToString(), Text = x.Nombre, Selected = x.IdPais == paciente.IdPais }).ToList();
            return new PacienteDataViewModel { Paciente = paciente, Provincias = provincias, Paises = paises };
        }

        public async Task<PacienteDataViewModel> GetPacienteDataViewModelAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente is null)
            {
                paciente = new Paciente();
            }

            var provincias = await _context.Provincias.Select(x => new SelectListItem { Value = x.IdProvincia.ToString(), Text = x.Nombre, Selected = x.IdProvincia == paciente.IdProvincia }).ToListAsync();
            var paises = await _context.Paises.Select(x => new SelectListItem { Value = x.IdPais.ToString(), Text = x.Nombre, Selected = x.IdPais == paciente.IdPais }).ToListAsync();
            return new PacienteDataViewModel { Paciente = paciente, Provincias = provincias, Paises = paises };
        }

        public List<PacientesAutoCompleteViewModel> GetPatientNames(string Nombre)
        {
            return _context.Pacientes.Where(x => x.Nombre.ToLower().Contains(Nombre.ToLower())).Select(x => new PacientesAutoCompleteViewModel { Nombre = x.Nombre, Id = x.IdPaciente }).ToList();
        }

        public Task<List<PacientesAutoCompleteViewModel>> GetPatientNamesAsync(string Nombre)
        {
            return _context.Pacientes.Where(x => x.Nombre.ToLower().Contains(Nombre.ToLower())).Select(x => new PacientesAutoCompleteViewModel { Nombre = x.Nombre, Id = x.IdPaciente }).ToListAsync();
        }

        public int CreateRegistry(int idPaciente, string registry)
        {
            var registro = new Historial { IdPaciente = idPaciente, Registro = registry, Fecha = DateTime.Now };
            _context.Add(registro);
            return SaveChanges();
        }

        public async Task<int> CreateRegistryAsync(int idPaciente, string registry)
        {
            var registro = new Historial { IdPaciente = idPaciente, Registro = registry, Fecha = DateTime.Now };
            _context.Add(registro);
            return await SaveChangesAsync();
        }

        public async Task<HistorialPageDataViewModel> GetHistorialPageAsync(int id, string text, int page, int pageSize, string sort, bool ascending)
        {
            IQueryable<Historial> usersQuery = _context.Historial.Where(x=>x.IdPaciente == id);
            switch (sort.ToLower())
            {
                case "fecha":
                    usersQuery = ascending
                       ? usersQuery.OrderBy(p => p.Fecha)
                       : usersQuery.OrderByDescending(p => p.Fecha);
                    break;
                default:
                    usersQuery = ascending
                       ? usersQuery.OrderBy(p => p.Id)
                       : usersQuery.OrderByDescending(p => p.Id);
                    break;
            }

            if (!String.IsNullOrWhiteSpace(text))
            {
                usersQuery = usersQuery.Where(u => u.Registro.Contains(text));
            }

            var count = usersQuery.Count();

            var data = await usersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = new HistorialPageDataViewModel
            {
                TotalRegistros = count,
                Registros = data,
            };
            return result;
        }

        public int Remove(Historial historia)
        {
            _context.Historial.Remove(historia);
            return _context.SaveChanges();
        }

        public async Task<int> RemoveAsync(Historial historia)
        {
            _context.Historial.Remove(historia);
            return await _context.SaveChangesAsync();
        }

        public Historial FindHistoriaById(int id)
        {
            return _context.Historial.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<Historial> FindHistoriaByIdAsync(int id)
        {
            return await _context.Historial.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateRegistryAsync(int id, string registry)
        {
            var historia = await _context.Historial.Where(x => x.Id == id).FirstOrDefaultAsync();
            historia.Registro = registry;
            return await _context.SaveChangesAsync();
        }
    }
}
