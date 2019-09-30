using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Areas.Pacientes.Data
{
    [Table("Referencias", Schema = "Facturas")]
    public class Referencia
    {
        public Referencia()
        {
            Lineas = new HashSet<FacturaLine>();
        }
        [Key]
        public int IdReferencia { get; set; }
        [Required]
        public string Identificador { get; set; }
        [Required]
        public string Concepto { get; set; }
        [Required]
        public double Precio { get; set; }

        //EFC
        public virtual ICollection<FacturaLine> Lineas { get; set; }
    }
}
