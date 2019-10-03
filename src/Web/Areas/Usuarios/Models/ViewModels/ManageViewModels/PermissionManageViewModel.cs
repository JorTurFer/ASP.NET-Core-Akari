using System.Collections.Generic;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels
{
    public class PermissionManageViewModel
    {
        public IEnumerable<PermissionItemViewModel> PolicyItems { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<int> Groups { get; set; }
    }
}
