using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using BookingPlatform.Infrastructure.Repositories;
using DinkToPdf;
using DinkToPdf.Contracts;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace BookingPlatform.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GeneratePdf(string content)
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var graphics = XGraphics.FromPdfPage(page);

                var font = new XFont("Arial", 12, XFontStyleEx.Regular);

                var textFormatter = new XTextFormatter(graphics);

                var rect = new XRect(40, 40, page.Width - 80, page.Height - 80);
                textFormatter.DrawString(content, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                using (var stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }
    }
}
