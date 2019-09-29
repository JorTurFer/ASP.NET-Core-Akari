using System;
using Microsoft.AspNetCore.Identity;

namespace Web.Areas.Usuarios.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }
        public DateTime? Nacimiento { get; set; }
    }
}
