using Microsoft.Extensions.DependencyInjection;
using Web.Areas.Facturas.Services.Referencias;
using Web.Areas.Pacientes.Services.Facturas;

namespace Web.Areas.Facturas.Extensions
{
    public static class RegisterExtensions
    {
        public static IServiceCollection AddFacturas(this IServiceCollection services)
        {
            services.AddScoped<IReferenciasService, ReferenciasService>();
            services.AddScoped<IFacturasServices, FacturasServices>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            services.AddHttpClient();
            return services;
        }
    }
}
