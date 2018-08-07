using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [StringLength(7, ErrorMessage = "El {0} debe contener entre {2} y {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Código de autenticación")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Recordar esta máquina")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
