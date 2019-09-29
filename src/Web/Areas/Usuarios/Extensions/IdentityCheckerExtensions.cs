using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Usuarios.Data;

namespace Akari_Net.Core.Extensions
{
    public static class IdentityCheckerExtensions
    {
        public static async Task<bool> IsUserNameAvalilable(this UserManager<ApplicationUser> userManager, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            return user is null;
        }

        public static async Task<bool> IsEmailAvalilable(this UserManager<ApplicationUser> userManager, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user is null;
        }
        public static string GetEmail(this UserManager<ApplicationUser> userManager, ClaimsPrincipal claims)
        {            
            return userManager.GetUserAsync(claims).Result.Email;
        }
    }
}
