using System.Collections.Generic;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class PacientesPageDataViewModel
    {
        public int TotalPacientes { get; set; }
        public IEnumerable<PacienteViewModel> Pacientes { get; set; }
    }
}
