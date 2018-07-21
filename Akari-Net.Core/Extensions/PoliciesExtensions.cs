using Akari_Net.Core.Areas.Usuarios.Models.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Extensions
{
    public static class PoliciesExtensions
    {
        public static void AddPolicies(this AuthorizationOptions options, PoliciesManager manager )
        {
            foreach (var policyItem in manager.GetPolicies())
            {
                options.AddPolicy(policyItem.PolicyName, policy => policy.RequireClaim(policyItem.PolicyName, policyItem.PolicyName).Build());
            }
        }
    }
}
