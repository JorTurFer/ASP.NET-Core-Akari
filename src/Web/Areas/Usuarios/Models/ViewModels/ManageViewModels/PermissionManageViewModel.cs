using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels
{
    public class PermissionManageViewModel
    {
        public IEnumerable<PermissionItemViewModel> PolicyItems { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<int> Groups { get; set; }
    }
}
