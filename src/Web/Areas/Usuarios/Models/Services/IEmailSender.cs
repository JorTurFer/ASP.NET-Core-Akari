﻿using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
