using Akari_Net.Core.Areas.Pacientes.Models.Data;
using System.Collections.Generic;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class CitasPacienteViewModel
    {
        public IEnumerable<CalendarEvent> Citas { get; set; }
        public Paciente Paciente { get; set; }
    }
}
