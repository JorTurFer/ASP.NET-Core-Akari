using Akari_Net.Core.Areas.Usuarios.Models.Services;
using Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.Helpers
{
    public static class ClaimsManageHelper
    {
        public static ClaimsManageViewModel GetClaimsManageViewModel(IList<Claim> claims, IdentityRole role, IPoliciesManager policiesManager)
        {
            List<PolicyItemViewModel> policyItemViewModel = new List<PolicyItemViewModel>();
            foreach (var policy in policiesManager.GetPolicies())
            {
                //Tiene la politica la coleccion de claims
                var exist = claims.Any(x => string.Compare(x.Type, policy.PolicyName, true) == 0);
                policyItemViewModel.Add(new PolicyItemViewModel { Id = policy.Id, PolicyName = policy.PolicyName, PolicyDesiption = policy.PolicyDesiption, IsActive = exist });
            }
            return new ClaimsManageViewModel { policyItems = policyItemViewModel, roleId = role.Id };
        }
    }
}
