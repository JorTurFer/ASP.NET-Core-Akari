using System.Collections.Generic;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Entities.ViewModels
{
    public class FacturasPageDataViewModel
    {
        public int TotalFacturas { get; set; }
        public IEnumerable<FacturasHeader> Facturas { get; set; }
    }
}
