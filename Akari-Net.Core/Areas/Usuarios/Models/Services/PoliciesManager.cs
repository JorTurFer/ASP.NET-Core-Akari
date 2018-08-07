using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Usuarios.Models.Attributes;
using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Akari_Net.Core.Areas.Usuarios.Models.Services
{
    public class PoliciesManager : IPoliciesManager
    {
        static readonly List<PolicyItem> _policies = null;

        static PoliciesManager()
        {
            _policies = new List<PolicyItem>();
            //Get the assembly
            Assembly asm = Assembly.GetExecutingAssembly();

            //Get the controllers
            var controllers = asm.GetTypes()
            .Where(type => typeof(Controller).IsAssignableFrom(type)); //filter controllers

            int nGroup = 0;
            foreach (var controller in controllers)
            {
                //Get the Policy attribute (if the controller has got it)
                var controllerPolicy = controller.GetCustomAttribute<AuthorizePolicyAttribute>();
                //If the controller has PolicyAttribute, we register it
                if (controllerPolicy != null)
                    _policies.Add(new PolicyItem { Id = _policies.Count, PolicyName = controllerPolicy.Policy, PolicyDesiption = controllerPolicy.Description,PolicyGroup = nGroup });

                var methodPolicies = controller.GetMethods()
                .Where(type => type.CustomAttributes.Any(x => x.AttributeType == (typeof(AuthorizePolicyAttribute))))
                .Select(x => x.GetCustomAttribute<AuthorizePolicyAttribute>()).Distinct();
                foreach (var methodPolicy in methodPolicies)
                {
                    _policies.Add(new PolicyItem { Id = _policies.Count, PolicyName = methodPolicy.Policy, PolicyDesiption = methodPolicy.Description, PolicyGroup = nGroup });
                }
                nGroup++;
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
