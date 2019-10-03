using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}
