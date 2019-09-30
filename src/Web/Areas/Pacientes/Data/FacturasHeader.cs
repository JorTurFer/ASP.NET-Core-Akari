using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Akari_Net.Core.Areas.Pacientes.Models.Data;

namespace Web.Areas.Pacientes.Data
{
    [Table("FacturasHeaders", Schema = "Facturas")]
    public class FacturasHeader
    {
        public FacturasHeader()
        {
            Lineas = new HashSet<FacturaLine>();
        }

        [Key]
        public int IdFactura { get; set; }
        [Required] 
        public int IdPaciente { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required]
        public double IRPF { get; set; }
        [Required]
        public double Descuento { get; set; }

        //EFC
        public virtual Paciente Paciente { get; set; }
        public ICollection<FacturaLine> Lineas { get; set; }
    }
}
