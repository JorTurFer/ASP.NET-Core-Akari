using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Entities
{
    public class Paciente : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es necesario")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es necesario")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Nacimiento { get; set; }

        [EmailAddress(ErrorMessage = "El campo '{0}' debe ser un correo electrónico válido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "El campo '{0}' número de telefono valido")]
        [Display(Name = "Teléfono de contacto")]
        public string Telefono { get; set; }

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