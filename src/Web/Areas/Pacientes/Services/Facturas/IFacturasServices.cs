﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Services.Referencias
{
    public interface IFacturasServices
    {
        FacturasPageDataViewModel GetReferenciasPageAsync(string text, int page, int pageSize, string sort, bool @ascending,int? year);
        Task<List<int>> GetAvailableYearsAsync();
        Task<FacturasHeader> FindReferenciaByIdAsync(int id);
        Task RemoveAsync(FacturasHeader factura);
    }
}
