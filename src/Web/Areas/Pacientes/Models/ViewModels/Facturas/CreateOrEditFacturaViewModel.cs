using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Pacientes.Models.ViewModels.Facturas
{
    public class CreateOrEditFacturaViewModel
    {
        public FacturasHeader Factura { get; set; }
        public FacturaLine Lineas { get; set; }

        [DisplayName("Paciente")]
        [Required]
        public string NombrePaciente { get; set; }
    }
}
