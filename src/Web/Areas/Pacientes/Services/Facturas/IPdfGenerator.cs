using System.IO;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Pacientes.Services.Facturas
{
    public interface IPdfGenerator
    {
        Stream GeneratePdf(FacturasHeader factura, string wwwroot);
    }
}
