using System.ComponentModel.DataAnnotations;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.AccountViewModels
{
    public class LoginWithRecoveryCodeViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Text)]
        [Display(Name = "Código de recuperación")]
        public string RecoveryCode { get; set; }
    }
}
