using System.Collections.Generic;
using Web.Areas.Facturas.Data;

namespace Web.Areas.Facturas.Entities.ViewModels
{
    public class GridReferenciasViewModel
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public bool Ascending { get; set; }
        public bool InvertAscending => !Ascending;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalReferencias { get; set; }
        public IEnumerable<Referencia> Referencias { get; set; }
    }
}
