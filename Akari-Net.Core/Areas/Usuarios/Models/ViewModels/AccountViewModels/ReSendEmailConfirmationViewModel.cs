using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.AccountViewModels
{
    public class ReSendEmailConfirmationViewModel
    {
        [Required(ErrorMessage = "Introduce una dirección válida")]
        [EmailAddress(ErrorMessage = "Introduce una dirección válida")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}