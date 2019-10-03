using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ProfileViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckEmailIsAvailable", HttpMethod = "POST")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "{0} debe ser un número de teléfono válido")]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
