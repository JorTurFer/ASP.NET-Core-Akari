using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Services.Referencias
{
    public class ReferenciasService : IReferenciasService
    {
        private readonly PatientsDbContext _patientsDbContext;

        public ReferenciasService(PatientsDbContext patientsDbContext)
        {
            _patientsDbContext = patientsDbContext;
        }

        public ReferenciasPageDataViewModel GetReferenciasPageAsync(string text, int page, int pageSize, string sort,
            bool @ascending)
        {
            IQueryable<Referencia> usersQuery = _patientsDbContext.Referencias;
            switch (sort.ToLower())
            {
                case "price":
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.Precio)
                        : usersQuery.OrderByDescending(p => p.Precio);
                    break;
                default:
                    usersQuery = ascending
                        ? usersQuery.OrderBy(p => p.Identificador)
                        : usersQuery.OrderByDescending(p => p.Identificador);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(text))
                usersQuery = usersQuery.Where(u => u.Identificador.Contains(text));

            var count = usersQuery.Count();

            var data = usersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var result = new ReferenciasPageDataViewModel()
            {
                TotalReferencias = count,
                Referencias = data,
            };
            return result;
        }

        public async Task AddAsync(Referencia referencia)
        {
            await _patientsDbContext.AddAsync(referencia);
            await _patientsDbContext.SaveChangesAsync();
        }

        public async Task<Referencia> FindReferenciaByIdAsync(int id)
        {
            return await _patientsDbContext.Referencias.FindAsync(id);
        }

        public async Task RemoveAsync(Referencia referencia)
        {
            _patientsDbContext.Remove(referencia);
            await _patientsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Referencia referencia)
        {
             _patientsDbContext.Update(referencia);
            await _patientsDbContext.SaveChangesAsync();
        }

        public async Task<List<Referencia>> GetReferenceNamesAsync(string nombre)
        {
            return await _patientsDbContext.Referencias.Where(x => x.Identificador.ToLower().Contains(nombre.ToLower())).ToListAsync();
        }
    }
}
