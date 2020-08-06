using Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Akari_Net.Core.Areas.Usuarios.Models.Helpers
{
    public static class PermissionsManageHelper
    {
        public static PermissionManageViewModel GetPermissionManageViewModel(IList<Claim> claims, IdentityRole role, IPermissionService permissionService)
        {
            var policyItemViewModel = new List<PermissionItemViewModel>();
            foreach (var policy in permissionService.GetPermissions())
            {
                if (policyItemViewModel.Any(x => x.PermissionName == policy.PermissionName)) continue;

                //Tiene la politica la coleccion de claims
                var exist = claims.Any(x => String.Compare(x.Type, policy.PermissionName, true) == 0);

                policyItemViewModel.Add(new PermissionItemViewModel { PermissionId = policy.PermissionId, PermissionName = policy.PermissionName, PermissionDescription = policy.PermissionDescription, IsActive = exist, PermissionGroup = policy.PermissionGroup });
            }
            return new PermissionManageViewModel { PolicyItems = policyItemViewModel, RoleId = role.Id, Groups = policyItemViewModel.GroupBy(x => x.PermissionGroup).Select(x => x.Key) };
        }
    }
}
