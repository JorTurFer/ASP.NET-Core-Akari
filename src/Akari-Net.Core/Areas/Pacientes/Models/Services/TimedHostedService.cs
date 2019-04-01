using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Models.Entities;
using Akari_Net.Core.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IServiceProvider _services;
        private readonly ISMSSender _smsService;
        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider services, ISMSSender smsService)
        {
            _logger = logger;
            _services = services;
            _smsService = smsService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("El servicio de aviso de citas se pone en marcha");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(300));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<PacientesDbContext>();

                var citasParaAvisar = _context.CalendarEvents.Where(x => !x.IsSMSSended && x.Start > DateTime.Now && (x.Start - DateTime.Now).Days < 1).Include(x => x.Paciente).Include(x=>x.TipoCita);
                foreach (var cita in citasParaAvisar)
                {
                    if (cita.Paciente is null && cita.TipoCita.Tipo == "Personal")
                    {
                        SMS mensaje = new SMS();
                        mensaje.message = $"Tienes {cita.Subject ?? "SinSujeto"}-{cita.Description ?? "SinDescripcion"} el día {cita.Start.ToString("dd-MM-yyyy")} a las {cita.Start.ToString("HH:mm")}";
                        mensaje.msisdn = null;
                        cita.IsSMSSended = _smsService.SendSMS(mensaje);
                    }
                    //TODO Añadir esta parte cuando Nuria lo vea bien
                    //else if (!(cita.Paciente.Telefono2 is null))
                    //{
                    //    string strMessage = $"Le recordamos su cita podológica el {cita.Start.ToString("dd-MM-yyyy")} a las {cita.Start.ToString("HH:mm")}";
                    //    cita.IsSMSSended = _smsService.SendSMS(cita.Paciente.Telefono2, "Akari Podología", strMessage);
                    //}
                }
                await _context.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("El servicio de aviso de citas se pone en marcha se detiene");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
