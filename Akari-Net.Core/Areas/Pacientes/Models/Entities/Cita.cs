using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Citas.Models.Entities
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdPaciente { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public int Duracion { get; set; }

        public string Descripcion { get; set; }

        [ForeignKey("IdPaciente")]
        public virtual Paciente Paciente { get; set; }
    }
}
