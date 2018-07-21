using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels
{
    public class EditUserRoleViewModel
    {
        public string Id { get; set; }

        public IEnumerable<UserRolesViewModel> Roles { get; set; }
    }
}
