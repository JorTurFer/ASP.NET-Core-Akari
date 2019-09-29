using System.Collections.Generic;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Facturas.Entities.ViewModels
{
    public class GridFacturasViewModel
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public bool Ascending { get; set; }
        public bool InvertAscending => !Ascending;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalReferencias { get; set; }
        public int? Year { get; set; }
        public IEnumerable<FacturasHeader> FacturasHeaders { get; set; }
    }
}
