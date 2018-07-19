using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.Services
{
    public interface IPoliciesManager
    {
        IEnumerable<PolicyItem> GetPolicies();
    }
}
