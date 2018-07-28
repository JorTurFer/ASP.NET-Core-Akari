using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels
{
    public class PacientesPageDataViewModel
    {
        public int TotalPacientes { get; set; }
        public IEnumerable<PacienteViewModel> Pacientes { get; set; }
    }
}
