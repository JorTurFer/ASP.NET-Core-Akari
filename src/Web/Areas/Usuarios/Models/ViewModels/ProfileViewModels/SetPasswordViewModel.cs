using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ProfileViewModels
{
    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckPassword", HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme contraseña")]
        [Compare("Password", ErrorMessage = "La nuevas contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
