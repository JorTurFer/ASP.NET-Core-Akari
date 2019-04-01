using Akari_Net.Core.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Akari_Net.Core.Models.Services
{
    public class SMSSender : ISMSSender
    {
        private readonly ILogger<SMSSender> _logger;

        public SMSSender(ILogger<SMSSender> logger, IOptions<SMSServiceOptions> optionsAccessor)
        {
            _logger = logger;
            Options = optionsAccessor.Value;
        }

        public SMSServiceOptions Options { get; } //set only via Secret Manager

        public bool SendSMS(SMS sms)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.QueryString.Add("username", Options.LabUser);
                client.QueryString.Add("password", Options.LabPassword);
                client.QueryString.Add("msisdn", sms.msisdn ?? Options.LabDefaultPhone);
                client.QueryString.Add("message", sms.message);
                string baseurl = "http://api.labsmobile.com/get/send.php";
                using (Stream data = client.OpenRead(baseurl))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        string s = reader.ReadToEnd();
                        return Regex.IsMatch(s, @"<code>0<\/code>");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
