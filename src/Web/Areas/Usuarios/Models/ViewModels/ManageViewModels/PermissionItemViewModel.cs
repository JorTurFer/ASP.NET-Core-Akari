using AspNetCore.Identity.ByPermissions;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels
{
    public class PermissionItemViewModel : PermissionItem
    {
        public bool IsActive { get; set; }
    }
}
