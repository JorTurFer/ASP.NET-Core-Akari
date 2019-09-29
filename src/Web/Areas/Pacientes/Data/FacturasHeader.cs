using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Akari_Net.Core.Areas.Pacientes.Models.Data;

namespace Web.Areas.Pacientes.Data
{
    [Table("FacturasHeaders", Schema = "Facturas")]
    public class FacturasHeader
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public int IdPaciente { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required]
        public double IRPF { get; set; }
        [Required]
        public double Descuento { get; set; }

        //EFC
        public virtual Paciente Paciente { get; set; }
    }
}
