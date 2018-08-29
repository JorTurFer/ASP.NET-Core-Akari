using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class PacienteDataViewModel
    {
        public Paciente Paciente { get; set; }
        public List<SelectListItem> Provincias { get; set; }
        public List<SelectListItem> Paises { get; set; }
    }
}
