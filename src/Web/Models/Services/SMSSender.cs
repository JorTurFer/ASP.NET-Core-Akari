using Akari_Net.Core.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Web.Models.Entities;

namespace Web.Models.Services
{
    public class SmsSender : ISmsSender
    {
        private readonly ILogger<SmsSender> _logger;

        public SmsSender(ILogger<SmsSender> logger, IOptions<SMSServiceOptions> optionsAccessor)
        {
            _logger = logger;
            Options = optionsAccessor.Value;
        }

        public SMSServiceOptions Options { get; } //set only via Secret Manager

        public bool SendSms(Sms sms)
        {
            try
            {
                var client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.QueryString.Add("username", Options.LabUser);
                client.QueryString.Add("password", Options.LabPassword);
                client.QueryString.Add("msisdn", sms.msisdn ?? Options.LabDefaultPhone);
                client.QueryString.Add("message", sms.message);
                var baseurl = "http://api.labsmobile.com/get/send.php";
                using (var data = client.OpenRead(baseurl))
                {
                    using (var reader = new StreamReader(data))
                    {
                        var s = reader.ReadToEnd();
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
