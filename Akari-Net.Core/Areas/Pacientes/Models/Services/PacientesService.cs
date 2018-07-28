using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Citas.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public class PacientesService : IPacientesService
    {
        private readonly PacientesDbContext _context;

        public PacientesService(PacientesDbContext context)
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
            return _context.Pacientes.Find(id);
        }

        public Task<Paciente> FindPacienteByIdAsync(int id)
        {
            return _context.Pacientes.FindAsync(id);
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
            return _context.Pacientes.Any(e => e.Id == id);
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

        public PacientesPageDataViewModel GetPacientesPageAsync(string text, int page, int pageSize, string sort, bool ascending)
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

            if (!string.IsNullOrWhiteSpace(text))
                usersQuery = usersQuery.Where(u => u.Nombre.Contains(text));

            var count = usersQuery.Count();

            var data = usersQuery.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new PacienteViewModel
            {
                Name = x.Nombre,
                Email = x.Email,
                Id = x.Id.ToString()
            }).ToList();
            var result = new PacientesPageDataViewModel
            {
                TotalPacientes = count,
                Pacientes = data,
            };
            return result;
        }

    }
}
