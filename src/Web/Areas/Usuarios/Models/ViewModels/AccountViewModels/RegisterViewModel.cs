﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckUsernameIsAvailable", HttpMethod = "POST")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckEmailIsAvailable", HttpMethod = "POST")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Remote(areaName: "Usuarios", controller: "Account", action: "CheckPassword", HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
