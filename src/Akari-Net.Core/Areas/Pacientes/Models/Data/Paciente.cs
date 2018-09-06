using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    public class Paciente : IValidatableObject
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es necesario")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Nacimiento { get; set; }

        [EmailAddress(ErrorMessage = "El campo '{0}' debe ser un correo electrónico válido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "El campo '{0}' número de telefono valido")]
        [Display(Name = "Teléfono de contacto")]
        public string Telefono { get; set; }

        [Display(Name = "DNI")]
        public string DNI { get; set; } = "";

        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = "";

        [Range(0,99999,ErrorMessage = "Introduce un valor válido")]
        [Display(Name = "Código Postal")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:00000}")]
        public int CP { get; set; } = 0;

        [Display(Name = "Provincia")]
        [DataType(DataType.PostalCode)]
        public int IdProvincia { get; set; } = 0;

        [Display(Name = "Pais")]
        public int IdPais { get; set; } = 0;

        [Display(Name = "RGPD")]
        public bool RGPD { get; set; } = false;

        [Display(Name = "Enfermedades")]
        [DataType(DataType.MultilineText)]
        public string Enfermedades { get; set; } = "";

        [Display(Name = "Medicación")]
        [DataType(DataType.MultilineText)]
        public string Medicación { get; set; } = "";

        [Display(Name = "Alergias")]
        [DataType(DataType.MultilineText)]
        public string Alergias { get; set; } = "";

        //Relaciones EF
        public virtual Provincia Provincia {get;set;}
        public virtual Pais Pais { get; set; }
        public ICollection<CalendarEvent> Citas { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Telefono) && string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult(
                    "Es necesario indicar un método de contacto",       // Error message
                    new[] { "Telefono", "Email" });                     // Array with invalid properties
            }
        }
    }
}