using Akari_Net.Core.Areas.Pacientes.Models.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Areas.Pacientes.Data
{
    [Table("Historial", Schema = "Patients")]
    public class Historial
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdPaciente { get; set; }
        [Required]
        public string Registro { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        //EFC
        public virtual Paciente Paciente { get; set; }
    }
}
