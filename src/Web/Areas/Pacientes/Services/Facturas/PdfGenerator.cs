using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using Web.Areas.Pacientes.Data;

namespace Web.Areas.Pacientes.Services.Facturas
{
    public class PdfGenerator : IPdfGenerator
    {
        public Stream GeneratePdf(FacturasHeader factura, string wwwroot)
        {
            var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 70, 70, 70, 70);
            var writer = PdfWriter.GetInstance(document, stream);

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            var clausulaFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here

            #region Top table
            // Create the header table 
            var headertable = new PdfPTable(3)
            {
                HorizontalAlignment = 0,
                WidthPercentage = 100
            };
            headertable.SetWidths(new float[] { 4, 2, 4 });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            headertable.SpacingAfter = 30;
            var nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.BOX;
            var nextPostCell1 = new PdfPCell(new Phrase("Jorge Turrado Ferrero", bodyFont))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
            };
            nested.AddCell(nextPostCell1);
            var nextPostCell2 = new PdfPCell(new Phrase("Andalucia 35 5C", bodyFont))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
            };
            nested.AddCell(nextPostCell2);
            var nextPostCell3 = new PdfPCell(new Phrase("Vitoria 01002", bodyFont))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
            };
            nested.AddCell(nextPostCell3);
            var nesthousing = new PdfPCell(nested)
            {
                Rowspan = 4,
                Padding = 0f
            };
            headertable.AddCell(nesthousing);

            headertable.AddCell("");
            var invoiceCell = new PdfPCell(new Phrase("Factura", titleFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.NO_BORDER
            };
            headertable.AddCell(invoiceCell);
            var noCell = new PdfPCell(new Phrase("Nº :", bodyFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.NO_BORDER
            };
            headertable.AddCell(noCell);
            headertable.AddCell(new Phrase(factura.Codigo, bodyFont));
            var dateCell = new PdfPCell(new Phrase("Fecha :", bodyFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.NO_BORDER
            };
            headertable.AddCell(dateCell);
            headertable.AddCell(new Phrase(factura.Fecha.ToString("dd-MM-yyyy"), bodyFont));
            var billCell = new PdfPCell(new Phrase("Para :", bodyFont))
            {
                HorizontalAlignment = 2,
                Border = Rectangle.NO_BORDER
            };
            headertable.AddCell(billCell);
            headertable.AddCell(new Phrase(factura.Paciente.Nombre + "\n" + factura.Paciente.Direccion, bodyFont));
            document.Add(headertable);
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
            itemTable.DefaultCell.Border = Rectangle.BOX;
            var cell1 = new PdfPCell(new Phrase("Concepto", boldTableFont))
            {
                HorizontalAlignment = 1
            };
            itemTable.AddCell(cell1);
            var cell2 = new PdfPCell(new Phrase("Cantidad", boldTableFont))
            {
                HorizontalAlignment = 1
            };
            itemTable.AddCell(cell2);
            var cell3 = new PdfPCell(new Phrase("Precio (€)", boldTableFont))
            {
                HorizontalAlignment = 1
            };
            itemTable.AddCell(cell3);
            var cell4 = new PdfPCell(new Phrase("Total (€)", boldTableFont))
            {
                HorizontalAlignment = 1
            };
            itemTable.AddCell(cell4);

            foreach (var row in factura.Lineas)
            {
                var numberCell = new PdfPCell(new Phrase(row.Concepto, bodyFont))
                {
                    HorizontalAlignment = 0,
                    PaddingLeft = 10f,
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
                };
                itemTable.AddCell(numberCell);

                var descCell = new PdfPCell(new Phrase(row.Cantidad.ToString(), bodyFont))
                {
                    HorizontalAlignment = 0,
                    PaddingLeft = 10f,
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
                };
                itemTable.AddCell(descCell);

                var qtyCell = new PdfPCell(new Phrase(row.Precio.ToString(), bodyFont))
                {
                    HorizontalAlignment = 0,
                    PaddingLeft = 10f,
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
                };
                itemTable.AddCell(qtyCell);

                var amtCell = new PdfPCell(new Phrase((row.Cantidad * row.Precio).ToString(), bodyFont))
                {
                    HorizontalAlignment = 1,
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
                };
                itemTable.AddCell(amtCell);

            }
            // Table footer
            var totalAmtCell1 = new PdfPCell(new Phrase(""))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER
            };
            itemTable.AddCell(totalAmtCell1);
            var totalAmtCell2 = new PdfPCell(new Phrase($"Descuento:{factura.Descuento}%\nIRPF:{factura.IRPF}%", clausulaFont))
            {
                Border = Rectangle.TOP_BORDER //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            };
            itemTable.AddCell(totalAmtCell2);
            var totalAmtStrCell = new PdfPCell(new Phrase("Cantidad total", boldTableFont))
            {
                Border = Rectangle.TOP_BORDER,   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
                HorizontalAlignment = 1
            };
            itemTable.AddCell(totalAmtStrCell);

            var totalSuma = factura.Lineas.Sum(x => x.Precio * x.Cantidad);
            var totalDescuento = totalSuma * (1 - factura.Descuento / 100.0);
            var totalFinal = totalDescuento * (1 + factura.IRPF / 100.0);

            var totalAmtCell = new PdfPCell(new Phrase(totalFinal.ToString("#,###.00"), boldTableFont))
            {
                HorizontalAlignment = 1
            };
            itemTable.AddCell(totalAmtCell);

            var cell = new PdfPCell(new Phrase("*** Mensaje o no ***", bodyFont))
            {
                Colspan = 4,
                HorizontalAlignment = 1
            };
            itemTable.AddCell(cell);
            document.Add(itemTable);
            #endregion

            var transferBank = new Chunk("Datos bancarios:", boldTableFont);
            transferBank.SetUnderline(0.1f, -2f); //0.1 thick, -2 y-location
            document.Add(transferBank);
            document.Add(Chunk.Newline);

            // Bank Account Info
            var bottomTable = new PdfPTable(3)
            {
                HorizontalAlignment = 0,
                TotalWidth = 300f
            };
            bottomTable.SetWidths(new int[] { 90, 10, 200 });
            bottomTable.LockedWidth = true;
            bottomTable.SpacingBefore = 20;
            bottomTable.DefaultCell.Border = Rectangle.NO_BORDER;
            bottomTable.AddCell(new Phrase("Nº Cuenta", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase("0000.0000.00.00.000000000000", bodyFont));
            bottomTable.AddCell(new Phrase("Titular", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase("Jorge Turrado Ferrero", bodyFont));
            bottomTable.AddCell(new Phrase("Banco", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase("Santander", bodyFont));
            document.Add(bottomTable);

            //Image Singature
            var logo = iTextSharp.text.Image.GetInstance(File.Open(Path.Combine(wwwroot, "logo", "akari.png"), FileMode.Open));
            logo.SetAbsolutePosition(pageSize.GetLeft(300), 140);
            document.Add(logo);

            writer.CloseStream = false; //set the closestream property
                                        // Close the Document without closing the underlying stream
            document.Close();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
