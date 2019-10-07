using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Areas.Facturas.Entities.ViewModels;
using Web.Areas.Pacientes.Data;
using Web.Areas.Pacientes.Models.ViewModels.Facturas;
using Web.Areas.Pacientes.Services.Facturas;

namespace Web.Areas.Facturas.Services.Referencias
{
    public interface IFacturasServices
    {
        FacturasPageDataViewModel GetReferenciasPageAsync(string text, int page, int pageSize, string sort, bool @ascending, int? year);
        Task<List<int>> GetAvailableYearsAsync();
        Task<FacturasHeader> FindFacturaByIdAsync(int id);
        Task<FacturasHeader> FindFacturaByIdForEditAsync(int id);
        Task RemoveAsync(FacturasHeader factura);
        Task<bool> CreateFacturaAsync(FacturasHeader factura, string NombrePaciente);
        Task<bool> UpdateFacturaAsync(FacturasHeader factura, string NombrePaciente);
        Task<FacturacionViewModel> GetFacturacion(int year);
    }
}
