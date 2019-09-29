using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Services.Referencias
{
    public class FacturasServices : IFacturasServices
    {
        private readonly PatientsDbContext _pacientesDbContext;

        public FacturasServices(PatientsDbContext pacientesDbContext)
        {
            _pacientesDbContext = pacientesDbContext;
        }

        public FacturasPageDataViewModel GetReferenciasPageAsync(string text, int page, int pageSize, string sort, bool @ascending,
            int? year)
        {
            var usersQuery = _pacientesDbContext.FacturasHeaders.Join(_pacientesDbContext.Pacientes, f => f.IdPaciente, p => p.IdPaciente, (f, p) => new FacturasHeader()
            {
                Id = f.Id,
                Fecha = f.Fecha,
                IdPaciente = f.IdPaciente,
                Codigo = f.Codigo,
                Descuento = f.Descuento,
                IRPF = f.IRPF,
                Paciente = p

            });
            switch (sort.ToLower())
            {
                case "paciente":
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.Paciente.Nombre)
                        : usersQuery.OrderByDescending(p => p.Paciente.Nombre);
                    break;
                default:
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.Codigo)
                        : usersQuery.OrderByDescending(p => p.Codigo);
                    break;
            }

            if (year.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Fecha.Year == year.Value);
            }

            if (!String.IsNullOrEmpty(text))
            {
                usersQuery = usersQuery.Where(u => u.Codigo.Contains(text) || u.Paciente.Nombre.Contains(text));
            }

            var count = usersQuery.Count();

            var data = usersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var result = new FacturasPageDataViewModel()
            {
                TotalFacturas = count,
                Facturas = data,
            };
            return result;
        }

        public async Task<List<int>> GetAvailableYearsAsync()
        {
            var years = await _pacientesDbContext.FacturasHeaders.Select(x => x.Fecha.Year).Distinct().ToListAsync();
            if (!years.Any())
            {
                return new List<int>() { DateTime.Now.Year };
            }

            return years;
        }

        public async Task<FacturasHeader> FindReferenciaByIdAsync(int id)
        {
            return await _pacientesDbContext.FacturasHeaders.FindAsync(id);
        }

        public async Task RemoveAsync(FacturasHeader factura)
        {
            _pacientesDbContext.FacturasHeaders.Remove(factura);
            await _pacientesDbContext.SaveChangesAsync();
        }
    }
}
