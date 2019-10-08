using System.IO;
using System.Threading.Tasks;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Pacientes.Services.Facturas
{
    public interface IPdfGenerator
    {
        Task<Stream> GeneratePdf(FacturasHeader factura);
    }
}
