using System.Collections.Generic;
using Web.Areas.Pacientes.Data;

namespace Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes
{
    public class HistorialPageDataViewModel
    {
        public int TotalRegistros { get; set; }
        public IEnumerable<Historial> Registros { get; set; }
    }
}
