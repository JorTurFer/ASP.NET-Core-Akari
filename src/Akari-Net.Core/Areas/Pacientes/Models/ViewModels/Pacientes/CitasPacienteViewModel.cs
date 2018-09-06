using Akari_Net.Core.Areas.Pacientes.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class CitasPacienteViewModel
    {
        public IEnumerable<CalendarEvent> Citas { get; set; }
        public Paciente Paciente { get; set; }
    }
}
