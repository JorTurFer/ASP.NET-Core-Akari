using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.ViewModels.ManageViewModels
{
    public class ClaimsManageViewModel
    {
        public IEnumerable<PolicyItemViewModel> policyItems { get; set; }
        public string roleId { get; set; }
    }
}
