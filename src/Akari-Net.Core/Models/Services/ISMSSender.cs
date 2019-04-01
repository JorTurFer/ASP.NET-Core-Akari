using Akari_Net.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Models.Services
{
    interface ISMSSender
    {
        bool SendSMS(SMS sms);
    }
}
