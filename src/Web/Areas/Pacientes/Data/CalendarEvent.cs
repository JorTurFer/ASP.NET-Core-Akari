using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    [Table("CalendarEvents", Schema = "Patients")]
    public class CalendarEvent
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatiorio")]
        [StringLength(100, ErrorMessage = "La longitud máxima es {1}")]
        [Display(Name = "Cabecera")]
        public string Subject { get; set; }

        [StringLength(300, ErrorMessage = "La longitud máxima es {1}")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatiorio")]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime Start { get; set; }

        [Display(Name = "Fin")]
        public DateTime End { get; set; }

        [Display(Name = "¿Todo el día?")]
        public bool IsFullDay { get; set; }

        public string ThemeColor
        {
            get
            {
                if (TipoCita != null)
                {
                    return TipoCita.Color;
                }
                else
                {
                    return "blue";
                }
            }
        }

        //En caso de ser una cita, id del paciente para el que es
        public int? IdPaciente { get; set; }
        //En caso de ser una cita, id del tipo de cita
        public int? IdTipoCita { get; set; }
        //Se ha enviado el mensaje ya?
        public bool IsSMSSended { get; set; }
        //EF
        public virtual Paciente Paciente { get; set; }

        public virtual TipoCita TipoCita { get; set; }
    }
}