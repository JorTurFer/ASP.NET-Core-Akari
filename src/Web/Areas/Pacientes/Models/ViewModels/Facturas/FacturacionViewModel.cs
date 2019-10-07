using System.Collections.Generic;

namespace Web.Areas.Pacientes.Models.ViewModels.Facturas
{
    public class FacturacionViewModel
    {
        public FacturacionViewModel()
        {
            FacturacionData = new List<FacturacionItem>();
        }

        public List<FacturacionItem> FacturacionData { get; set; }
        public double Total { get; set; }
    }
}
