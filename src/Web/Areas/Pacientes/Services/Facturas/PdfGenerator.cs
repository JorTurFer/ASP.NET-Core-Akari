using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Web.Areas.Facturas.Services.Referencias;
using Web.Areas.Pacientes.Data;
using Web.Areas.Pacientes.Entities.ZipCode;

namespace Web.Areas.Pacientes.Services.Facturas
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IFacturasServices _facturasServices;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpClientFactory _clientFactory;

        public PdfGenerator( IFacturasServices facturasServices, IHostingEnvironment hostingEnvironment, IHttpClientFactory clientFactory)
        {
            _facturasServices = facturasServices;
            _hostingEnvironment = hostingEnvironment;
            _clientFactory = clientFactory;
        }

        public async Task<Stream> GeneratePdf(FacturasHeader factura)
        {
            var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 70, 70, 70, 70);
            var writer = PdfWriter.GetInstance(document, stream);

            // First, create our fonts
            var datosPropiosFont = FontFactory.GetFont("Arial", 8);
            var metadatosFont = FontFactory.GetFont("Arial", 11);
            var totalFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            var clausulaFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var pageSize = writer.PageSize;

            document.Open();

            //Logo
            var logo = iTextSharp.text.Image.GetInstance(File.Open(Path.Combine(_hostingEnvironment.WebRootPath, "logo", "akari.jfif"), FileMode.Open));
            logo.SetAbsolutePosition(pageSize.GetLeft(45), 650);
            logo.ScalePercent(12f);
            document.Add(logo);

            #region Metadatos
            var tablaMetadatos = new PdfPTable(1)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100
            };
            tablaMetadatos.DefaultCell.Border = Rectangle.NO_BORDER;

            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            tablaMetadatos.AddCell("");
            var celda = new PdfPCell(new Phrase($"Factura: {factura.Codigo}", metadatosFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = 0
            };
            tablaMetadatos.AddCell(celda);
            celda = new PdfPCell(new Phrase($"Fecha: {factura.Fecha.ToString("dd-MM-yyyy")}", metadatosFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = 0
            };
            tablaMetadatos.AddCell(celda);
            //document.Add(tablaMetadatos);
            #endregion

            //Datos propios
            #region Datos propios
            var emptyCell = new PdfPCell(new Phrase(""))
            {
                Border = Rectangle.NO_BORDER
            };

            var cabecera = new PdfPTable(3)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100,
            };
            cabecera.DefaultCell.Border = Rectangle.NO_BORDER;
            cabecera.SetWidths(new float[] { 4, 4, 3 });

            cabecera.AddCell(tablaMetadatos);
            cabecera.AddCell(emptyCell);

            var datosPropios = new PdfPTable(1);
            datosPropios.DefaultCell.Border = Rectangle.NO_BORDER;

            celda = new PdfPCell(new Phrase("Nuria Turrado Ferrero", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("NIF: 72737422Z", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("Nº Colegiado: 270", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("Colegio: Euskadi", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("AKARI PODOLOGIA", datosPropiosFont))
            {
                Border = Rectangle.TOP_BORDER,
                BorderWidth = 1
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("945 23 40 61", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("695 82 37 77", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("Senda del rio Ali, 2", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            celda = new PdfPCell(new Phrase("01015 Vitoria-Gasteiz, Álava", datosPropiosFont))
            {
                Border = Rectangle.NO_BORDER
            };
            datosPropios.AddCell(celda);
            cabecera.AddCell(datosPropios);
            document.Add(cabecera);

            #endregion

            #region Paciente
            // Create the header table 
            var seccionPaciente = new PdfPTable(2)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100
            };
            seccionPaciente.SetWidths(new float[] { 5, 5 });  // then set the column's __relative__ widths
            seccionPaciente.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            seccionPaciente.SpacingAfter = 30;
            var pacienteTabla = new PdfPTable(1);
            pacienteTabla.DefaultCell.Border = Rectangle.BOX;
            var nombrePaciente = new PdfPCell(new Phrase($"{factura.Paciente.Nombre}", bodyFont))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER
            };
            pacienteTabla.AddCell(nombrePaciente);
            var telefonoPaciente = new PdfPCell(new Phrase($"NIF:{factura.Paciente.DNI ?? ""}", bodyFont))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
            };
            pacienteTabla.AddCell(telefonoPaciente);


            var ciudad = await GetCiudad(factura.Paciente.CP);
            var direccionPaciente = new PdfPCell(new Phrase($"{factura.Paciente.Direccion} {factura.Paciente.CP:D5} {ciudad}", bodyFont))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
            };
            pacienteTabla.AddCell(direccionPaciente);
            seccionPaciente.AddCell(pacienteTabla);

            seccionPaciente.AddCell("");
            document.Add(seccionPaciente);
            #endregion

            #region Items Table
            //Create body table
            var itemTable = new PdfPTable(4)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100
            };
            itemTable.SetWidths(new float[] { 55, 15, 15, 15 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.NO_BORDER;
            var cell1 = new PdfPCell(new Phrase("Descripción", boldTableFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.BOTTOM_BORDER
            };
            itemTable.AddCell(cell1);
            var cell2 = new PdfPCell(new Phrase("Cantidad", boldTableFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.BOTTOM_BORDER
            };
            itemTable.AddCell(cell2);
            var cell3 = new PdfPCell(new Phrase("Precio unitario", boldTableFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.BOTTOM_BORDER
            };
            itemTable.AddCell(cell3);
            var cell4 = new PdfPCell(new Phrase("Total (€)", boldTableFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.BOTTOM_BORDER
            };
            itemTable.AddCell(cell4);

            foreach (var row in factura.Lineas)
            {
                var numberCell = new PdfPCell(new Phrase(row.Concepto, bodyFont))
                {
                    HorizontalAlignment = 0,
                    PaddingLeft = 10f,
                    Border = Rectangle.BOTTOM_BORDER
                };
                itemTable.AddCell(numberCell);

                var descCell = new PdfPCell(new Phrase(row.Cantidad.ToString(), bodyFont))
                {
                    HorizontalAlignment = 2,
                    PaddingLeft = 10f,
                    Border = Rectangle.BOTTOM_BORDER
                };
                itemTable.AddCell(descCell);

                var qtyCell = new PdfPCell(new Phrase(row.Precio.ToString("0.00"), bodyFont))
                {
                    HorizontalAlignment = 2,
                    PaddingLeft = 10f,
                    Border = Rectangle.BOTTOM_BORDER
                };
                itemTable.AddCell(qtyCell);

                var amtCell = new PdfPCell(new Phrase((row.Cantidad * row.Precio).ToString("0.00"), bodyFont))
                {
                    HorizontalAlignment = 2,
                    Border = Rectangle.BOTTOM_BORDER
                };
                itemTable.AddCell(amtCell);

            }
            document.Add(itemTable);
            #endregion

            #region Footer
            var cb = writer.DirectContent;
            var footerTable = new PdfPTable(6)
            {
                HorizontalAlignment = 0,
                TotalWidth = 500f
            };
            footerTable.SetWidths(new float[] { 20, 20, 20, 10, 15, 15 });  // then set the column's __relative__ widths
            footerTable.DefaultCell.Border = Rectangle.NO_BORDER;

            var sumaCell = new PdfPCell(new Phrase("Importe", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(sumaCell);
            var descuentoCell = new PdfPCell(new Phrase($"{factura.Descuento}% Desc", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(descuentoCell);
            var irpfCell = new PdfPCell(new Phrase($"{factura.IRPF}% IRPF", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(irpfCell);
            var ivaCell = new PdfPCell(new Phrase("% IVA", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(ivaCell);
            var cuotaIvaCell = new PdfPCell(new Phrase("Cuota IVA", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(cuotaIvaCell);
            var totalCell = new PdfPCell(new Phrase("Total", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(totalCell);

            var totalBruto = factura.Lineas.Sum(x => x.Precio * x.Cantidad);

            sumaCell = new PdfPCell(new Phrase($"{totalBruto:0.00}  €", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(sumaCell);

            var descuento = (totalBruto * factura.Descuento) / 100.0;
            descuentoCell = new PdfPCell(new Phrase($"{descuento:0.00}  €", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(descuentoCell);


            var descontado = totalBruto - descuento;
            var irpf = (descontado * factura.IRPF) / 100.0;
            irpfCell = new PdfPCell(new Phrase($"{irpf:0.00} €", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(irpfCell);
            ivaCell = new PdfPCell(new Phrase("EXENTO", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(ivaCell);
            cuotaIvaCell = new PdfPCell(new Phrase("0.00 €", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(cuotaIvaCell);

            var totalSuma = factura.Lineas.Sum(x => x.Precio * x.Cantidad);
            var totalDescuento = totalSuma * (1 - factura.Descuento / 100.0);
            var totalFinal = totalDescuento * (1 - factura.IRPF / 100.0);

            totalCell = new PdfPCell(new Phrase($"{totalFinal:0.00} €", bodyFont))
            {
                HorizontalAlignment = 0,
                Border = Rectangle.NO_BORDER
            };
            footerTable.AddCell(totalCell);


            footerTable.WriteSelectedRows(0, -1, 75, 150, cb);

            #endregion

            #region Total
            var totalTable = new PdfPTable(2)
            {
                HorizontalAlignment = 0,
                TotalWidth = 200f
            };
            totalTable.SetWidths(new float[] { 5, 5 });  // then set the column's __relative__ widths
            totalTable.DefaultCell.Border = Rectangle.NO_BORDER;
            var sumaTotalCell = new PdfPCell(new Phrase($"SUMA: ", totalFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = 2
            };
            totalTable.AddCell(sumaTotalCell);
            var valorSumaTotalCell = new PdfPCell(new Phrase($"{ totalFinal:0.00} €", totalFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = 2
            };
            totalTable.AddCell(valorSumaTotalCell);
            totalTable.WriteSelectedRows(0, -1, 349, 100, cb);
            #endregion

            #region Disclaimer

            cb.BeginText();
            var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(75, 50);
            cb.ShowText("*Los servicios sanitarios estan exentos de IVA");
            cb.EndText();

            #endregion


            writer.CloseStream = false; //set the closestream property
                                        // Close the Document without closing the underlying stream
            document.Close();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private async Task<string> GetCiudad(int postalCode)
        {
            var ciudad = await _facturasServices.GetCityAsync(postalCode);
            if(!(ciudad is null))
            {
                return ciudad.Localidad;
            }

            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"http://api.zippopotam.us/es/{postalCode:D5}");
            if (!response.IsSuccessStatusCode)
            {
                return "Unkown";
            }

            var message = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ZipCodeApi>(message);
            if (apiResponse is null || apiResponse.Places is null || apiResponse.Places.Length == 0)
            {
                return "Unkown";
            }

            var newCiudad = new Ciudad
            {
                Comunidad = apiResponse.Places[0].State,
                ZipCode = postalCode.ToString("D5"),
                Localidad = apiResponse.Places[0].PlaceName
            };

            await _facturasServices.InsertCityAsync(newCiudad);

            return apiResponse.Places[0].PlaceName;
        }
    }
}
