using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario
{
    public class CalendarioViewModel
    {
        public List<SelectListItem> TipoCitas { get; set; }
    }
}
