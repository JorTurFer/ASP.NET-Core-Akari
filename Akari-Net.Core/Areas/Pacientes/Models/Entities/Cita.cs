using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Entities
{
    public class Cita
    {
        [Key]
        public int ID { get; set; }

        [StringLength(100,ErrorMessage = "La longitud máxima es {1}")]
        [Display(Name = "Cabecera")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatiorio")]
        [StringLength(300, ErrorMessage = "La longitud máxima es {1}")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Inicio")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatiorio")]
        [Display(Name = "Fin")]
        public DateTime End { get; set; }

        [StringLength(10, ErrorMessage = "La longitud máxima es {1}")]
        public string ThemeColor { get; set; }

        [Display(Name = "¿Todo el día?")]
        public bool IsFullDay { get; set; }

        //En caso de ser una cita, id del paciente para el que es
        public int? IdPaciente { get; set; }
        
    }
}
