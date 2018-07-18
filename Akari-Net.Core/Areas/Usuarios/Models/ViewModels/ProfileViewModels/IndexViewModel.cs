using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ProfileViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "{0} debe ser un número de teléfono válido")]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
