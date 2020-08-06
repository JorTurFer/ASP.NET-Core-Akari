using System.Collections.Generic;
using Web.Areas.Pacientes.Data;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class GridHistorialViewModel
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public bool Ascending { get; set; }
        public bool InvertAscending => !Ascending;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRegistros { get; set; }
        public IEnumerable<Historial> Registros { get; set; }
        public int Id { get; set; }
    }
}
