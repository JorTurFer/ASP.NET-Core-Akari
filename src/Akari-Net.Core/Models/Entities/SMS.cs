using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Models.Entities
{

    public class SMS
    {
        //Mensaje
        public string message { get; set; }
        //Desinatario
        public string msisdn { get; set; }

    }
}
