using Microsoft.Extensions.DependencyInjection;
using Web.Areas.Facturas.Services.Referencias;

namespace Web.Areas.Facturas.Extensions
{
    public static class RegisterExtensions
    {
        public static IServiceCollection AddFacturas(this IServiceCollection services)
        {
            services.AddScoped<IReferenciasService, ReferenciasService>();

            return services;
        }
    }
}
