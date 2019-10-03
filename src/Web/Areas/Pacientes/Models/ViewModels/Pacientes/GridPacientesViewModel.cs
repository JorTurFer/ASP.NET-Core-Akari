using System.Collections.Generic;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class GridPacientesViewModel
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public bool Ascending { get; set; }
        public bool InvertAscending => !Ascending;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPacientes { get; set; }
        public IEnumerable<PacienteViewModel> Pacientes { get; set; }
    }
}
