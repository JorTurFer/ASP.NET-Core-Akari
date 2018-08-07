using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.Entities
{
    public class PolicyItem
    {
        //Policy Id
        public int Id { get; set; }
        //Name to register
        public string PolicyName { get; set; }
        //Description to show
        public string PolicyDesiption { get; set; }
        //Group of the policy
        public int PolicyGroup { get; set; }
    }
}
