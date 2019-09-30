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
        public int IdReferencia { get; set; }
        
        [Required] 
        public int Cantidad { get; set; }

        

        //EFC
        public virtual FacturasHeader Factura { get; set; }
        public virtual Referencia Referencia { get; set; }
    }
}
