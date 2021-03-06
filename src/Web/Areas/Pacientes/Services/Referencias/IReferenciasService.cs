﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Services.Referencias
{
    public interface IReferenciasService
    {
        ReferenciasPageDataViewModel GetReferenciasPageAsync(string text, int page, int pageSize, string sort, bool @ascending);
        Task AddAsync(Referencia referencia);
        Task<Referencia> FindReferenciaByIdAsync(int id);
        Task RemoveAsync(Referencia referencia);
        Task UpdateAsync(Referencia referencia);
        Task<List<Referencia>> GetReferenceNamesAsync(string nombre);
    }
}
