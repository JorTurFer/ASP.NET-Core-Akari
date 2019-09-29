using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Services.Referencias
{
    public class ReferenciasService : IReferenciasService
    {
        private readonly PatientsDbContext _facturasDbContext;

        public ReferenciasService(PatientsDbContext facturasDbContext)
        {
            _facturasDbContext = facturasDbContext;
        }

        public ReferenciasPageDataViewModel GetReferenciasPageAsync(string text, int page, int pageSize, string sort,
            bool @ascending)
        {
            IQueryable<Referencia> usersQuery = _facturasDbContext.Referencias;
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
            await _facturasDbContext.AddAsync(referencia);
            await _facturasDbContext.SaveChangesAsync();
        }

        public async Task<Referencia> FindReferenciaByIdAsync(int id)
        {
            return await _facturasDbContext.Referencias.FindAsync(id);
        }

        public async Task RemoveAsync(Referencia referencia)
        {
            _facturasDbContext.Remove(referencia);
            await _facturasDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Referencia referencia)
        {
             _facturasDbContext.Update(referencia);
            await _facturasDbContext.SaveChangesAsync();
        }
    }
}
