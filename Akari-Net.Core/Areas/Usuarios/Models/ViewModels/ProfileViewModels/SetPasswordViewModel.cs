using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ProfileViewModels
{
    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckPassword", HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nuevas contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
