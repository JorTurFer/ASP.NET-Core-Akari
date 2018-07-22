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
            _policies.Add(new PolicyItem { Id = 0, PolicyName = "UsersManager", PolicyDesiption = "Gestión de Usuarios", });
            _policies.Add(new PolicyItem { Id = 1, PolicyName = "RolesManager", PolicyDesiption = "Gestión de Permisos", });
            _policies.Add(new PolicyItem { Id = 2, PolicyName = "CitasManager", PolicyDesiption = "Gestión de Citas", });
        }
        public IEnumerable<PolicyItem> GetPolicies()
        {
            return _policies;
        }

        public PolicyItem GetPolicyId(int Id)
        {
            return _policies.Where(x => x.Id == Id).FirstOrDefault();
        }
    }
}
