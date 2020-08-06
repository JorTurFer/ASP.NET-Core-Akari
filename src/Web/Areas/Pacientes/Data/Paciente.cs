using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Areas.Pacientes.Data;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    [Table("Pacientes", Schema = "Patients")]
    public class Paciente : IValidatableObject
    {
        [Key]
        [Display(Name = "Nº de historia")]
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
        [Display(Name = "Teléfono fijo")]
        public string Telefono1 { get; set; }

        [Phone(ErrorMessage = "El campo '{0}' número de telefono valido")]
        [Display(Name = "Teléfono movil")]
        public string Telefono2 { get; set; }

        [Display(Name = "DNI")]
        public string DNI { get; set; } = "";

        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = "";

        [Range(0, 99999, ErrorMessage = "Introduce un valor válido")]
        [Display(Name = "Código Postal")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:00000}")]
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
        public virtual Provincia Provincia { get; set; }
        public virtual Pais Pais { get; set; }
        public virtual ICollection<CalendarEvent> Citas { get; set; }
        public virtual ICollection<FacturasHeader> Facturas { get; set; }
        public virtual ICollection<Historial> Registros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrWhiteSpace(Telefono1) && String.IsNullOrWhiteSpace(Telefono2) && String.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult(
                    "Es necesario indicar un método de contacto",       // Error message
                    new[] { "Telefono", "Email" });                     // Array with invalid properties
            }
        }
    }
}