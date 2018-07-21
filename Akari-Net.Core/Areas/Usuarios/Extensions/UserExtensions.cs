﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim.Value;
        }
    }
}
