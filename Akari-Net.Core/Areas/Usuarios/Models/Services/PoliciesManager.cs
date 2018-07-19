using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Usuarios.Models.Entities;

namespace Akari_Net.Core.Areas.Usuarios.Models.Services
{
    public class PoliciesManager : IPoliciesManager
    {
        List<PolicyItem> _policies = new List<PolicyItem>();
        public PoliciesManager()
        {
            _policies.Add(new PolicyItem { Id = _policies.Count, PolicyName = "UsersManager", PolicyDesiption = "Gestión de Usuarios", });
            _policies.Add(new PolicyItem { Id = _policies.Count, PolicyName = "RolesManager", PolicyDesiption = "Gestión de Permisos", });
        }
        public IEnumerable<PolicyItem> GetPolicies()
        {
            return _policies;
        }
    }
}
