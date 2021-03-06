﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckPassword", HttpMethod = "POST")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma contraseña")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
