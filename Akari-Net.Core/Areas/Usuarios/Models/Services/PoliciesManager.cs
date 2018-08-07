using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Usuarios.Models.Attributes;
using Akari_Net.Core.Areas.Usuarios.Models.Entities;

namespace Akari_Net.Core.Areas.Usuarios.Models.Services
{
    public class PoliciesManager : IPoliciesManager
    {
        readonly List<PolicyItem> _policies = new List<PolicyItem>();
        public PoliciesManager()
        {
            //Get the assembly
            Assembly asm = Assembly.GetExecutingAssembly();

            var policies = asm.GetTypes()
                .Where(type => type.CustomAttributes.Any(x => x.AttributeType == (typeof(AuthorizePolicyAttribute)))).Select(x=>x.GetCustomAttribute<AuthorizePolicyAttribute>());
            foreach (var policy in policies)
            {
                _policies.Add(new PolicyItem { Id = _policies.Count, PolicyName = policy.Policy, PolicyDesiption = policy.Description });
            }
              
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
