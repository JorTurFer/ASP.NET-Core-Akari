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
                IdFactura = f.IdFactura,
                Fecha = f.Fecha,
                IdPaciente = f.IdPaciente,
                Codigo = f.Codigo,
                Descuento = f.Descuento,
                IRPF = f.IRPF,
                Lineas = f.Lineas,
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

        public async Task<FacturasHeader> FindFacturaByIdAsync(int id)
        {
            return await _pacientesDbContext.FacturasHeaders.FindAsync(id);
        }
        public async Task<FacturasHeader> FindFacturaByIdForEditAsync(int id)
        {
            return await _pacientesDbContext.FacturasHeaders.Where(x => x.IdFactura == id).Include(x => x.Paciente).Include(x => x.Lineas).FirstAsync();
        }

        public async Task RemoveAsync(FacturasHeader factura)
        {
            _pacientesDbContext.FacturasHeaders.Remove(factura);
            await _pacientesDbContext.SaveChangesAsync();
        }

        public async Task<bool> CreateFacturaAsync(FacturasHeader factura, string NombrePaciente)
        {
            var paciente = await _pacientesDbContext.Pacientes.FirstOrDefaultAsync(x => x.Nombre == NombrePaciente);
            if (paciente is null)
            {
                return false;
            }
            var lastFacturas = _pacientesDbContext.FacturasHeaders
                .Where(x => x.Codigo.Contains($"/{factura.Fecha:yy}"))
                .Select(x => Convert.ToInt32(x.Codigo.Substring(0, 7)));
            if (await lastFacturas.AnyAsync())
            {
                var last = await lastFacturas.MaxAsync();
                last++;
                factura.Codigo = $"{last.ToString("D" + 7)}/{factura.Fecha:yy}";
            }
            else
            {
                factura.Codigo = $"0000001/{factura.Fecha:yy}";
            }

            factura.IdPaciente = paciente.IdPaciente;
            factura.Paciente = paciente;

            await _pacientesDbContext.FacturasHeaders.AddAsync(factura);
            await _pacientesDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFacturaAsync(FacturasHeader factura, string NombrePaciente)
        {
            var paciente = await _pacientesDbContext.Pacientes.FirstOrDefaultAsync(x => x.Nombre == NombrePaciente);
            if (paciente is null)
            {
                return false;
            }

            factura.IdPaciente = paciente.IdPaciente;
            factura.Paciente = paciente;

            var idLines = factura.Lineas.Select(x => x.IdLine).ToList();
            var previousLines = _pacientesDbContext.FacturasLineas.Where(x => x.IdFactura == factura.IdFactura).ToList();
            foreach (var line in previousLines)
            {
                if (!idLines.Contains(line.IdLine))
                {
                    _pacientesDbContext.FacturasLineas.Remove(line);
                }
                else
                {
                    var lineToRemove = factura.Lineas.First(x => x.IdLine == line.IdLine);
                    factura.Lineas.Remove(lineToRemove);
                }
            }

            await _pacientesDbContext.SaveChangesAsync();

            _pacientesDbContext.FacturasHeaders.Update(factura);
            await _pacientesDbContext.SaveChangesAsync();


            return true;
        }
    }
}
