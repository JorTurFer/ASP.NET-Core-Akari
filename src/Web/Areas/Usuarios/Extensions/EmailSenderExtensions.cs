using Akari_Net.Core.Areas.Usuarios.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Akari_Net.Core.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirma tu email",
                $"Por favor, confirma tu email haciendo click en enlace: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}