using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ProfileViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

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
