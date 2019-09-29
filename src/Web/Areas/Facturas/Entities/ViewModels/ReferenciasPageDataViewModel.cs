using System.Collections.Generic;
using Web.Areas.Facturas.Data;

namespace Web.Areas.Facturas.Entities.ViewModels
{
    public class ReferenciasPageDataViewModel
    {
        public int TotalReferencias { get; set; }
        public IEnumerable<Referencia> Referencias { get; set; }
    }
}
