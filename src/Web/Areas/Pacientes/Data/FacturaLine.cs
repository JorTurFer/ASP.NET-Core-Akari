using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Areas.Pacientes.Data
{
    [Table("FacturasLineas", Schema = "Facturas")]
    public class FacturaLine
    {
        [Key]
        public int IdLine { get; set; }

        [Required]
        public int IdFactura { get; set; }

        [Required]
        public string Concepto { get; set; }

        [Required]
        public double Precio { get; set; }
        
        [Required] 
        public int Cantidad { get; set; }
        

        //EFC
        public virtual FacturasHeader Factura { get; set; }
    }
}
