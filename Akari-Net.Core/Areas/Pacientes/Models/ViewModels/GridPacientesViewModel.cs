using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels
{
    public class GridPacientesViewModel
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public bool Ascending { get; set; }
        public bool InvertAscending { get => !Ascending; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPacientes { get; set; }
        public IEnumerable<PacienteViewModel> Pacientes { get; set; }
    }
}
