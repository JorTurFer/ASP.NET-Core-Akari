using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Extensions
{
    public static class PoliciesExtensions
    {
        public static void AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy("UsersManager", policy => policy.RequireRole("WebMaster", "Podólogo"));
        }
    }
}
