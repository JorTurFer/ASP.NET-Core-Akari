using Web.Models.Entities;

namespace Web.Models.Services
{
    internal interface ISmsSender
    {
        bool SendSms(Sms sms);
    }
}
