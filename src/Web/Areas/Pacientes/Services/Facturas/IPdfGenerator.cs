using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Pacientes.Services.Facturas
{
    public interface IPdfGenerator
    {
        Stream GeneratePdf(FacturasHeader factura, string wwwroot);
    }
}
