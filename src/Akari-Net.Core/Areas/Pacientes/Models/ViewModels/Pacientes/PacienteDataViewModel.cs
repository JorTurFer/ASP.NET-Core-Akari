using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class PacienteDataViewModel
    {
        public Paciente Paciente { get; set; }
        public List<SelectListItem> Provincias { get; set; }
        public List<SelectListItem> Paises { get; set; }
    }
}
