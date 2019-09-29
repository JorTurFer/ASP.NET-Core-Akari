using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Areas.Pacientes.Data
{
    [Table("Referencias", Schema = "Facturas")]
    public class Referencia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Identificador { get; set; }
        [Required]
        public string Concepto { get; set; }
        [Required]
        public double Precio { get; set; }
    }
}
