using Microsoft.AspNetCore.Authorization;
using System;

namespace Akari_Net.Core.Areas.Usuarios.Models.Attributes
{
    public class AuthorizePolicyAttribute : AuthorizeAttribute
    {
        public AuthorizePolicyAttribute() : base()
        {
        }

        public AuthorizePolicyAttribute(string policy) : base(policy)
        {
        }

        public string Description { get; set; }
    }
}